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
                MessageBox.Show("Please Enter Your Email!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Network_Handle.Is_Connected)
            {
                string IP = Network_Handle.Get_Server_IP();

                if (string.IsNullOrEmpty(IP))
                {
                    IP = "127.0.0.1";
                }

                if (!Network_Handle.Connect(IP, 1111))
                {
                    MessageBox.Show("Server Not Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Network_Handle.Secure())
                {
                    MessageBox.Show("Security Setup Error! Cannot Continue.");
                    Network_Handle.Close_Connection();
                    return;
                }

                Network_Handle.Start_Listening();
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
                    MessageBox.Show(Dict["message"], "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    string Msg = Dict.ContainsKey("message") ? Dict["message"] : "Cannot Send Password!";
                    MessageBox.Show(Msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Cannot Connect To Server. Check IP And Port!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException)
            {
                MessageBox.Show("Response From Server Is Invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error: " + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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