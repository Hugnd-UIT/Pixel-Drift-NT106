using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;
using WMPLib;

namespace Pixel_Drift
{
    public partial class Game_Window : Form
    {
        // --- Game Configuration & State (Cấu hình và Trạng thái game) ---
        private int My_Player_ID = 0;
        private string My_Username;
        private bool Is_Returning_To_Lobby = false;
        private bool Is_Left_Pressed = false;
        private bool Is_Right_Pressed = false;

        private long Score_Player_1 = 0;
        private long Score_Player_2 = 0;
        private int Crash_Count = 0;

        // --- Collections (Danh sách lưu trữ) ---
        private List<string> Keys_Buffer = new List<string>();
        private Dictionary<string, Point> Object_Positions = new Dictionary<string, Point>();

        // --- Assets (Hình ảnh) ---
        private Image Img_Player_1 = Properties.Resources.Player_Car_1;
        private Image Img_Player_2 = Properties.Resources.Player_Car_2;
        private Image Img_Road = Properties.Resources.Road;
        private Image Img_Buff = Properties.Resources.Buff;
        private Image Img_Debuff = Properties.Resources.Debuff;
        private Image Img_Black_Car = Properties.Resources.Black_Car;
        private Image Img_Blue_Car = Properties.Resources.Blue_Car;
        private Image Img_Orange_Car = Properties.Resources.Orange_Car;
        private Image Img_Red_Car = Properties.Resources.Green_Car;

        // --- Dimensions (Kích thước) ---
        private Size Size_Player = new Size(80, 175);
        private Size Size_Road = new Size(610, 910);
        private Size Size_Item = new Size(60, 60);
        private Size Size_AI_Car = new Size(80, 175);

        // --- Audio & Timers (Âm thanh và Bộ đếm) ---
        private WindowsMediaPlayer Music_Player;
        private SoundPlayer SFX_Countdown;
        private SoundPlayer SFX_Buff;
        private SoundPlayer SFX_Debuff;
        private SoundPlayer SFX_Crash;
        private System.Windows.Forms.Timer Game_Loop_Timer;

        public Game_Window()
        {
            InitializeComponent();
        }

        public Game_Window(string User_Name, int Player_Num, string Room_ID)
        {
            InitializeComponent();

            // Tối ưu hóa đồ họa (Graphics Optimization)
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
            this.KeyPreview = true;

            this.My_Username = User_Name;
            this.My_Player_ID = Player_Num;

            if (btn_ID != null) btn_ID.Text = "ID: " + Room_ID;

            string Color_Name = (My_Player_ID == 1) ? "Red Car" : "Blue Car";
            this.Text = $"Pixel Drift - PLAYER {My_Player_ID} ({Color_Name}) - {My_Username}";

            Network_Handle.Incoming_Request += On_Server_Message_Received;
        }

        // --- Form Events (KHÔNG ĐỔI TÊN HÀM CONTROL) ---

        private void Game_Window_Load(object sender, EventArgs e)
        {
            try
            {
                Clear_Old_Controls();
                Enable_Double_Buffering(pn_road_left);
                Enable_Double_Buffering(pn_road_right);

                pn_road_left.BackColor = Color.Black;
                pn_road_right.BackColor = Color.Black;

                Game_Loop_Timer = new System.Windows.Forms.Timer();
                Game_Loop_Timer.Interval = 16;
                Game_Loop_Timer.Tick += Game_Loop_Timer_Tick;
                Game_Loop_Timer.Start();

                Initialize_Audio();
                Reset_To_Lobby_State();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Game Initialization Error: " + Ex.Message);
                this.Close();
            }
        }

        private void Game_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Network_Handle.Incoming_Request -= On_Server_Message_Received;
            try
            {
                Game_Loop_Timer?.Stop();
                Game_Loop_Timer?.Dispose();
                Music_Player?.controls.stop();
                Music_Player?.close();
                SFX_Countdown?.Stop();
                SFX_Crash?.Stop();
                SFX_Buff?.Stop();
                SFX_Debuff?.Stop();
            }
            catch { }

            if (!Is_Returning_To_Lobby) Send_Network_Request(new { action = "leave_room" });
        }

