using BCrypt.Net;
using Pixel_Drift;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pixel_Drift_Server
{
    public class TCP_Handler
    {
        private TcpClient Client;
        private NetworkStream Stream;

        private string Session_Key = null;
        private const int Max_Connect_Per_IP = 3;
        private Dictionary<string, DateTime> Action_Cooldowns_Per_IP = new Dictionary<string, DateTime>();
        private static ConcurrentDictionary<string, int> Attempts_Per_IP = new ConcurrentDictionary<string, int>();
        private static ConcurrentDictionary<string, int> Connections_Per_IP = new ConcurrentDictionary<string, int>();
        private static ConcurrentDictionary<string, int> SoftBan_Count_Per_IP = new ConcurrentDictionary<string, int>();
        private static ConcurrentDictionary<string, DateTime> Ban_Peroid_Per_IP = new ConcurrentDictionary<string, DateTime>();

        private int Request_Count = 0;
        private DateTime Request_Time = DateTime.Now;
        private const int Max_Requests_Per_Second = 40;

        private static ConcurrentDictionary<string, TcpClient> Client_By_Email = new ConcurrentDictionary<string, TcpClient>();
        private static ConcurrentDictionary<TcpClient, string> Client_By_Room = new ConcurrentDictionary<TcpClient, string>();
        private static ConcurrentDictionary<string, string> Token_By_Email = new ConcurrentDictionary<string, string>();

        public TCP_Handler(TcpClient TCP_Client)
        {
            this.Client = TCP_Client;
            this.Client.ReceiveTimeout = 5000;
            this.Client.SendTimeout = 5000;
            this.Stream = TCP_Client.GetStream();
        }

        private string Handle_Get_IP()
        {
            try
            {
                if (Client != null && Client.Client != null && Client.Client.RemoteEndPoint != null)
                {
                    return ((IPEndPoint)Client.Client.RemoteEndPoint).Address.ToString();
                }
            }
            catch
            {
                // Continue
            }
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
                    string IP = ((IPEndPoint)TCP_Client.Client.RemoteEndPoint).Address.ToString();

                    if (Ban_Peroid_Per_IP.TryGetValue(IP, out DateTime Ban_Time))
                    {
                        if (Ban_Peroid_Per_IP.ContainsKey(IP))
                        {
                            if (DateTime.Now < Ban_Peroid_Per_IP[IP])
                            {
                                TCP_Client.Close();
                                continue; 
                            }
                            else
                            {
                                Ban_Peroid_Per_IP.TryRemove(IP, out _);
                            }
                        }
                    }

                    int Current_Connenctions = Connections_Per_IP.AddOrUpdate(IP, 1, (Key, Old_Value) => Old_Value + 1);

                    if (Connections_Per_IP.ContainsKey(IP) && Connections_Per_IP[IP] >= Max_Connect_Per_IP)
                    {
                        DateTime Ban_Until = DateTime.Now.AddMinutes(5);
                        Ban_Peroid_Per_IP.AddOrUpdate(IP, Ban_Until, (Key, Old_Value) => Ban_Until);

                        Security_Logger.Log(Security_Logger.Level.ALERT, IP, "BLOCKED", "DoS Detected! Too Many Concurrent Connections");
                        SQL_Handle.Handle_Add_Blacklist(IP, "DDoS");
                        OS_Handle.Handle_Block(IP);

                        try
                        {
                            TCP_Client.Close();
                            Console.WriteLine($"[TCP] Client {IP} Disconnected");
                        }
                        catch
                        {
                            // Continue
                        }
                        continue;
                    }

                    Task.Run(() => new TCP_Handler(TCP_Client).Process());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Server Start Failed: {ex.Message}");
            }
        }

        private bool Handle_Spam_Cooldown(string Action_Name, int Cooldown_Milliseconds)
        {
            if (Action_Cooldowns_Per_IP.TryGetValue(Action_Name, out DateTime Allowed_Time))
            {
                if (DateTime.Now < Allowed_Time)
                {
                    return false; 
                }
            }
            Action_Cooldowns_Per_IP[Action_Name] = DateTime.Now.AddMilliseconds(Cooldown_Milliseconds);
            return true; 
        }

        public void Process()
        {
            try
            {
                StreamReader Reader = new StreamReader(Stream, Encoding.UTF8);
                bool Is_Handshake = true;
                string Message;

                while ((Message = Reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(Message))
                    {
                        continue;
                    }

                    string IP = Handle_Get_IP();

                    if (Message.Length > 4096)
                    {
                        DateTime Ban_Time = DateTime.Now.AddMinutes(5);
                        Ban_Peroid_Per_IP.AddOrUpdate(IP, Ban_Time, (Key, Old_Value) => Ban_Time);
                        
                        Security_Logger.Log(Security_Logger.Level.ALERT, IP, "BLOCKED", $"Buffer Overflow Detected! Packet Size {Message.Length} > 4096 Bytes");
                        SQL_Handle.Handle_Add_Blacklist(IP, "Buffer Overflow");
                        OS_Handle.Handle_Block(IP);
                       
                        break;
                    }

                    TimeSpan Diff = DateTime.Now - Request_Time;
                    if (Diff.TotalSeconds >= 1)
                    {
                        Request_Time = DateTime.Now;
                        Request_Count = 0;
                    }
                    else
                    {
                        Request_Count++;
                        if (Request_Count > Max_Requests_Per_Second)
                        {
                            DateTime Ban_Time = DateTime.Now.AddMinutes(5);
                            Ban_Peroid_Per_IP.AddOrUpdate(IP, Ban_Time, (Key, Old_Value) => Ban_Time);

                            Security_Logger.Log(Security_Logger.Level.ALERT, IP, "BLOCKED", "Spam Detected!");
                            SQL_Handle.Handle_Add_Blacklist(IP, "Spam");
                            OS_Handle.Handle_Block(IP);

                            break;
                        }
                    }

                    string Json = Message;
                    if (!string.IsNullOrEmpty(Session_Key) && !Message.Trim().StartsWith("{"))
                    {
                        string Decrypted = AES_Handle.Decrypt(Message, Session_Key);
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

                        if (Data.ContainsKey("timestamp"))
                        {
                            long Client_Ticks = Data["timestamp"].GetInt64();
                            double Diff_Seconds = Math.Abs(TimeSpan.FromTicks(DateTime.UtcNow.Ticks - Client_Ticks).TotalSeconds);

                            if (Diff_Seconds > 5.0)
                            {
                                DateTime Ban_Time = DateTime.Now.AddMinutes(5);
                                Ban_Peroid_Per_IP.AddOrUpdate(IP, Ban_Time, (Key, Old_Value) => Ban_Time);

                                Security_Logger.Log(Security_Logger.Level.ALERT, IP, "BLOCKED", $"Replay Detected! Delay: {Diff_Seconds:F2}s");
                                SQL_Handle.Handle_Add_Blacklist(IP, "Replay Attack");
                                OS_Handle.Handle_Block(IP);

                                break;
                            }
                        }

                        if (!Data.ContainsKey("action"))
                        {
                            continue;
                        }

                        string Action = Data["action"].GetString();
                        string Response = null;

                        if (Is_Handshake)
                        {
                            if (Action != "get_public_key")
                            {
                                Security_Logger.Log(Security_Logger.Level.WARNING, IP, "KICKED", $"Protocol Violation Detected! First Packet Was '{Action}'");
                                break; 
                            }
                            Is_Handshake = false;
                        }

                        if (Action != "move" && Action != "ping")
                        {
                            Console.WriteLine($"[TCP] {Client.Client.RemoteEndPoint}: {Action}");
                        }

                        switch (Action)
                        {
                            case "get_session_key":
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
                                if (!Handle_Spam_Cooldown("create_room", 2000))
                                {
                                    Send_Message(JsonSerializer.Serialize(new { status = "error", message = "Too Fast! Please Wait 2 Seconds." }));
                                    break;
                                }
                                Response = Handle_Create_Room(Data);
                                break;

                            case "join_room":
                                if (!Handle_Spam_Cooldown("join_room", 1000))
                                {
                                    Send_Message(JsonSerializer.Serialize(new { status = "error", message = "Too Fast! Please Wait 1 Seconds." }));
                                    break;
                                }
                                Response = Handle_Join_Room(Data);
                                break;

                            case "get_scoreboard":
                                if (!Handle_Spam_Cooldown("db_query", 1000))
                                {
                                    break;
                                }
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
                // Continue
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
                    if (!string.IsNullOrEmpty(Session_Key))
                    {
                        Message = AES_Handle.Encrypt(Message, Session_Key);
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
                string Encrypted_Key = Data["session_key"].GetString();
                string Decrypted_Key = RSA_Handle.Decrypt(Encrypted_Key, Program.SERVER_PRIVATE_KEY);

                if (Decrypted_Key != null && Decrypted_Key.Length == 32)
                {
                    this.Session_Key = Decrypted_Key;
                    this.Client.ReceiveTimeout = 20000;
                    return JsonSerializer.Serialize(new
                    {
                        status = "success",
                        message = "Secure Connection Established"
                    });
                }
            }
            catch
            {
            }
            return JsonSerializer.Serialize(new
            {
                status = "error",
                message = "Key Exchange Error"
            });
        }

        private void Handle_Game_Action(Dictionary<string, JsonElement> Data, string Action)
        {
            if (Client_By_Room.ContainsKey(Client))
            {
                string Room_ID = Client_By_Room[Client];
                lock (Program.Rooms)
                {
                    if (Program.Rooms.ContainsKey(Room_ID))
                    {
                        Program.Rooms[Room_ID].Process_Player_Action(Client, Action, Data);
                    }
                }
            }
        }

        private string Handle_Login(Dictionary<string, JsonElement> Data)
        {
            string IP = Handle_Get_IP();

            if (Ban_Peroid_Per_IP.ContainsKey(IP))
            {
                if (DateTime.Now < Ban_Peroid_Per_IP[IP])
                {
                    TimeSpan Remaining_Time = Ban_Peroid_Per_IP[IP] - DateTime.Now;
                    
                    Security_Logger.Log(Security_Logger.Level.CRITICAL, IP, "BLOCKED", $"Brute Force Detected! Remaining {Remaining_Time.TotalSeconds:F0}s");
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = $"You Are Temporarily Blocked. Please Try Again In {Math.Ceiling(Remaining_Time.TotalMinutes)} Minutes."
                    });
                }
                else
                {
                    Ban_Peroid_Per_IP.TryRemove(IP, out _);
                    if (Attempts_Per_IP.ContainsKey(IP))
                    {
                        Attempts_Per_IP.TryRemove(IP, out _);
                    }
                }
            }

            if (!Data.ContainsKey("username") || !Data.ContainsKey("password"))
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Incomplete Data"
                });
            }

            string User = Data["username"].GetString();
            string Encrypted_Pass = Data["password"].GetString();
            string Decrypted_Pass = RSA_Handle.Decrypt(Encrypted_Pass, Program.SERVER_PRIVATE_KEY);

            if (Decrypted_Pass == null)
            {
                Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", "Decryption Failed");
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Security Error: Invalid Packet"
                });
            }

            if (SQL_Handle.Handle_Login(User, Decrypted_Pass))
            {
                string Email = SQL_Handle.Handle_Get_Email(User);

                if (Email != null)
                {
                    Client_By_Email.AddOrUpdate(Email, Client, (Key, Old_Client) => 
                    {
                        try
                        {
                            if (Old_Client.Connected)
                            {
                                string Kick_Msg = JsonSerializer.Serialize(new
                                {
                                    status = "force_logout",
                                    message = "Account Logged In From Another Location"
                                });
                                byte[] Bytes = Encoding.UTF8.GetBytes(Kick_Msg + "\n");
                                Old_Client.GetStream().Write(Bytes, 0, Bytes.Length);
                                Old_Client.Close();
                            }
                        }
                        catch 
                        { 
                            // Continue
                        }
                        return Client;
                    });
                }

                Attempts_Per_IP.TryRemove(IP, out _);
                Ban_Peroid_Per_IP.TryRemove(IP, out _);
                SoftBan_Count_Per_IP.TryRemove(IP, out _);

                Security_Logger.Log(Security_Logger.Level.INFO, IP, "SUCCESS", $"User {User} Logged In");
                return JsonSerializer.Serialize(new
                {
                    status = "success",
                    message = "Login Success",
                    username = User
                });
            }
            else
            {
                int Fails = Attempts_Per_IP.AddOrUpdate(IP, 1, (Key, Value) => Value + 1);
                int Max_Soft_Ban = 5;

                if (Fails >= Max_Soft_Ban)
                {
                    DateTime Ban_Time = DateTime.Now.AddMinutes(5);
                    Ban_Peroid_Per_IP.AddOrUpdate(IP, Ban_Time, (Key, Value) => Ban_Time);

                    Attempts_Per_IP.TryRemove(IP, out _);

                    int Ban_Count = SoftBan_Count_Per_IP.AddOrUpdate(IP, 1, (Key, Value) => Value + 1);

                    if (Ban_Count >= 3)
                    {
                        string Message = JsonSerializer.Serialize(new
                        {                       
                            status = "error",
                            message = "Your IP Has Been Permanently Blocked Due To Suspicious Activity."
                        });

                        Task.Run(async () =>
                        {
                            await Task.Delay(500);
                            SQL_Handle.Handle_Add_Blacklist(IP, "Brute Force");
                            OS_Handle.Handle_Block(IP);

                            Security_Logger.Log(Security_Logger.Level.ALERT, IP, "BLOCKED", "Brute Force Detected! Persistent Attacker Blocked By Firewall!");

                            try 
                            { 
                                Client.Close();
                            } 
                            catch 
                            {
                                // Continue
                            }
                        });

                        return Message;
                    }

                    Security_Logger.Log(Security_Logger.Level.CRITICAL, IP, "BLOCKED", "Brute Force Detected! Soft Ban 5 mins.");
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = "Too Many Failed Attempts. Please Wait 5 Minutes Before Trying Again."
                    });
                }
                else
                {
                    int Remaining = Max_Soft_Ban - Fails;

                    Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", $"Wrong Username Or Wrong Password.");
                    
                    return JsonSerializer.Serialize(new
                    {
                        status = "error",
                        message = $"Invalid Username Or Password. {Remaining} Attempts Remaining."
                    });
                }
            }
        }

        private string Handle_Register(Dictionary<string, JsonElement> Data)
        {
            string IP = Handle_Get_IP();
            string Username = Data["username"].GetString();
            string Email = Data["email"].GetString();
            string Birthday = Data.ContainsKey("birthday") ? Data["birthday"].GetString() : "";
            string Encrypted_Pass = Data["password"].GetString();
            string Decrypted_Pass = RSA_Handle.Decrypt(Encrypted_Pass, Program.SERVER_PRIVATE_KEY);

            if (Decrypted_Pass == null)
            {
                Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", "Decryption Failed");
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Security Error: Invalid Packet"
                });
            }

            int Result = SQL_Handle.Handle_Register(Username, Email, Decrypted_Pass, Birthday);

            if (Result == 1)
            {
                Security_Logger.Log(Security_Logger.Level.INFO, IP, "SUCCESS", $"User Registered {Username}");
                return JsonSerializer.Serialize(new
                {
                    status = "success",
                    message = "Registration Successful"
                });
            }
            if (Result == -1)
            {
                Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", $"User {Username} Duplicated Name Or Email");
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Username Or Email Already Exists"
                });
            }
            return JsonSerializer.Serialize(new
            {
                status = "error",
                message = "Registration System Error"
            });
        }

        private string Handle_Get_Info(Dictionary<string, JsonElement> Data)
        {
            string User = Data["username"].GetString();
            string Info = SQL_Handle.Handle_Get_Info(User);

            if (!string.IsNullOrEmpty(Info))
            {
                var Parts = Info.Split('|');
                Console.WriteLine($"[TCP] Retrieved Info For User: {User}");
                return JsonSerializer.Serialize(new
                {
                    status = "success",
                    username = User,
                    email = Parts[0],
                    birthday = Parts[1]
                });
            }
            return JsonSerializer.Serialize(new
            {
                status = "error",
                message = "User Not Found"
            });
        }

        private string Handle_Forgot_Password(Dictionary<string, JsonElement> Data)
        {
            string IP = Handle_Get_IP();
            string Email = Data["email"].GetString();
            string Username = SQL_Handle.Handle_Get_Username(Email);

            if (!string.IsNullOrEmpty(Username))
            {
                string Token = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

                Token_By_Email.AddOrUpdate(Email, Token, (Key, Old_Value) => Token);

                bool Sent = Send_Email(Email, "Reset Password - Pixel Drift", $"Hello {Username}, your verification code is: {Token}");

                if (Sent)
                {
                    Security_Logger.Log(Security_Logger.Level.INFO, IP, "SUCCESS", $"User {Username} Received Token");
                    return JsonSerializer.Serialize(new
                    {
                        status = "success",
                        message = "Verification Code Sent Via Email"
                    });
                }

                Console.WriteLine($"[Error] Failed To Send Email To {Email}");
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Error Sending Email"
                });
            }

            Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", $"User {Username} Invalid Requested");
            return JsonSerializer.Serialize(new
            {
                status = "error",
                message = "Email Does Not Exist In The System"
            });
        }

        private string Handle_Change_Password(Dictionary<string, JsonElement> Data)
        {
            string IP = Handle_Get_IP();
            string Email = Data["email"].GetString();
            string Token = Data["token"].GetString();
            string Username = SQL_Handle.Handle_Get_Username(Email);
            string Encrypted_Pass = Data["new_password"].GetString();
            string Decrypted_Pass = RSA_Handle.Decrypt(Encrypted_Pass, Program.SERVER_PRIVATE_KEY);

            if (Decrypted_Pass == null)
            {
                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Security Error: Invalid Packet"
                });
            }

            if (Token_By_Email.TryGetValue(Email, out string savedToken))
            {
                if (savedToken == Token)
                {
                    if (SQL_Handle.Handle_Change_Password(Email, Decrypted_Pass))
                    {
                        Token_By_Email.TryRemove(Email, out _);

                        Security_Logger.Log(Security_Logger.Level.CRITICAL, IP, "SUCCESS", $"User {Username} Changed Password");
                        return JsonSerializer.Serialize(new
                        {
                            status = "success",
                            message = "Password Changed Successfully"
                        });
                    }
                }
            }

            Security_Logger.Log(Security_Logger.Level.WARNING, IP, "FAILED", $"User {Username} Entered Wrong Token");
            return JsonSerializer.Serialize(new
            {
                status = "error",
                message = "Invalid Or Expired Verification Code"
            });
        }

        private string Handle_Create_Room(Dictionary<string, JsonElement> Data)
        {
            string User = Data["username"].GetString();
            string Room_ID = new Random().Next(100000, 999999).ToString();
            Game_Room New_Room;

            do
            {
                Room_ID = new Random().Next(100000, 999999).ToString();
                New_Room = new Game_Room(Room_ID);
                New_Room.Add_Player(Client, User);
            }
            while (!Program.Rooms.TryAdd(Room_ID, New_Room));

            Client_By_Room.AddOrUpdate(Client, Room_ID, (k, v) => Room_ID);

            Console.WriteLine($"[Success] User {User} Created Room {Room_ID}");

            return JsonSerializer.Serialize(new
            {
                status = "create_room_success",
                room_id = Room_ID,
                player_number = 1
            });
        }

        private string Handle_Join_Room(Dictionary<string, JsonElement> Data)
        {
            string User = Data["username"].GetString();
            string Room_ID = Data["room_id"].GetString();

            if (Program.Rooms.TryGetValue(Room_ID, out Game_Room Room))
            {
                int P_Num = Room.Add_Player(Client, User);

                if (P_Num != -1)
                {
                    Client_By_Room.AddOrUpdate(Client, Room_ID, (k, v) => Room_ID);

                    Console.WriteLine($"[Success] User {User} Joined Room {Room_ID}");
                    return JsonSerializer.Serialize(new
                    {
                        status = "join_room_success",
                        room_id = Room_ID,
                        player_number = P_Num
                    });
                }

                return JsonSerializer.Serialize(new
                {
                    status = "error",
                    message = "Room Is Full"
                });
            }

            return JsonSerializer.Serialize(new
            {
                status = "error",
                message = "Room Not Found"
            });
        }

        private string Handle_Get_Scoreboard(Dictionary<string, JsonElement> Data)
        {
            int Limit = Data.ContainsKey("top_count") ? Data["top_count"].GetInt32() : 50;
            string Board = SQL_Handle.Handle_Get_Scoreboard(Limit);
            return JsonSerializer.Serialize(new
            {
                action = "scoreboard_data",
                data = Board
            });
        }

        private string Handle_Search_Player(Dictionary<string, JsonElement> Data)
        {
            string Keyword = Data["search_text"].GetString();
            string Result = SQL_Handle.Handle_Search_Player(Keyword);
            return JsonSerializer.Serialize(new
            {
                action = "search_result",
                data = Result
            });
        }

        private void Handle_Disconnect()
        {
            try
            {
                string IP = Handle_Get_IP();

                Console.WriteLine($"[TCP] Client {IP} Disconnected");

                if (Client_By_Room.TryRemove(Client, out string Room_ID))
                {
                    lock (Program.Rooms)
                    {
                        if (Program.Rooms.TryGetValue(Room_ID, out Game_Room Room))
                        {
                            Room.Remove_Player(Client);
                            if (Room.Is_Empty())
                            {
                                Program.Rooms.TryRemove(Room_ID, out _);
                                Console.WriteLine($"[Success] Room {Room_ID} Has Been Cancelled");
                            }
                        }
                    }
                }

                Connections_Per_IP.AddOrUpdate(IP, 0, (key, count) => Math.Max(0, count - 1));

                foreach (var tmp in Client_By_Email)
                {
                    if (tmp.Value == Client)
                    {
                        Client_By_Email.TryRemove(tmp.Key, out _);
                        break; 
                    }
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
            catch
            {
                // Continue
            }
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