using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pixel_Drift
{
    public static class Client_Manager
    {
        private static TcpClient Tcp_Client;
        private static StreamWriter Stream_Writer;
        private static StreamReader Stream_Reader;
        private static NetworkStream Network_Stream;

        public static event Action<string> On_Message_Received;
        private static bool Is_Global_Listening = false;

        public static TcpClient Get_Client() => Tcp_Client;

        public static string Get_Server_IP()
        {
            string Server_IP = null;
            using (UdpClient Udp_Client = new UdpClient())
            {
                Udp_Client.EnableBroadcast = true;
                var Endpoint = new IPEndPoint(IPAddress.Broadcast, 2222);
                byte[] Bytes = Encoding.UTF8.GetBytes("discover_server");
                try
                {
                    Udp_Client.Send(Bytes, Bytes.Length, Endpoint);
                    var Async_Result = Udp_Client.BeginReceive(null, null);
                    if (Async_Result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2)))
                    {
                        IPEndPoint Server_Ep = new IPEndPoint(IPAddress.Any, 0);
                        byte[] Received_Bytes = Udp_Client.EndReceive(Async_Result, ref Server_Ep);
                        if (Encoding.UTF8.GetString(Received_Bytes) == "server_here")
                            Server_IP = Server_Ep.Address.ToString();
                    }
                }
                catch { }
            }
            return Server_IP;
        }

        public static bool Connect(string IP, int Port)
        {
            try
            {
                if (Tcp_Client != null) Close_Connection();
                Tcp_Client = new TcpClient();

                string Final_IP = string.IsNullOrEmpty(IP) ? Get_Server_IP() : IP;
                if (string.IsNullOrEmpty(Final_IP)) 
                    Final_IP = "127.0.0.1";

                Tcp_Client.Connect(Final_IP, Port);
                Network_Stream = Tcp_Client.GetStream();
                Stream_Writer = new StreamWriter(Network_Stream, Encoding.UTF8) { 
                    AutoFlush = true 
                };
                Stream_Reader = new StreamReader(Network_Stream, Encoding.UTF8);

                return true;
            }
            catch 
            { 
                return false; 
            }
        }

        public static void Start_Global_Listening()
        {
            if (Is_Global_Listening) 
                return;
            Is_Global_Listening = true;

            Task.Run(async () =>
            {
                try
                {
                    while (Is_Connected)
                    {
                        string Message = await Stream_Reader.ReadLineAsync();
                        if (Message != null)
                        {
                            On_Message_Received?.Invoke(Message);
                        }
                        else break;
                    }
                }
                catch 
                { 
                    // Nuốt lỗi
                }
                finally 
                { 
                    Is_Global_Listening = false; 
                }
            });
        }

        public static void Send_And_Forget(object Data)
        {
            if (Is_Connected && Stream_Writer != null)
            {
                try 
                { 
                    Stream_Writer.WriteLine(JsonSerializer.Serialize(Data)); 
                }
                catch 
                { 
                    Close_Connection(); 
                }
            }
        }

        public static string Send_And_Wait(object Request_Data)
        {
            if (!Is_Connected) throw new Exception("Mất kết nối");
            try
            {
                Stream_Writer.WriteLine(JsonSerializer.Serialize(Request_Data));
                return Stream_Reader.ReadLine();
            }
            catch { Close_Connection(); throw; }
        }

        public static bool Is_Connected => Tcp_Client != null && Tcp_Client.Connected;

        public static void Close_Connection()
        {
            Is_Global_Listening = false;
            Stream_Writer?.Close(); Stream_Reader?.Close(); Network_Stream?.Close(); Tcp_Client?.Close();
            Tcp_Client = null;
        }
    }
}