using Pixel_Drift;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Pixel_Drift_Server
{
    class Program
    {
        public static string SERVER_PUBLIC_KEY;

        public static string SERVER_PRIVATE_KEY;

        public static ConcurrentDictionary<string, Game_Room> Rooms = new ConcurrentDictionary<string, Game_Room>();

        static void Main(string[] args)
        {
            Console.WriteLine("PIXEL DRIFT");
            Console.WriteLine("Server Is Running, Press 'E' For Exit.");

            List<string> Blacklist = SQL_Handle.Handle_Get_Blacklist();
            foreach (var IP in Blacklist)
            {
                OS_Handle.Handle_Block(IP);
            }

            RSA_Handle.Generate_Keys(out SERVER_PUBLIC_KEY, out SERVER_PRIVATE_KEY);

            SQL_Handle.Initialize();

            UDP_Handle.Start(2222);

            Task.Run(() => TCP_Handler.Start(1111));

            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.E)
                {
                    break;
                }
            }
        }
    }
}