using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net.Sockets;
using System.Text.Json;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_ScoreBoard : Form
    {
        public Form_ScoreBoard(TcpClient Tcp_Client)
        {
            InitializeComponent();

            Network_Handle.Incoming_Request += Handle_Server_Message;

            Load_Score_Board();
        }

        private void Handle_Server_Message(string Message)
        {
            if (this.Disposing || this.IsDisposed || !this.IsHandleCreated)
            {
                return;
            }

            this.Invoke(new Action(() =>
            {
                try
                {
                    var Data = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(Message);
                    if (!Data.ContainsKey("action"))
                    {
                        return;
                    }

                    string Action = Data["action"].GetString();

                    if (Action == "scoreboard_data" || Action == "search_result")
                    {
                        if (Data.ContainsKey("data"))
                        {
                            string Score_Data = Data["data"].GetString();
                            Display_Score_Board(Score_Data);
                        }
                    }
                }
                catch
                {
                }
            }));
        }

        private void Load_Score_Board()
        {
            var Request = new
            {
                action = "get_scoreboard",
                top_count = 50
            };
            Network_Handle.Send_And_Forget(Request);
        }

        public void Display_Score_Board(string Json_Data)
        {
            try
            {
                if (string.IsNullOrEmpty(Json_Data) || Json_Data == "EMPTY" || Json_Data == "ERROR")
                {
                    dgv_ScoreBoard.DataSource = null;
                    return;
                }

                DataTable Dt = new DataTable();
                Dt.Columns.Add("Rank", typeof(int));
                Dt.Columns.Add("Username", typeof(string));
                Dt.Columns.Add("Win", typeof(int));
                Dt.Columns.Add("Crash", typeof(int));
                Dt.Columns.Add("Total", typeof(double));
                Dt.Columns.Add("Date", typeof(string));

                string[] Lines = Json_Data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                int Rank = 1;
                foreach (string Line in Lines)
                {
                    string[] Parts = Line.Split('|');
                    if (Parts.Length == 6)
                    {
                        string Player_Name = Parts[1];
                        int Win_Count = int.Parse(Parts[2]);
                        int Crash_Count = int.Parse(Parts[3]);
                        double Total_Score = double.Parse(Parts[4]);
                        string Date_Played = Parts[5];

                        Dt.Rows.Add(Rank++, Player_Name, Win_Count, Crash_Count, Total_Score, Date_Played);
                    }
                }

                dgv_ScoreBoard.DataSource = Dt;

                if (dgv_ScoreBoard.Columns.Count > 0)
                {
                    dgv_ScoreBoard.Columns[0].Width = 60;
                    dgv_ScoreBoard.Columns[1].Width = 90;
                    dgv_ScoreBoard.Columns[3].Width = 80;
                    dgv_ScoreBoard.Columns[4].Width = 60;
                    dgv_ScoreBoard.Columns[5].Width = 60;
                }

                Highlight_Top_Players();
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Scoreboard Display Error: " + Ex.Message);
            }
        }

        private void btn_Search_Click(object sender, EventArgs e)
        {
            string Search_Text = tb_username.Text.Trim();
            if (string.IsNullOrEmpty(Search_Text))
            {
                Load_Score_Board();
                return;
            }
            var Request = new
            {
                action = "search_player",
                search_text = Search_Text
            };
            Network_Handle.Send_And_Forget(Request);
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            Load_Score_Board();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btn_Search_Click(sender, e);
                e.Handled = true;
            }
        }

        private void Highlight_Top_Players()
        {
            if (dgv_ScoreBoard.Rows.Count > 0)
            {
                dgv_ScoreBoard.Rows[0].DefaultCellStyle.BackColor = Color.Gold;
                dgv_ScoreBoard.Rows[0].DefaultCellStyle.ForeColor = Color.Black;
                dgv_ScoreBoard.Rows[0].DefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);

                if (dgv_ScoreBoard.Rows.Count > 1)
                {
                    dgv_ScoreBoard.Rows[1].DefaultCellStyle.BackColor = Color.Silver;
                    dgv_ScoreBoard.Rows[1].DefaultCellStyle.ForeColor = Color.Black;
                    dgv_ScoreBoard.Rows[1].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }

                if (dgv_ScoreBoard.Rows.Count > 2)
                {
                    dgv_ScoreBoard.Rows[2].DefaultCellStyle.BackColor = Color.SandyBrown;
                    dgv_ScoreBoard.Rows[2].DefaultCellStyle.ForeColor = Color.Black;
                    dgv_ScoreBoard.Rows[2].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Network_Handle.Incoming_Request -= Handle_Server_Message;
            base.OnFormClosing(e);
        }
    }
}