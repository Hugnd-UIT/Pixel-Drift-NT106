using Pixel_Drift;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_Dang_Nhap : Form
    {
        public static string Current_Username = "";

        public Form_Dang_Nhap()
        {
            InitializeComponent();
        }

        private void btn_dangnhap_Click(object sender, EventArgs e)
        {
            string Username = tb_username.Text.Trim();
            string Password = tb_matkhau.Text.Trim();

            if (Username == "" || Password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!Network_Handle.Is_Connected)
                {
                    string IP = Network_Handle.Get_Server_IP();

                    if (string.IsNullOrEmpty(IP)) IP = "127.0.0.1";

                    if (!Network_Handle.Connect(IP, 1111))
                    {
                        MessageBox.Show("Không tìm thấy server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!Network_Handle.Secure())
                    {
                        MessageBox.Show("Lỗi thiết lập bảo mật! Không thể tiếp tục.");
                        Network_Handle.Close_Connection();
                        return;
                    }

                    Network_Handle.Start_Global_Listening();
                }

                string Response_Key = Network_Handle.Send_And_Wait(new { action = "get_public_key" });

                var Json_Key = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Response_Key);
                string Public_Key = Json_Key["public_key"].GetString();
                string Secure_Pass = RSA_Handle.Encrypt(Password, Public_Key);

                if (Secure_Pass == null)
                {
                    MessageBox.Show("Lỗi mã hóa mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var Request = new
                {
                    action = "login",
                    username = Username,
                    password = Secure_Pass
                };

                string Response = Network_Handle.Send_And_Wait(Request);

                if (string.IsNullOrEmpty(Response))
                {
                    MessageBox.Show("Server không phản hồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var Dict = JsonSerializer.Deserialize<Dictionary<string, string>>(Response);

                if (Dict.ContainsKey("status") && Dict["status"] == "success")
                {
                    DialogResult Result = MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Current_Username = Username;

                    if (Result == DialogResult.OK)
                    {
                        Form_Thong_Tin Form_Thong_Tin = new Form_Thong_Tin(Username);
                        Form_Thong_Tin.Show();
                        this.Close();
                    }
                }
                else
                {
                    string Msg = Dict.ContainsKey("message") ? Dict["message"] : "Sai tài khoản hoặc mật khẩu!";
                    MessageBox.Show(Msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Server chưa sẵn sàng", "Mất kết nối server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException)
            {
                MessageBox.Show("Dữ liệu từ server không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Lỗi: " + Ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_quaylaidk_Click(object sender, EventArgs e)
        {
            Form_Dang_Ki Form = Application.OpenForms.OfType<Form_Dang_Ki>().FirstOrDefault();

            if (Form != null)
            {
                Form.Show();
            }
            else
            {
                Form = new Form_Dang_Ki();
                Form.Show();
            }
            this.Hide();
        }

        private void btn_quenmatkhau_Click(object sender, EventArgs e)
        {
            Form_Quen_Mat_Khau Form = Application.OpenForms.OfType<Form_Quen_Mat_Khau>().FirstOrDefault();

            if (Form != null)
            {
                Form.Show();
            }
            else
            {
                Form = new Form_Quen_Mat_Khau();
                Form.Show();
            }
            this.Hide();
        }
    }
}