        private void Game_Window_KeyDown(object sender, KeyEventArgs e)
        {
            string Direction = null;
            if (e.KeyCode == Keys.Left)
            {
                if (Is_Left_Pressed) return;
                Is_Left_Pressed = true;
                Direction = "left";
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (Is_Right_Pressed) return;
                Is_Right_Pressed = true;
                Direction = "right";
            }

            if (Direction != null)
                Send_Network_Request(new { action = "move", player = My_Player_ID, direction = Direction, state = "down" });
        }

        private void Game_Window_KeyUp(object sender, KeyEventArgs e)
        {
            string Direction = null;
            if (e.KeyCode == Keys.Left)
            {
                Is_Left_Pressed = false;
                Direction = "left";
            }
            else if (e.KeyCode == Keys.Right)
            {
                Is_Right_Pressed = false;
                Direction = "right";
            }

            if (Direction != null)
                Send_Network_Request(new { action = "move", player = My_Player_ID, direction = Direction, state = "up" });
        }

        private void btn_Ready_Click(object sender, EventArgs e)
        {
            Send_Network_Request(new { action = "set_ready", ready_status = "true" });
            btn_Ready.Enabled = false;
            btn_Ready.Text = "Waiting...";
            this.Focus();
        }

        private void btn_Scoreboard_Click(object sender, EventArgs e)
        {
            var Board = Application.OpenForms.OfType<Form_ScoreBoard>().FirstOrDefault();
            if (Board != null) Board.Show();
            else new Form_ScoreBoard(Network_Handle.Get_Client()).Show();
        }

        private void Game_Loop_Timer_Tick(object sender, EventArgs e)
        {
            if (pn_road_left != null) pn_road_left.Invalidate();
            if (pn_road_right != null) pn_road_right.Invalidate();
        }

        private void Road_Left_Paint(object sender, PaintEventArgs e)
        {
            Setup_Graphics_Quality(e.Graphics);

            Draw_Game_Entity(e.Graphics, "ptb_road_1", Img_Road, pn_road_left.Size);
            Draw_Game_Entity(e.Graphics, "ptb_road_1_dup", Img_Road, pn_road_left.Size);
            Draw_Game_Entity(e.Graphics, "ptb_buff_road_1", Img_Buff, Size_Item);
            Draw_Game_Entity(e.Graphics, "ptb_debuff_road_1", Img_Debuff, Size_Item);
            Draw_Game_Entity(e.Graphics, "ptb_AICar1", Img_Black_Car, Size_AI_Car);
            Draw_Game_Entity(e.Graphics, "ptb_AICar5", Img_Blue_Car, Size_AI_Car);
            Draw_Game_Entity(e.Graphics, "ptb_player1", Img_Player_1, Size_Player);
        }

        private void Road_Right_Paint(object sender, PaintEventArgs e)
        {
            Setup_Graphics_Quality(e.Graphics);

            Draw_Game_Entity(e.Graphics, "ptb_road_2", Img_Road, pn_road_right.Size);
            Draw_Game_Entity(e.Graphics, "ptb_road_2_dup", Img_Road, pn_road_right.Size);
            Draw_Game_Entity(e.Graphics, "ptb_buff_road_2", Img_Buff, Size_Item);
            Draw_Game_Entity(e.Graphics, "ptb_debuff_road_2", Img_Debuff, Size_Item);
            Draw_Game_Entity(e.Graphics, "ptb_AICar3", Img_Orange_Car, Size_AI_Car);
            Draw_Game_Entity(e.Graphics, "ptb_AICar6", Img_Red_Car, Size_AI_Car);
            Draw_Game_Entity(e.Graphics, "ptb_player2", Img_Player_2, Size_Player);
        }

        // --- Network Logic (Xử lý mạng) ---

        private void Send_Network_Request(object Msg)
        {
            Network_Handle.Send_And_Forget(Msg);
        }

