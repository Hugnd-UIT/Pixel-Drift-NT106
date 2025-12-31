using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Data.SqlClient;

namespace Pixel_Drift_Server
{
    public static class SQL_Handle
    {
        private static string Connection_String = "Server=LAPTOP-RI9PA5KK\\SQLEXPRESS;Database=QlyPixelDrift;Integrated Security=True;TrustServerCertificate=True;";

        public static void Initialize()
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Connection_String)) 
                { 
                    Connection.Open();

                    string Create_User = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
                    CREATE TABLE Users (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Username NVARCHAR(50) UNIQUE NOT NULL,
                        Email NVARCHAR(100) UNIQUE NOT NULL,
                        Password NVARCHAR(256) NOT NULL,
                        Birthday DATETIME NULL,          
                        Created_Time DATETIME DEFAULT GETDATE()
                    )";

                    using (SqlCommand Cmd = new SqlCommand(Create_User, Connection))
                    {
                        Cmd.ExecuteNonQuery();
                    }

                    string Create_Match = @"
                    IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Match' AND xtype='U')
                    CREATE TABLE Match (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Player_Id INT NOT NULL,      
                        Opponent_Id INT NULL,         
                        Is_Win BIT DEFAULT 0,         
                        Score INT DEFAULT 0,         
                        Crash_Count INT DEFAULT 0,    
                        Played_Time DATETIME DEFAULT GETDATE(),
                        CONSTRAINT FK_Match_User FOREIGN KEY (Player_Id) REFERENCES Users(Id) ON DELETE CASCADE
                    )";

                    using (SqlCommand Cmd = new SqlCommand(Create_Match, Connection))
                    {
                        Cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("[Database] Initialized Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
            }
        }

        public static int Handle_Register(string Username, string Email, string Password, string Birthday)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Connection_String))
                {
                    Connection.Open();

                    string Check = "SELECT COUNT(1) FROM Users WHERE Username=@u OR Email=@e";

                    using (SqlCommand Cmd = new SqlCommand(Check, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@u", Username);
                        Cmd.Parameters.AddWithValue("@e", Email);
                        if ((int)Cmd.ExecuteScalar() > 0) 
                            return -1; 
                    }

                    string Hashed_Password = BCrypt.Net.BCrypt.HashPassword(Password);

                    string Insert = "INSERT INTO Users (Username, Email, Password, Birthday) VALUES (@u, @e, @p, @b)";

                    using (SqlCommand Cmd = new SqlCommand(Insert, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@u", Username);
                        Cmd.Parameters.AddWithValue("@e", Email);
                        Cmd.Parameters.AddWithValue("@p", Hashed_Password);
                        if (DateTime.TryParse(Birthday, out DateTime dt)) 
                            Cmd.Parameters.AddWithValue("@b", dt);
                        else Cmd.Parameters.AddWithValue("@b", DBNull.Value);
                        Cmd.ExecuteNonQuery();
                        return 1; 
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"[Error] {ex.Message}");
                return 0; 
            }
        }

        public static bool Handle_Login(string Username, string Password)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Connection_String))
                {
                    Connection.Open();

                    string Query = "SELECT Password FROM Users WHERE Username=@u";

                    using (SqlCommand Cmd = new SqlCommand(Query, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@u", Username);
                        object Result = Cmd.ExecuteScalar();

                        if (Result == null) return false;

                        bool Is_Valid = BCrypt.Net.BCrypt.Verify(Password, Result.ToString());
                        return Is_Valid;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
                return false;
            }
        }

        public static string Handle_Get_Info(string Username)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Connection_String))
                {
                    Connection.Open();

                    string Query = "SELECT Email, Birthday FROM Users WHERE Username=@u";

                    using (SqlCommand Cmd = new SqlCommand(Query, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@u", Username);
                        using (SqlDataReader Reader = Cmd.ExecuteReader())
                        {
                            if (Reader.Read())
                            {
                                string Email = Reader["Email"].ToString();
                                string Birthday = Reader["Birthday"] != DBNull.Value ? Convert.ToDateTime(Reader["Birthday"]).ToString("yyyy-MM-dd") : "N/A";
                                return $"{Email}|{Birthday}";
                            }
                        }
                    }
                }
                return "";
            }
            catch 
            { 
                return ""; 
            }
        }

        public static bool Handle_Change_Password(string Email, string New_Password)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Connection_String))
                {
                    Connection.Open();

                    string Query = "UPDATE Users SET Password=@p WHERE Email=@e";

                    string Hashed_New_Pass = BCrypt.Net.BCrypt.HashPassword(New_Password);

                    using (SqlCommand Cmd = new SqlCommand(Query, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@p", Hashed_New_Pass);
                        Cmd.Parameters.AddWithValue("@e", Email);
                        return Cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch 
            { 
                return false; 
            }
        }

        public static string Handle_Get_Email(string Username)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Connection_String))
                {
                    Connection.Open();

                    string Query = "SELECT Email FROM Users WHERE Username=@u";

                    using (SqlCommand Cmd = new SqlCommand(Query, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@u", Username);
                        object Result = Cmd.ExecuteScalar();
                        return Result?.ToString();
                    }
                }
            }
            catch 
            { 
                return ""; 
            }
        }

        public static string Handle_Get_Username(string Email)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Connection_String))
                {
                    Connection.Open();
                    string Query = "SELECT Username FROM Users WHERE Email=@e";

                    using (SqlCommand Cmd = new SqlCommand(Query, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@e", Email);
                        object Result = Cmd.ExecuteScalar();
                        return Result?.ToString(); 
                    }
                }
            }
            catch
            {
                return "";
            }
        }

        public static int Handle_Get_ID(string Username)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Connection_String))
                {
                    Connection.Open();

                    string Query = "SELECT Id FROM Users WHERE Username=@u";

                    using (SqlCommand Cmd = new SqlCommand(Query, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@u", Username);
                        object Result = Cmd.ExecuteScalar();
                        return Result != null ? Convert.ToInt32(Result) : -1;
                    }
                }
            }
            catch 
            { 
                return -1; 
            }
        }

        public static void Handle_Match_Result(int Player_Id, bool Is_Win, int Score, int Crash_Count)
        {
            try
            {
                using (SqlConnection Connection = new SqlConnection(Connection_String))
                {
                    Connection.Open();

                    string Query = @"INSERT INTO Match (Player_Id, Is_Win, Score, Crash_Count) VALUES (@pid, @win, @score, @crash)";

                    using (SqlCommand Cmd = new SqlCommand(Query, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@pid", Player_Id);
                        Cmd.Parameters.AddWithValue("@win", Is_Win);
                        Cmd.Parameters.AddWithValue("@score", Score);
                        Cmd.Parameters.AddWithValue("@crash", Crash_Count);
                        Cmd.ExecuteNonQuery();
                    }
                }
                Console.WriteLine($"[Success] Saved Match For User: {Player_Id}");
            }
            catch (Exception Ex)
            {
                Console.WriteLine($"[Error] {Ex.Message}");
            }
        }

        public static string Handle_Get_Scoreboard(int Limit = 50)
        {
            try
            {
                StringBuilder Sb = new StringBuilder();
                using (SqlConnection Connection = new SqlConnection(Connection_String))
                {
                    Connection.Open();
                    
                    string Query = @"
                        SELECT TOP (@limit)
                            u.Username,
                            SUM(CASE WHEN m.Is_Win = 1 THEN 1 ELSE 0 END) as Win_Count,
                            SUM(m.Crash_Count) as Total_Crash,
                            SUM(m.Score) as Total_Score,
                            MAX(m.Played_Time) as Last_Played
                        FROM Users u
                        JOIN Match m ON u.Id = m.Player_Id
                        GROUP BY u.Username
                        ORDER BY Total_Score DESC";

                    using (SqlCommand Cmd = new SqlCommand(Query, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@limit", Limit);
                        using (SqlDataReader Reader = Cmd.ExecuteReader())
                        {
                            int Rank = 1;
                            while (Reader.Read())
                            {
                                Sb.AppendLine($"{Rank}|{Reader["Username"]}|{Reader["Win_Count"]}|{Reader["Total_Crash"]}|{Reader["Total_Score"]}|{Reader["Last_Played"]}");
                                Rank++;
                            }
                        }
                    }
                }
                return Sb.Length > 0 ? Sb.ToString() : "EMPTY";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
                return "ERROR";
            }
        }

        public static string Handle_Search_Player(string Keyword)
        {
            try
            {
                StringBuilder Sb = new StringBuilder();
                using (SqlConnection Connection = new SqlConnection(Connection_String))
                {
                    Connection.Open();

                    string Query = @"
                        SELECT TOP 50
                            u.Username,
                            SUM(CASE WHEN m.Is_Win = 1 THEN 1 ELSE 0 END) as Win_Count,
                            SUM(m.Crash_Count) as Total_Crash,
                            SUM(m.Score) as Total_Score,
                            MAX(m.Played_Time) as Last_Played
                        FROM Users u
                        JOIN Match m ON u.Id = m.Player_Id
                        WHERE u.Username LIKE @kw
                        GROUP BY u.Username
                        ORDER BY Total_Score DESC";

                    using (SqlCommand Cmd = new SqlCommand(Query, Connection))
                    {
                        Cmd.Parameters.AddWithValue("@kw", $"%{Keyword}%");
                        using (SqlDataReader Reader = Cmd.ExecuteReader())
                        {
                            int Rank = 1;
                            while (Reader.Read())
                            {
                                Sb.AppendLine($"{Rank}|{Reader["Username"]}|{Reader["Win_Count"]}|{Reader["Total_Crash"]}|{Reader["Total_Score"]}|{Reader["Last_Played"]}");
                                Rank++;
                            }
                        }
                    }
                }
                return Sb.Length > 0 ? Sb.ToString() : "EMPTY";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] {ex.Message}");
                return "ERROR";
            }
        }
    }
}
