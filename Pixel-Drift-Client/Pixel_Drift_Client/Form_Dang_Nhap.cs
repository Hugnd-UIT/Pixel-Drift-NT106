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

        private void btn_dang_nhap_Click(object sender, EventArgs e)
        {
            string Username = tb_username.Text.Trim();
            string Password = tb_matkhau.Text.Trim();

            if (Username == "" || Password == "")
            {
                MessageBox.Show("Please Enter Full Information!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
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

                string Secure_Pass = RSA_Handle.Encrypt(Password, Network_Handle.Public_Key);

                if (Secure_Pass == null)
                {
                    MessageBox.Show("Password Encryption Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show("Server Is Not Responding!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var Dict = JsonSerializer.Deserialize<Dictionary<string, string>>(Response);

                if (Dict.ContainsKey("status") && Dict["status"] == "success")
                {
                    DialogResult Result = MessageBox.Show("Login Successful!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    string Msg = Dict.ContainsKey("message") ? Dict["message"] : "Invalid Username Or Password!";
                    MessageBox.Show(Msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Server Not Ready", "Server Connection Lost", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (JsonException)
            {
                MessageBox.Show("Data From Server Is Invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error: " + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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