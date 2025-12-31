using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.Json; 
using System.IO;
using BCrypt.Net;
using Pixel_Drift;

namespace Pixel_Drift_Server
{
    public class TCP_Handler
    {
        private TcpClient Client;
        private NetworkStream Stream;

        private string Session_AES_Key = null;

        private int Request_Count = 0;
        private const int MAX_REQUESTS_PER_SECOND = 20;
        private DateTime Last_Request_Time = DateTime.Now;

        private static Dictionary<string, TcpClient> Active_Connections = new Dictionary<string, TcpClient>();
        private static Dictionary<TcpClient, string> Client_Room_Map = new Dictionary<TcpClient, string>();
        private static Dictionary<string, string> Reset_Tokens = new Dictionary<string, string>();

        public TCP_Handler(TcpClient TCP_Client)
        {
            this.Client = TCP_Client;
            this.Client.ReceiveTimeout = 10000;
            this.Stream = TCP_Client.GetStream();
        }

        public static void Start(int port)
        {
            try
            {
                TcpListener TCP_Listener = new TcpListener(IPAddress.Any, port);
                TCP_Listener.Start();
                Console.WriteLine($"[TCP] Server Is Listening On Port {port}...");

                while (true)
                {
                    TcpClient TCP_Client = TCP_Listener.AcceptTcpClient();
                    Console.WriteLine($"[TCP] Client Connected: {TCP_Client.Client.RemoteEndPoint}");
                    Task.Run(() => new TCP_Handler(TCP_Client).Process());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }
        }

        public void Process()
        {
            try
            {
                StreamReader Reader = new StreamReader(Stream, Encoding.UTF8);
                
                string Message;

                while ((Message = Reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(Message)) continue;

                    TimeSpan Diff = DateTime.Now - Last_Request_Time;
                    if (Diff.TotalSeconds >= 1)
                    {
                        Last_Request_Time = DateTime.Now;
                        Request_Count = 0;
                    }
                    else
                    {
                        Request_Count++;
                        if (Request_Count > MAX_REQUESTS_PER_SECOND)
                        {
                            Console.WriteLine($"[Warning] {Client.Client.RemoteEndPoint} Is Spamming! Dropping Packet!");
                            break;
                        }
                    }

                    string Json = Message;

                    if (!string.IsNullOrEmpty(Session_AES_Key) && !Message.Trim().StartsWith("{"))
                    {
                        string Decrypted = AES_Handle.Decrypt(Message, Session_AES_Key);
                        if (Decrypted != null)
                        {
                            Json = Decrypted;
                        }
                        else
                        {
                            Console.WriteLine("[Warning] Decryption Failed From " + Client.Client.RemoteEndPoint);
                            continue; 
                        }
                    }

                    try
                    {
                        var Data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Json);
                        if (!Data.ContainsKey("action")) continue;

                        string Action = Data["action"].GetString();
                        string Response = null;

                        if (Action != "move" && Action != "ping") Console.WriteLine($"[TCP] {Client.Client.RemoteEndPoint}: {Action}");

                        switch (Action)
                        {
                            case "get_aes_key":
                                Response = Handle_Get_AES(Data);
                                break;

                            case "get_public_key":
                                string Key = JsonSerializer.Serialize(new
                                {
                                    status = "success",
                                    public_key = Program.SERVER_PUBLIC_KEY
                                });
                                Send_Message(Key);
                                break;

                            case "login":
                                Response = Handle_Login(Data);
                                break;

                            case "register":
                                Response = Handle_Register(Data);
                                break;

                            case "get_info":
                                Response = Handle_Get_Info(Data);
                                break;

                            case "forgot_password":
                                Response = Handle_Forgot_Password(Data);
                                break;

                            case "change_password":
                                Response = Handle_Change_Password(Data);
                                break;

                            case "create_room":
                                Response = Handle_Create_Room(Data);
                                break;

                            case "join_room":
                                Response = Handle_Join_Room(Data);
                                break;

                            case "get_scoreboard":
                                Response = Handle_Get_Scoreboard(Data);
                                break;

                            case "search_player":
                                Response = Handle_Search_Player(Data);
                                break;

                            case "set_ready":
                                Handle_Game_Action(Data, Action);
                                break;

                            case "move":
                                Handle_Game_Action(Data, Action);
                                break;

                            case "leave_room":
                                Handle_Game_Action(Data, Action);
                                break;

                            case "ping":
                                break;
                        }

                        if (Response != null)
                        {
                            Send_Message(Response);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[Error] {ex.Message}");
                    }
                }
            }
            catch 
            {
                
            }
            finally
            {
                Handle_Disconnect();
            }
        }

        private void Send_Message(string Message)
        {
            try
            {
                if (Client.Connected && Stream.CanWrite)
                {
                    if (!string.IsNullOrEmpty(Session_AES_Key))
                    {
                        Message = AES_Handle.Encrypt(Message, Session_AES_Key);
                    }

                    byte[] Buffer = Encoding.UTF8.GetBytes(Message + "\n");
                    Stream.Write(Buffer, 0, Buffer.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }
        }

        private string Handle_Get_AES(Dictionary<string, JsonElement> Data)
        {
            try
            {
                string Encrypted_Key = Data["aes_key"].GetString();

                string Decrypted_Key = RSA_Handle.Decrypt(Encrypted_Key, Program.SERVER_PRIVATE_KEY);

                if (Decrypted_Key != null && Decrypted_Key.Length == 32)
                {
                    this.Session_AES_Key = Decrypted_Key;
                    return JsonSerializer.Serialize(new { status = "success", message = "Kết nối bảo mật đã thiết lập" });
                }
            }
            catch { }
            return JsonSerializer.Serialize(new { status = "error", message = "Lỗi trao đổi khóa" });
        }

        private void Handle_Game_Action(Dictionary<string, JsonElement> Data, string Action)
        {
            if (Client_Room_Map.ContainsKey(Client))
            {
                string Room_ID = Client_Room_Map[Client];
                lock (Program.Rooms)
                {
                    if (Program.Rooms.ContainsKey(Room_ID))
                    {
                        Program.Rooms[Room_ID].Handle_Input(Client, Action, Data);
                    }
                }
            }
        }

        private string Handle_Login(Dictionary<string, JsonElement> Data)
        {
            string User = Data["username"].GetString();
            string Encrypted_Pass = Data["password"].GetString();
            string Decrypted_Pass = RSA_Handle.Decrypt(Encrypted_Pass, Program.SERVER_PRIVATE_KEY);

            if (Decrypted_Pass == null)
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Lỗi bảo mật: Gói tin không hợp lệ." });
            }

            if (SQL_Handle.Handle_Login(User, Decrypted_Pass))
            {
                string Email = SQL_Handle.Handle_Get_Email(User);

                lock (Active_Connections)
                {
                    if (Email != null && Active_Connections.ContainsKey(Email))
                    {
                        try
                        {
                            var Old_Client = Active_Connections[Email];
                            string Kick_Msg = JsonSerializer.Serialize(new { status = "force_logout", message = "Tài khoản đăng nhập nơi khác" });
                            byte[] Bytes = Encoding.UTF8.GetBytes(Kick_Msg + "\n");
                            Old_Client.GetStream().Write(Bytes, 0, Bytes.Length);
                            Old_Client.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[Error] {ex.Message}");
                        }
                        Active_Connections.Remove(Email);
                    }
                    if (Email != null) Active_Connections[Email] = Client;
                }

                return JsonSerializer.Serialize(new { status = "success", message = "Login success", username = User });
            }
            return JsonSerializer.Serialize(new { status = "error", message = "Sai tài khoản hoặc mật khẩu" });
        }

        private string Handle_Register(Dictionary<string, JsonElement> Data)
        {
            string Username = Data["username"].GetString();
            string Email = Data["email"].GetString();
            string Birthday = Data.ContainsKey("birthday") ? Data["birthday"].GetString() : "";
            string Encrypted_Pass = Data["password"].GetString();
            string Decrypted_Pass = RSA_Handle.Decrypt(Encrypted_Pass, Program.SERVER_PRIVATE_KEY);

            if (Decrypted_Pass == null)
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Lỗi bảo mật: Gói tin không hợp lệ." });
            }

            int Result = SQL_Handle.Handle_Register(Username, Email, Decrypted_Pass, Birthday);

            if (Result == 1) return JsonSerializer.Serialize(new { status = "success", message = "Đăng ký thành công" });
            if (Result == -1) return JsonSerializer.Serialize(new { status = "error", message = "Username hoặc Email đã tồn tại" });
            return JsonSerializer.Serialize(new { status = "error", message = "Lỗi hệ thống đăng ký" });
        }

        private string Handle_Get_Info(Dictionary<string, JsonElement> Data)
        {
            string Info = SQL_Handle.Handle_Get_Info(Data["username"].GetString());
            if (!string.IsNullOrEmpty(Info))
            {
                var Parts = Info.Split('|');
                return JsonSerializer.Serialize(new
                {
                    status = "success",
                    username = Data["username"].GetString(),
                    email = Parts[0],
                    birthday = Parts[1]
                });
            }
            return JsonSerializer.Serialize(new { status = "error", message = "Không tìm thấy user" });
        }

        private string Handle_Forgot_Password(Dictionary<string, JsonElement> Data)
        {
            string Email = Data["email"].GetString();
            string Username = SQL_Handle.Handle_Get_Username(Email);

            if (!string.IsNullOrEmpty(Username))
            {
                string Token = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                lock (Reset_Tokens)
                {
                    Reset_Tokens[Email] = Token;
                }

                bool Sent = Send_Email(Email, "Reset Password - Pixel Drift", $"Chào {Username}, mã xác nhận của bạn là: {Token}");

                if (Sent) return JsonSerializer.Serialize(new { status = "success", message = "Đã gửi mã xác nhận qua Email" });
                return JsonSerializer.Serialize(new { status = "error", message = "Lỗi gửi mail" });
            }
            return JsonSerializer.Serialize(new { status = "error", message = "Email không tồn tại trong hệ thống" });
        }

        private string Handle_Change_Password(Dictionary<string, JsonElement> Data)
        {
            string Email = Data["email"].GetString();
            string Token = Data["token"].GetString();
            string Encrypted_Pass = Data["new_password"].GetString();
            string Decrypted_Pass = RSA_Handle.Decrypt(Encrypted_Pass, Program.SERVER_PRIVATE_KEY);

            if (Decrypted_Pass == null)
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Lỗi bảo mật: Gói tin không hợp lệ." });
            }

            lock (Reset_Tokens)
            {
                if (Reset_Tokens.ContainsKey(Email) && Reset_Tokens[Email] == Token)
                {
                    if (SQL_Handle.Handle_Change_Password(Email, Decrypted_Pass))
                    {
                        Reset_Tokens.Remove(Email);
                        return JsonSerializer.Serialize(new { status = "success", message = "Đổi mật khẩu thành công" });
                    }
                }
            }
            return JsonSerializer.Serialize(new { status = "error", message = "Mã xác nhận sai hoặc hết hạn" });
        }

