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

            try
            {
                if (!Client_Manager.Is_Connected)
                {

                    if (!Client_Manager.Connect(Client_Manager.Get_Server_IP(), 1111))
                    {
                        throw new SocketException();
                    }
                }

                var Request = new
                {
                    action = "forgot_password",
                    email = Email
                };

                string Response = Client_Manager.Send_And_Wait(Request);

                if (Response == null)
                {
                    throw new SocketException();
                }

                var Dict = JsonSerializer.Deserialize<Dictionary<string, string>>(Response);

                if (Dict.ContainsKey("status") && Dict["status"] == "success")
                {
                    MessageBox.Show(Dict["message"], "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form_Doi_Mat_Khau Form_Doi = Application.OpenForms.OfType<Form_Doi_Mat_Khau>().FirstOrDefault();

                    if (Form_Doi != null)
                    {
                        Form_Doi.Show();
                    }
                    else
                    {
                        Form_Doi = new Form_Doi_Mat_Khau(Email);
                        Form_Doi.Show();
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