using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

namespace Pixel_Drift_Server
{
    public class Game_Room
    {
        // --- Properties ---
        public string Room_ID { get; private set; }

        // --- Constants & Config ---
        private const int Config_Logic_FPS = 60;
        private const int Config_Network_FPS = 40;
        private const int Config_Game_Duration = 60;
        private const int Config_Move_Speed = 10;
        private const int Map_Height = 800;
        private const int Map_Min_X = 6;
        private const int Map_Max_X = 600;

        // --- Player Data ---
        private Game_Player Player_1;
        private Game_Player Player_2;
        private readonly object Lock_Object = new object();

        // --- Game State ---
        private bool Is_Running = false;
        private int Time_Remaining = 60;
        private DateTime Last_Network_Send_Time = DateTime.Now;
        private Timer Countdown_Timer;
        private int Countdown_Value = 5;

        // --- Physics & Inputs ---
        private Dictionary<string, Size> Object_Sizes = new Dictionary<string, Size>();
        private Dictionary<string, Point> Object_Positions = new Dictionary<string, Point>();
        private Dictionary<string, object> Game_State_Packet = new Dictionary<string, object>();
        private Random Random_Gen = new Random();

        // Inputs
        private bool Input_P1_Left, Input_P1_Right;
        private bool Input_P2_Left, Input_P2_Right;

        // Stats
        private int Score_P1 = 0, Speed_P1 = 12, Crash_Count_P1 = 0;
        private int Score_P2 = 0, Speed_P2 = 12, Crash_Count_P2 = 0;

        public Game_Room(string ID)
        {
            this.Room_ID = ID;
            Game_State_Packet["action"] = "update_game_state";
        }

        public bool Is_Empty()
        {
            return Player_1 == null && Player_2 == null;
        }

        // --- Public Methods (Join, Leave, Action) ---

        public int Add_Player(TcpClient Client, string Raw_Username)
        {
            lock (Lock_Object)
            {
                if (Is_Running) return -1;
                string Clean_Name = Clean_Username(Raw_Username);

                if (Player_1 == null)
                {
                    Player_1 = new Game_Player { Client = Client, Stream = Client.GetStream(), Username = Clean_Name, Player_ID = 1 };
                    Broadcast_Ready_Status();
                    return 1;
                }
                else if (Player_2 == null)
                {
                    Player_2 = new Game_Player { Client = Client, Stream = Client.GetStream(), Username = Clean_Name, Player_ID = 2 };
                    Broadcast_Ready_Status();
                    return 2;
                }
                return -1;
            }
        }

        public void Remove_Player(TcpClient Client)
        {
            lock (Lock_Object)
            {
                string Leaver_Name = "Unknown";
                Game_Player Survivor = null;

                if (Player_1 != null && Player_1.Client == Client)
                {
                    Leaver_Name = Player_1.Username;
                    Player_1 = null;
                    Survivor = Player_2;
                }
                else if (Player_2 != null && Player_2.Client == Client)
                {
                    Leaver_Name = Player_2.Username;
                    Player_2 = null;
                    Survivor = Player_1;
                }

                Stop_Game();

                var Msg = new
                {
                    action = "player_disconnected",
                    name = Leaver_Name,
                    target_action = (Survivor != null) ? "opponent_left" : null
                };
                Broadcast_To_All(JsonSerializer.Serialize(Msg));
                Broadcast_Ready_Status();
            }
        }

