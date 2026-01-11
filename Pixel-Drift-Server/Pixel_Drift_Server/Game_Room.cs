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
        public string Room_ID { get; private set; }

        private const int Config_Logic_FPS = 60;
        private const int Config_Network_FPS = 40;
        private const int Config_Game_Duration = 60;
        private const int Config_Move_Speed = 10;
        private const int Map_Height = 800;
        private const int Map_Min_X = 6;
        private const int Map_Max_X = 600;

        private Game_Player Player_1;
        private Game_Player Player_2;
        private readonly object Lock_Object = new object();

        private bool Is_Running = false;
        private int Time_Remaining = 60;
        private DateTime Last_Network_Send_Time = DateTime.Now;
        private Timer Countdown_Timer;
        private int Countdown_Value = 5;

        private Dictionary<string, Size> Object_Sizes = new Dictionary<string, Size>();
        private Dictionary<string, Point> Object_Positions = new Dictionary<string, Point>();
        private Dictionary<string, object> Game_State_Packet = new Dictionary<string, object>();
        private Random Random_Gen = new Random();

        private bool Input_P1_Left, Input_P1_Right;
        private bool Input_P2_Left, Input_P2_Right;

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
                Game_Player Survivor_Player = null;

                if (Player_1 != null && Player_1.Client == Client)
                {
                    Leaver_Name = Player_1.Username;
                    Player_1 = null;
                    Survivor_Player = Player_2;
                }
                else if (Player_2 != null && Player_2.Client == Client)
                {
                    Leaver_Name = Player_2.Username;
                    Player_2 = null;
                    Survivor_Player = Player_1;
                }

                Stop_Game();

                var Disconnect_Message = new
                {
                    action = "player_disconnected",
                    name = Leaver_Name,
                    target_action = (Survivor_Player != null) ? "opponent_left" : null
                };
                Broadcast_To_All(JsonSerializer.Serialize(Disconnect_Message));
                Broadcast_Ready_Status();
            }
        }

        public void Process_Player_Action(TcpClient Client, string Action_Type, Dictionary<string, JsonElement> Action_Data)
        {
            int Player_Index = 0;
            if (Player_1 != null && Player_1.Client == Client) Player_Index = 1;
            else if (Player_2 != null && Player_2.Client == Client) Player_Index = 2;

            if (Player_Index == 0) return;

            switch (Action_Type)
            {
                case "set_ready":
                    if (Is_Running) return;
                    bool Is_Ready_Status = Action_Data["ready_status"].GetString() == "true";
                    lock (Lock_Object)
                    {
                        if (Player_Index == 1) Player_1.Is_Ready = Is_Ready_Status;
                        else Player_2.Is_Ready = Is_Ready_Status;
                    }
                    Broadcast_Ready_Status();
                    Check_Start_Condition();
                    break;

                case "move":
                    if (!Is_Running) return;
                    string Move_Direction = Action_Data["direction"].GetString();
                    bool Key_State_Down = Action_Data["state"].GetString() == "down";

                    lock (Lock_Object)
                    {
                        if (Player_Index == 1)
                        {
                            if (Move_Direction == "left") Input_P1_Left = Key_State_Down;
                            else if (Move_Direction == "right") Input_P1_Right = Key_State_Down;
                        }
                        else
                        {
                            if (Move_Direction == "left") Input_P2_Left = Key_State_Down;
                            else if (Move_Direction == "right") Input_P2_Right = Key_State_Down;
                        }
                    }
                    break;

                case "leave_room":
                    Remove_Player(Client);
                    break;
            }
        }

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

        private void On_Countdown_Tick(object State_Object)
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
            int Loop_Delay_Ms = 1000 / Config_Logic_FPS;

            while (Is_Running && Time_Remaining > 0)
            {
                Update_Physics();

                if ((DateTime.Now - Last_Second_Tick).TotalSeconds >= 1)
                {
                    Time_Remaining--;
                    Last_Second_Tick = DateTime.Now;
                    Score_P1 += Speed_P1;
                    Score_P2 += Speed_P2;

                    Broadcast_To_All(JsonSerializer.Serialize(new { action = "update_time", time = Time_Remaining }));
                    Broadcast_To_All(JsonSerializer.Serialize(new { action = "update_score", p1_score = Score_P1, p2_score = Score_P2 }));
                }

                if ((DateTime.Now - Last_Network_Send_Time).TotalMilliseconds >= (1000 / Config_Network_FPS))
                {
                    Broadcast_Game_State();
                    Last_Network_Send_Time = DateTime.Now;
                }

                await Task.Delay(Loop_Delay_Ms);
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

        private void Update_Physics()
        {
            lock (Lock_Object)
            {
                if (!Is_Running) return;

                if (Player_1 != null && Object_Positions.ContainsKey("ptb_player1"))
                {
                    Point Player_Pos = Object_Positions["ptb_player1"];
                    if (Input_P1_Left && Player_Pos.X > Map_Min_X) Player_Pos.X -= Config_Move_Speed;
                    if (Input_P1_Right && Player_Pos.X < Map_Max_X - Object_Sizes["ptb_player1"].Width) Player_Pos.X += Config_Move_Speed;
                    Player_Pos.X = Math.Clamp(Player_Pos.X, Map_Min_X, Map_Max_X - Object_Sizes["ptb_player1"].Width);
                    Object_Positions["ptb_player1"] = Player_Pos;
                }

                if (Player_2 != null && Object_Positions.ContainsKey("ptb_player2"))
                {
                    Point Player_Pos = Object_Positions["ptb_player2"];
                    if (Input_P2_Left && Player_Pos.X > Map_Min_X) Player_Pos.X -= Config_Move_Speed;
                    if (Input_P2_Right && Player_Pos.X < Map_Max_X - Object_Sizes["ptb_player2"].Width) Player_Pos.X += Config_Move_Speed;
                    Player_Pos.X = Math.Clamp(Player_Pos.X, Map_Min_X, Map_Max_X - Object_Sizes["ptb_player2"].Width);
                    Object_Positions["ptb_player2"] = Player_Pos;
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

        private void Move_Item_Down(string Object_Name, int Speed_Value, bool Is_Road_Object)
        {
            if (!Object_Positions.ContainsKey(Object_Name)) return;
            Point Item_Position = Object_Positions[Object_Name];
            Item_Position.Y += Speed_Value;

            if (Item_Position.Y > Map_Height)
            {
                if (Is_Road_Object)
                {
                    string Duplicate_Name = (Object_Name == "ptb_road_1") ? "ptb_road_1_dup" :
                                     (Object_Name == "ptb_road_1_dup") ? "ptb_road_1" :
                                     (Object_Name == "ptb_road_2") ? "ptb_road_2_dup" : "ptb_road_2";
                    Item_Position.Y = Object_Positions[Duplicate_Name].Y - Map_Height;
                }
                else
                {
                    Item_Position = Calculate_Respawn_Position(Object_Name, Map_Min_X, Map_Max_X);
                }
            }
            Object_Positions[Object_Name] = Item_Position;
        }

        private void Check_Collisions()
        {
            if (Player_1 != null)
            {
                if (Is_Colliding("ptb_player1", "ptb_buff_road_1")) { Speed_P1 += 4; Score_P1 += 400; Respawn_Object("ptb_buff_road_1"); Send_Sound_To_Player(Player_1, "buff"); }
                if (Is_Colliding("ptb_player1", "ptb_debuff_road_1")) { Speed_P1 -= 2; Score_P1 -= 100; Respawn_Object("ptb_debuff_road_1"); Send_Sound_To_Player(Player_1, "debuff"); }
                if (Is_Colliding("ptb_player1", "ptb_AICar1")) { Speed_P1 -= 4; Score_P1 -= 100; Crash_Count_P1++; Respawn_Object("ptb_AICar1"); Send_Sound_To_Player(Player_1, "hit_car"); }
                if (Is_Colliding("ptb_player1", "ptb_AICar5")) { Speed_P1 -= 4; Score_P1 -= 100; Crash_Count_P1++; Respawn_Object("ptb_AICar5"); Send_Sound_To_Player(Player_1, "hit_car"); }
                Speed_P1 = Math.Max(10, Speed_P1);
            }

            if (Player_2 != null)
            {
                if (Is_Colliding("ptb_player2", "ptb_buff_road_2")) { Speed_P2 += 4; Score_P2 += 400; Respawn_Object("ptb_buff_road_2"); Send_Sound_To_Player(Player_2, "buff"); }
                if (Is_Colliding("ptb_player2", "ptb_debuff_road_2")) { Speed_P2 -= 2; Score_P2 -= 100; Respawn_Object("ptb_debuff_road_2"); Send_Sound_To_Player(Player_2, "debuff"); }
                if (Is_Colliding("ptb_player2", "ptb_AICar3")) { Speed_P2 -= 4; Score_P2 -= 100; Crash_Count_P2++; Respawn_Object("ptb_AICar3"); Send_Sound_To_Player(Player_2, "hit_car"); }
                if (Is_Colliding("ptb_player2", "ptb_AICar6")) { Speed_P2 -= 4; Score_P2 -= 100; Crash_Count_P2++; Respawn_Object("ptb_AICar6"); Send_Sound_To_Player(Player_2, "hit_car"); }
                Speed_P2 = Math.Max(10, Speed_P2);
            }
        }

        private bool Is_Colliding(string Player_Key, string Object_Key)
        {
            if (!Object_Positions.ContainsKey(Player_Key) || !Object_Positions.ContainsKey(Object_Key)) return false;
            return new Rectangle(Object_Positions[Player_Key], Object_Sizes[Player_Key]).IntersectsWith(new Rectangle(Object_Positions[Object_Key], Object_Sizes[Object_Key]));
        }

        private void Respawn_Object(string Object_Name)
        {
            Calculate_Respawn_Position(Object_Name, Map_Min_X, Map_Max_X);
        }

        private Point Calculate_Respawn_Position(string Object_Name, int Minimum_X, int Maximum_X)
        {
            Size Current_Size = Object_Sizes.ContainsKey(Object_Name) ? Object_Sizes[Object_Name] : new Size(40, 40);

            int Safe_Maximum_X = Maximum_X - Current_Size.Width - 5;
            if (Safe_Maximum_X <= Minimum_X) Safe_Maximum_X = Minimum_X + 1;

            int Max_Retries = 30; 
            int Attempts = 0;
            Point Respawn_Position;
            bool Is_Overlapping;

            int Random_Min_Y = -1500;
            int Random_Max_Y = -200;

            do
            {
                Is_Overlapping = false;

                Respawn_Position = new Point(Random_Gen.Next(Minimum_X, Safe_Maximum_X), Random_Gen.Next(Random_Min_Y, Random_Max_Y));

                Rectangle Respawn_Rect = new Rectangle(Respawn_Position, Current_Size);

                Respawn_Rect.Inflate(20, 400);

                foreach (var Key_Name in Object_Positions.Keys)
                {
                    if (Key_Name == Object_Name || Key_Name.Contains("road") || Key_Name.Contains("player")) continue;

                    if (Object_Positions.ContainsKey(Key_Name))
                    {
                        int Other_X = Object_Positions[Key_Name].X;
                        bool Is_Same_Side = (Minimum_X < 300 && Other_X < 500) || (Minimum_X > 500 && Other_X > 500);

                        if (Is_Same_Side)
                        {
                            if (Respawn_Rect.IntersectsWith(new Rectangle(Object_Positions[Key_Name], Object_Sizes[Key_Name])))
                            {
                                Is_Overlapping = true;
                                break;
                            }
                        }
                    }
                }
                Attempts++;
            } while (Is_Overlapping && Attempts < Max_Retries);

            if (Is_Overlapping)
            {
                int Highest_Y = -200;

                foreach (var Key_Name in Object_Positions.Keys)
                {
                    if (Key_Name == Object_Name || Key_Name.Contains("road") || Key_Name.Contains("player")) continue;

                    if (Object_Positions.ContainsKey(Key_Name))
                    {
                        int Other_X = Object_Positions[Key_Name].X;
                        bool Is_Same_Side = (Minimum_X < 300 && Other_X < 500) || (Minimum_X > 500 && Other_X > 500);

                        if (Is_Same_Side && Object_Positions[Key_Name].Y < Highest_Y)
                        {
                            Highest_Y = Object_Positions[Key_Name].Y;
                        }
                    }
                }

                Respawn_Position.Y = Highest_Y - 400;

                Respawn_Position.X = Random_Gen.Next(Minimum_X, Safe_Maximum_X);
            }

            if (Object_Positions.ContainsKey(Object_Name)) Object_Positions[Object_Name] = Respawn_Position;
            return Respawn_Position;
        }

        private void Broadcast_Game_State()
        {
            lock (Lock_Object)
            {
                foreach (var Map_Object in Object_Positions) Game_State_Packet[Map_Object.Key] = Map_Object.Value;
            }
            Broadcast_To_All(JsonSerializer.Serialize(Game_State_Packet));
        }

        private void Broadcast_To_All(string Message_Content)
        {
            lock (Lock_Object)
            {
                if (Player_1 != null) Send_Message(Player_1.Stream, Message_Content);
                if (Player_2 != null) Send_Message(Player_2.Stream, Message_Content);
            }
        }

        private void Broadcast_Ready_Status()
        {
            lock (Lock_Object)
            {
                var Ready_Status_Data = new
                {
                    action = "update_ready_status",
                    player1_ready = Player_1?.Is_Ready ?? false,
                    player1_name = Player_1?.Username ?? "Waiting...",
                    player2_ready = Player_2?.Is_Ready ?? false,
                    player2_name = Player_2?.Username ?? "Waiting..."
                };
                Broadcast_To_All(JsonSerializer.Serialize(Ready_Status_Data));
            }
        }

        private void Send_Message(NetworkStream Target_Stream, string Message_Content)
        {
            if (Target_Stream == null || !Target_Stream.CanWrite) return;
            try
            {
                byte[] Buffer_Bytes = Encoding.UTF8.GetBytes(Message_Content + "\n");
                lock (Target_Stream) { Target_Stream.Write(Buffer_Bytes, 0, Buffer_Bytes.Length); }
            }
            catch { }
        }

        private void Send_Sound_To_Player(Game_Player Target_Player, string Sound_Name)
        {
            if (Target_Player != null)
                Send_Message(Target_Player.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = Sound_Name }));
        }

        private void Save_Match_Results()
        {
            try
            {
                lock (Lock_Object)
                {
                    bool P1_Wins = Score_P1 > Score_P2;
                    bool P2_Wins = Score_P2 > Score_P1;
                    double P1_Final_Score = Score_P1 + (P1_Wins ? 500 : 0) - (Crash_Count_P1 * 50);
                    double P2_Final_Score = Score_P2 + (P2_Wins ? 500 : 0) - (Crash_Count_P2 * 50);

                    if (Player_1 != null && !string.IsNullOrEmpty(Player_1.Username))
                        SQL_Handle.Handle_Match_Result(1, P1_Wins, (int)P1_Final_Score, Crash_Count_P1);

                    if (Player_2 != null && !string.IsNullOrEmpty(Player_2.Username))
                        SQL_Handle.Handle_Match_Result(2, P2_Wins, (int)P2_Final_Score, Crash_Count_P2);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"[Error] Save Scores Failed: {Ex.Message}");
            }
        }

        private void Setup_Map_Objects()
        {
            int Safe_Margin_X = 50;
            int Minimum_X = Safe_Margin_X;
            int Maximum_X = 500 - Safe_Margin_X;

            Object_Sizes["ptb_player1"] = new Size(72, 117);
            Object_Positions["ptb_player1"] = new Point(202, 470);
            Object_Sizes["ptb_player2"] = new Size(72, 117);
            Object_Positions["ptb_player2"] = new Point(202, 470);

            Object_Sizes["ptb_road_1"] = new Size(617, 734); Object_Positions["ptb_road_1"] = new Point(0, -2);
            Object_Sizes["ptb_road_1_dup"] = new Size(617, 734); Object_Positions["ptb_road_1_dup"] = new Point(0, 734);
            Object_Sizes["ptb_road_2"] = new Size(458, 596); Object_Positions["ptb_road_2"] = new Point(0, 2);
            Object_Sizes["ptb_road_2_dup"] = new Size(458, 596); Object_Positions["ptb_road_2_dup"] = new Point(0, 596);

            string[] List_AI_Cars = { "ptb_AICar1", "ptb_AICar3", "ptb_AICar5", "ptb_AICar6" };
            Size AI_Car_Size = new Size(74, 128);
            foreach (var Car_Name in List_AI_Cars)
            {
                Object_Sizes[Car_Name] = AI_Car_Size;
                Object_Positions[Car_Name] = Calculate_Respawn_Position(Car_Name, Minimum_X, Maximum_X);
            }

            string[] List_Items = { "ptb_buff_road_1", "ptb_debuff_road_1", "ptb_buff_road_2", "ptb_debuff_road_2" };
            Size Item_Size = new Size(30, 30);
            foreach (var Item_Name in List_Items)
            {
                Object_Sizes[Item_Name] = Item_Size;
                Object_Positions[Item_Name] = Calculate_Respawn_Position(Item_Name, Minimum_X, Maximum_X);
            }
        }

        private string Clean_Username(string Input_Name)
        {
            if (string.IsNullOrEmpty(Input_Name)) return "Unknown";
            int Bracket_Index = Input_Name.LastIndexOf('(');
            return (Bracket_Index > 0) ? Input_Name.Substring(0, Bracket_Index).Trim() : Input_Name.Trim();
        }
    }
}