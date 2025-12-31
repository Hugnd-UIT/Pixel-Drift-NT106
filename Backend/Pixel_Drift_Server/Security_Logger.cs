using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixel_Drift_Server
{
    public static class Security_Logger
    {
        private static string Log_Path = "Security_Events.log";
        private static readonly object Log_Lock = new object();

        public enum Level { INFO, WARNING, CRITICAL, ALERT }

        public static void Log(Level Lv, string IP, string Action, string Detail)
        {
            string Log_Line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{Lv}] [IP:{IP}] [{Action}] {Detail}";

            Console.ForegroundColor = Get_Color(Lv);
            Console.WriteLine(Log_Line);
            Console.ResetColor();

            try
            {
                lock (Log_Lock) 
                {
                    using (StreamWriter Sw = new StreamWriter(Log_Path, true))
                    {
                        Sw.WriteLine(Log_Line);
                    }
                }
            }
            catch { }
        }

        private static ConsoleColor Get_Color(Level Lv)
        {
            switch (Lv)
            {
                case Level.INFO: return ConsoleColor.Green;
                case Level.WARNING: return ConsoleColor.Yellow;
                case Level.CRITICAL: return ConsoleColor.Red;
                case Level.ALERT: return ConsoleColor.Magenta;
                default: return ConsoleColor.White;
            }
        }
    }
}