        private string Handle_Create_Room(Dictionary<string, JsonElement> Data)
        {
            string User = Data["username"].GetString();
            string Room_ID = new Random().Next(100000, 999999).ToString();

            lock (Program.Rooms)
            {
                while (Program.Rooms.ContainsKey(Room_ID))
                {
                    Room_ID = new Random().Next(100000, 999999).ToString();
                }

                Game_Room New_Room = new Game_Room(Room_ID);
                int P_Num = New_Room.Add_Player(Client, User);

                Program.Rooms.Add(Room_ID, New_Room);
                Client_Room_Map[Client] = Room_ID;

                Console.WriteLine($"[Success] {User} Create Room {Room_ID}");
                return JsonSerializer.Serialize(new { status = "create_room_success", room_id = Room_ID, player_number = P_Num });
            }
        }

        private string Handle_Join_Room(Dictionary<string, JsonElement> Data)
        {
            string User = Data["username"].GetString();
            string Room_ID = Data["room_id"].GetString();

            lock (Program.Rooms)
            {
                if (Program.Rooms.ContainsKey(Room_ID))
                {
                    Game_Room Room = Program.Rooms[Room_ID];
                    int P_Num = Room.Add_Player(Client, User);

                    if (P_Num != -1)
                    {
                        Client_Room_Map[Client] = Room_ID;
                        Console.WriteLine($"[Success] {User} Join Room {Room_ID}");
                        return JsonSerializer.Serialize(new { status = "join_room_success", room_id = Room_ID, player_number = P_Num });
                    }
                    return JsonSerializer.Serialize(new { status = "error", message = "Phòng đã đầy" });
                }
            }
            return JsonSerializer.Serialize(new { status = "error", message = "Không tìm thấy phòng" });
        }

