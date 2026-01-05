using System;
using Guna.UI2.WinForms;
using System.Drawing;



namespace Pixel_Drift

{

    partial class Game_Window

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Game_Window));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Game_Timer = new System.Windows.Forms.Timer(components);
            pn_road_left = new Panel();
            ptb_debuff_road_1 = new PictureBox();
            ptb_player1 = new PictureBox();
            ptb_AICar5 = new PictureBox();
            ptb_AICar1 = new PictureBox();
            ptb_buff_road_1 = new PictureBox();
            ptb_road_1_dup = new PictureBox();
            ptb_road_1 = new PictureBox();
            pn_road_right = new Panel();
            ptb_debuff_road_2 = new PictureBox();
            ptb_AICar6 = new PictureBox();
            ptb_AICar3 = new PictureBox();
            ptb_player2 = new PictureBox();
            ptb_buff_road_2 = new PictureBox();
            ptb_road_2_dup = new PictureBox();
            ptb_road_2 = new PictureBox();
            btn_Scoreboard = new Guna2GradientButton();
            btn_Ready = new Guna2GradientButton();
            btn_ID = new Guna2GradientButton();
            lbl_P2_Status = new Label();
            lbl_Countdown = new Label();
            lbl_GameTimer = new Label();
            lbl_Score1 = new Label();
            lbl_Score2 = new Label();
            lbl_P1_Status = new Label();
            pn_road_left.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ptb_debuff_road_1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_player1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_AICar5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_AICar1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_buff_road_1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_road_1_dup).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_road_1).BeginInit();
            pn_road_right.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ptb_debuff_road_2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_AICar6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_AICar3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_player2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_buff_road_2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_road_2_dup).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ptb_road_2).BeginInit();
            SuspendLayout();
            // 
            // Game_Timer
            // 
            Game_Timer.Interval = 20;
            // 
            // pn_road_left
            // 
            pn_road_left.BackColor = SystemColors.ControlText;
            pn_road_left.Controls.Add(ptb_debuff_road_1);
            pn_road_left.Controls.Add(ptb_player1);
            pn_road_left.Controls.Add(ptb_AICar5);
            pn_road_left.Controls.Add(ptb_AICar1);
            pn_road_left.Controls.Add(ptb_buff_road_1);
            pn_road_left.Controls.Add(ptb_road_1_dup);
            pn_road_left.Controls.Add(ptb_road_1);
            pn_road_left.Location = new Point(12, 15);
            pn_road_left.Margin = new Padding(3, 1, 3, 1);
            pn_road_left.Name = "pn_road_left";
            pn_road_left.Size = new Size(617, 919);
            pn_road_left.TabIndex = 0;
            pn_road_left.Paint += Road_Right_Paint;
            // 
            // ptb_debuff_road_1
            // 
            ptb_debuff_road_1.Image = (Image)resources.GetObject("ptb_debuff_road_1.Image");
            ptb_debuff_road_1.Location = new Point(387, -88);
            ptb_debuff_road_1.Margin = new Padding(3, 1, 3, 1);
            ptb_debuff_road_1.Name = "ptb_debuff_road_1";
            ptb_debuff_road_1.Size = new Size(67, 81);
            ptb_debuff_road_1.SizeMode = PictureBoxSizeMode.Zoom;
            ptb_debuff_road_1.TabIndex = 5;
            ptb_debuff_road_1.TabStop = false;
            // 
            // ptb_player1
            // 
            ptb_player1.BackColor = Color.Transparent;
            ptb_player1.ErrorImage = null;
            ptb_player1.Image = (Image)resources.GetObject("ptb_player1.Image");
            ptb_player1.Location = new Point(271, 712);
            ptb_player1.Margin = new Padding(4, 5, 4, 5);
            ptb_player1.Name = "ptb_player1";
            ptb_player1.Size = new Size(80, 175);
            ptb_player1.SizeMode = PictureBoxSizeMode.Zoom;
            ptb_player1.TabIndex = 9;
            ptb_player1.TabStop = false;
            // 
            // ptb_AICar5
            // 
            ptb_AICar5.Image = Properties.Resources.Black_Car;
            ptb_AICar5.Location = new Point(151, 369);
            ptb_AICar5.Margin = new Padding(4, 5, 4, 5);
            ptb_AICar5.Name = "ptb_AICar5";
            ptb_AICar5.Size = new Size(80, 175);
            ptb_AICar5.SizeMode = PictureBoxSizeMode.StretchImage;
            ptb_AICar5.TabIndex = 9;
            ptb_AICar5.TabStop = false;
            // 
            // ptb_AICar1
            // 
            ptb_AICar1.Image = Properties.Resources.Blue_Car;
            ptb_AICar1.Location = new Point(407, 369);
            ptb_AICar1.Margin = new Padding(4, 5, 4, 5);
            ptb_AICar1.Name = "ptb_AICar1";
            ptb_AICar1.Size = new Size(80, 175);
            ptb_AICar1.SizeMode = PictureBoxSizeMode.StretchImage;
            ptb_AICar1.TabIndex = 7;
            ptb_AICar1.TabStop = false;
            // 
            // ptb_buff_road_1
            // 
            ptb_buff_road_1.Image = (Image)resources.GetObject("ptb_buff_road_1.Image");
            ptb_buff_road_1.Location = new Point(120, -88);
            ptb_buff_road_1.Margin = new Padding(3, 1, 3, 1);
            ptb_buff_road_1.Name = "ptb_buff_road_1";
            ptb_buff_road_1.Size = new Size(67, 81);
            ptb_buff_road_1.SizeMode = PictureBoxSizeMode.Zoom;
            ptb_buff_road_1.TabIndex = 6;
            ptb_buff_road_1.TabStop = false;
            // 
            // ptb_road_1_dup
            // 
            ptb_road_1_dup.Image = Properties.Resources.Road;
            ptb_road_1_dup.Location = new Point(0, 919);
            ptb_road_1_dup.Margin = new Padding(3, 1, 3, 1);
            ptb_road_1_dup.Name = "ptb_road_1_dup";
            ptb_road_1_dup.Size = new Size(617, 919);
            ptb_road_1_dup.SizeMode = PictureBoxSizeMode.StretchImage;
            ptb_road_1_dup.TabIndex = 1;
            ptb_road_1_dup.TabStop = false;
            // 
            // ptb_road_1
            // 
            ptb_road_1.Image = Properties.Resources.Road;
            ptb_road_1.Location = new Point(0, -1);
            ptb_road_1.Margin = new Padding(3, 1, 3, 1);
            ptb_road_1.Name = "ptb_road_1";
            ptb_road_1.Size = new Size(617, 919);
            ptb_road_1.SizeMode = PictureBoxSizeMode.StretchImage;
            ptb_road_1.TabIndex = 10;
            ptb_road_1.TabStop = false;
            // 
            // pn_road_right
            // 
            pn_road_right.BackColor = SystemColors.ControlText;
            pn_road_right.Controls.Add(ptb_debuff_road_2);
            pn_road_right.Controls.Add(ptb_AICar6);
            pn_road_right.Controls.Add(ptb_AICar3);
            pn_road_right.Controls.Add(ptb_player2);
            pn_road_right.Controls.Add(ptb_buff_road_2);
            pn_road_right.Controls.Add(ptb_road_2_dup);
            pn_road_right.Controls.Add(ptb_road_2);
            pn_road_right.Location = new Point(660, 15);
            pn_road_right.Margin = new Padding(3, 1, 3, 1);
            pn_road_right.Name = "pn_road_right";
            pn_road_right.Size = new Size(611, 919);
            pn_road_right.TabIndex = 1;
            pn_road_right.Paint += Road_Left_Paint;
            // 
            // ptb_debuff_road_2
            // 
            ptb_debuff_road_2.BackColor = SystemColors.ControlText;
            ptb_debuff_road_2.Image = (Image)resources.GetObject("ptb_debuff_road_2.Image");
            ptb_debuff_road_2.Location = new Point(400, -88);
            ptb_debuff_road_2.Margin = new Padding(3, 1, 3, 1);
            ptb_debuff_road_2.Name = "ptb_debuff_road_2";
            ptb_debuff_road_2.Size = new Size(67, 81);
            ptb_debuff_road_2.SizeMode = PictureBoxSizeMode.Zoom;
            ptb_debuff_road_2.TabIndex = 7;
            ptb_debuff_road_2.TabStop = false;
            // 
            // ptb_AICar6
            // 
            ptb_AICar6.Image = Properties.Resources.Red_Car;
            ptb_AICar6.Location = new Point(403, 369);
            ptb_AICar6.Margin = new Padding(4, 5, 4, 5);
            ptb_AICar6.Name = "ptb_AICar6";
            ptb_AICar6.Size = new Size(80, 175);
            ptb_AICar6.SizeMode = PictureBoxSizeMode.StretchImage;
            ptb_AICar6.TabIndex = 11;
            ptb_AICar6.TabStop = false;
            // 
            // ptb_AICar3
            // 
            ptb_AICar3.Image = Properties.Resources.Orange_Car;
            ptb_AICar3.Location = new Point(144, 369);
            ptb_AICar3.Margin = new Padding(4, 5, 4, 5);
            ptb_AICar3.Name = "ptb_AICar3";
            ptb_AICar3.Size = new Size(80, 175);
            ptb_AICar3.SizeMode = PictureBoxSizeMode.StretchImage;
            ptb_AICar3.TabIndex = 9;
            ptb_AICar3.TabStop = false;
            // 
            // ptb_player2
            // 
            ptb_player2.BackColor = Color.Transparent;
            ptb_player2.Image = (Image)resources.GetObject("ptb_player2.Image");
            ptb_player2.Location = new Point(265, 721);
            ptb_player2.Margin = new Padding(4, 5, 4, 5);
            ptb_player2.Name = "ptb_player2";
            ptb_player2.Size = new Size(80, 175);
            ptb_player2.SizeMode = PictureBoxSizeMode.Zoom;
            ptb_player2.TabIndex = 10;
            ptb_player2.TabStop = false;
            // 
            // ptb_buff_road_2
            // 
            ptb_buff_road_2.BackColor = SystemColors.ControlText;
            ptb_buff_road_2.Image = (Image)resources.GetObject("ptb_buff_road_2.Image");
            ptb_buff_road_2.Location = new Point(131, -88);
            ptb_buff_road_2.Margin = new Padding(3, 1, 3, 1);
            ptb_buff_road_2.Name = "ptb_buff_road_2";
            ptb_buff_road_2.Size = new Size(67, 81);
            ptb_buff_road_2.SizeMode = PictureBoxSizeMode.Zoom;
            ptb_buff_road_2.TabIndex = 8;
            ptb_buff_road_2.TabStop = false;
            // 
            // ptb_road_2_dup
            // 
            ptb_road_2_dup.Image = Properties.Resources.Road;
            ptb_road_2_dup.Location = new Point(0, 919);
            ptb_road_2_dup.Margin = new Padding(3, 1, 3, 1);
            ptb_road_2_dup.Name = "ptb_road_2_dup";
            ptb_road_2_dup.Size = new Size(611, 919);
            ptb_road_2_dup.SizeMode = PictureBoxSizeMode.StretchImage;
            ptb_road_2_dup.TabIndex = 1;
            ptb_road_2_dup.TabStop = false;
            // 
            // ptb_road_2
            // 
            ptb_road_2.BackColor = SystemColors.ControlText;
            ptb_road_2.Image = Properties.Resources.Road;
            ptb_road_2.Location = new Point(0, 1);
            ptb_road_2.Margin = new Padding(3, 1, 3, 1);
            ptb_road_2.Name = "ptb_road_2";
            ptb_road_2.Size = new Size(611, 919);
            ptb_road_2.SizeMode = PictureBoxSizeMode.StretchImage;
            ptb_road_2.TabIndex = 1;
            ptb_road_2.TabStop = false;
            // 
            // btn_Scoreboard
            // 
            btn_Scoreboard.BackColor = Color.Transparent;
            btn_Scoreboard.BorderRadius = 20;
            btn_Scoreboard.CustomizableEdges = customizableEdges7;
            btn_Scoreboard.DisabledState.BorderColor = Color.DarkGray;
            btn_Scoreboard.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Scoreboard.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Scoreboard.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            btn_Scoreboard.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Scoreboard.FillColor = Color.DarkBlue;
            btn_Scoreboard.FillColor2 = Color.Purple;
            btn_Scoreboard.Font = new Font("Segoe UI Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Scoreboard.ForeColor = Color.White;
            btn_Scoreboard.Location = new Point(119, 988);
            btn_Scoreboard.Margin = new Padding(3, 1, 3, 1);
            btn_Scoreboard.Name = "btn_Scoreboard";
            btn_Scoreboard.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btn_Scoreboard.Size = new Size(380, 120);
            btn_Scoreboard.TabIndex = 4;
            btn_Scoreboard.Text = "🏆 SCORE BOARD 🏆";
            btn_Scoreboard.Click += btn_Scoreboard_Click;
            // 
            // btn_Ready
            // 
            btn_Ready.BackColor = Color.Transparent;
            btn_Ready.BorderRadius = 20;
            btn_Ready.CustomizableEdges = customizableEdges9;
            btn_Ready.DisabledState.BorderColor = Color.DarkGray;
            btn_Ready.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Ready.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Ready.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            btn_Ready.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Ready.FillColor = Color.FromArgb(0, 192, 0);
            btn_Ready.FillColor2 = Color.Teal;
            btn_Ready.Font = new Font("Segoe UI Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btn_Ready.ForeColor = Color.White;
            btn_Ready.Location = new Point(530, 988);
            btn_Ready.Margin = new Padding(3, 1, 3, 1);
            btn_Ready.Name = "btn_Ready";
            btn_Ready.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btn_Ready.Size = new Size(224, 120);
            btn_Ready.TabIndex = 5;
            btn_Ready.Text = "READY";
            btn_Ready.Click += btn_Ready_Click;
            // 
            // btn_ID
            // 
            btn_ID.BackColor = Color.Transparent;
            btn_ID.BorderRadius = 20;
            btn_ID.CustomizableEdges = customizableEdges11;
            btn_ID.FillColor = Color.DarkBlue;
            btn_ID.FillColor2 = Color.Purple;
            btn_ID.Font = new Font("Segoe UI Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_ID.ForeColor = Color.White;
            btn_ID.Location = new Point(779, 988);
            btn_ID.Margin = new Padding(3, 1, 3, 1);
            btn_ID.Name = "btn_ID";
            btn_ID.ShadowDecoration.CustomizableEdges = customizableEdges12;
            btn_ID.Size = new Size(380, 120);
            btn_ID.TabIndex = 12;
            btn_ID.Text = "ID:";
            // 
            // lbl_P2_Status
            // 
            lbl_P2_Status.AutoSize = true;
            lbl_P2_Status.BackColor = Color.Transparent;
            lbl_P2_Status.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_P2_Status.ForeColor = Color.WhiteSmoke;
            lbl_P2_Status.Location = new Point(854, 934);
            lbl_P2_Status.Margin = new Padding(4, 0, 4, 0);
            lbl_P2_Status.Name = "lbl_P2_Status";
            lbl_P2_Status.Size = new Size(251, 29);
            lbl_P2_Status.TabIndex = 7;
            lbl_P2_Status.Text = "Waiting for player 2";
            // 
            // lbl_Countdown
            // 
            lbl_Countdown.AutoSize = true;
            lbl_Countdown.BackColor = Color.Transparent;
            lbl_Countdown.Font = new Font("Microsoft Sans Serif", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Countdown.ForeColor = Color.WhiteSmoke;
            lbl_Countdown.Location = new Point(625, 934);
            lbl_Countdown.Margin = new Padding(4, 0, 4, 0);
            lbl_Countdown.Name = "lbl_Countdown";
            lbl_Countdown.Size = new Size(40, 42);
            lbl_Countdown.TabIndex = 8;
            lbl_Countdown.Text = "5";
            lbl_Countdown.Visible = false;
            // 
            // lbl_GameTimer
            // 
            lbl_GameTimer.AutoSize = true;
            lbl_GameTimer.BackColor = Color.Transparent;
            lbl_GameTimer.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_GameTimer.ForeColor = Color.WhiteSmoke;
            lbl_GameTimer.Location = new Point(585, 934);
            lbl_GameTimer.Margin = new Padding(4, 0, 4, 0);
            lbl_GameTimer.Name = "lbl_GameTimer";
            lbl_GameTimer.Size = new Size(120, 29);
            lbl_GameTimer.TabIndex = 9;
            lbl_GameTimer.Text = "Time: 60";
            lbl_GameTimer.Visible = false;
            // 
            // lbl_Score1
            // 
            lbl_Score1.BackColor = Color.Transparent;
            lbl_Score1.Font = new Font("Microsoft Sans Serif", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Score1.ForeColor = Color.WhiteSmoke;
            lbl_Score1.Location = new Point(217, 934);
            lbl_Score1.Name = "lbl_Score1";
            lbl_Score1.Size = new Size(251, 39);
            lbl_Score1.TabIndex = 10;
            lbl_Score1.Text = "Score:";
            // 
            // lbl_Score2
            // 
            lbl_Score2.BackColor = Color.Transparent;
            lbl_Score2.Font = new Font("Microsoft Sans Serif", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_Score2.ForeColor = Color.WhiteSmoke;
            lbl_Score2.Location = new Point(854, 936);
            lbl_Score2.Name = "lbl_Score2";
            lbl_Score2.Size = new Size(251, 34);
            lbl_Score2.TabIndex = 11;
            lbl_Score2.Text = "Score:";
            // 
            // lbl_P1_Status
            // 
            lbl_P1_Status.AutoSize = true;
            lbl_P1_Status.BackColor = Color.Transparent;
            lbl_P1_Status.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_P1_Status.ForeColor = Color.WhiteSmoke;
            lbl_P1_Status.Location = new Point(217, 934);
            lbl_P1_Status.Margin = new Padding(4, 0, 4, 0);
            lbl_P1_Status.Name = "lbl_P1_Status";
            lbl_P1_Status.Size = new Size(251, 29);
            lbl_P1_Status.TabIndex = 6;
            lbl_P1_Status.Text = "Waiting for player 1";
            // 
            // Game_Window
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Background_Game_Play;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1285, 1124);
            Controls.Add(btn_ID);
            Controls.Add(lbl_Score2);
            Controls.Add(lbl_Score1);
            Controls.Add(lbl_Countdown);
            Controls.Add(lbl_P2_Status);
            Controls.Add(lbl_P1_Status);
            Controls.Add(btn_Ready);
            Controls.Add(btn_Scoreboard);
            Controls.Add(pn_road_right);
            Controls.Add(pn_road_left);
            Controls.Add(lbl_GameTimer);
            KeyPreview = true;
            Margin = new Padding(3, 1, 3, 1);
            MaximizeBox = false;
            Name = "Game_Window";
            Text = "Game Play";
            FormClosing += Game_Window_FormClosing;
            Load += Game_Window_Load;
            KeyDown += Game_Window_KeyDown;
            KeyUp += Game_Window_KeyUp;
            pn_road_left.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ptb_debuff_road_1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_player1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_AICar5).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_AICar1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_buff_road_1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_road_1_dup).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_road_1).EndInit();
            pn_road_right.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ptb_debuff_road_2).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_AICar6).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_AICar3).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_player2).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_buff_road_2).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_road_2_dup).EndInit();
            ((System.ComponentModel.ISupportInitialize)ptb_road_2).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }



        private void game_timer_Tick(object sender, EventArgs e)

        {

            throw new NotImplementedException();

        }



        #endregion

        private System.Windows.Forms.Timer Game_Timer;

        private System.Windows.Forms.PictureBox ptb_road_1_dup;

        private System.Windows.Forms.PictureBox ptb_buff_road_1;

        private System.Windows.Forms.PictureBox ptb_debuff_road_1;

        private System.Windows.Forms.Panel pn_road_left;

        private System.Windows.Forms.PictureBox ptb_road_2_dup;

        private System.Windows.Forms.PictureBox ptb_buff_road_2;

        private System.Windows.Forms.PictureBox ptb_debuff_road_2;

        private System.Windows.Forms.Panel pn_road_right;

        private System.Windows.Forms.PictureBox ptb_player1;

        private System.Windows.Forms.PictureBox ptb_player2;

        private System.Windows.Forms.PictureBox ptb_AICar6;

        private System.Windows.Forms.PictureBox ptb_AICar5;

        private System.Windows.Forms.PictureBox ptb_AICar3;

        private System.Windows.Forms.PictureBox ptb_AICar1;

        private System.Windows.Forms.PictureBox ptb_road_2;

        private Guna.UI2.WinForms.Guna2GradientButton btn_Scoreboard;

        private System.Windows.Forms.PictureBox ptb_road_1;

        private Guna.UI2.WinForms.Guna2GradientButton btn_Ready;

        private System.Windows.Forms.Label lbl_P2_Status;

        private System.Windows.Forms.Label lbl_Countdown;

        private System.Windows.Forms.Label lbl_GameTimer;

        private System.Windows.Forms.Label lbl_Score1;

        private System.Windows.Forms.Label lbl_Score2;

        private Guna.UI2.WinForms.Guna2GradientButton btn_ID;
        private System.Windows.Forms.Label lbl_P1_Status;
    }

}