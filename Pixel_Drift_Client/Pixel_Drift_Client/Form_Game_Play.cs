using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
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

        private Dictionary<string, Control> Game_Objects = new Dictionary<string, Control>();

        // Chống Spam phím
        private bool Is_Left_Pressed = false;
        private bool Is_Right_Pressed = false;

        // Âm thanh
        private WindowsMediaPlayer Music;
        private SoundPlayer CountDown_5Sec, Buff, Debuff, Car_Hit;

        // Điểm số
        private long Player1_Score = 0;
        private long Player2_Score = 0;
        private int Crash_Count = 0;

        public Game_Window(string Username, int Player_Num, string Room_ID)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            this.My_Username = Username;
            this.My_Player_Number = Player_Num;
            if (btn_ID != null) btn_ID.Text = "ID: " + Room_ID;

            string Player_Color = (My_Player_Number == 1) ? "Xe Đỏ" : "Xe Xanh";
            this.Text = $"Pixel Drift - PLAYER {My_Player_Number} ({Player_Color}) - {My_Username}";

            Client_Manager.On_Message_Received += Handle_Server_Message;
        }

        public Game_Window() { InitializeComponent(); }

        private void Game_Window_Load(object sender, EventArgs e)
        {
            try
            {
                Init_Audio();
                Init_Controls_Cache();
                Reset_To_Lobby();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Lỗi khởi tạo Game: " + Ex.Message);
                this.Close();
            }
        }

        private void Handle_Server_Message(string Message)
        {
            if (this.Disposing || this.IsDisposed || !this.IsHandleCreated) return;

            this.Invoke(new Action(() =>
            {
                try
                {
                    var Data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Message);
                    if (!Data.ContainsKey("action")) return;

                    string Action = Data["action"].GetString();

                    switch (Action)
                    {
                        case "update_game_state":
                            Update_Game_Objects(Data);
                            break;

                        case "update_score":
                            Player1_Score = Data["p1_score"].GetInt64();
                            Player2_Score = Data["p2_score"].GetInt64();
                            if (lbl_Score1 != null) lbl_Score1.Text = "Score: " + Player1_Score;
                            if (lbl_Score2 != null) lbl_Score2.Text = "Score: " + Player2_Score;
                            break;

                        case "update_time":
                            if (lbl_GameTimer != null)
                                lbl_GameTimer.Text = "Time: " + Data["time"].GetInt32();
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
                                if (Time == 5) { Music.controls.stop(); CountDown_5Sec?.Play(); }
                            }
                            break;

                        case "update_ready_status":
                            string P1_Name = Data["player1_name"].GetString();
                            string P2_Name = Data["player2_name"].GetString();
                            bool P1_Ready = Data["player1_ready"].GetBoolean();
                            bool P2_Ready = Data["player2_ready"].GetBoolean();
                            if (lbl_P1_Status != null) lbl_P1_Status.Text = $"P1 ({P1_Name}): {(P1_Ready ? "Sẵn sàng" : "...")}";
                            if (lbl_P2_Status != null) lbl_P2_Status.Text = $"P2 ({P2_Name}): {(P2_Ready ? "Sẵn sàng" : "...")}";
                            break;

                        case "game_over":
                            Music?.controls.stop();
                            MessageBox.Show("Hết giờ! Trò chơi kết thúc.", "Thông báo");
                            End_Game();
                            Reset_To_Lobby();
                            break;

                        case "player_disconnected":
                            string Name = Data.ContainsKey("name") ? Data["name"].GetString() : "Đối thủ";
                            Music?.controls.stop();
                            CountDown_5Sec?.Stop();
                            MessageBox.Show($"{Name} đã thoát. Bạn sẽ về Lobby.", "Thông báo");

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
                            MessageBox.Show("Tài khoản đăng nhập nơi khác!", "Cảnh báo");
                            Application.Exit();
                            break;
                    }
                }
                catch { }
            }));
        }

        private void Init_Controls_Cache()
        {
            string[] Object_Names = {
                "ptb_player1", "ptb_player2",
                "ptb_roadtrack1", "ptb_roadtrack1dup", "ptb_roadtrack2", "ptb_roadtrack2dup",
                "ptb_AICar1", "ptb_AICar3", "ptb_AICar5", "ptb_AICar6",
                "ptb_increasingroad1", "ptb_decreasingroad1", "ptb_increasingroad2", "ptb_decreasingroad2"
            };

            foreach (string Name in Object_Names)
            {
                Control C = this.Controls.Find(Name, true).FirstOrDefault();
                if (C != null) Game_Objects[Name] = C;
            }
        }

        private void Update_Game_Objects(Dictionary<string, JsonElement> Data)
        {
            foreach (var Kvp in Game_Objects)
            {
                if (Data.ContainsKey(Kvp.Key))
                {
                    JsonElement El = Data[Kvp.Key];
                    int X = El.GetProperty("X").GetInt32();
                    int Y = El.GetProperty("Y").GetInt32();

                    if (Kvp.Value.Location.X != X || Kvp.Value.Location.Y != Y)
                    {
                        Kvp.Value.Location = new Point(X, Y);
                    }
                }
            }
        }

        private void Send(object Msg) => Client_Manager.Send_And_Forget(Msg);

        private void btn_Ready_Click(object sender, EventArgs e)
        {
            Send(new { action = "set_ready", ready_status = "true" });
            btn_Ready.Enabled = false;
            btn_Ready.Text = "Đang chờ...";
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
            {
                Send(new { action = "move", player = My_Player_Number, direction = Direction, state = "down" });
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
                Send(new { action = "move", player = My_Player_Number, direction = Direction, state = "up" });
            }
        }

        private void Game_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            Client_Manager.On_Message_Received -= Handle_Server_Message;

            try
            {
                Music?.controls.stop(); Music?.close();
                CountDown_5Sec?.Stop(); Car_Hit?.Stop(); Buff?.Stop(); Debuff?.Stop();
            }
            catch { }

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
                CountDown_5Sec.LoadAsync(); Buff.LoadAsync(); Debuff.LoadAsync(); Car_Hit.LoadAsync();
            }
            catch { }
        }

        private void Play_Music_Loop(string Music_File)
        {
            try
            {
                string Path_File = Path.Combine(Application.StartupPath, Music_File);
                if (File.Exists(Path_File)) { Music.URL = Path_File; Music.controls.play(); }
            }
            catch { }
        }

        private void Play_Sound_Effect(string Sound_Type)
        {
            if (Sound_Type == "buff") Buff?.Play();
            else if (Sound_Type == "debuff") Debuff?.Play();
            else if (Sound_Type == "hit_car") { Car_Hit?.Play(); Crash_Count++; }
        }

        private void Toggle_Game_Objects(bool Show)
        {
            foreach (var Ctrl in Game_Objects.Values) Ctrl.Visible = Show;
        }

        private void Start_Game()
        {
            Crash_Count = 0; Player1_Score = 0; Player2_Score = 0;
            btn_Ready.Visible = false; lbl_P1_Status.Visible = false;
            lbl_P2_Status.Visible = false; lbl_Countdown.Visible = false;
            btn_Scoreboard.Enabled = false;

            Toggle_Game_Objects(true);
            lbl_GameTimer.Visible = true; lbl_GameTimer.Text = "Time: 60";
            if (lbl_Score1 != null) { lbl_Score1.Visible = true; lbl_Score1.Text = "Score: 0"; }
            if (lbl_Score2 != null) { lbl_Score2.Visible = true; lbl_Score2.Text = "Score: 0"; }

            if (Game_Objects.ContainsKey("ptb_roadtrack1")) Game_Objects["ptb_roadtrack1"].SendToBack();
            if (Game_Objects.ContainsKey("ptb_roadtrack1dup")) Game_Objects["ptb_roadtrack1dup"].SendToBack();
            if (Game_Objects.ContainsKey("ptb_roadtrack2")) Game_Objects["ptb_roadtrack2"].SendToBack();
            if (Game_Objects.ContainsKey("ptb_roadtrack2dup")) Game_Objects["ptb_roadtrack2dup"].SendToBack();

            if (Game_Objects.ContainsKey("ptb_player1")) Game_Objects["ptb_player1"].BringToFront();
            if (Game_Objects.ContainsKey("ptb_player2")) Game_Objects["ptb_player2"].BringToFront();

            if (Game_Objects.ContainsKey("ptb_AICar1")) Game_Objects["ptb_AICar1"].BringToFront();
            if (Game_Objects.ContainsKey("ptb_AICar3")) Game_Objects["ptb_AICar3"].BringToFront();
            if (Game_Objects.ContainsKey("ptb_AICar5")) Game_Objects["ptb_AICar5"].BringToFront();
            if (Game_Objects.ContainsKey("ptb_AICar6")) Game_Objects["ptb_AICar6"].BringToFront();

            if (Game_Objects.ContainsKey("ptb_increasingroad1")) Game_Objects["ptb_increasingroad1"].BringToFront();
            if (Game_Objects.ContainsKey("ptb_decreasingroad1")) Game_Objects["ptb_decreasingroad1"].BringToFront();
            if (Game_Objects.ContainsKey("ptb_increasingroad2")) Game_Objects["ptb_increasingroad2"].BringToFront();
            if (Game_Objects.ContainsKey("ptb_decreasingroad2")) Game_Objects["ptb_decreasingroad2"].BringToFront();

            CountDown_5Sec?.Stop();
            Play_Music_Loop("compete.wav");

            this.Focus();
        }

        private void Reset_To_Lobby()
        {
            CountDown_5Sec?.Stop();
            Play_Music_Loop("wait.wav");
            game_timer.Stop();

            btn_Ready.Visible = true; btn_Ready.Enabled = true; btn_Ready.Text = "Sẵn sàng";
            lbl_P1_Status.Visible = true; lbl_P2_Status.Visible = true;
            btn_Scoreboard.Enabled = true;

            Toggle_Game_Objects(false);
            lbl_Countdown.Visible = false; lbl_GameTimer.Visible = false;
            if (lbl_Score1 != null) lbl_Score1.Visible = false;
            if (lbl_Score2 != null) lbl_Score2.Visible = false;

            btn_Ready.Focus();
        }

        private void End_Game()
        {
            try
            {
                int Win_Count = 0;
                if (My_Player_Number == 1 && Player1_Score > Player2_Score) Win_Count = 1;
                else if (My_Player_Number == 2 && Player2_Score > Player1_Score) Win_Count = 1;

                double Total_Score = Player1_Score + (Win_Count * 500) - (Crash_Count * 50);
                if (My_Player_Number == 2) Total_Score = Player2_Score + (Win_Count * 500) - (Crash_Count * 50);

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
            catch { }
            finally { Crash_Count = 0; Player1_Score = 0; Player2_Score = 0; }
        }

        private void btn_Scoreboard_Click(object sender, EventArgs e)
        {
            var Sb = Application.OpenForms.OfType<Form_ScoreBoard>().FirstOrDefault();
            if (Sb != null) Sb.Show();
            else { new Form_ScoreBoard(Client_Manager.Get_Client()).Show(); }
        }
    }
}