        private void On_Server_Message_Received(string Raw_Message)
        {
            if (this.Disposing || this.IsDisposed || !this.IsHandleCreated) return;

            try
            {
                var Data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Raw_Message);
                if (!Data.ContainsKey("action")) return;
                string Action = Data["action"].GetString();

                this.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        Process_Server_Action(Action, Data);
                    }
                    catch { }
                }));
            }
            catch { }
        }

        private void Process_Server_Action(string Action, Dictionary<string, JsonElement> Data)
        {
            switch (Action)
            {
                case "update_game_state":
                    Update_Object_Positions(Data);
                    break;

                case "update_score":
                    Score_Player_1 = Data["p1_score"].GetInt64();
                    Score_Player_2 = Data["p2_score"].GetInt64();
                    if (lbl_Score1 != null) lbl_Score1.Text = "Score: " + Score_Player_1;
                    if (lbl_Score2 != null) lbl_Score2.Text = "Score: " + Score_Player_2;
                    break;

                case "update_time":
                    if (lbl_GameTimer != null) lbl_GameTimer.Text = "Time: " + Data["time"].GetInt32();
                    break;

                case "start_game":
                    Start_Game_Session();
                    break;

                case "countdown":
                    if (lbl_Countdown != null)
                    {
                        lbl_Countdown.Visible = true;
                        int Time = Data["time"].GetInt32();
                        lbl_Countdown.Text = Time.ToString();
                        if (Time == 5) { Music_Player.controls.stop(); SFX_Countdown?.Play(); }
                    }
                    break;

                case "update_ready_status":
                    string P1_Name = Data["player1_name"].GetString();
                    string P2_Name = Data["player2_name"].GetString();
                    bool P1_Ready = Data["player1_ready"].GetBoolean();
                    bool P2_Ready = Data["player2_ready"].GetBoolean();
                    if (lbl_P1_Status != null) lbl_P1_Status.Text = $"P1 ({P1_Name}): {(P1_Ready ? "Ready" : "...")}";
                    if (lbl_P2_Status != null) lbl_P2_Status.Text = $"P2 ({P2_Name}): {(P2_Ready ? "Ready" : "...")}";
                    break;

                case "game_over":
                    Music_Player?.controls.stop();
                    MessageBox.Show("Time Is Up! Game Over.", "Notification");
                    End_Game_Session();
                    Reset_To_Lobby_State();
                    break;

                case "player_disconnected":
                    string Name = Data.ContainsKey("name") ? Data["name"].GetString() : "Opponent";
                    Music_Player?.controls.stop();
                    SFX_Countdown?.Stop();
                    MessageBox.Show($"{Name} Has Disconnected. You Will Return To Lobby.", "Notification");
                    Is_Returning_To_Lobby = true;
                    Send_Network_Request(new { action = "leave_room" });
                    this.Close();
                    break;

                case "play_sound":
                    Play_Sound_Effect(Data["sound"].GetString());
                    break;

                case "force_logout":
                    Music_Player?.controls.stop();
                    MessageBox.Show("Account Logged In From Another Location!", "Warning");
                    Application.Exit();
                    break;
            }
        }

        private void Update_Object_Positions(Dictionary<string, JsonElement> Data)
        {
            foreach (var Key in Data.Keys)
            {
                if (Key == "action") continue;
                try
                {
                    JsonElement El = Data[Key];
                    if (El.ValueKind == JsonValueKind.Object && El.TryGetProperty("X", out var X_Val) && El.TryGetProperty("Y", out var Y_Val))
                    {
                        int X = X_Val.GetInt32();
                        int Y = Y_Val.GetInt32();

                        if (Object_Positions.ContainsKey(Key))
                            Object_Positions[Key] = new Point(X, Y);
                        else
                            Object_Positions.Add(Key, new Point(X, Y));
                    }
                }
                catch { }
            }
        }

        // --- Game State Logic (Logic Trạng thái game) ---

        private void Start_Game_Session()
        {
            Crash_Count = 0;
            Score_Player_1 = 0;
            Score_Player_2 = 0;

            // UI Updates
            btn_Ready.Visible = false;
            lbl_P1_Status.Visible = false;
            lbl_P2_Status.Visible = false;
            lbl_Countdown.Visible = false;
            btn_Scoreboard.Enabled = false;
            btn_Scoreboard.Visible = false;
            btn_ID.Visible = false;
            lbl_GameTimer.Visible = true;
            lbl_GameTimer.Text = "Time: 60";

            if (lbl_Score1 != null) { lbl_Score1.Visible = true; lbl_Score1.Text = "Score: 0"; }
            if (lbl_Score2 != null) { lbl_Score2.Visible = true; lbl_Score2.Text = "Score: 0"; }

            SFX_Countdown?.Stop();
            Play_Background_Music("compete.wav");
            this.Focus();
        }

        private void End_Game_Session()
        {
            try
            {
                Crash_Count = 0;
                Score_Player_1 = 0;
                Score_Player_2 = 0;
                Music_Player?.controls.stop();
                SFX_Crash?.Stop();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error Resetting Game: " + Ex.Message);
            }
        }

        private void Reset_To_Lobby_State()
        {
            SFX_Countdown?.Stop();
            Play_Background_Music("wait.wav");

            Object_Positions.Clear();
            if (pn_road_left != null) pn_road_left.Invalidate();
            if (pn_road_right != null) pn_road_right.Invalidate();

            // Restore UI
            btn_Ready.Visible = true;
            btn_Ready.Enabled = true;
            btn_Ready.Text = "Ready";
            lbl_P1_Status.Visible = true;
            lbl_P2_Status.Visible = true;
            btn_Scoreboard.Enabled = true;
            btn_Scoreboard.Visible = true;
            btn_ID.Visible = true;
            lbl_Countdown.Visible = false;
            lbl_GameTimer.Visible = false;
            if (lbl_Score1 != null) lbl_Score1.Visible = false;
            if (lbl_Score2 != null) lbl_Score2.Visible = false;

            btn_Ready.Focus();
        }

        // --- Helper Methods (Các hàm phụ trợ) ---

        private void Initialize_Audio()
        {
            Music_Player = new WindowsMediaPlayer();
            Music_Player.settings.setMode("loop", true);
            Music_Player.settings.volume = 30;
            try
            {
                SFX_Countdown = new SoundPlayer("countdown.wav");
                SFX_Buff = new SoundPlayer("buff.wav");
                SFX_Debuff = new SoundPlayer("debuff.wav");
                SFX_Crash = new SoundPlayer("crash.wav");

                SFX_Countdown.LoadAsync();
                SFX_Buff.LoadAsync();
                SFX_Debuff.LoadAsync();
                SFX_Crash.LoadAsync();
            }
            catch { }
        }

        private void Play_Background_Music(string File_Name)
        {
            try
            {
                string Path_File = System.IO.Path.Combine(Application.StartupPath, File_Name);
                if (System.IO.File.Exists(Path_File))
                {
                    Music_Player.URL = Path_File;
                    Music_Player.controls.play();
                }
            }
            catch { }
        }

        private void Play_Sound_Effect(string Type)
        {
            if (Type == "buff") SFX_Buff?.Play();
            else if (Type == "debuff") SFX_Debuff?.Play();
            else if (Type == "hit_car") { SFX_Crash?.Play(); Crash_Count++; }
        }

        private void Draw_Game_Entity(Graphics G, string Key, Image Img, Size Size_Obj)
        {
            if (Object_Positions.ContainsKey(Key) && Img != null)
            {
                Point P = Object_Positions[Key];
                G.DrawImage(Img, P.X, P.Y, Size_Obj.Width, Size_Obj.Height);
            }
        }

        private void Setup_Graphics_Quality(Graphics G)
        {
            G.CompositingMode = CompositingMode.SourceOver;
            G.CompositingQuality = CompositingQuality.HighSpeed;
            G.InterpolationMode = InterpolationMode.NearestNeighbor;
            G.SmoothingMode = SmoothingMode.None;
            G.PixelOffsetMode = PixelOffsetMode.HighSpeed;
        }

        private void Enable_Double_Buffering(Panel P)
        {
            if (P == null) return;
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, P, new object[] { true });
        }

        private void Clear_Old_Controls()
        {
            void Scan_And_Remove(Control.ControlCollection Cols)
            {
                var Items_To_Remove = new List<Control>();
                foreach (Control C in Cols)
                {
                    if (C is PictureBox && C.Name.StartsWith("ptb_")) Items_To_Remove.Add(C);
                    if (C.HasChildren) Scan_And_Remove(C.Controls);
                }
                foreach (var Item in Items_To_Remove) Cols.Remove(Item);
            }
            Scan_And_Remove(this.Controls);
        }
    }
}