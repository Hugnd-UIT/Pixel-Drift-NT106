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

        private void btn_doimk_Click(object sender, EventArgs e)
        {
            string Token = txt_mkcu.Text.Trim();
            string New_Pass = txt_mkmoi.Text.Trim();
            string Confirm = txt_xacnhanmk.Text.Trim();

            if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(New_Pass) || string.IsNullOrEmpty(Confirm))
            {
                MessageBox.Show("Please Enter Full Information!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (New_Pass != Confirm)
            {
                MessageBox.Show("Confirm Password Does Not Match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string Response_Key = Network_Handle.Send_And_Wait(new
                {
                    action = "get_public_key"
                });

                var Json_Key = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Response_Key);
                string Public_Key = Json_Key["public_key"].GetString();
                string Secure_Pass = RSA_Handle.Encrypt(New_Pass, Public_Key);

                if (Secure_Pass == null)
                {
                    MessageBox.Show("Password Encryption Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var Request = new
                {
                    action = "change_password",
                    email = User_Email,
                    token = Token,
                    new_password = Secure_Pass
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
                    string Msg = Dict.ContainsKey("message") ? Dict["message"] : "Change Password Failed!";
                    MessageBox.Show(Msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SocketException)
            {
                MessageBox.Show("Cannot Connect Or Lost Connection To Server!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form_Dang_Nhap Form_Dang_Nhap = new Form_Dang_Nhap();
            Form_Dang_Nhap.ShowDialog();
            this.Close();
        }
    }
}