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
        private int My_Player_Number = 0;
        private string My_Username;
        private bool Is_Returning_To_Lobby = false;

        private Dictionary<string, Point> Object_Positions = new Dictionary<string, Point>();

        private Image Img_Player_1 = Properties.Resources.Player_Car_1;
        private Image Img_Player_2 = Properties.Resources.Player_Car_2;
        private Image Img_Road = Properties.Resources.Road;
        private Image Img_Buff = Properties.Resources.Buff;
        private Image Img_Debuff = Properties.Resources.Debuff;

        private Image Img_Black_Car = Properties.Resources.Black_Car;
        private Image Img_Blue_Car = Properties.Resources.Blue_Car;
        private Image Img_Green_Car = Properties.Resources.Green_Car;
        private Image Img_Red_Car = Properties.Resources.Red_Car;

        private Size Size_Player = new Size(80, 175);
        private Size Size_Road = new Size(610, 910);
        private Size Size_Item = new Size(60, 60);
        private Size Size_AICar = new Size(80, 175);

        private bool Is_Left_Pressed = false;
        private bool Is_Right_Pressed = false;
        private WindowsMediaPlayer Music;
        private SoundPlayer CountDown_5Sec;
        private SoundPlayer Buff;
        private SoundPlayer Debuff;
        private SoundPlayer Car_Hit;
        private long Player1_Score = 0;
        private long Player2_Score = 0;
        private int Crash_Count = 0;

        public Game_Window(string Username, int Player_Num, string Room_ID)
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            this.KeyPreview = true;
            this.My_Username = Username;
            this.My_Player_Number = Player_Num;

            if (btn_ID != null)
            {
                btn_ID.Text = "ID: " + Room_ID;
            }

            string Player_Color = (My_Player_Number == 1) ? "Red Car" : "Blue Car";
            this.Text = $"Pixel Drift - PLAYER {My_Player_Number} ({Player_Color}) - {My_Username}";

            Network_Handle.On_Message_Received += Handle_Server_Message;
        }

        public Game_Window()
        {
            InitializeComponent();
        }

        private void Game_Window_Load(object sender, EventArgs e)
        {
            try
            {
                Remove_Old_PictureBoxes();
                Enable_Panel_DoubleBuffer(panel1);
                Enable_Panel_DoubleBuffer(panel2);
                panel1.Paint += Panel1_Paint;
                panel2.Paint += Panel2_Paint;
                panel1.BackColor = Color.Black;
                panel2.BackColor = Color.Black;

                Init_Audio();
                Reset_To_Lobby();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Game Initialization Error: " + Ex.Message);
                this.Close();
            }
        }

        private void Enable_Panel_DoubleBuffer(Panel p)
        {
            if (p == null)
            {
                return;
            }
            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, p, new object[] { true });
        }

        private void Remove_Old_PictureBoxes()
        {
            void ScanAndRemove(Control.ControlCollection controls)
            {
                var itemsToRemove = new List<Control>();
                foreach (Control c in controls)
                {
                    if (c is PictureBox && c.Name.StartsWith("ptb_"))
                    {
                        itemsToRemove.Add(c);
                    }
                    if (c.HasChildren)
                    {
                        ScanAndRemove(c.Controls);
                    }
                }
                foreach (var item in itemsToRemove)
                {
                    controls.Remove(item);
                }
            }
            ScanAndRemove(this.Controls);
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            Draw_Object(g, "ptb_roadtrack1", Img_Road, panel1.Size);
            Draw_Object(g, "ptb_roadtrack1dup", Img_Road, panel1.Size);

            Draw_Object(g, "ptb_increasingroad1", Img_Buff, Size_Item);
            Draw_Object(g, "ptb_decreasingroad1", Img_Debuff, Size_Item);

            Draw_Object(g, "ptb_AICar1", Img_Black_Car, Size_AICar);
            Draw_Object(g, "ptb_AICar5", Img_Blue_Car, Size_AICar);

            Draw_Object(g, "ptb_player1", Img_Player_1, Size_Player);
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            Draw_Object(g, "ptb_roadtrack2", Img_Road, panel2.Size);
            Draw_Object(g, "ptb_roadtrack2dup", Img_Road, panel2.Size);

            Draw_Object(g, "ptb_increasingroad2", Img_Buff, Size_Item);
            Draw_Object(g, "ptb_decreasingroad2", Img_Debuff, Size_Item);

            Draw_Object(g, "ptb_AICar3", Img_Green_Car, Size_AICar);
            Draw_Object(g, "ptb_AICar6", Img_Red_Car, Size_AICar);

            Draw_Object(g, "ptb_player2", Img_Player_2, Size_Player);
        }

        private void Draw_Object(Graphics g, string key, Image img, Size size)
        {
            if (Object_Positions.ContainsKey(key) && img != null)
            {
                Point p = Object_Positions[key];
                g.DrawImage(img, p.X, p.Y, size.Width, size.Height);
            }
        }



        private void Handle_Server_Message(string Message)
        {
            if (this.Disposing || this.IsDisposed || !this.IsHandleCreated)
            {
                return;
            }

            this.Invoke(new Action(() =>
            {
                try
                {
                    var Data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Message);
                    if (!Data.ContainsKey("action"))
                    {
                        return;
                    }

                    string Action = Data["action"].GetString();

                    switch (Action)
                    {
                        case "update_game_state":
                            Update_Game_Positions(Data);
                            break;

                        case "update_score":
                            Player1_Score = Data["p1_score"].GetInt64();
                            Player2_Score = Data["p2_score"].GetInt64();
                            if (lbl_Score1 != null)
                            {
                                lbl_Score1.Text = "Score: " + Player1_Score;
                            }
                            if (lbl_Score2 != null)
                            {
                                lbl_Score2.Text = "Score: " + Player2_Score;
                            }
                            break;

                        case "update_time":
                            if (lbl_GameTimer != null)
                            {
                                lbl_GameTimer.Text = "Time: " + Data["time"].GetInt32();
                            }
                            break;

                        case "start_game":
                            Start_Game();
                            break;

                        case "countdown":
                            if (lbl_Countdown != null)
                            {
                                lbl_Countdown.Visible = true;
                                int Time = Data["time"].GetInt32();
                                lbl_Countdown.Text = Time.ToString();
                                if (Time == 5)
                                {
                                    Music.controls.stop();
                                    CountDown_5Sec?.Play();
                                }
                            }
                            break;

                        case "update_ready_status":
                            string P1_Name = Data["player1_name"].GetString();
                            string P2_Name = Data["player2_name"].GetString();
                            bool P1_Ready = Data["player1_ready"].GetBoolean();
                            bool P2_Ready = Data["player2_ready"].GetBoolean();
                            if (lbl_P1_Status != null)
                            {
                                lbl_P1_Status.Text = $"P1 ({P1_Name}): {(P1_Ready ? "Ready" : "...")}";
                            }
                            if (lbl_P2_Status != null)
                            {
                                lbl_P2_Status.Text = $"P2 ({P2_Name}): {(P2_Ready ? "Ready" : "...")}";
                            }
                            break;

                        case "game_over":
                            Music?.controls.stop();
                            MessageBox.Show("Time Is Up! Game Over.", "Notification");
                            End_Game();
                            Reset_To_Lobby();
                            break;

                        case "player_disconnected":
                            string Name = Data.ContainsKey("name") ? Data["name"].GetString() : "Opponent";
                            Music?.controls.stop();
                            CountDown_5Sec?.Stop();
                            MessageBox.Show($"{Name} Has Disconnected. You Will Return To Lobby.", "Notification");

                            Is_Returning_To_Lobby = true;
                            Send(new { action = "leave_room" });
                            this.Close();
                            break;

                        case "play_sound":
                            string Sound = Data["sound"].GetString();
                            Play_Sound_Effect(Sound);
                            break;

                        case "force_logout":
                            Music?.controls.stop();
                            MessageBox.Show("Account Logged In From Another Location!", "Warning");
                            Application.Exit();
                            break;
                    }
                }
                catch
                {
                }
            }));
        }

        private void Update_Game_Positions(Dictionary<string, JsonElement> Data)
        {
            foreach (var key in Data.Keys)
            {
                if (key == "action")
                {
                    continue;
                }
                try
                {
                    JsonElement El = Data[key];
                    if (El.ValueKind == JsonValueKind.Object &&
                        El.TryGetProperty("X", out var xVal) &&
                        El.TryGetProperty("Y", out var yVal))
                    {
                        Object_Positions[key] = new Point(xVal.GetInt32(), yVal.GetInt32());
                    }
                }
                catch
                {
                }
            }
            if (panel1 != null)
            {
                panel1.Invalidate();
            }
            if (panel2 != null)
            {
                panel2.Invalidate();
            }
        }

        private void Send(object Msg)
        {
            Network_Handle.Send_And_Forget(Msg);
        }

        private void btn_Ready_Click(object sender, EventArgs e)
        {
            Send(new
            {
                action = "set_ready",
                ready_status = "true"
            });
            btn_Ready.Enabled = false;
            btn_Ready.Text = "Waiting...";
            this.Focus();
        }

        private void Game_Window_KeyDown(object sender, KeyEventArgs e)
        {
            string Direction = null;
            if (e.KeyCode == Keys.Left)
            {
                if (Is_Left_Pressed)
                {
                    return;
                }
                Is_Left_Pressed = true;
                Direction = "left";
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (Is_Right_Pressed)
                {
                    return;
                }
                Is_Right_Pressed = true;
                Direction = "right";
            }

            if (Direction != null)
            {
                Send(new
                {
                    action = "move",
                    player = My_Player_Number,
                    direction = Direction,
                    state = "down"
                });
            }
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
            {
                Send(new
                {
                    action = "move",
                    player = My_Player_Number,
                    direction = Direction,
                    state = "up"
                });
            }
        }

        private void Game_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Network_Handle.On_Message_Received -= Handle_Server_Message;
            try
            {
                Music?.controls.stop();
                Music?.close();
                CountDown_5Sec?.Stop();
                Car_Hit?.Stop();
                Buff?.Stop();
                Debuff?.Stop();
            }
            catch
            {
            }

            if (!Is_Returning_To_Lobby)
            {
                Send(new { action = "leave_room" });
            }
        }

        private void Init_Audio()
        {
            Music = new WindowsMediaPlayer();
            Music.settings.setMode("loop", true);
            Music.settings.volume = 30;
            try
            {
                CountDown_5Sec = new SoundPlayer("countdown.wav");
                Buff = new SoundPlayer("buff.wav");
                Debuff = new SoundPlayer("debuff.wav");
                Car_Hit = new SoundPlayer("crash.wav");
                CountDown_5Sec.LoadAsync();
                Buff.LoadAsync();
                Debuff.LoadAsync();
                Car_Hit.LoadAsync();
            }
            catch
            {
            }
        }

        private void Play_Music_Loop(string Music_File)
        {
            try
            {
                string Path_File = System.IO.Path.Combine(Application.StartupPath, Music_File);
                if (System.IO.File.Exists(Path_File))
                {
                    Music.URL = Path_File;
                    Music.controls.play();
                }
            }
            catch
            {
            }
        }

        private void Play_Sound_Effect(string Sound_Type)
        {
            if (Sound_Type == "buff")
            {
                Buff?.Play();
            }
            else if (Sound_Type == "debuff")
            {
                Debuff?.Play();
            }
            else if (Sound_Type == "hit_car")
            {
                Car_Hit?.Play();
                Crash_Count++;
            }
        }

        private void Start_Game()
        {
            Crash_Count = 0;
            Player1_Score = 0;
            Player2_Score = 0;
            btn_Ready.Visible = false;
            lbl_P1_Status.Visible = false;
            lbl_P2_Status.Visible = false;
            lbl_Countdown.Visible = false;
            btn_Scoreboard.Enabled = false;
            btn_Scoreboard.Visible = false;
            btn_ID.Visible = false;

            lbl_GameTimer.Visible = true;
            lbl_GameTimer.Text = "Time: 60";
            if (lbl_Score1 != null)
            {
                lbl_Score1.Visible = true;
                lbl_Score1.Text = "Score: 0";
            }
            if (lbl_Score2 != null)
            {
                lbl_Score2.Visible = true;
                lbl_Score2.Text = "Score: 0";
            }

            CountDown_5Sec?.Stop();
            Play_Music_Loop("compete.wav");
            this.Focus();
        }

        private void Reset_To_Lobby()
        {
            CountDown_5Sec?.Stop();
            Play_Music_Loop("wait.wav");

            Object_Positions.Clear();
            if (panel1 != null)
            {
                panel1.Invalidate();
            }
            if (panel2 != null)
            {
                panel2.Invalidate();
            }

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
            if (lbl_Score1 != null)
            {
                lbl_Score1.Visible = false;
            }
            if (lbl_Score2 != null)
            {
                lbl_Score2.Visible = false;
            }

            btn_Ready.Focus();
        }

        private void End_Game()
        {
            try
            {
                int Win_Count = 0;
                if (My_Player_Number == 1 && Player1_Score > Player2_Score)
                {
                    Win_Count = 1;
                }
                else if (My_Player_Number == 2 && Player2_Score > Player1_Score)
                {
                    Win_Count = 1;
                }

                double Total_Score = Player1_Score + (Win_Count * 500) - (Crash_Count * 50);
                if (My_Player_Number == 2)
                {
                    Total_Score = Player2_Score + (Win_Count * 500) - (Crash_Count * 50);
                }

                var Score_Data = new
                {
                    action = "add_score",
                    player_name = My_Username,
                    win_count = Win_Count,
                    crash_count = Crash_Count,
                    total_score = Total_Score
                };
                Send(Score_Data);
            }
            catch
            {
                // Continue
            }
            finally
            {
                Crash_Count = 0;
                Player1_Score = 0;
                Player2_Score = 0;
            }
        }

        private void btn_Scoreboard_Click(object sender, EventArgs e)
        {
            var Sb = Application.OpenForms.OfType<Form_ScoreBoard>().FirstOrDefault();
            if (Sb != null)
            {
                Sb.Show();
            }
            else
            {
                new Form_ScoreBoard(Network_Handle.Get_Client()).Show();
            }
        }
    }
}