        public void Process_Player_Action(TcpClient Client, string Action, Dictionary<string, JsonElement> Data)
        {
            int PID = 0;
            if (Player_1 != null && Player_1.Client == Client) PID = 1;
            else if (Player_2 != null && Player_2.Client == Client) PID = 2;

            if (PID == 0) return;

            switch (Action)
            {
                case "set_ready":
                    if (Is_Running) return;
                    bool Ready = Data["ready_status"].GetString() == "true";
                    lock (Lock_Object)
                    {
                        if (PID == 1) Player_1.Is_Ready = Ready;
                        else Player_2.Is_Ready = Ready;
                    }
                    Broadcast_Ready_Status();
                    Check_Start_Condition();
                    break;

                case "move":
                    if (!Is_Running) return;
                    string Dir = Data["direction"].GetString();
                    bool Is_Pressed = Data["state"].GetString() == "down";

                    lock (Lock_Object)
                    {
                        if (PID == 1)
                        {
                            if (Dir == "left") Input_P1_Left = Is_Pressed;
                            else if (Dir == "right") Input_P1_Right = Is_Pressed;
                        }
                        else
                        {
                            if (Dir == "left") Input_P2_Left = Is_Pressed;
                            else if (Dir == "right") Input_P2_Right = Is_Pressed;
                        }
                    }
                    break;

                case "leave_room":
                    Remove_Player(Client);
                    break;
            }
        }

        // --- Game Flow Control ---

        private void Check_Start_Condition()
        {
            lock (Lock_Object)
            {
                if (Player_1 != null && Player_1.Is_Ready && Player_2 != null && Player_2.Is_Ready && Countdown_Timer == null)
                {
                    Countdown_Value = 5;
                    Countdown_Timer = new Timer(On_Countdown_Tick, null, 0, 1000);
                }
            }
        }

        private void On_Countdown_Tick(object State)
        {
            if (Countdown_Value > 0)
            {
                Broadcast_To_All(JsonSerializer.Serialize(new { action = "countdown", time = Countdown_Value }));
                Countdown_Value--;
            }
            else
            {
                Countdown_Timer?.Dispose();
                Countdown_Timer = null;
                Start_Game();
            }
        }

        private void Start_Game()
        {
            Initialize_Game_Session();
            Is_Running = true;
            Broadcast_To_All(JsonSerializer.Serialize(new { action = "start_game" }));
            Broadcast_Game_State();
            Task.Run(Game_Loop_Async);
        }

        private void Initialize_Game_Session()
        {
            lock (Lock_Object)
            {
                Score_P1 = 0; Score_P2 = 0;
                Speed_P1 = 12; Speed_P2 = 12;
                Crash_Count_P1 = 0; Crash_Count_P2 = 0;
                Input_P1_Left = false; Input_P1_Right = false;
                Input_P2_Left = false; Input_P2_Right = false;

                Object_Positions.Clear();
                Object_Sizes.Clear();
                Setup_Map_Objects();
            }
        }

        private async Task Game_Loop_Async()
        {
            Time_Remaining = Config_Game_Duration;
            DateTime Last_Second_Tick = DateTime.Now;
            int Delay_Ms = 1000 / Config_Logic_FPS;

            while (Is_Running && Time_Remaining > 0)
            {
                Update_Physics();

                // 1-second interval updates
                if ((DateTime.Now - Last_Second_Tick).TotalSeconds >= 1)
                {
                    Time_Remaining--;
                    Last_Second_Tick = DateTime.Now;
                    Score_P1 += Speed_P1;
                    Score_P2 += Speed_P2;

                    Broadcast_To_All(JsonSerializer.Serialize(new { action = "update_time", time = Time_Remaining }));
                    Broadcast_To_All(JsonSerializer.Serialize(new { action = "update_score", p1_score = Score_P1, p2_score = Score_P2 }));
                }

                // Network Sync
                if ((DateTime.Now - Last_Network_Send_Time).TotalMilliseconds >= (1000 / Config_Network_FPS))
                {
                    Broadcast_Game_State();
                    Last_Network_Send_Time = DateTime.Now;
                }

                await Task.Delay(Delay_Ms);
            }

            if (Is_Running) End_Game();
        }

        private void Stop_Game()
        {
            Is_Running = false;
            Countdown_Timer?.Dispose();
            Countdown_Timer = null;
        }

        private void End_Game()
        {
            Is_Running = false;
            Save_Match_Results();
            Broadcast_To_All(JsonSerializer.Serialize(new { action = "game_over" }));

            Object_Positions.Clear();
            lock (Lock_Object)
            {
                if (Player_1 != null) Player_1.Is_Ready = false;
                if (Player_2 != null) Player_2.Is_Ready = false;
            }
            Broadcast_Ready_Status();
        }

        // --- Physics Engine ---

