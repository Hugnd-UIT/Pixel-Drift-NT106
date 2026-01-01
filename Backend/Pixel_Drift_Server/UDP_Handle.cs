using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pixel_Drift_Server
{
    public static class UDP_Handle
    {
        public static void Start(int Port)
        {
            Task.Run(async () =>
            {
                try
                {
                    using (UdpClient UDP = new UdpClient(Port))
                    {
                        Console.WriteLine($"[UDP] Server Is Broadcasting On Port {Port}...");

                        while (true)
                        {
                            var Result = await UDP.ReceiveAsync();
                            string Message = Encoding.UTF8.GetString(Result.Buffer);

                            if (Message == "discover_server")
                            {
                                byte[] Response = Encoding.UTF8.GetBytes("server_here");
                                await UDP.SendAsync(Response, Response.Length, Result.RemoteEndPoint);
                                Console.WriteLine($"[UDP] {Result.RemoteEndPoint.Address} Got IP");
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Console.WriteLine($"[Error] {Ex.Message}");
                }
            });
        }
    }
}