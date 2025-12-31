using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Drift_Server
{
    public class Game_Player
    {
        public TcpClient Client { get; set; }

        public NetworkStream Stream { get; set; }

        public string Username { get; set; }

        public bool Is_Ready { get; set; } = false;

        public int Player_ID { get; set; }
    }
}
