using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Lobby : Form
    {
        private string My_Username;

        public Lobby(string Username)
        {
            InitializeComponent();
            My_Username = Username;

            Network_Handle.Start_Global_Listening();

            Network_Handle.On_Message_Received += Handle_Server_Message;
        }

        private void Handle_Server_Message(string Message)
        {
            if (this.Disposing || this.IsDisposed || !this.IsHandleCreated) return;

            this.Invoke(new Action(() =>
            {
                try
                {
                    var Data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Message);
                    string Status = "";
                    if (Data.ContainsKey("status")) Status = Data["status"].GetString();
                    else if (Data.ContainsKey("action")) Status = Data["action"].GetString();

                    if (Status == "create_room_success" || Status == "join_room_success")
                    {
                        string Room_ID = Data["room_id"].GetString();
                        int Player_Num = Data["player_number"].GetInt32();

                        Game_Window Game_Form = new Game_Window(My_Username, Player_Num, Room_ID);

                        Game_Form.FormClosed += (s, args) =>
                        {
                            if (!this.IsDisposed) this.Show();
                        };

                        this.Hide();
                        Game_Form.Show();
                    }
                    else if (Status == "force_logout")
                    {
                        string Msg = Data.ContainsKey("message") ? Data["message"].GetString() : "Account Logged In From Another Location!";

                        MessageBox.Show(Msg, "Login Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        Application.Exit();
                    }
                    else if (Status == "error")
                    {
                        MessageBox.Show(Data["message"].GetString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch { }
            }));
        }

        private void btn_CreateRoom_Click(object sender, EventArgs e)
        {
            Network_Handle.Send_And_Forget(new { action = "create_room", username = My_Username });
        }

        private void btn_JoinRoom_Click(object sender, EventArgs e)
        {
            using (Form_ID Input_Form = new Form_ID())
            {
                if (Input_Form.ShowDialog() == DialogResult.OK)
                {
                    Network_Handle.Send_And_Forget(new { action = "join_room", room_id = Input_Form.Room_ID, username = My_Username });
                }
            }
        }

        private void Lobby_FormClosed(object sender, FormClosedEventArgs e)
        {
            Network_Handle.On_Message_Received -= Handle_Server_Message;
            Application.Exit();
        }

        private void btn_Scoreboard_Click(object sender, EventArgs e)
        {
            var Sb = Application.OpenForms.OfType<Form_ScoreBoard>().FirstOrDefault();
            if (Sb != null) Sb.Show();
            else
            {
                new Form_ScoreBoard(Network_Handle.Get_Client()).Show();
            }
        }
    }
}