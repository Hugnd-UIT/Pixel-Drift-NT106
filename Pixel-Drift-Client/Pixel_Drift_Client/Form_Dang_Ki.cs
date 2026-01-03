using Pixel_Drift;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_Dang_Ki : Form
    {
        public Form_Dang_Ki()
        {
            InitializeComponent();
        }

        private string Dinh_Dang_Ngay(string Day)
        {
            if (DateTime.TryParse(Day, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime Parsed_Day))
            {
                return Parsed_Day.ToString("yyyy-MM-dd");
            }
            return null;
        }

        private bool Kiem_Tra_Do_Manh_Mat_Khau(string Password)
        {
            if (Password.Length < 8)
            {
                return false;
            }

            bool Co_Chu_Hoa = Regex.IsMatch(Password, "[A-Z]");
            bool Co_Chu_Thuong = Regex.IsMatch(Password, "[a-z]");
            bool Co_So = Regex.IsMatch(Password, "[0-9]");
            bool Co_Ky_Tu_Dac_Biet = Regex.IsMatch(Password, @"[@$!%*?&#]");

            return Co_Chu_Hoa && Co_Chu_Thuong && Co_So && Co_Ky_Tu_Dac_Biet;
        }

        private void btn_xacnhan_Click(object sender, EventArgs e)
        {
            string Username = tb_username.Text.Trim();
            string Password = tb_matkhau.Text.Trim();
            string Confirm_Pass = tb_xacnhanmk.Text.Trim();
            string Email = tb_email.Text.Trim();
            string Birthday = tb_birthday.Text.Trim();

            if (Username == "" || Password == "" || Email == "")
            {
                MessageBox.Show("Please Enter Full Information!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Birthday = Dinh_Dang_Ngay(Birthday);
            if (Birthday == null)
            {
                MessageBox.Show("Invalid Birthday Format Entered", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool Is_Email = Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$");
            if (!Is_Email)
            {
                MessageBox.Show("Invalid Email!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Kiem_Tra_Do_Manh_Mat_Khau(Password))
            {
                MessageBox.Show("Password Must Have At Least 8 Characters, Including Uppercase, Lowercase, Numbers And Special Characters!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Password != Confirm_Pass)
            {
                MessageBox.Show("Passwords Do Not Match!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show("Security Setup Error! Cannot Continue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Network_Handle.Close_Connection();
                        return;
                    }

                    Network_Handle.Start_Listening();
                }

                string Response_Key = Network_Handle.Send_And_Wait(new
                {
                    action = "get_public_key"
                });

                var Json_Key = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Response_Key);
                string Public_Key = Json_Key["public_key"].GetString();
                string Secure_Pass = RSA_Handle.Encrypt(Password, Public_Key);

                if (Secure_Pass == null)
                {
                    MessageBox.Show("Password Encryption Error!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var Request = new
                {
                    action = "register",
                    email = Email,
                    username = Username,
                    password = Secure_Pass,
                    birthday = Birthday
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
                    DialogResult Result = MessageBox.Show("Registration Successful!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (Result == DialogResult.OK)
                    {
                        Form_Dang_Nhap Form_Dang_Nhap = new Form_Dang_Nhap();
                        Form_Dang_Nhap.Show();
                        this.Close();
                    }
                }
                else
                {
                    string Msg = Dict.ContainsKey("message") ? Dict["message"] : "Registration Failed!";
                    MessageBox.Show(Msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (JsonException)
            {
                MessageBox.Show("Response Data From Server Is Invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SocketException)
            {
                MessageBox.Show("Cannot Connect To Server. Check IP And Port!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error: " + Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_backdn_Click(object sender, EventArgs e)
        {
            Form_Dang_Nhap Dn = Application.OpenForms.OfType<Form_Dang_Nhap>().FirstOrDefault();

            if (Dn != null)
            {
                Dn.Show();
            }
            else
            {
                Dn = new Form_Dang_Nhap();
                Dn.Show();
            }
            this.Hide();
        }
    }
}