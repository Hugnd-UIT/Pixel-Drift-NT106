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

        // Hàm chuẩn hóa định dạng ngày
        private string Dinh_Dang_Ngay(string Day)
        {
            if (DateTime.TryParse(Day, CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime Parsed_Day))
                return Parsed_Day.ToString("yyyy-MM-dd");
            return null;
        }

        // Kiểm tra độ mạnh của mật khẩu
        private bool Kiem_Tra_Do_Manh_Mat_Khau(string Password)
        {
            if (Password.Length < 8) return false;
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

            // Kiểm tra dữ liệu đầu vào
            if (Username == "" || Password == "" || Email == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Birthday = Dinh_Dang_Ngay(Birthday);
            if (Birthday == null)
            {
                MessageBox.Show("Nhập sai định dạng ngày sinh nhật");
                return;
            }

            bool Is_Email = Regex.IsMatch(Email, @"^[a-zA-Z0-9._%+-]+@gmail\.com$");
            if (!Is_Email)
            {
                MessageBox.Show("Email không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!Kiem_Tra_Do_Manh_Mat_Khau(Password))
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (Password != Confirm_Pass)
            {
                MessageBox.Show("Mật khẩu không khớp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    action = "register",
                    email = Email,
                    username = Username,
                    password = Secure_Pass,
                    birthday = Birthday
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
                    DialogResult Result = MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (Result == DialogResult.OK)
                    {
                        Form_Dang_Nhap Form_Dang_Nhap = new Form_Dang_Nhap();
                        Form_Dang_Nhap.Show();
                        this.Close();
                    }
                }
                else
                {
                    string Msg = Dict.ContainsKey("message") ? Dict["message"] : "Đăng ký thất bại!";
                    MessageBox.Show(Msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (JsonException)
            {
                MessageBox.Show("Dữ liệu phản hồi từ server không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (SocketException)
            {
                MessageBox.Show("Không thể kết nối đến server. Kiểm tra IP và cổng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Lỗi: " + Ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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