        private void Update_Physics()
        {
            lock (Lock_Object)
            {
                if (!Is_Running) return;

                // Player 1 Logic
                if (Player_1 != null && Object_Positions.ContainsKey("ptb_player1"))
                {
                    Point P = Object_Positions["ptb_player1"];
                    if (Input_P1_Left && P.X > Map_Min_X) P.X -= Config_Move_Speed;
                    if (Input_P1_Right && P.X < Map_Max_X - Object_Sizes["ptb_player1"].Width) P.X += Config_Move_Speed;
                    P.X = Math.Clamp(P.X, Map_Min_X, Map_Max_X - Object_Sizes["ptb_player1"].Width);
                    Object_Positions["ptb_player1"] = P;
                }

                // Player 2 Logic
                if (Player_2 != null && Object_Positions.ContainsKey("ptb_player2"))
                {
                    Point P = Object_Positions["ptb_player2"];
                    if (Input_P2_Left && P.X > Map_Min_X) P.X -= Config_Move_Speed;
                    if (Input_P2_Right && P.X < Map_Max_X - Object_Sizes["ptb_player2"].Width) P.X += Config_Move_Speed;
                    P.X = Math.Clamp(P.X, Map_Min_X, Map_Max_X - Object_Sizes["ptb_player2"].Width);
                    Object_Positions["ptb_player2"] = P;
                }

                Move_All_Objects();
                Check_Collisions();
            }
        }

        private void Move_All_Objects()
        {
            Move_Item_Down("ptb_road_1", Speed_P1, true);
            Move_Item_Down("ptb_road_1_dup", Speed_P1, true);
            Move_Item_Down("ptb_AICar1", Speed_P1, false);
            Move_Item_Down("ptb_AICar5", Speed_P1, false);
            Move_Item_Down("ptb_buff_road_1", Speed_P1, false);
            Move_Item_Down("ptb_debuff_road_1", Speed_P1, false);

            Move_Item_Down("ptb_road_2", Speed_P2, true);
            Move_Item_Down("ptb_road_2_dup", Speed_P2, true);
            Move_Item_Down("ptb_AICar3", Speed_P2, false);
            Move_Item_Down("ptb_AICar6", Speed_P2, false);
            Move_Item_Down("ptb_buff_road_2", Speed_P2, false);
            Move_Item_Down("ptb_debuff_road_2", Speed_P2, false);
        }

        private void Move_Item_Down(string Name, int Speed, bool Is_Road)
        {
            if (!Object_Positions.ContainsKey(Name)) return;
            Point Pos = Object_Positions[Name];
            Pos.Y += Speed;

            if (Pos.Y > Map_Height)
            {
                if (Is_Road)
                {
                    string Dup_Name = (Name == "ptb_road_1") ? "ptb_road_1_dup" :
                                      (Name == "ptb_road_1_dup") ? "ptb_road_1" :
                                      (Name == "ptb_road_2") ? "ptb_road_2_dup" : "ptb_road_2";
                    Pos.Y = Object_Positions[Dup_Name].Y - Map_Height;
                }
                else
                {
                    Pos = Calculate_Respawn_Position(Name, Map_Min_X, Map_Max_X);
                }
            }
            Object_Positions[Name] = Pos;
        }

