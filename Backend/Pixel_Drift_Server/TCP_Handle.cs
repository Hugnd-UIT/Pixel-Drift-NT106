using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;
using BCrypt.Net;
using Pixel_Drift;
using System.Linq;

namespace Pixel_Drift_Server
{
    public class TCP_Handler
    {
        private TcpClient Client;
        private NetworkStream Stream;

        private string Session_AES_Key = null;
        private static Dictionary<string, int> Login_Attempts = new Dictionary<string, int>();
        private static Dictionary<string, DateTime> IP_Block_List = new Dictionary<string, DateTime>();

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

        private string Get_Client_IP()
        {
            try
            {
                if (Client != null && Client.Client != null && Client.Client.RemoteEndPoint != null)
                {
                    return ((IPEndPoint)Client.Client.RemoteEndPoint).Address.ToString();
                }
            }
            catch { }
            return "Unknown";
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
                Console.WriteLine($"[Error] Server Start Failed: {ex.Message}");
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
                    string IP = Get_Client_IP();

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
                            Security_Logger.Log(Security_Logger.Level.ALERT, IP, "FAILED", "Client Is Spamming Dropping Packet");
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
                            Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", "Decryption Failed");
                            continue;
                        }
                    }

                    try
                    {
                        var Data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Json);
                        if (!Data.ContainsKey("action")) continue;

                        string Action = Data["action"].GetString();
                        string Response = null;

                        if (Action != "move" && Action != "ping")
                            Console.WriteLine($"[TCP] {Client.Client.RemoteEndPoint}: {Action}");

