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
        private int My_Player_ID = 0;
        private string My_Username;
        private bool Is_Returning_To_Lobby = false;
        private bool Is_Left_Pressed = false;
        private bool Is_Right_Pressed = false;

        private long Score_Player_1 = 0;
        private long Score_Player_2 = 0;
        private int Crash_Count = 0;

        private List<string> Keys_Buffer = new List<string>();
        private Dictionary<string, Point> Object_Positions = new Dictionary<string, Point>();

        private Image Img_Player_1 = Properties.Resources.Player_Car_1;
        private Image Img_Player_2 = Properties.Resources.Player_Car_2;
        private Image Img_Road = Properties.Resources.Road;
        private Image Img_Buff = Properties.Resources.Buff;
        private Image Img_Debuff = Properties.Resources.Debuff;
        private Image Img_Black_Car = Properties.Resources.Black_Car;
        private Image Img_Blue_Car = Properties.Resources.Blue_Car;
        private Image Img_Orange_Car = Properties.Resources.Orange_Car;
        private Image Img_Red_Car = Properties.Resources.Green_Car;

        private Size Size_Player = new Size(80, 175);
        private Size Size_Road = new Size(610, 910);
        private Size Size_Item = new Size(60, 60);
        private Size Size_AI_Car = new Size(80, 175);

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

        public Game_Window(string Input_Username, int Input_Player_ID, string Input_Room_ID)
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
            this.KeyPreview = true;

            this.My_Username = Input_Username;
            this.My_Player_ID = Input_Player_ID;

            if (btn_ID != null) btn_ID.Text = "ID: " + Input_Room_ID;

            string Car_Color_Name = (My_Player_ID == 1) ? "Red Car" : "Blue Car";
            this.Text = $"Pixel Drift - PLAYER {My_Player_ID} - {My_Username}";

            Network_Handle.Incoming_Request += On_Server_Message_Received;
        }

        private void Game_Window_Load(object Sender, EventArgs Event_Args)
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

        private void Game_Window_FormClosing(object Sender, FormClosingEventArgs Event_Args)
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

        private void Game_Window_KeyDown(object Sender, KeyEventArgs Event_Args)
        {
            string Move_Direction = null;
            if (Event_Args.KeyCode == Keys.Left)
            {
                if (Is_Left_Pressed) return;
                Is_Left_Pressed = true;
                Move_Direction = "left";
            }
            else if (Event_Args.KeyCode == Keys.Right)
            {
                if (Is_Right_Pressed) return;
                Is_Right_Pressed = true;
                Move_Direction = "right";
            }

            if (Move_Direction != null)
                Send_Network_Request(new { action = "move", player = My_Player_ID, direction = Move_Direction, state = "down" });
        }

        private void Game_Window_KeyUp(object Sender, KeyEventArgs Event_Args)
        {
            string Move_Direction = null;
            if (Event_Args.KeyCode == Keys.Left)
            {
                Is_Left_Pressed = false;
                Move_Direction = "left";
            }
            else if (Event_Args.KeyCode == Keys.Right)
            {
                Is_Right_Pressed = false;
                Move_Direction = "right";
            }

            if (Move_Direction != null)
                Send_Network_Request(new { action = "move", player = My_Player_ID, direction = Move_Direction, state = "up" });
        }

        private void btn_Ready_Click(object Sender, EventArgs Event_Args)
        {
            Send_Network_Request(new { action = "set_ready", ready_status = "true" });
            btn_Ready.Enabled = false;
            btn_Ready.Text = "Waiting...";
            this.Focus();
        }

        private void btn_Scoreboard_Click(object Sender, EventArgs Event_Args)
        {
            var Board_Form = Application.OpenForms.OfType<Form_ScoreBoard>().FirstOrDefault();
            if (Board_Form != null) Board_Form.Show();
            else new Form_ScoreBoard(Network_Handle.Get_Client()).Show();
        }

        private void Game_Loop_Timer_Tick(object Sender, EventArgs Event_Args)
        {
            if (pn_road_left != null) pn_road_left.Invalidate();
            if (pn_road_right != null) pn_road_right.Invalidate();
        }

        private void Road_Left_Paint(object Sender, PaintEventArgs Event_Args)
        {
            Setup_Graphics_Quality(Event_Args.Graphics);

            Draw_Game_Entity(Event_Args.Graphics, "ptb_road_1", Img_Road, pn_road_left.Size);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_road_1_dup", Img_Road, pn_road_left.Size);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_buff_road_1", Img_Buff, Size_Item);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_debuff_road_1", Img_Debuff, Size_Item);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_AICar1", Img_Black_Car, Size_AI_Car);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_AICar5", Img_Blue_Car, Size_AI_Car);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_player1", Img_Player_1, Size_Player);
        }

        private void Road_Right_Paint(object Sender, PaintEventArgs Event_Args)
        {
            Setup_Graphics_Quality(Event_Args.Graphics);

            Draw_Game_Entity(Event_Args.Graphics, "ptb_road_2", Img_Road, pn_road_right.Size);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_road_2_dup", Img_Road, pn_road_right.Size);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_buff_road_2", Img_Buff, Size_Item);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_debuff_road_2", Img_Debuff, Size_Item);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_AICar3", Img_Orange_Car, Size_AI_Car);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_AICar6", Img_Red_Car, Size_AI_Car);
            Draw_Game_Entity(Event_Args.Graphics, "ptb_player2", Img_Player_2, Size_Player);
        }

        private void Send_Network_Request(object Message_Content)
        {
            Network_Handle.Send_And_Forget(Message_Content);
        }

        private void On_Server_Message_Received(string Raw_Message)
        {
            if (this.Disposing || this.IsDisposed || !this.IsHandleCreated) return;

            try
            {
                var Deserialized_Data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Raw_Message);
                if (!Deserialized_Data.ContainsKey("action")) return;
                string Action_Type = Deserialized_Data["action"].GetString();

                this.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        Process_Server_Action(Action_Type, Deserialized_Data);
                    }
                    catch { }
                }));
            }
            catch { }
        }

        private void Process_Server_Action(string Action_Type, Dictionary<string, JsonElement> Action_Data)
        {
            switch (Action_Type)
            {
                case "update_game_state":
                    Update_Object_Positions(Action_Data);
                    break;

                case "update_score":
                    Score_Player_1 = Action_Data["p1_score"].GetInt64();
                    Score_Player_2 = Action_Data["p2_score"].GetInt64();
                    if (lbl_Score1 != null) lbl_Score1.Text = "Score: " + Score_Player_1;
                    if (lbl_Score2 != null) lbl_Score2.Text = "Score: " + Score_Player_2;
                    break;

                case "update_time":
                    if (lbl_GameTimer != null) lbl_GameTimer.Text = "Time: " + Action_Data["time"].GetInt32();
                    break;

                case "start_game":
                    Start_Game_Session();
                    break;

                case "countdown":
                    if (lbl_Countdown != null)
                    {
                        lbl_Countdown.Visible = true;
                        int Time_Value = Action_Data["time"].GetInt32();
                        lbl_Countdown.Text = Time_Value.ToString();
                        if (Time_Value == 5) { Music_Player.controls.stop(); SFX_Countdown?.Play(); }
                    }
                    break;

                case "update_ready_status":
                    string P1_Name = Action_Data["player1_name"].GetString();
                    string P2_Name = Action_Data["player2_name"].GetString();
                    bool P1_Is_Ready = Action_Data["player1_ready"].GetBoolean();
                    bool P2_Is_Ready = Action_Data["player2_ready"].GetBoolean();
                    if (lbl_P1_Status != null) lbl_P1_Status.Text = $"P1 ({P1_Name}): {(P1_Is_Ready ? "Ready" : "...")}";
                    if (lbl_P2_Status != null) lbl_P2_Status.Text = $"P2 ({P2_Name}): {(P2_Is_Ready ? "Ready" : "...")}";
                    break;

                case "game_over":
                    Music_Player?.controls.stop();
                    MessageBox.Show("Time Is Up! Game Over.", "Notification");
                    End_Game_Session();
                    Reset_To_Lobby_State();
                    break;

                case "player_disconnected":
                    string Opponent_Name = Action_Data.ContainsKey("name") ? Action_Data["name"].GetString() : "Opponent";
                    Music_Player?.controls.stop();
                    SFX_Countdown?.Stop();
                    MessageBox.Show($"{Opponent_Name} Has Disconnected. You Will Return To Lobby.", "Notification");
                    Is_Returning_To_Lobby = true;
                    Send_Network_Request(new { action = "leave_room" });
                    this.Close();
                    break;

                case "play_sound":
                    Play_Sound_Effect(Action_Data["sound"].GetString());
                    break;

                case "force_logout":
                    Music_Player?.controls.stop();
                    MessageBox.Show("Account Logged In From Another Location!", "Warning");
                    Application.Exit();
                    break;
            }
        }

        private void Update_Object_Positions(Dictionary<string, JsonElement> Position_Data)
        {
            foreach (var Key_Name in Position_Data.Keys)
            {
                if (Key_Name == "action") continue;
                try
                {
                    JsonElement Json_Item = Position_Data[Key_Name];
                    if (Json_Item.ValueKind == JsonValueKind.Object && Json_Item.TryGetProperty("X", out var Value_X) && Json_Item.TryGetProperty("Y", out var Value_Y))
                    {
                        int X_Pos = Value_X.GetInt32();
                        int Y_Pos = Value_Y.GetInt32();

                        if (Object_Positions.ContainsKey(Key_Name))
                            Object_Positions[Key_Name] = new Point(X_Pos, Y_Pos);
                        else
                            Object_Positions.Add(Key_Name, new Point(X_Pos, Y_Pos));
                    }
                }
                catch { }
            }
        }

        private void Start_Game_Session()
        {
            Crash_Count = 0;
            Score_Player_1 = 0;
            Score_Player_2 = 0;

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
                string File_Path = System.IO.Path.Combine(Application.StartupPath, File_Name);
                if (System.IO.File.Exists(File_Path))
                {
                    Music_Player.URL = File_Path;
                    Music_Player.controls.play();
                }
            }
            catch { }
        }

        private void Play_Sound_Effect(string Sound_Type)
        {
            if (Sound_Type == "buff") SFX_Buff?.Play();
            else if (Sound_Type == "debuff") SFX_Debuff?.Play();
            else if (Sound_Type == "hit_car") { SFX_Crash?.Play(); Crash_Count++; }
        }

        private void Draw_Game_Entity(Graphics Graphics_Handle, string Object_Key, Image Source_Image, Size Draw_Size)
        {
            if (Object_Positions.ContainsKey(Object_Key) && Source_Image != null)
            {
                Point Draw_Position = Object_Positions[Object_Key];
                Graphics_Handle.DrawImage(Source_Image, Draw_Position.X, Draw_Position.Y, Draw_Size.Width, Draw_Size.Height);
            }
        }

        private void Setup_Graphics_Quality(Graphics Graphics_Handle)
        {
            Graphics_Handle.CompositingMode = CompositingMode.SourceOver;
            Graphics_Handle.CompositingQuality = CompositingQuality.HighSpeed;
            Graphics_Handle.InterpolationMode = InterpolationMode.NearestNeighbor;
            Graphics_Handle.SmoothingMode = SmoothingMode.None;
            Graphics_Handle.PixelOffsetMode = PixelOffsetMode.HighSpeed;
        }

        private void Enable_Double_Buffering(Panel Target_Panel)
        {
            if (Target_Panel == null) return;
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, Target_Panel, new object[] { true });
        }

        private void Clear_Old_Controls()
        {
            void Scan_And_Remove(Control.ControlCollection Target_Collection)
            {
                var Controls_To_Remove = new List<Control>();
                foreach (Control Current_Control in Target_Collection)
                {
                    if (Current_Control is PictureBox && Current_Control.Name.StartsWith("ptb_")) Controls_To_Remove.Add(Current_Control);
                    if (Current_Control.HasChildren) Scan_And_Remove(Current_Control.Controls);
                }
                foreach (var Item in Controls_To_Remove) Target_Collection.Remove(Item);
            }
            Scan_And_Remove(this.Controls);
        }
    }
}