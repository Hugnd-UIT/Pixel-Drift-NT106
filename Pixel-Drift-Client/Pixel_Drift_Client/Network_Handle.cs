using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public static class Network_Handle
    {
        private static TcpClient Tcp_Client;
        private static StreamWriter Stream_Writer;
        private static StreamReader Stream_Reader;
        private static NetworkStream Network_Stream;

        private static string My_AES_Key = null;
        public static event Action<string> On_Message_Received;
        private static TaskCompletionSource<string> Pending_Request = null;

        private static bool Is_Listening = false;
        private static bool Is_Handshaking = false;
        private static bool Is_Heartbeating = false;

        public static TcpClient Get_Client()
        {
            return Tcp_Client;
        }

        public static bool Is_Connected
        {
            get
            {
                return Tcp_Client != null && Tcp_Client.Connected;
            }
        }

        public static string Get_Server_IP()
        {
            string Server_IP = null;
            try
            {
                using (UdpClient Udp_Client = new UdpClient())
                {
                    Udp_Client.EnableBroadcast = true;
                    var Endpoint = new IPEndPoint(IPAddress.Broadcast, 2222);
                    byte[] Bytes = Encoding.UTF8.GetBytes("discover_server");
                    Udp_Client.Send(Bytes, Bytes.Length, Endpoint);
                    var Async_Result = Udp_Client.BeginReceive(null, null);
                    if (Async_Result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2)))
                    {
                        IPEndPoint Server_Ep = new IPEndPoint(IPAddress.Any, 0);
                        byte[] Received_Bytes = Udp_Client.EndReceive(Async_Result, ref Server_Ep);
                        if (Encoding.UTF8.GetString(Received_Bytes) == "server_here")
                        {
                            Server_IP = Server_Ep.Address.ToString();
                        }
                    }
                }
            }
            catch
            {
            }
            return Server_IP;
        }

        private static void Start_Heartbeat()
        {
            if (Is_Heartbeating)
            {
                return;
            }
            Is_Heartbeating = true;

            Task.Run(async () =>
            {
                try
                {
                    while (Is_Connected)
                    {
                        await Task.Delay(3000);
                        if (Is_Connected && !Is_Handshaking)
                        {
                            Send_And_Forget(new { action = "ping" });
                        }
                    }
                }
                catch
                {
                    // Continue
                }
                finally
                {
                    Is_Heartbeating = false;
                }
            });
        }

        public static bool Connect(string IP, int Port)
        {
            try
            {
                Close_Connection();
                Tcp_Client = new TcpClient();

                string Final_IP = string.IsNullOrEmpty(IP) ? Get_Server_IP() : IP;
                if (string.IsNullOrEmpty(Final_IP))
                {
                    Final_IP = "127.0.0.1";
                }

                Tcp_Client.Connect(Final_IP, Port);
                Network_Stream = Tcp_Client.GetStream();
                Stream_Writer = new StreamWriter(Network_Stream, Encoding.UTF8) { AutoFlush = true };
                Stream_Reader = new StreamReader(Network_Stream, Encoding.UTF8);
                Start_Heartbeat();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool Secure()
        {
            try
            {
                Is_Handshaking = true;

                Stream_Writer.WriteLine(JsonSerializer.Serialize(new
                {
                    action = "get_public_key"
                }));

                string KeyJson = Stream_Reader.ReadLine();
                if (string.IsNullOrEmpty(KeyJson))
                {
                    return false;
                }

                var Json_Key = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(KeyJson);
                string PublicKey = Json_Key["public_key"].GetString();

                string Temp_Key = AES_Handle.Generate_Key();
                string Encrypted_AES_Key = RSA_Handle.Encrypt(Temp_Key, PublicKey);

                var Handshake_Data = new
                {
                    action = "get_aes_key",
                    aes_key = Encrypted_AES_Key
                };
                Stream_Writer.WriteLine(JsonSerializer.Serialize(Handshake_Data));

                string Encrypted_Response = Stream_Reader.ReadLine();

                if (!string.IsNullOrEmpty(Encrypted_Response))
                {
                    string Decrypted = AES_Handle.Decrypt(Encrypted_Response, Temp_Key);

                    if (Decrypted != null && Decrypted.Contains("success"))
                    {
                        My_AES_Key = Temp_Key;
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                My_AES_Key = null;
                return false;
            }
            finally
            {
                Is_Handshaking = false;
            }
        }

        public static void Start_Global_Listening()
        {
            if (Is_Listening)
            {
                return;
            }
            Is_Listening = true;

            Task.Run(async () =>
            {
                try
                {
                    while (Is_Connected)
                    {
                        if (Is_Handshaking)
                        {
                            await Task.Delay(10);
                            continue;
                        }

                        string Message = await Stream_Reader.ReadLineAsync();
                        if (Message == null)
                        {
                            break;
                        }

                        string Real_Data = Message;

                        if (!string.IsNullOrEmpty(My_AES_Key) && !Message.Trim().StartsWith("{"))
                        {
                            string Decrypted = AES_Handle.Decrypt(Message, My_AES_Key);
                            if (Decrypted != null)
                            {
                                Real_Data = Decrypted;
                            }
                        }

                        if (Pending_Request != null && !Pending_Request.Task.IsCompleted)
                        {
                            Pending_Request.SetResult(Real_Data);
                        }
                        else
                        {
                            On_Message_Received?.Invoke(Real_Data);
                        }
                    }
                }
                catch
                {
                    Close_Connection();
                }
                finally
                {
                    Is_Listening = false;
                }
            });
        }

        public static string Send_And_Wait(object Request_Data)
        {
            if (!Is_Connected || Is_Handshaking)
            {
                return null;
            }

            Pending_Request = new TaskCompletionSource<string>();

            try
            {
                string Json_Raw = JsonSerializer.Serialize(Request_Data);
                var Dict_Data = JsonSerializer.Deserialize<Dictionary<string, object>>(Json_Raw);

                if (!Dict_Data.ContainsKey("timestamp"))
                {
                    Dict_Data["timestamp"] = DateTime.UtcNow.Ticks;
                }

                string Json_Final = JsonSerializer.Serialize(Dict_Data);
                string Data_To_Send = Json_Final;

                if (!string.IsNullOrEmpty(My_AES_Key))
                {
                    Data_To_Send = AES_Handle.Encrypt(Json_Final, My_AES_Key);
                }

                Stream_Writer.WriteLine(Data_To_Send);

                if (Pending_Request.Task.Wait(5000))
                {
                    string Res = Pending_Request.Task.Result;
                    Pending_Request = null;
                    return Res;
                }
                else
                {
                    Pending_Request = null;
                    return null;
                }
            }
            catch
            {
                Close_Connection();
                return null;
            }
        }

        public static void Send_And_Forget(object Data)
        {
            if (Is_Connected)
            {
                try
                {
                    string Json_Raw = JsonSerializer.Serialize(Data);
                    var Dict_Data = JsonSerializer.Deserialize<Dictionary<string, object>>(Json_Raw);

                    if (!Dict_Data.ContainsKey("timestamp"))
                    {
                        Dict_Data["timestamp"] = DateTime.UtcNow.Ticks;
                    }

                    string Json_Final = JsonSerializer.Serialize(Dict_Data);
                    string Data_To_Send = Json_Final;

                    if (!string.IsNullOrEmpty(My_AES_Key))
                    {
                        Data_To_Send = AES_Handle.Encrypt(Json_Final, My_AES_Key);
                    }
                    Stream_Writer.WriteLine(Data_To_Send);
                }
                catch
                {
                    Close_Connection();
                }
            }
        }

        public static void Close_Connection()
        {
            Is_Listening = false;
            try
            {
                Tcp_Client?.Close();
                Stream_Writer?.Close();
                Stream_Reader?.Close();
            }
            catch
            {
                // Continue
            }
            Tcp_Client = null;
            My_AES_Key = null;
        }
    }
}