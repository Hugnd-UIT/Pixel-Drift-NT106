using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Drift_Server
{
    public static class OS_Handle
    {
        public static void Handle_Block(string IP)
        {
            try
            {
                if (IP == "127.0.0.1" || IP == "localhost" ||  IP == "::1") { return; }

                string Rule_Name = $"Pixel_Drift_Auto_Block_{IP}";

                string Command = $"advfirewall firewall add rule name=\"{Rule_Name}\" dir=in action=block remoteip={IP}";

                ProcessStartInfo Proc_Info = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = Command,
                    Verb = "runas", 
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                Process.Start(Proc_Info);

                Console.WriteLine($"[SUCCESS] IP {IP} Has Been HARD-BLOCKED By Windows Firewall!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Firewall Block Failed: {ex.Message}");
            }
        }
    }
}
