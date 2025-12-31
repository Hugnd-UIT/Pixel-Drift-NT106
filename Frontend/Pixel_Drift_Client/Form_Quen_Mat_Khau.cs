using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text.Json;
using System.Windows.Forms;


namespace Pixel_Drift
{
    public partial class Form_Quen_Mat_Khau : Form
    {
        public Form_Quen_Mat_Khau()
        {
            InitializeComponent();
        }

        private void btn_quenmatkhau_Click(object sender, EventArgs e)
        {
            string Email = txt_email.Text.Trim();

            if (string.IsNullOrEmpty(Email))
            {
                MessageBox.Show("Vui lòng nhập email của bạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            try
            {
                var Request = new
                {
                    action = "forgot_password",
                    email = Email
                };

                string Response = Network_Handle.Send_And_Wait(Request);

                if (Response == null)
                {
                    throw new SocketException();
                }

                var Dict = JsonSerializer.Deserialize<Dictionary<string, string>>(Response);

                if (Dict.ContainsKey("status") && Dict["status"] == "success")
                {
                    MessageBox.Show(Dict["message"], "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form_Doi_Mat_Khau Form = Application.OpenForms.OfType<Form_Doi_Mat_Khau>().FirstOrDefault();

                    if (Form != null)
                    {
                        Form.Show();
                    }
                    else
                    {
                        Form = new Form_Doi_Mat_Khau(Email);
                        Form.Show();
                    }
                    this.Close();
                }
                else
                {
                    string Msg = Dict.ContainsKey("message") ? Dict["message"] : "Không thể gửi mật khẩu!";
                    MessageBox.Show(Msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Không thể kết nối đến server. Kiểm tra IP và cổng!", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btn_quaylai_Click(object sender, EventArgs e)
        {
            Form_Dang_Nhap Form = Application.OpenForms.OfType<Form_Dang_Nhap>().FirstOrDefault();

            if (Form != null)
            {
                Form.Show();
            }
            else
            {
                Form = new Form_Dang_Nhap();
                Form.Show();
            }
            this.Close();
        }
    }
}