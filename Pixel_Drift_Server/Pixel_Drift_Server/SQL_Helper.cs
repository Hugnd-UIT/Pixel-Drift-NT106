using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace Pixel_Drift_Server
{
    public static class SQL_Helper
    {
        public static SQLiteConnection Connection;

        public static void Initialize()
        {
            string Database_Path = "Data Source=Qly_Nguoi_Dung.db;Version=3;";
            Connection = new SQLiteConnection(Database_Path);
            Connection.Open();

            string SQL_Query_User = @"
            CREATE TABLE IF NOT EXISTS Info_User (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT UNIQUE,
                Email TEXT UNIQUE,
                Password TEXT,
                Birthday TEXT
            )";

            using (var Command = new SQLiteCommand(SQL_Query_User, Connection))
            {
                Command.ExecuteNonQuery();
            }

            string SQL_Query_ScoreBoard = @"
            CREATE TABLE IF NOT EXISTS ScoreBoard (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                PlayerName TEXT NOT NULL,
                WinCount INTEGER DEFAULT 0,
                CrashCount INTEGER DEFAULT 0,
                TotalScore REAL DEFAULT 0,
                DatePlayed DATETIME DEFAULT CURRENT_TIMESTAMP
            )";

            using (var Command = new SQLiteCommand(SQL_Query_ScoreBoard, Connection))
            {
                Command.ExecuteNonQuery();
            }

            Console.WriteLine("Database initialized successfully!");
        }

        public static bool Add_Score(string Player_Name, int Win_Count, int Crash_Count, double Total_Score)
        {
            try
            {
                string Query = @"
                INSERT INTO ScoreBoard (PlayerName, WinCount, CrashCount, TotalScore) 
                VALUES (@PlayerName, @WinCount, @CrashCount, @TotalScore)";

                using (var Cmd = new SQLiteCommand(Query, Connection))
                {
                    Cmd.Parameters.AddWithValue("@PlayerName", Player_Name);
                    Cmd.Parameters.AddWithValue("@WinCount", Win_Count);
                    Cmd.Parameters.AddWithValue("@CrashCount", Crash_Count);
                    Cmd.Parameters.AddWithValue("@TotalScore", Total_Score);
                    Cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error adding score: {Ex.Message}");
                return false;
            }
        }

        public static string Get_Top_Scores(int Limit = 50)
        {
            try
            {
                StringBuilder Result = new StringBuilder();
                string Query = @"
                SELECT 
                    PlayerName, 
                    SUM(WinCount) as WinCount, 
                    SUM(CrashCount) as CrashCount, 
                    SUM(TotalScore) as TotalScore, 
                    MAX(DatePlayed) as DatePlayed 
                FROM ScoreBoard 
                GROUP BY PlayerName
                ORDER BY TotalScore DESC, WinCount DESC, CrashCount ASC 
                LIMIT @Limit";

                using (var Cmd = new SQLiteCommand(Query, Connection))
                {
                    Cmd.Parameters.AddWithValue("@Limit", Limit);

                    using (var Reader = Cmd.ExecuteReader())
                    {
                        int Rank = 1;
                        while (Reader.Read())
                        {
                            string Player_Name = Reader.GetString(0);
                            int Win_Count = Reader.GetInt32(1);
                            int Crash_Count = Reader.GetInt32(2);
                            double Total_Score = Reader.GetDouble(3);
                            string Date_Played = Reader.GetString(4);

                            Result.AppendLine($"{Rank}|{Player_Name}|{Win_Count}|{Crash_Count}|{Total_Score:F2}|{Date_Played}");
                            Rank++;
                        }
                    }
                }

                return Result.Length > 0 ? Result.ToString() : "EMPTY";
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error getting top scores: {Ex.Message}");
                return "ERROR";
            }
        }

        public static string Search_Player(string Search_Text)
        {
            try
            {
                StringBuilder Result = new StringBuilder();
                string Query = @"
                SELECT 
                    PlayerName, 
                    SUM(WinCount) as WinCount, 
                    SUM(CrashCount) as CrashCount, 
                    SUM(TotalScore) as TotalScore, 
                    MAX(DatePlayed) as DatePlayed 
                FROM ScoreBoard 
                WHERE PlayerName LIKE @SearchText 
                GROUP BY PlayerName
                ORDER BY TotalScore DESC 
                LIMIT 50";

                using (var Cmd = new SQLiteCommand(Query, Connection))
                {
                    Cmd.Parameters.AddWithValue("@SearchText", $"%{Search_Text}%");

                    using (var Reader = Cmd.ExecuteReader())
                    {
                        int Rank = 1;
                        while (Reader.Read())
                        {
                            string Player_Name = Reader.GetString(0);
                            int Win_Count = Reader.GetInt32(1);
                            int Crash_Count = Reader.GetInt32(2);
                            double Total_Score = Reader.GetDouble(3);
                            string Date_Played = Reader.GetString(4);

                            Result.AppendLine($"{Rank}|{Player_Name}|{Win_Count}|{Crash_Count}|{Total_Score:F2}|{Date_Played}");
                            Rank++;
                        }
                    }
                }

                return Result.Length > 0 ? Result.ToString() : "EMPTY";
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"Error searching player: {Ex.Message}");
                return "ERROR";
            }
        }
    }
}