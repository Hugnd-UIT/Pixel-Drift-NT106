using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_Doi_Mat_Khau : Form
    {
        private string User_Email;

        public Form_Doi_Mat_Khau(string Email)
        {
            InitializeComponent();
            User_Email = Email;
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

        private void btn_doimk_Click(object sender, EventArgs e)
        {
            string Token = txt_mkcu.Text.Trim();
            string New_Pass = txt_mkmoi.Text.Trim();
            string Confirm = txt_xacnhanmk.Text.Trim();

            if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(New_Pass) || string.IsNullOrEmpty(Confirm))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (New_Pass != Confirm)
            {
                MessageBox.Show("Mật khẩu xác nhận không trùng khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            try
            {
                var Request = new
                {
                    action = "change_password",
                    email = User_Email,
                    token = Token,
                    new_password = Ma_Hoa(New_Pass)
                };

                string Response = Client_Manager.Send_And_Wait(Request);

                if (Response == null)
                {
                    throw new SocketException();
                }

                var Dict = JsonSerializer.Deserialize<Dictionary<string, string>>(Response);

                if (Dict.ContainsKey("status") && Dict["status"] == "success")
                {
                    MessageBox.Show(Dict["message"], "Thành công",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form_Dang_Nhap Existing_Login = Application.OpenForms.OfType<Form_Dang_Nhap>().FirstOrDefault();

                    if (Existing_Login != null)
                    {
                        Existing_Login.Show();
                    }
                    else
                    {
                        Form_Dang_Nhap Dn = new Form_Dang_Nhap();
                        Dn.Show();
                    }
                    this.Hide();
                }
                else
                {
                    string Msg = Dict.ContainsKey("message") ? Dict["message"] : "Đổi mật khẩu thất bại!";
                    MessageBox.Show(Msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Không thể kết nối hoặc mất kết nối đến server!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException)
            {
                MessageBox.Show("Phản hồi từ server không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Lỗi: " + Ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Dang_Nhap Form_Dang_Nhap = new Form_Dang_Nhap();
            Form_Dang_Nhap.ShowDialog();
            this.Close();
        }
    }
}