        private string Handle_Get_Scoreboard(Dictionary<string, JsonElement> Data)
        {
            int Limit = Data.ContainsKey("top_count") ? Data["top_count"].GetInt32() : 50;
            string Board = SQL_Handle.Handle_Get_Scoreboard(Limit);
            return JsonSerializer.Serialize(new { action = "scoreboard_data", data = Board });
        }

        private string Handle_Search_Player(Dictionary<string, JsonElement> Data)
        {
            string Keyword = Data["search_text"].GetString();
            string Result = SQL_Handle.Handle_Search_Player(Keyword);
            return JsonSerializer.Serialize(new { action = "search_result", data = Result });
        }

        private void Handle_Disconnect()
        {
            try
            {
                Console.WriteLine($"[TCP] Client {Client.Client.RemoteEndPoint} Disconnected.");

                if (Client_Room_Map.ContainsKey(Client))
                {
                    string Room_ID = Client_Room_Map[Client];
                    lock (Program.Rooms)
                    {
                        if (Program.Rooms.ContainsKey(Room_ID))
                        {
                            Program.Rooms[Room_ID].Remove_Player(Client);
                            if (Program.Rooms[Room_ID].Is_Empty())
                            {
                                Program.Rooms.Remove(Room_ID);
                                Console.WriteLine($"[Success] Room {Room_ID} Has Been Cancelled.");
                            }
                        }
                    }
                    Client_Room_Map.Remove(Client);
                }

                lock (Active_Connections)
                {
                    string Key_To_Remove = null;
                    foreach (var kvp in Active_Connections)
                    {
                        if (kvp.Value == Client)
                        {
                            Key_To_Remove = kvp.Key;
                            break;
                        }
                    }
                    if (Key_To_Remove != null) Active_Connections.Remove(Key_To_Remove);
                }

                try
                {
                    Client?.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Error] {ex.Message}");
                }
            }
            catch { }
        }

        private bool Send_Email(string To, string Subject, string Body)
        {
            try
            {
                string From_Email = Environment.GetEnvironmentVariable("MAIL_ACCOUNT");
                string From_Pass = Environment.GetEnvironmentVariable("MAIL_PASSWORD");

                if (string.IsNullOrEmpty(From_Email) || string.IsNullOrEmpty(From_Pass))
                {
                    Console.WriteLine("[Error] MAIL_ACCOUNT Or MAIL_PASSWORD Hasn't Been Set Up!");
                    return false;
                }

                MailMessage Mail = new MailMessage();
                Mail.From = new MailAddress(From_Email);
                Mail.To.Add(To);
                Mail.Subject = Subject;
                Mail.Body = Body;

                SmtpClient Smtp = new SmtpClient("smtp.gmail.com", 587);
                Smtp.Credentials = new NetworkCredential(From_Email, From_Pass);
                Smtp.EnableSsl = true;
                Smtp.Send(Mail);
                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"[Error] {Ex.Message}");
                return false;
            }
        }
    }
}