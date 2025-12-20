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

        // Hàm mã hóa SHA-256
        private string Ma_Hoa(string Password)
        {
            using (SHA256 Sha = SHA256.Create())
            {
                byte[] Bytes = Sha.ComputeHash(Encoding.UTF8.GetBytes(Password));
                StringBuilder Builder = new StringBuilder();
                foreach (byte B in Bytes)
                    Builder.Append(B.ToString("x2"));
                return Builder.ToString();
            }
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

        private void btn_backdk_Click(object sender, EventArgs e)
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

        private void btn_vaogame_Click(object sender, EventArgs e)
        {
            string Username = textBox1.Text.Trim();
            string Password = textBox2.Text.Trim();

            if (Username == "" || Password == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (!Client_Manager.Is_Connected)
                {
                    string IP = Client_Manager.Get_Server_IP();

                    if (string.IsNullOrEmpty(IP)) IP = "127.0.0.1";

                    if (!Client_Manager.Connect(IP, 1111))
                    {
                        MessageBox.Show("Không tìm thấy server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                string Hashed_Password = Ma_Hoa(Password);

                var Request = new
                {
                    action = "login",
                    username = Username,
                    password = Hashed_Password
                };

                string Response = Client_Manager.Send_And_Wait(Request);

                if (string.IsNullOrEmpty(Response))
                {
                    MessageBox.Show("Server không phản hồi!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var Dict = JsonSerializer.Deserialize<Dictionary<string, string>>(Response);

                if (Dict.ContainsKey("status") && Dict["status"] == "success")
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Form_Dang_Nhap.Current_Username = Username;
                    this.Hide();
                    Form_Thong_Tin Form_Thong_Tin = new Form_Thong_Tin(Username);
                    Form_Thong_Tin.ShowDialog();
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
    }
}