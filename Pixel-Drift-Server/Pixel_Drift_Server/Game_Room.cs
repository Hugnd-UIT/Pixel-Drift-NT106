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
        private Game_Player Player_1;
        private Game_Player Player_2;
        private readonly object Player_Lock = new object();

        // Config Constants
        private const int Config_Logic_FPS = 60;
        private const int Config_Network_FPS = 40;
        private const int Config_Game_Duration = 60;
        private const int Config_Move_Speed = 10;
        private const int Config_Map_Height = 800;

        // Game State
        private bool State_Is_Running = false;
        private int State_Time_Remaining = 60;
        private DateTime State_Last_Network_Send = DateTime.Now;
        private Timer State_Countdown_Timer;
        private int State_Countdown_Value = 5;

        // Physics Data
        private Dictionary<string, Size> Dict_Object_Sizes = new Dictionary<string, Size>();
        private Dictionary<string, Point> Dict_Object_Pos = new Dictionary<string, Point>();
        private Dictionary<string, object> Dict_Game_State_Packet = new Dictionary<string, object>();
        private Random Rand = new Random();

        // Player Inputs
        private bool Input_P1_Left, Input_P1_Right;
        private bool Input_P2_Left, Input_P2_Right;

        // Player Stats (Score, Speed, Crash)
        private int Stats_P1_Score = 0, Stats_P1_Speed = 12, Stats_P1_Crash = 0;
        private int Stats_P2_Score = 0, Stats_P2_Speed = 12, Stats_P2_Crash = 0;

        // Map Boundaries
        private const int Map_P1_MinX = 6, Map_P1_MaxX = 600;
        private const int Map_P2_MinX = 6, Map_P2_MaxX = 600;

        public Game_Room(string ID)
        {
            this.Room_ID = ID;
            Dict_Game_State_Packet["action"] = "update_game_state";
        }

        public bool Is_Empty()
        {
            return Player_1 == null && Player_2 == null;
        }

        public int Handle_Player_Join(TcpClient Client, string Username)
        {
            lock (Player_Lock)
            {
                if (State_Is_Running) return -1;
                string Clean_Name = Format_Username(Username);

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

        public void Handle_Player_Leave(TcpClient Client)
        {
            lock (Player_Lock)
            {
                string Left_User = "Unknown";
                Game_Player Remaining_Player = null;

                if (Player_1 != null && Player_1.Client == Client)
                {
                    Left_User = Player_1.Username;
                    Player_1 = null;
                    Remaining_Player = Player_2;
                }
                else if (Player_2 != null && Player_2.Client == Client)
                {
                    Left_User = Player_2.Username;
                    Player_2 = null;
                    Remaining_Player = Player_1;
                }

                Handle_Game_Stop();
                var Disconnect_Msg = new
                {
                    action = "player_disconnected",
                    name = Left_User,
                    target_action = (Remaining_Player != null) ? "opponent_left" : null
                };
                Broadcast_Message(JsonSerializer.Serialize(Disconnect_Msg));
                Broadcast_Ready_Status();
            }
        }

        public void Handle_Request(TcpClient Client, string Action, Dictionary<string, JsonElement> Data)
        {
            int Player_ID = 0;
            if (Player_1 != null && Player_1.Client == Client) Player_ID = 1;
            else if (Player_2 != null && Player_2.Client == Client) Player_ID = 2;
            if (Player_ID == 0) return;

            switch (Action)
            {
                case "set_ready":
                    if (State_Is_Running) return;
                    bool Ready = Data["ready_status"].GetString() == "true";
                    lock (Player_Lock)
                    {
                        if (Player_ID == 1) Player_1.Is_Ready = Ready;
                        else Player_2.Is_Ready = Ready;
                    }
                    Broadcast_Ready_Status();
                    Handle_Countdown_Check();
                    break;

                case "move":
                    if (!State_Is_Running) return;
                    string Direction = Data["direction"].GetString();
                    string State = Data["state"].GetString();
                    bool Is_Pressed = (State == "down");
                    lock (Player_Lock)
                    {
                        if (Player_ID == 1)
                        {
                            if (Direction == "left") Input_P1_Left = Is_Pressed;
                            else if (Direction == "right") Input_P1_Right = Is_Pressed;
                        }
                        else
                        {
                            if (Direction == "left") Input_P2_Left = Is_Pressed;
                            else if (Direction == "right") Input_P2_Right = Is_Pressed;
                        }
                    }
                    break;

                case "leave_room":
                    Handle_Player_Leave(Client);
                    break;
            }
        }

        private void Handle_Countdown_Check()
        {
            lock (Player_Lock)
            {
                if (Player_1 != null && Player_1.Is_Ready && Player_2 != null && Player_2.Is_Ready && State_Countdown_Timer == null)
                {
                    State_Countdown_Value = 5;
                    State_Countdown_Timer = new Timer(Handle_Countdown_Tick, null, 0, 1000);
                }
            }
        }

        private void Handle_Countdown_Tick(object State)
        {
            if (State_Countdown_Value > 0)
            {
                Broadcast_Message(JsonSerializer.Serialize(new { action = "countdown", time = State_Countdown_Value }));
                State_Countdown_Value--;
            }
            else
            {
                State_Countdown_Timer?.Dispose();
                State_Countdown_Timer = null;
                Handle_Game_Start();
            }
        }

        private void Handle_Game_Start()
        {
            Init_Game_Session();
            State_Is_Running = true;
            Broadcast_Message(JsonSerializer.Serialize(new { action = "start_game" }));
            Broadcast_State_Update();
            Task.Run(Handle_Game_Loop_Async);
        }

        private void Init_Game_Session()
        {
            lock (Player_Lock)
            {
                Stats_P1_Score = 0; Stats_P2_Score = 0;
                Stats_P1_Speed = 12; Stats_P2_Speed = 12;
                Stats_P1_Crash = 0; Stats_P2_Crash = 0;
                Input_P1_Left = false; Input_P1_Right = false;
                Input_P2_Left = false; Input_P2_Right = false;

                Dict_Object_Pos.Clear();
                Dict_Object_Sizes.Clear();
                Init_Object_Config();
            }
        }

        private async Task Handle_Game_Loop_Async()
        {
            State_Time_Remaining = Config_Game_Duration;
            DateTime Last_Second_Tick = DateTime.Now;
            int Loop_Delay = 1000 / Config_Logic_FPS;

            while (State_Is_Running && State_Time_Remaining > 0)
            {
                Handle_Physics_Update();

                if ((DateTime.Now - Last_Second_Tick).TotalSeconds >= 1)
                {
                    State_Time_Remaining--;
                    Last_Second_Tick = DateTime.Now;
                    Stats_P1_Score += Stats_P1_Speed;
                    Stats_P2_Score += Stats_P2_Speed;

                    var Time_Data = new { action = "update_time", time = State_Time_Remaining };
                    var Score_Data = new { action = "update_score", p1_score = Stats_P1_Score, p2_score = Stats_P2_Score };

                    Broadcast_Message(JsonSerializer.Serialize(Time_Data));
                    Broadcast_Message(JsonSerializer.Serialize(Score_Data));
                }

                if ((DateTime.Now - State_Last_Network_Send).TotalMilliseconds >= (1000 / Config_Network_FPS))
                {
                    Broadcast_State_Update();
                    State_Last_Network_Send = DateTime.Now;
                }

                await Task.Delay(Loop_Delay);
            }

            if (State_Is_Running)
            {
                Handle_Game_End();
            }
        }

        private void Handle_Game_Stop()
        {
            State_Is_Running = false;
            State_Countdown_Timer?.Dispose();
            State_Countdown_Timer = null;
        }

        private void Handle_Game_End()
        {
            State_Is_Running = false;
            Handle_Database_Save();
            Broadcast_Message(JsonSerializer.Serialize(new { action = "game_over" }));

            Dict_Object_Pos.Clear();
            lock (Player_Lock)
            {
                if (Player_1 != null) Player_1.Is_Ready = false;
                if (Player_2 != null) Player_2.Is_Ready = false;
            }
            Broadcast_Ready_Status();
        }

        private void Handle_Physics_Update()
        {
            lock (Player_Lock)
            {
                if (!State_Is_Running) return;

                // Update Player 1 Position
                if (Player_1 != null && Dict_Object_Pos.ContainsKey("ptb_player1"))
                {
                    Point P = Dict_Object_Pos["ptb_player1"];
                    if (Input_P1_Left && P.X > Map_P1_MinX) P.X -= Config_Move_Speed;
                    if (Input_P1_Right && P.X < Map_P1_MaxX - Dict_Object_Sizes["ptb_player1"].Width) P.X += Config_Move_Speed;
                    P.X = Math.Clamp(P.X, Map_P1_MinX, Map_P1_MaxX - Dict_Object_Sizes["ptb_player1"].Width);
                    Dict_Object_Pos["ptb_player1"] = P;
                }

                // Update Player 2 Position
                if (Player_2 != null && Dict_Object_Pos.ContainsKey("ptb_player2"))
                {
                    Point P = Dict_Object_Pos["ptb_player2"];
                    if (Input_P2_Left && P.X > Map_P2_MinX) P.X -= Config_Move_Speed;
                    if (Input_P2_Right && P.X < Map_P2_MaxX - Dict_Object_Sizes["ptb_player2"].Width) P.X += Config_Move_Speed;
                    P.X = Math.Clamp(P.X, Map_P2_MinX, Map_P2_MaxX - Dict_Object_Sizes["ptb_player2"].Width);
                    Dict_Object_Pos["ptb_player2"] = P;
                }

                Handle_Object_Movement();
                Handle_Collision_Check();
            }
        }

        private void Handle_Object_Movement()
        {
            Process_Object_Down("ptb_road_1", Stats_P1_Speed, Config_Map_Height, true);
            Process_Object_Down("ptb_road_1_dup", Stats_P1_Speed, Config_Map_Height, true);
            Process_Object_Down("ptb_AICar1", Stats_P1_Speed, Config_Map_Height, false, Map_P1_MinX, Map_P1_MaxX);
            Process_Object_Down("ptb_AICar5", Stats_P1_Speed, Config_Map_Height, false, Map_P1_MinX, Map_P1_MaxX);
            Process_Object_Down("ptb_buff_road_1", Stats_P1_Speed, Config_Map_Height, false, Map_P1_MinX, Map_P1_MaxX);
            Process_Object_Down("ptb_debuff_road_1", Stats_P1_Speed, Config_Map_Height, false, Map_P1_MinX, Map_P1_MaxX);

            Process_Object_Down("ptb_road_2", Stats_P2_Speed, Config_Map_Height, true);
            Process_Object_Down("ptb_road_2_dup", Stats_P2_Speed, Config_Map_Height, true);
            Process_Object_Down("ptb_AICar3", Stats_P2_Speed, Config_Map_Height, false, Map_P2_MinX, Map_P2_MaxX);
            Process_Object_Down("ptb_AICar6", Stats_P2_Speed, Config_Map_Height, false, Map_P2_MinX, Map_P2_MaxX);
            Process_Object_Down("ptb_buff_road_2", Stats_P2_Speed, Config_Map_Height, false, Map_P2_MinX, Map_P2_MaxX);
            Process_Object_Down("ptb_debuff_road_2", Stats_P2_Speed, Config_Map_Height, false, Map_P2_MinX, Map_P2_MaxX);
        }

        private void Process_Object_Down(string Name, int Speed, int Screen_Height, bool Is_Road, int Min_X = 0, int Max_X = 0)
        {
            if (!Dict_Object_Pos.ContainsKey(Name)) return;
            Point Pos = Dict_Object_Pos[Name];
            Pos.Y += Speed;

            if (Pos.Y > Screen_Height)
            {
                if (Is_Road)
                {
                    string Dup_Name = (Name == "ptb_road_1") ? "ptb_road_1_dup" :
                                      (Name == "ptb_road_1_dup") ? "ptb_road_1" :
                                      (Name == "ptb_road_2") ? "ptb_road_2_dup" : "ptb_road_2";
                    Pos.Y = Dict_Object_Pos[Dup_Name].Y - Screen_Height;
                }
                else
                {
                    Pos = Calculate_New_Position(Name, Min_X, Max_X);
                }
            }
            Dict_Object_Pos[Name] = Pos;
        }

        private void Handle_Collision_Check()
        {
            if (Player_1 != null)
            {
                if (Is_Colliding("ptb_player1", "ptb_buff_road_1")) { Stats_P1_Speed += 3; Calculate_New_Position("ptb_buff_road_1", Map_P1_MinX, Map_P1_MaxX); Send_Sound(Player_1, "buff"); }
                if (Is_Colliding("ptb_player1", "ptb_debuff_road_1")) { Stats_P1_Speed -= 3; Calculate_New_Position("ptb_debuff_road_1", Map_P1_MinX, Map_P1_MaxX); Send_Sound(Player_1, "debuff"); }
                if (Is_Colliding("ptb_player1", "ptb_AICar1")) { Stats_P1_Speed -= 4; Stats_P1_Crash++; Calculate_New_Position("ptb_AICar1", Map_P1_MinX, Map_P1_MaxX); Send_Sound(Player_1, "hit_car"); }
                if (Is_Colliding("ptb_player1", "ptb_AICar5")) { Stats_P1_Speed -= 4; Stats_P1_Crash++; Calculate_New_Position("ptb_AICar5", Map_P1_MinX, Map_P1_MaxX); Send_Sound(Player_1, "hit_car"); }
                Stats_P1_Speed = Math.Max(4, Stats_P1_Speed);
            }

            if (Player_2 != null)
            {
                if (Is_Colliding("ptb_player2", "ptb_buff_road_2")) { Stats_P2_Speed += 3; Calculate_New_Position("ptb_buff_road_2", Map_P2_MinX, Map_P2_MaxX); Send_Sound(Player_2, "buff"); }
                if (Is_Colliding("ptb_player2", "ptb_debuff_road_2")) { Stats_P2_Speed -= 3; Calculate_New_Position("ptb_debuff_road_2", Map_P2_MinX, Map_P2_MaxX); Send_Sound(Player_2, "debuff"); }
                if (Is_Colliding("ptb_player2", "ptb_AICar3")) { Stats_P2_Speed -= 4; Stats_P2_Crash++; Calculate_New_Position("ptb_AICar3", Map_P2_MinX, Map_P2_MaxX); Send_Sound(Player_2, "hit_car"); }
                if (Is_Colliding("ptb_player2", "ptb_AICar6")) { Stats_P2_Speed -= 4; Stats_P2_Crash++; Calculate_New_Position("ptb_AICar6", Map_P2_MinX, Map_P2_MaxX); Send_Sound(Player_2, "hit_car"); }
                Stats_P2_Speed = Math.Max(4, Stats_P2_Speed);
            }
        }

        private bool Is_Colliding(string Player, string Obj)
        {
            if (!Dict_Object_Pos.ContainsKey(Player) || !Dict_Object_Pos.ContainsKey(Obj)) return false;
            return new Rectangle(Dict_Object_Pos[Player], Dict_Object_Sizes[Player]).IntersectsWith(new Rectangle(Dict_Object_Pos[Obj], Dict_Object_Sizes[Obj]));
        }

        private Point Calculate_New_Position(string Name, int Min_X, int Max_X)
        {
            Size Current_Size = Dict_Object_Sizes.ContainsKey(Name) ? Dict_Object_Sizes[Name] : new Size(30, 30);
            int Safe_Max_X = Max_X - Current_Size.Width - 70;
            if (Safe_Max_X <= Min_X) Safe_Max_X = Min_X + 1;

            int Max_Retries = 20;
            int Attempt = 0;
            Point New_Pos;
            bool Overlap;

            do
            {
                Overlap = false;
                New_Pos = new Point(Rand.Next(Min_X, Safe_Max_X), Rand.Next(-1000, -150));
                Rectangle New_Rect = new Rectangle(New_Pos, Current_Size);
                New_Rect.Inflate(30, 150);

                foreach (var Key in Dict_Object_Pos.Keys)
                {
                    if (Key == Name || Key.Contains("roadtrack") || Key.Contains("player")) continue;
                    if (Dict_Object_Sizes.ContainsKey(Key))
                    {
                        if (New_Rect.IntersectsWith(new Rectangle(Dict_Object_Pos[Key], Dict_Object_Sizes[Key])))
                        {
                            Overlap = true; break;
                        }
                    }
                }
                Attempt++;
            } while (Overlap && Attempt < Max_Retries);

            if (Overlap) New_Pos.Y -= 300;
            if (Dict_Object_Pos.ContainsKey(Name)) Dict_Object_Pos[Name] = New_Pos;
            return New_Pos;
        }

        private void Broadcast_State_Update()
        {
            lock (Player_Lock)
            {
                foreach (var Kvp in Dict_Object_Pos) Dict_Game_State_Packet[Kvp.Key] = Kvp.Value;
            }
            Broadcast_Message(JsonSerializer.Serialize(Dict_Game_State_Packet));
        }

        private void Broadcast_Message(string Message)
        {
            lock (Player_Lock)
            {
                if (Player_1 != null) Send_Message(Player_1.Stream, Message);
                if (Player_2 != null) Send_Message(Player_2.Stream, Message);
            }
        }

        private void Broadcast_Ready_Status()
        {
            lock (Player_Lock)
            {
                var Status = new
                {
                    action = "update_ready_status",
                    player1_ready = Player_1?.Is_Ready ?? false,
                    player1_name = Player_1?.Username ?? "Waiting...",
                    player2_ready = Player_2?.Is_Ready ?? false,
                    player2_name = Player_2?.Username ?? "Waiting..."
                };
                Broadcast_Message(JsonSerializer.Serialize(Status));
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

        private void Send_Sound(Game_Player Player, string Sound_Name)
        {
            if (Player != null)
            {
                Send_Message(Player.Stream, JsonSerializer.Serialize(new { action = "play_sound", sound = Sound_Name }));
            }
        }

        private void Handle_Database_Save()
        {
            try
            {
                lock (Player_Lock)
                {
                    bool P1_Win = Stats_P1_Score > Stats_P2_Score;
                    bool P2_Win = Stats_P2_Score > Stats_P1_Score;
                    double P1_Total = Stats_P1_Score + (P1_Win ? 500 : 0) - (Stats_P1_Crash * 50);
                    double P2_Total = Stats_P2_Score + (P2_Win ? 500 : 0) - (Stats_P2_Crash * 50);

                    if (Player_1 != null && !string.IsNullOrEmpty(Player_1.Username))
                        SQL_Handle.Handle_Match_Result(1, P1_Win, (int)P1_Total, Stats_P1_Crash);

                    if (Player_2 != null && !string.IsNullOrEmpty(Player_2.Username))
                        SQL_Handle.Handle_Match_Result(2, P2_Win, (int)P2_Total, Stats_P2_Crash);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Save Scores Failed: {ex.Message}");
            }
        }

        private void Init_Object_Config()
        {
            int Road_Width = 500;
            int Safe_Margin = 50;
            int Min_X = Safe_Margin;
            int Max_X = Road_Width - Safe_Margin;

            Dict_Object_Sizes["ptb_player1"] = new Size(72, 117);
            Dict_Object_Pos["ptb_player1"] = new Point(202, 470);
            Dict_Object_Sizes["ptb_player2"] = new Size(72, 117);
            Dict_Object_Pos["ptb_player2"] = new Point(202, 470);

            Dict_Object_Sizes["ptb_road_1"] = new Size(617, 734); Dict_Object_Pos["ptb_road_1"] = new Point(0, -2);
            Dict_Object_Sizes["ptb_road_1_dup"] = new Size(617, 734); Dict_Object_Pos["ptb_road_1_dup"] = new Point(0, 734);
            Dict_Object_Sizes["ptb_road_2"] = new Size(458, 596); Dict_Object_Pos["ptb_road_2"] = new Point(0, 2);
            Dict_Object_Sizes["ptb_road_2_dup"] = new Size(458, 596); Dict_Object_Pos["ptb_road_2_dup"] = new Point(0, 596);

            string[] AI_Cars = { "ptb_AICar1", "ptb_AICar3", "ptb_AICar5", "ptb_AICar6" };
            Size AI_Size = new Size(74, 128);
            foreach (var car in AI_Cars) { Dict_Object_Sizes[car] = AI_Size; Dict_Object_Pos[car] = Calculate_New_Position(car, Min_X, Max_X); }

            string[] Items = { "ptb_buff_road_1", "ptb_debuff_road_1", "ptb_buff_road_2", "ptb_debuff_road_2" };
            Size Item_Size = new Size(30, 30);
            foreach (var item in Items) { Dict_Object_Sizes[item] = Item_Size; Dict_Object_Pos[item] = Calculate_New_Position(item, Min_X, Max_X); }
        }

        private string Format_Username(string Username)
        {
            if (string.IsNullOrEmpty(Username)) return "Unknown";
            int Last_Paren = Username.LastIndexOf('(');
            return (Last_Paren > 0) ? Username.Substring(0, Last_Paren).Trim() : Username.Trim();
        }
    }
}