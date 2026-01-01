using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Pixel_Drift_Server
{
    public static class Security_Logger
    {
        private static string Log_Path = "Security_Events.log";
        private static readonly object Log_Lock = new object();

        private static readonly HttpClient HttpClient = new HttpClient();
        private static string Logstash_Url = "http://127.0.0.1:5000";

        public enum Level
        {
            INFO,
            WARNING,
            CRITICAL,
            ALERT
        }

        public static void Log(Level Lv, string IP, string Action, string Detail)
        {
            string Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string Log_Line = $"[{Timestamp}] [{Lv}] [IP:{IP}] [{Action}] {Detail}";

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
            catch
            {
                // Bỏ qua lỗi tránh crash server
            }

            Task.Run(() => Send_Log_To_SIEM(Lv.ToString(), IP, Action, Detail));
        }

        private static async Task Send_Log_To_SIEM(string Level, string IP, string Action, string Detail)
        {
            try
            {
                var Log_Data = new
                {
                    timestamp = DateTime.Now,
                    level = Level,
                    ip = IP,
                    action = Action,
                    message = Detail,
                    service = "Pixel_Drift_Server"
                };

                string Json_Payload = JsonSerializer.Serialize(Log_Data);
                var Content = new StringContent(Json_Payload, Encoding.UTF8, "application/json");

                await HttpClient.PostAsync(Logstash_Url, Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Could not send log: {ex.Message}");
            }
        }

        private static ConsoleColor Get_Color(Level Lv)
        {
            switch (Lv)
            {
                case Level.INFO:
                    return ConsoleColor.Green;
                case Level.WARNING:
                    return ConsoleColor.Yellow;
                case Level.CRITICAL:
                    return ConsoleColor.Red;
                case Level.ALERT:
                    return ConsoleColor.Magenta;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}