        private void Check_Collisions()
        {
            // Player 1
            if (Player_1 != null)
            {
                if (Is_Colliding("ptb_player1", "ptb_buff_road_1")) { Speed_P1 += 4; Score_P1 += 400; Respawn_Object("ptb_buff_road_1"); Send_Sound_To_Player(Player_1, "buff"); }
                if (Is_Colliding("ptb_player1", "ptb_debuff_road_1")) { Speed_P1 -= 4; Score_P1 -= 200; Respawn_Object("ptb_debuff_road_1"); Send_Sound_To_Player(Player_1, "debuff"); }
                if (Is_Colliding("ptb_player1", "ptb_AICar1")) { Speed_P1 -= 4; Score_P1 -= 100; Crash_Count_P1++; Respawn_Object("ptb_AICar1"); Send_Sound_To_Player(Player_1, "hit_car"); }
                if (Is_Colliding("ptb_player1", "ptb_AICar5")) { Speed_P1 -= 4; Score_P1 -= 100; Crash_Count_P1++; Respawn_Object("ptb_AICar5"); Send_Sound_To_Player(Player_1, "hit_car"); }
                Speed_P1 = Math.Max(10, Speed_P1);
            }

            // Player 2
            if (Player_2 != null)
            {
                if (Is_Colliding("ptb_player2", "ptb_buff_road_2")) { Speed_P2 += 4; Score_P2 += 400; Respawn_Object("ptb_buff_road_2"); Send_Sound_To_Player(Player_2, "buff"); }
                if (Is_Colliding("ptb_player2", "ptb_debuff_road_2")) { Speed_P2 -= 4; Score_P2 -= 200; Respawn_Object("ptb_debuff_road_2"); Send_Sound_To_Player(Player_2, "debuff"); }
                if (Is_Colliding("ptb_player2", "ptb_AICar3")) { Speed_P2 -= 4; Score_P2 -= 100; Crash_Count_P2++; Respawn_Object("ptb_AICar3"); Send_Sound_To_Player(Player_2, "hit_car"); }
                if (Is_Colliding("ptb_player2", "ptb_AICar6")) { Speed_P2 -= 4; Score_P2 -= 100; Crash_Count_P2++; Respawn_Object("ptb_AICar6"); Send_Sound_To_Player(Player_2, "hit_car"); }
                Speed_P2 = Math.Max(10, Speed_P2);
            }
        }

        private bool Is_Colliding(string Player_Key, string Obj_Key)
        {
            if (!Object_Positions.ContainsKey(Player_Key) || !Object_Positions.ContainsKey(Obj_Key)) return false;
            return new Rectangle(Object_Positions[Player_Key], Object_Sizes[Player_Key]).IntersectsWith(new Rectangle(Object_Positions[Obj_Key], Object_Sizes[Obj_Key]));
        }

        private void Respawn_Object(string Name)
        {
            Calculate_Respawn_Position(Name, Map_Min_X, Map_Max_X);
        }

        private Point Calculate_Respawn_Position(string Name, int Min_X, int Max_X)
        {
            Size Current_Size = Object_Sizes.ContainsKey(Name) ? Object_Sizes[Name] : new Size(30, 30);
            int Safe_Max_X = Max_X - Current_Size.Width - 150;
            if (Safe_Max_X <= Min_X) Safe_Max_X = Min_X + 1;

            int Max_Retries = 40;
            int Attempts = 0;
            Point New_Pos;
            bool Overlap;

            do
            {
                Overlap = false;
                New_Pos = new Point(Random_Gen.Next(Min_X, Safe_Max_X), Random_Gen.Next(-1000, -150));
                Rectangle New_Rect = new Rectangle(New_Pos, Current_Size);
                New_Rect.Inflate(40, 140); // Margin

                foreach (var Key in Object_Positions.Keys)
                {
                    if (Key == Name || Key.Contains("road") || Key.Contains("player")) continue;
                    if (Object_Sizes.ContainsKey(Key))
                    {
                        if (New_Rect.IntersectsWith(new Rectangle(Object_Positions[Key], Object_Sizes[Key])))
                        {
                            Overlap = true; break;
                        }
                    }
                }
                Attempts++;
            } while (Overlap && Attempts < Max_Retries);

            if (Overlap) New_Pos.Y -= 1000;
            if (Object_Positions.ContainsKey(Name)) Object_Positions[Name] = New_Pos;
            return New_Pos;
        }

        // --- Network & Helpers ---

        private void Broadcast_Game_State()
        {
            lock (Lock_Object)
            {
                foreach (var Kvp in Object_Positions) Game_State_Packet[Kvp.Key] = Kvp.Value;
            }
            Broadcast_To_All(JsonSerializer.Serialize(Game_State_Packet));
        }

        private void Broadcast_To_All(string Message)
        {
            lock (Lock_Object)
            {
                if (Player_1 != null) Send_Message(Player_1.Stream, Message);
                if (Player_2 != null) Send_Message(Player_2.Stream, Message);
            }
        }

