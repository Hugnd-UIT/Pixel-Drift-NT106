using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pixel_Drift_Server
{
    public class Game_Room
    {
        public string Room_ID
        {
            get;
            private set;
        }

        private Game_Player Player_1;
        private Game_Player Player_2;
        private readonly object Player_Lock = new object();

        private const int Logic_FPS = 60;
        private const int Network_FPS = 40;
        private DateTime Last_Network_Send = DateTime.Now;

        private Timer Countdown_Timer;
        private int Countdown_Value = 5;
        private bool Is_Game_Running = false;

        private Dictionary<string, Size> Object_Sizes = new Dictionary<string, Size>();
        private Dictionary<string, Point> Game_Objects = new Dictionary<string, Point>();
        private Dictionary<string, object> Game_State = new Dictionary<string, object>();

        private bool P1_Left;
        private bool P1_Right;
        private bool P2_Left;
        private bool P2_Right;
        private int P1_Score = 0;
        private int P1_Speed = 11;
        private int P2_Score = 0;
        private int P2_Speed = 11;
        private int Game_Time_Remaining = 60;
        private const int Player_Move_Speed = 10;

        private const int Game_Height = 800;
        private const int P1_Min_X = 6;
        private const int P1_Max_X = 600;
        private const int P2_Min_X = 6;
        private const int P2_Max_X = 600;

        private Random Rand = new Random();

        public Game_Room(string ID)
        {
            this.Room_ID = ID;
            Game_State["action"] = "update_game_state";
        }

        private string Clean_Username(string Username)
        {
            if (string.IsNullOrEmpty(Username))
            {
                return Username;
            }

            int Last_Paren_Index = Username.LastIndexOf('(');
            if (Last_Paren_Index > 0)
            {
                return Username.Substring(0, Last_Paren_Index).Trim();
            }

            return Username.Trim();
        }

        public bool Is_Empty()
        {
            return Player_1 == null && Player_2 == null;
        }

        public int Add_Player(TcpClient Client, string Username)
        {
            lock (Player_Lock)
            {
                string Clean_Name = Clean_Username(Username);

                if (Player_1 == null)
                {
                    Player_1 = new Game_Player
                    {
                        Client = Client,
                        Stream = Client.GetStream(),
                        Username = Clean_Name,
                        Player_ID = 1
                    };
                    Broadcast_Ready_Status();
                    return 1;
                }
                else if (Player_2 == null)
                {
                    Player_2 = new Game_Player
                    {
                        Client = Client,
                        Stream = Client.GetStream(),
                        Username = Clean_Name,
                        Player_ID = 2
                    };
                    Broadcast_Ready_Status();
                    return 2;
                }
                return -1;
            }
        }

        public void Remove_Player(TcpClient Client)
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

                Stop_Game();

                var Disconnect_Msg = new
                {
                    action = "player_disconnected",
                    name = Left_User,
                    target_action = (Remaining_Player != null) ? "opponent_left" : null
                };

                Broadcast(JsonSerializer.Serialize(Disconnect_Msg));
                Broadcast_Ready_Status();
            }
        }

        public void Handle_Input(TcpClient Client, string Action, Dictionary<string, JsonElement> Data)
        {
            int Player_ID = 0;

            if (Player_1 != null && Player_1.Client == Client)
            {
                Player_ID = 1;
            }
            else if (Player_2 != null && Player_2.Client == Client)
            {
                Player_ID = 2;
            }

            if (Player_ID == 0)
            {
                return;
            }

            switch (Action)
            {
                case "set_ready":
                    bool Ready = Data["ready_status"].GetString() == "true";
                    lock (Player_Lock)
                    {
                        if (Player_ID == 1)
                        {
                            Player_1.Is_Ready = Ready;
                        }
                        else
                        {
                            Player_2.Is_Ready = Ready;
                        }
                    }
                    Broadcast_Ready_Status();
                    Check_Start_Countdown();
                    break;

                case "move":
                    string Direction = Data["direction"].GetString();
                    bool Is_Pressed = Data["state"].GetString() == "down";

                    lock (Player_Lock)
                    {
                        if (Player_ID == 1)
                        {
                            if (Direction == "left")
                            {
                                P1_Left = Is_Pressed;
                            }
                            else if (Direction == "right")
                            {
                                P1_Right = Is_Pressed;
                            }
                        }
                        else
                        {
                            if (Direction == "left")
                            {
                                P2_Left = Is_Pressed;
                            }
                            else if (Direction == "right")
                            {
                                P2_Right = Is_Pressed;
                            }
                        }
                    }
                    break;

                case "leave_room":
                    Remove_Player(Client);
                    break;
            }
        }

        private async void Send_Message(NetworkStream Stream, string Message)
        {
            if (Stream == null || !Stream.CanWrite)
            {
                return;
            }

            try
            {
                byte[] Buffer = Encoding.UTF8.GetBytes(Message + "\n");
                await Stream.WriteAsync(Buffer, 0, Buffer.Length);
            }
            catch (Exception ex)
            {
                if (ex is IOException || ex is ObjectDisposedException || ex.Message.Contains("aborted") || ex.Message.Contains("forcibly closed"))
                {
                    return;
                }
                Console.WriteLine($"[Error] {ex.Message}");
            }
        }

        private void Broadcast(string Message)
        {
            NetworkStream S1 = null;
            NetworkStream S2 = null;

            lock (Player_Lock)
            {
                if (Player_1 != null)
                {
                    S1 = Player_1.Stream;
                }
                if (Player_2 != null)
                {
                    S2 = Player_2.Stream;
                }
            }

            if (S1 != null)
            {
                Send_Message(S1, Message);
            }
            if (S2 != null)
            {
                Send_Message(S2, Message);
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
                Broadcast(JsonSerializer.Serialize(Status));
            }
        }

        private void Send_Sound(Game_Player Player, string Sound_Name)
        {
            if (Player != null && Player.Stream != null)
            {
                Send_Message(Player.Stream, JsonSerializer.Serialize(new
                {
                    action = "play_sound",
                    sound = Sound_Name
                }));
            }
        }

        private void Check_Start_Countdown()
        {
            lock (Player_Lock)
            {
                if (Player_1 != null && Player_1.Is_Ready && Player_2 != null && Player_2.Is_Ready && Countdown_Timer == null)
                {
                    Countdown_Value = 5;
                    Countdown_Timer = new System.Threading.Timer(Countdown_Tick, null, 0, 1000);
                }
            }
        }

        private void Countdown_Tick(object State)
        {
            if (Countdown_Value > 0)
            {
                Broadcast(JsonSerializer.Serialize(new
                {
                    action = "countdown",
                    time = Countdown_Value
                }));
                Countdown_Value--;
            }
            else
            {
                Countdown_Timer?.Dispose();
                Countdown_Timer = null;
                Start_Game_Logic();
            }
        }

        private void Start_Game_Logic()
        {
            Initialize_Game();
            Is_Game_Running = true;
            Broadcast(JsonSerializer.Serialize(new
            {
                action = "start_game"
            }));

            Task.Run(Game_Loop_Async);
        }

        private async Task Game_Loop_Async()
        {
            Game_Time_Remaining = 60;
            DateTime Last_Second_Tick = DateTime.Now;
            int Loop_Delay = 1000 / Logic_FPS;

            while (Is_Game_Running && Game_Time_Remaining > 0)
            {
                Update_Physics();

                if ((DateTime.Now - Last_Second_Tick).TotalSeconds >= 1)
                {
                    Game_Time_Remaining--;
                    Last_Second_Tick = DateTime.Now;
                    P1_Score += P1_Speed;
                    P2_Score += P2_Speed;

                    var Time_Data = new
                    {
                        action = "update_time",
                        time = Game_Time_Remaining
                    };
                    var Score_Data = new
                    {
                        action = "update_score",
                        p1_score = P1_Score,
                        p2_score = P2_Score
                    };

                    Broadcast(JsonSerializer.Serialize(Time_Data));
                    Broadcast(JsonSerializer.Serialize(Score_Data));
                }

                if ((DateTime.Now - Last_Network_Send).TotalMilliseconds >= (1000 / Network_FPS))
                {
                    Broadcast_Game_State();
                    Last_Network_Send = DateTime.Now;
                }

                await Task.Delay(Loop_Delay);
            }

            if (Is_Game_Running)
            {
                End_Game();
            }
        }

        private void Stop_Game()
        {
            Is_Game_Running = false;
            Countdown_Timer?.Dispose();
            Countdown_Timer = null;
        }

        private void End_Game()
        {
            Is_Game_Running = false;
            Save_Game_Scores();
            Broadcast(JsonSerializer.Serialize(new
            {
                action = "game_over"
            }));

            lock (Player_Lock)
            {
                if (Player_1 != null)
                {
                    Player_1.Is_Ready = false;
                }
                if (Player_2 != null)
                {
                    Player_2.Is_Ready = false;
                }
            }
            Broadcast_Ready_Status();
        }

        private void Update_Physics()
        {
            lock (Player_Lock)
            {
                if (Player_1 != null)
                {
                    Point P = Game_Objects["ptb_player1"];
                    if (P1_Left && P.X > P1_Min_X)
                    {
                        P.X -= Player_Move_Speed;
                    }
                    if (P1_Right && P.X < P1_Max_X - Object_Sizes["ptb_player1"].Width)
                    {
                        P.X += Player_Move_Speed;
                    }
                    Game_Objects["ptb_player1"] = P;
                }

                if (Player_2 != null)
                {
                    Point P = Game_Objects["ptb_player2"];
                    if (P2_Left && P.X > P2_Min_X)
                    {
                        P.X -= Player_Move_Speed;
                    }
                    if (P2_Right && P.X < P2_Max_X - Object_Sizes["ptb_player2"].Width)
                    {
                        P.X += Player_Move_Speed;
                    }
                    Game_Objects["ptb_player2"] = P;
                }

                Move_Objects_Logic();
                Check_Collisions_Logic();
            }
        }

        private void Move_Objects_Logic()
        {
            Move_Object_Down("ptb_roadtrack1", P1_Speed, Game_Height, true);
            Move_Object_Down("ptb_roadtrack1dup", P1_Speed, Game_Height, true);
            Move_Object_Down("ptb_AICar1", P1_Speed, Game_Height, false, P1_Min_X, P1_Max_X);
            Move_Object_Down("ptb_AICar5", P1_Speed, Game_Height, false, P1_Min_X, P1_Max_X);
            Move_Object_Down("ptb_increasingroad1", P1_Speed, Game_Height, false, P1_Min_X, P1_Max_X);
            Move_Object_Down("ptb_decreasingroad1", P1_Speed, Game_Height, false, P1_Min_X, P1_Max_X);

            Move_Object_Down("ptb_roadtrack2", P2_Speed, Game_Height, true);
            Move_Object_Down("ptb_roadtrack2dup", P2_Speed, Game_Height, true);
            Move_Object_Down("ptb_AICar3", P2_Speed, Game_Height, false, P2_Min_X, P2_Max_X);
            Move_Object_Down("ptb_AICar6", P2_Speed, Game_Height, false, P2_Min_X, P2_Max_X);
            Move_Object_Down("ptb_increasingroad2", P2_Speed, Game_Height, false, P2_Min_X, P2_Max_X);
            Move_Object_Down("ptb_decreasingroad2", P2_Speed, Game_Height, false, P2_Min_X, P2_Max_X);
        }

        private void Check_Collisions_Logic()
        {
            if (Player_1 != null)
            {
                if (Check_Collision("ptb_player1", "ptb_increasingroad1"))
                {
                    P1_Speed += 3;
                    Reposition_Object("ptb_increasingroad1", P1_Min_X, P1_Max_X);
                    Send_Sound(Player_1, "buff");
                }
                if (Check_Collision("ptb_player1", "ptb_decreasingroad1"))
                {
                    P1_Speed -= 3;
                    Reposition_Object("ptb_decreasingroad1", P1_Min_X, P1_Max_X);
                    Send_Sound(Player_1, "debuff");
                }
                if (Check_Collision("ptb_player1", "ptb_AICar1"))
                {
                    P1_Speed -= 4;
                    Reposition_Object("ptb_AICar1", P1_Min_X, P1_Max_X);
                    Send_Sound(Player_1, "hit_car");
                }
                if (Check_Collision("ptb_player1", "ptb_AICar5"))
                {
                    P1_Speed -= 4;
                    Reposition_Object("ptb_AICar5", P1_Min_X, P1_Max_X);
                    Send_Sound(Player_1, "hit_car");
                }
                P1_Speed = Math.Max(4, P1_Speed);
            }

            if (Player_2 != null)
            {
                if (Check_Collision("ptb_player2", "ptb_increasingroad2"))
                {
                    P2_Speed += 3;
                    Reposition_Object("ptb_increasingroad2", P2_Min_X, P2_Max_X);
                    Send_Sound(Player_2, "buff");
                }
                if (Check_Collision("ptb_player2", "ptb_decreasingroad2"))
                {
                    P2_Speed -= 3;
                    Reposition_Object("ptb_decreasingroad2", P2_Min_X, P2_Max_X);
                    Send_Sound(Player_2, "debuff");
                }
                if (Check_Collision("ptb_player2", "ptb_AICar3"))
                {
                    P2_Speed -= 4;
                    Reposition_Object("ptb_AICar3", P2_Min_X, P2_Max_X);
                    Send_Sound(Player_2, "hit_car");
                }
                if (Check_Collision("ptb_player2", "ptb_AICar6"))
                {
                    P2_Speed -= 4;
                    Reposition_Object("ptb_AICar6", P2_Min_X, P2_Max_X);
                    Send_Sound(Player_2, "hit_car");
                }
                P2_Speed = Math.Max(4, P2_Speed);
            }
        }

        private void Broadcast_Game_State()
        {
            lock (Player_Lock)
            {
                foreach (var Kvp in Game_Objects)
                {
                    Game_State[Kvp.Key] = Kvp.Value;
                }
            }
            Broadcast(JsonSerializer.Serialize(Game_State));
        }

        private void Initialize_Game()
        {
            lock (Player_Lock)
            {
                P1_Score = 0;
                P2_Score = 0;
                P1_Speed = 12;
                P2_Speed = 12;
                P1_Left = false;
                P1_Right = false;
                P2_Left = false;
                P2_Right = false;
                Game_Objects.Clear();
                Object_Sizes.Clear();
                Setup_Objects_Config();
            }
        }

        private void Setup_Objects_Config()
        {
            int Road_Width = 500;
            int Safe_Margin = 50;
            int Min_X = Safe_Margin;
            int Max_X = Road_Width - Safe_Margin;

            Object_Sizes["ptb_player1"] = new Size(72, 117);
            Game_Objects["ptb_player1"] = new Point(202, 470);
            Object_Sizes["ptb_player2"] = new Size(72, 117);
            Game_Objects["ptb_player2"] = new Point(202, 470);

            Object_Sizes["ptb_roadtrack1"] = new Size(617, 734);
            Game_Objects["ptb_roadtrack1"] = new Point(0, -2);
            Object_Sizes["ptb_roadtrack1dup"] = new Size(617, 734);
            Game_Objects["ptb_roadtrack1dup"] = new Point(0, 734);
            Object_Sizes["ptb_roadtrack2"] = new Size(458, 596);
            Game_Objects["ptb_roadtrack2"] = new Point(0, 2);
            Object_Sizes["ptb_roadtrack2dup"] = new Size(458, 596);
            Game_Objects["ptb_roadtrack2dup"] = new Point(0, 596);

            Object_Sizes["ptb_AICar1"] = new Size(50, 100);
            Game_Objects["ptb_AICar1"] = Reposition_Object("ptb_AICar1", Min_X, Max_X);
            Object_Sizes["ptb_AICar3"] = new Size(74, 128);
            Game_Objects["ptb_AICar3"] = Reposition_Object("ptb_AICar3", Min_X, Max_X);
            Object_Sizes["ptb_AICar5"] = new Size(50, 100);
            Game_Objects["ptb_AICar5"] = Reposition_Object("ptb_AICar5", Min_X, Max_X);
            Object_Sizes["ptb_AICar6"] = new Size(74, 128);
            Game_Objects["ptb_AICar6"] = Reposition_Object("ptb_AICar6", Min_X, Max_X);

            Object_Sizes["ptb_increasingroad1"] = new Size(30, 30);
            Game_Objects["ptb_increasingroad1"] = Reposition_Object("ptb_increasingroad1", Min_X, Max_X);
            Object_Sizes["ptb_decreasingroad1"] = new Size(30, 30);
            Game_Objects["ptb_decreasingroad1"] = Reposition_Object("ptb_decreasingroad1", Min_X, Max_X);
            Object_Sizes["ptb_increasingroad2"] = new Size(30, 30);
            Game_Objects["ptb_increasingroad2"] = Reposition_Object("ptb_increasingroad2", Min_X, Max_X);
            Object_Sizes["ptb_decreasingroad2"] = new Size(30, 30);
            Game_Objects["ptb_decreasingroad2"] = Reposition_Object("ptb_decreasingroad2", Min_X, Max_X);
        }

        private void Move_Object_Down(string Name, int Speed, int Screen_Height, bool Is_Road, int Min_X = 0, int Max_X = 0)
        {
            if (!Game_Objects.ContainsKey(Name))
            {
                return;
            }

            Point Pos = Game_Objects[Name];
            Pos.Y += Speed;

            if (Pos.Y > Screen_Height)
            {
                if (Is_Road)
                {
                    string Dup_Name = (Name == "ptb_roadtrack1") ? "ptb_roadtrack1dup" :
                                      (Name == "ptb_roadtrack1dup") ? "ptb_roadtrack1" :
                                      (Name == "ptb_roadtrack2") ? "ptb_roadtrack2dup" : "ptb_roadtrack2";
                    Pos.Y = Game_Objects[Dup_Name].Y - Screen_Height;
                }
                else
                {
                    Pos = Reposition_Object(Name, Min_X, Max_X);
                }
            }
            Game_Objects[Name] = Pos;
        }

        private Point Reposition_Object(string Name, int Min_X, int Max_X)
        {
            Size Current_Size = Object_Sizes.ContainsKey(Name) ? Object_Sizes[Name] : new Size(30, 30);
            int Safe_Max_X = Max_X - Current_Size.Width - 70;

            if (Safe_Max_X <= Min_X)
            {
                Safe_Max_X = Min_X + 1;
            }

            int Max_Retries = 20;
            int Attempt = 0;
            Point New_Pos = new Point(0, 0);
            bool Overlap;

            do
            {
                Overlap = false;
                int Random_X = Rand.Next(Min_X, Safe_Max_X);
                int Random_Y = Rand.Next(-1000, -150);
                New_Pos = new Point(Random_X, Random_Y);
                Rectangle New_Rect = new Rectangle(New_Pos, Current_Size);
                New_Rect.Inflate(30, 150);

                foreach (var Key in Game_Objects.Keys)
                {
                    if (Key == Name || Key.Contains("roadtrack") || Key.Contains("player"))
                    {
                        continue;
                    }
                    if (Object_Sizes.ContainsKey(Key))
                    {
                        Rectangle Existing_Rect = new Rectangle(Game_Objects[Key], Object_Sizes[Key]);
                        if (New_Rect.IntersectsWith(Existing_Rect))
                        {
                            Overlap = true;
                            break;
                        }
                    }
                }
                Attempt++;
            } while (Overlap && Attempt < Max_Retries);

            if (Overlap)
            {
                New_Pos.Y -= 300;
            }

            if (Game_Objects.ContainsKey(Name))
            {
                Game_Objects[Name] = New_Pos;
            }

            return New_Pos;
        }

        private bool Check_Collision(string Player, string Obj)
        {
            if (!Game_Objects.ContainsKey(Player) || !Game_Objects.ContainsKey(Obj))
            {
                return false;
            }

            Rectangle Rect_Player = new Rectangle(Game_Objects[Player], Object_Sizes[Player]);
            Rectangle Rect_Obj = new Rectangle(Game_Objects[Obj], Object_Sizes[Obj]);
            return Rect_Player.IntersectsWith(Rect_Obj);
        }

        private void Save_Game_Scores()
        {
            try
            {
                lock (Player_Lock)
                {
                    bool P1_Win_Count = P1_Score > P2_Score;
                    bool P2_Win_Count = P2_Score > P1_Score;
                    int P1_Crash_Count = Math.Max(0, (100 - P1_Speed) / 10);
                    int P2_Crash_Count = Math.Max(0, (100 - P2_Speed) / 10);

                    if (Player_1 != null && !string.IsNullOrEmpty(Player_1.Username))
                    {
                        SQL_Handle.Handle_Match_Result(1, P1_Win_Count, P1_Score, P1_Crash_Count);
                    }

                    if (Player_2 != null && !string.IsNullOrEmpty(Player_2.Username))
                    {
                        SQL_Handle.Handle_Match_Result(2, P2_Win_Count, P2_Score, P2_Crash_Count);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }
        }
    }
}