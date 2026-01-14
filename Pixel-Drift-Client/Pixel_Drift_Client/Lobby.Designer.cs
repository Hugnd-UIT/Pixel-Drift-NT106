namespace Pixel_Drift
{
    partial class Lobby
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ptb_Lobby = new PictureBox();
            btn_CreateRoom = new Button();
            btn_JoinRoom = new Button();
            btn_Scoreboard = new Button();
            ((System.ComponentModel.ISupportInitialize)ptb_Lobby).BeginInit();
            SuspendLayout();
            // 
            // ptb_Lobby
            // 
            ptb_Lobby.Image = Properties.Resources.Background_Lobby;
            ptb_Lobby.Location = new Point(0, 0);
            ptb_Lobby.Margin = new Padding(4, 5, 4, 5);
            ptb_Lobby.Name = "ptb_Lobby";
            ptb_Lobby.Size = new Size(1191, 731);
            ptb_Lobby.SizeMode = PictureBoxSizeMode.StretchImage;
            ptb_Lobby.TabIndex = 0;
            ptb_Lobby.TabStop = false;
            // 
            // btn_CreateRoom
            // 
            btn_CreateRoom.BackColor = Color.Transparent;
            btn_CreateRoom.FlatAppearance.BorderSize = 0;
            btn_CreateRoom.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn_CreateRoom.FlatStyle = FlatStyle.Flat;
            btn_CreateRoom.Font = new Font("Consolas", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_CreateRoom.ForeColor = Color.DarkCyan;
            btn_CreateRoom.Location = new Point(321, 229);
            btn_CreateRoom.Margin = new Padding(4, 5, 4, 5);
            btn_CreateRoom.Name = "btn_CreateRoom";
            btn_CreateRoom.Size = new Size(260, 134);
            btn_CreateRoom.TabIndex = 1;
            btn_CreateRoom.Text = "CREATE ROOM";
            btn_CreateRoom.UseVisualStyleBackColor = false;
            btn_CreateRoom.Click += btn_CreateRoom_Click;
            // 
            // btn_JoinRoom
            // 
            btn_JoinRoom.BackColor = Color.Transparent;
            btn_JoinRoom.FlatAppearance.BorderSize = 0;
            btn_JoinRoom.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn_JoinRoom.FlatStyle = FlatStyle.Flat;
            btn_JoinRoom.Font = new Font("Consolas", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_JoinRoom.ForeColor = Color.DarkCyan;
            btn_JoinRoom.Location = new Point(613, 229);
            btn_JoinRoom.Margin = new Padding(4, 5, 4, 5);
            btn_JoinRoom.Name = "btn_JoinRoom";
            btn_JoinRoom.Size = new Size(255, 134);
            btn_JoinRoom.TabIndex = 2;
            btn_JoinRoom.Text = "JOIN ROOM";
            btn_JoinRoom.UseVisualStyleBackColor = false;
            btn_JoinRoom.Click += btn_JoinRoom_Click;
            // 
            // btn_Scoreboard
            // 
            btn_Scoreboard.BackColor = Color.Transparent;
            btn_Scoreboard.FlatAppearance.BorderSize = 0;
            btn_Scoreboard.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btn_Scoreboard.FlatStyle = FlatStyle.Flat;
            btn_Scoreboard.Font = new Font("Consolas", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Scoreboard.ForeColor = Color.DarkCyan;
            btn_Scoreboard.Location = new Point(468, 399);
            btn_Scoreboard.Margin = new Padding(4, 5, 4, 5);
            btn_Scoreboard.Name = "btn_Scoreboard";
            btn_Scoreboard.Size = new Size(260, 134);
            btn_Scoreboard.TabIndex = 3;
            btn_Scoreboard.Text = "SCOREBOARD";
            btn_Scoreboard.UseVisualStyleBackColor = false;
            btn_Scoreboard.Click += btn_Scoreboard_Click;
            // 
            // Lobby
            // 
            AcceptButton = btn_CreateRoom;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1190, 731);
            Controls.Add(btn_Scoreboard);
            Controls.Add(btn_JoinRoom);
            Controls.Add(btn_CreateRoom);
            Controls.Add(ptb_Lobby);
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "Lobby";
            Text = "Lobby";
            FormClosed += Lobby_FormClosed;
            ((System.ComponentModel.ISupportInitialize)ptb_Lobby).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ptb_Lobby;
        private System.Windows.Forms.Button btn_CreateRoom;
        private System.Windows.Forms.Button btn_JoinRoom;
        private Button btn_Scoreboard;
    }
}