        private void Broadcast_Ready_Status()
        {
            lock (Lock_Object)
            {
                var Status = new
                {
                    action = "update_ready_status",
                    player1_ready = Player_1?.Is_Ready ?? false,
                    player1_name = Player_1?.Username ?? "Waiting...",
                    player2_ready = Player_2?.Is_Ready ?? false,
                    player2_name = Player_2?.Username ?? "Waiting..."
                };
                Broadcast_To_All(JsonSerializer.Serialize(Status));
            }
        }

        private void Send_Message(NetworkStream Stream, string Message)
        {
            if (Stream == null || !Stream.CanWrite) return;
            try
            {
                byte[] Buffer = Encoding.UTF8.GetBytes(Message + "\n");
                lock (Stream) { Stream.Write(Buffer, 0, Buffer.Length); }
            }
            catch { }
        }

        private void Send_Sound_To_Player(Game_Player P, string Sound_Name)
        {
            if (P != null)
                Send_Message(P.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = Sound_Name }));
        }

        private void Save_Match_Results()
        {
            try
            {
                lock (Lock_Object)
                {
                    bool P1_Win = Score_P1 > Score_P2;
                    bool P2_Win = Score_P2 > Score_P1;
                    double P1_Final = Score_P1 + (P1_Win ? 500 : 0) - (Crash_Count_P1 * 50);
                    double P2_Final = Score_P2 + (P2_Win ? 500 : 0) - (Crash_Count_P2 * 50);

                    if (Player_1 != null && !string.IsNullOrEmpty(Player_1.Username))
                        SQL_Handle.Handle_Match_Result(1, P1_Win, (int)P1_Final, Crash_Count_P1);

                    if (Player_2 != null && !string.IsNullOrEmpty(Player_2.Username))
                        SQL_Handle.Handle_Match_Result(2, P2_Win, (int)P2_Final, Crash_Count_P2);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"[Error] Save Scores Failed: {Ex.Message}");
            }
        }

        private void Setup_Map_Objects()
        {
            int Safe_Margin = 50;
            int Min_X = Safe_Margin;
            int Max_X = 500 - Safe_Margin;

            // Players
            Object_Sizes["ptb_player1"] = new Size(80, 175);
            Object_Positions["ptb_player1"] = new Point(270, 710);
            Object_Sizes["ptb_player2"] = new Size(80, 175);
            Object_Positions["ptb_player2"] = new Point(270, 710);

            // Roads
            Object_Sizes["ptb_road_1"] = new Size(617, 919); Object_Positions["ptb_road_1"] = new Point(0, -1);
            Object_Sizes["ptb_road_1_dup"] = new Size(617, 919); Object_Positions["ptb_road_1_dup"] = new Point(0, 919);
            Object_Sizes["ptb_road_2"] = new Size(617, 919); Object_Positions["ptb_road_2"] = new Point(0, 1);
            Object_Sizes["ptb_road_2_dup"] = new Size(617, 919); Object_Positions["ptb_road_2_dup"] = new Point(0, 919);

            string[] AI_Objects =
            {
                "ptb_AICar1",
                "ptb_buff_road_1",
                "ptb_AICar3",
                "ptb_debuff_road_1",
                "ptb_AICar5",
                "ptb_buff_road_2",
                "ptb_AICar6",
                "ptb_debuff_road_2"
            };

            Size Car_Size = new Size(80, 175);
            Size Item_Size = new Size(50, 50);

            foreach (var obj in AI_Objects)
            {
                bool isItem = obj.Contains("buff") || obj.Contains("debuff");

                Object_Sizes[obj] = isItem ? Item_Size : Car_Size;
                Object_Positions[obj] = Calculate_Respawn_Position(obj, Min_X, Max_X);
            }
        }

        private string Clean_Username(string Name)
        {
            if (string.IsNullOrEmpty(Name)) return "Unknown";
            int Idx = Name.LastIndexOf('(');
            return (Idx > 0) ? Name.Substring(0, Idx).Trim() : Name.Trim();
        }
    }
}