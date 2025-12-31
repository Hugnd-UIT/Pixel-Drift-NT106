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
                    MessageBox.Show("Server không phản hồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show("Không thể tải thông tin.");
                    }
                }
                else
                {
                    MessageBox.Show("Phản hồi từ server không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Server chưa sẵn sàng", "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException Ex)
            {
                MessageBox.Show($"Dữ liệu từ server không hợp lệ: {Ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Lỗi khi tải thông tin: " + Ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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