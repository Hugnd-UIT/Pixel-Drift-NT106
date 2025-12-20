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
            this.ptb_Lobby = new System.Windows.Forms.PictureBox();
            this.btn_CreateRoom = new System.Windows.Forms.Button();
            this.btn_JoinRoom = new System.Windows.Forms.Button();
            this.btn_Scoreboard = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Lobby)).BeginInit();
            this.SuspendLayout();
            // 
            // ptb_Lobby
            // 
            this.ptb_Lobby.Image = global::Pixel_Drift.Properties.Resources.Gemini_Generated_Image_afaixrafaixrafai;
            this.ptb_Lobby.Location = new System.Drawing.Point(0, 0);
            this.ptb_Lobby.Margin = new System.Windows.Forms.Padding(4);
            this.ptb_Lobby.Name = "ptb_Lobby";
            this.ptb_Lobby.Size = new System.Drawing.Size(1069, 558);
            this.ptb_Lobby.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_Lobby.TabIndex = 0;
            this.ptb_Lobby.TabStop = false;
            // 
            // btn_CreateRoom
            // 
            this.btn_CreateRoom.BackColor = System.Drawing.Color.Transparent;
            this.btn_CreateRoom.FlatAppearance.BorderSize = 0;
            this.btn_CreateRoom.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_CreateRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_CreateRoom.Font = new System.Drawing.Font("Consolas", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CreateRoom.ForeColor = System.Drawing.Color.DarkCyan;
            this.btn_CreateRoom.Location = new System.Drawing.Point(296, 177);
            this.btn_CreateRoom.Margin = new System.Windows.Forms.Padding(4);
            this.btn_CreateRoom.Name = "btn_CreateRoom";
            this.btn_CreateRoom.Size = new System.Drawing.Size(219, 102);
            this.btn_CreateRoom.TabIndex = 1;
            this.btn_CreateRoom.Text = "CREATE ROOM";
            this.btn_CreateRoom.UseVisualStyleBackColor = false;
            this.btn_CreateRoom.Click += new System.EventHandler(this.btn_CreateRoom_Click);
            // 
            // btn_JoinRoom
            // 
            this.btn_JoinRoom.BackColor = System.Drawing.Color.Transparent;
            this.btn_JoinRoom.FlatAppearance.BorderSize = 0;
            this.btn_JoinRoom.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_JoinRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_JoinRoom.Font = new System.Drawing.Font("Consolas", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_JoinRoom.ForeColor = System.Drawing.Color.DarkCyan;
            this.btn_JoinRoom.Location = new System.Drawing.Point(555, 177);
            this.btn_JoinRoom.Margin = new System.Windows.Forms.Padding(4);
            this.btn_JoinRoom.Name = "btn_JoinRoom";
            this.btn_JoinRoom.Size = new System.Drawing.Size(219, 102);
            this.btn_JoinRoom.TabIndex = 2;
            this.btn_JoinRoom.Text = "JOIN ROOM";
            this.btn_JoinRoom.UseVisualStyleBackColor = false;
            this.btn_JoinRoom.Click += new System.EventHandler(this.btn_JoinRoom_Click);
            // 
            // btn_Scoreboard
            // 
            this.btn_Scoreboard.BorderThickness = 2;
            this.btn_Scoreboard.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Scoreboard.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_Scoreboard.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_Scoreboard.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_Scoreboard.FillColor = System.Drawing.Color.White;
            this.btn_Scoreboard.Font = new System.Drawing.Font("Consolas", 16.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Scoreboard.ForeColor = System.Drawing.Color.DarkCyan;
            this.btn_Scoreboard.Location = new System.Drawing.Point(420, 308);
            this.btn_Scoreboard.Name = "btn_Scoreboard";
            this.btn_Scoreboard.Size = new System.Drawing.Size(219, 102);
            this.btn_Scoreboard.TabIndex = 3;
            this.btn_Scoreboard.Text = "SCOREBOARD";
            this.btn_Scoreboard.Click += new System.EventHandler(this.btn_Scoreboard_Click);
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.btn_Scoreboard);
            this.Controls.Add(this.btn_JoinRoom);
            this.Controls.Add(this.btn_CreateRoom);
            this.Controls.Add(this.ptb_Lobby);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Lobby";
            this.Text = "Lobby";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Lobby_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Lobby)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ptb_Lobby;
        private System.Windows.Forms.Button btn_CreateRoom;
        private System.Windows.Forms.Button btn_JoinRoom;
        private Guna.UI2.WinForms.Guna2Button btn_Scoreboard;
    }
}