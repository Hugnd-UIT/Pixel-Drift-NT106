using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_Thong_Tin : Form
    {
        private string Current_Username;

        public Form_Thong_Tin(string Username)
        {
            InitializeComponent();
            Current_Username = Username;
        }

        private void Form_Thong_Tin_Load(object sender, EventArgs e)
        {
            try
            {
                var Request = new
                {
                    action = "get_info",
                    username = Current_Username
                };

                string Response = Network_Handle.Send_And_Wait(Request);

                if (string.IsNullOrEmpty(Response))
                {
                    MessageBox.Show("Server Is Not Responding!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var Dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Response);

                if (Dict.ContainsKey("status"))
                {
                    string Status = Dict["status"].GetString();
                    if (Status == "success")
                    {
                        lbl_username.Text = Dict.ContainsKey("username") ? Dict["username"].GetString() : "N/A";
                        lbl_email.Text = Dict.ContainsKey("email") ? Dict["email"].GetString() : "N/A";
                        lbl_birthday.Text = Dict.ContainsKey("birthday") ? Dict["birthday"].GetString() : "N/A";
                    }
                    else
                    {
                        MessageBox.Show("Unable To Load Information.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Response From Server!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Server Is Not Ready", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException Ex)
            {
                MessageBox.Show($"Data From Server Is Invalid: {Ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error Loading Information: " + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form_Thong_Tin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btn_vao_game_Click(object sender, EventArgs e)
        {
            Lobby Lobby_Form = new Lobby(Current_Username);
            Lobby_Form.Show();
            this.Hide();
        }
    }
}