                        switch (Action)
                        {
                            case "get_aes_key":
                                Response = Handle_Get_AES(Data);
                                break;

                            case "get_public_key":
                                string Key = JsonSerializer.Serialize(new { status = "success", public_key = Program.SERVER_PUBLIC_KEY });
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
                            case "move":
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
                        Console.WriteLine($"[Error] Processing Data: {ex.Message}");
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
                Console.WriteLine($"[Error] Send Message Failed: {ex.Message}");
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
                    Security_Logger.Log(Security_Logger.Level.INFO, Get_Client_IP(), "SUCCESS", "Secure Connection Established");
                    return JsonSerializer.Serialize(new { status = "success", message = "Secure Connection Established" });
                }
            }
            catch { }
            Security_Logger.Log(Security_Logger.Level.WARNING, Get_Client_IP(), "FAILED", "Key Exchange Error");
            return JsonSerializer.Serialize(new { status = "error", message = "Key Exchange Error" });
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
            string IP = Get_Client_IP();

            if (IP_Block_List.ContainsKey(IP))
            {
                if (DateTime.Now < IP_Block_List[IP])
                {
                    TimeSpan Remaining_Time = IP_Block_List[IP] - DateTime.Now;
                    Security_Logger.Log(Security_Logger.Level.ALERT, IP, "FAILED", $"Ip Address Is Blocked To Prevent Brute Force Remaining {Remaining_Time.TotalSeconds:F0}s");
                    return JsonSerializer.Serialize(new { status = "error", message = $"You Are Temporarily Blocked. Please Try Again In {Math.Ceiling(Remaining_Time.TotalMinutes)} Minutes." });
                }
                else
                {
                    IP_Block_List.Remove(IP);
                    if (Login_Attempts.ContainsKey(IP)) Login_Attempts.Remove(IP);
                }
            }

            if (!Data.ContainsKey("username") || !Data.ContainsKey("password"))
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Incomplete Data" });
            }

            string User = Data["username"].GetString();
            string Encrypted_Pass = Data["password"].GetString();

            string Decrypted_Pass = RSA_Handle.Decrypt(Encrypted_Pass, Program.SERVER_PRIVATE_KEY);
            if (Decrypted_Pass == null)
            {
                Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", "Login Packet Decryption Failed");
                return JsonSerializer.Serialize(new { status = "error", message = "Security Error: Invalid Packet" });
            }

            if (SQL_Handle.Handle_Login(User, Decrypted_Pass))
            {
                string Email = SQL_Handle.Handle_Get_Email(User);

                lock (Active_Connections)
                {
                    if (Email != null)
                    {
                        if (Active_Connections.ContainsKey(Email))
                        {
                            try
                            {
                                var Old_Client = Active_Connections[Email];
                                if (Old_Client.Connected)
                                {
                                    string Kick_Msg = JsonSerializer.Serialize(new { status = "force_logout", message = "Account Logged In From Another Location" });
                                    byte[] Bytes = Encoding.UTF8.GetBytes(Kick_Msg + "\n");
                                    Old_Client.GetStream().Write(Bytes, 0, Bytes.Length);
                                    Old_Client.Close();
                                }
                            }
                            catch { }
                            Active_Connections.Remove(Email);
                        }
                        Active_Connections[Email] = Client;
                    }
                }

                if (Login_Attempts.ContainsKey(IP)) Login_Attempts.Remove(IP);
                if (IP_Block_List.ContainsKey(IP)) IP_Block_List.Remove(IP);

                Security_Logger.Log(Security_Logger.Level.INFO, IP, "SUCCESS", $"User {User} Logged In");
                return JsonSerializer.Serialize(new { status = "success", message = "Login Success", username = User });
            }
            else
            {
                if (!Login_Attempts.ContainsKey(IP)) Login_Attempts[IP] = 0;
                Login_Attempts[IP]++;

                int Max_Attempts = 5;

                if (Login_Attempts[IP] >= Max_Attempts)
                {
                    IP_Block_List[IP] = DateTime.Now.AddMinutes(5);
                    Security_Logger.Log(Security_Logger.Level.ALERT, IP, "FAILED", "Ip Address Is Blocked To Prevent Brute Force");
                    return JsonSerializer.Serialize(new { status = "error", message = "Too Many Failed Attempts. You Are Blocked For 5 Minutes." });
                }
                else
                {
                    int Remaining = Max_Attempts - Login_Attempts[IP];
                    Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", $"Wrong Password For User {User}");
                    return JsonSerializer.Serialize(new { status = "error", message = $"Invalid Username Or Password. {Remaining} Attempts Remaining." });
                }
            }
        }

        private string Handle_Register(Dictionary<string, JsonElement> Data)
        {
            string IP = Get_Client_IP();
            string Username = Data["username"].GetString();
            string Email = Data["email"].GetString();
            string Birthday = Data.ContainsKey("birthday") ? Data["birthday"].GetString() : "";
            string Encrypted_Pass = Data["password"].GetString();
            string Decrypted_Pass = RSA_Handle.Decrypt(Encrypted_Pass, Program.SERVER_PRIVATE_KEY);

            if (Decrypted_Pass == null)
            {
                Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", "Register Packet Decryption Failed");
                return JsonSerializer.Serialize(new { status = "error", message = "Security Error: Invalid Packet" });
            }

            int Result = SQL_Handle.Handle_Register(Username, Email, Decrypted_Pass, Birthday);

            if (Result == 1)
            {
                Security_Logger.Log(Security_Logger.Level.INFO, IP, "SUCCESS", $"New User Registered {Username} With Email {Email}");
                return JsonSerializer.Serialize(new { status = "success", message = "Registration Successful" });
            }
            if (Result == -1)
            {
                Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", $"Duplicate Username Or Email Attempt For {Username}");
                return JsonSerializer.Serialize(new { status = "error", message = "Username Or Email Already Exists" });
            }
            return JsonSerializer.Serialize(new { status = "error", message = "Registration System Error" });
        }

        private string Handle_Get_Info(Dictionary<string, JsonElement> Data)
        {
            string User = Data["username"].GetString();
            string Info = SQL_Handle.Handle_Get_Info(User);

            if (!string.IsNullOrEmpty(Info))
            {
                var Parts = Info.Split('|');
                Console.WriteLine($"[Info] Retrieved Info For User: {User}");
                return JsonSerializer.Serialize(new
                {
                    status = "success",
                    username = User,
                    email = Parts[0],
                    birthday = Parts[1]
                });
            }
            return JsonSerializer.Serialize(new { status = "error", message = "User Not Found" });
        }

        private string Handle_Forgot_Password(Dictionary<string, JsonElement> Data)
        {
            string IP = Get_Client_IP();
            string Email = Data["email"].GetString();
            string Username = SQL_Handle.Handle_Get_Username(Email);

            if (!string.IsNullOrEmpty(Username))
            {
                string Token = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                lock (Reset_Tokens)
                {
                    Reset_Tokens[Email] = Token;
                }

                bool Sent = Send_Email(Email, "Reset Password - Pixel Drift", $"Hello {Username}, your verification code is: {Token}");

                if (Sent)
                {
                    Security_Logger.Log(Security_Logger.Level.INFO, IP, "SUCCESS", $"Reset Code Sent To {Email}");
                    return JsonSerializer.Serialize(new { status = "success", message = "Verification Code Sent Via Email" });
                }

                Console.WriteLine($"[Error] Failed To Send Email To {Email}");
                return JsonSerializer.Serialize(new { status = "error", message = "Error Sending Email" });
            }

            Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", $"Invalid Email Requested {Email}");
            return JsonSerializer.Serialize(new { status = "error", message = "Email Does Not Exist In The System" });
        }

        private string Handle_Change_Password(Dictionary<string, JsonElement> Data)
        {
            string IP = Get_Client_IP();
            string Email = Data["email"].GetString();
            string Token = Data["token"].GetString();
            string Encrypted_Pass = Data["new_password"].GetString();
            string Decrypted_Pass = RSA_Handle.Decrypt(Encrypted_Pass, Program.SERVER_PRIVATE_KEY);

            if (Decrypted_Pass == null)
            {
                return JsonSerializer.Serialize(new { status = "error", message = "Security Error: Invalid Packet" });
            }

            lock (Reset_Tokens)
            {
                if (Reset_Tokens.ContainsKey(Email) && Reset_Tokens[Email] == Token)
                {
                    if (SQL_Handle.Handle_Change_Password(Email, Decrypted_Pass))
                    {
                        Reset_Tokens.Remove(Email);
                        Security_Logger.Log(Security_Logger.Level.CRITICAL, IP, "SUCCESS", $"Password For Email {Email} Has Been Changed");
                        return JsonSerializer.Serialize(new { status = "success", message = "Password Changed Successfully" });
                    }
                }
            }

            Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", $"Invalid Token Attempt For {Email}");
            return JsonSerializer.Serialize(new { status = "error", message = "Invalid Or Expired Verification Code" });
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
                Console.WriteLine($"[Success] User {User} Created Room {Room_ID}");
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
                        Console.WriteLine($"[Success] User {User} Joined Room {Room_ID}");
                        return JsonSerializer.Serialize(new { status = "join_room_success", room_id = Room_ID, player_number = P_Num });
                    }
                    return JsonSerializer.Serialize(new { status = "error", message = "Room Is Full" });
                }
            }
            return JsonSerializer.Serialize(new { status = "error", message = "Room Not Found" });
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
                Console.WriteLine($"[TCP] Client {Client.Client.RemoteEndPoint} Disconnected");

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
                                Console.WriteLine($"[Success] Room {Room_ID} Has Been Cancelled");
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
                    Console.WriteLine($"[Error] Close Client Error: {ex.Message}");
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
                    Console.WriteLine("[Error] Mail Account Or Mail Password Hasn't Been Set Up!");
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
                Console.WriteLine($"[Error] Email Send Error: {Ex.Message}");
                return false;
            }
        }
    }
}