using System.Drawing;
using System.Windows.Forms;

namespace Pixel_Drift
{
    partial class Form_Thong_Tin
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
            btn_vao_game = new Button();
            panel1 = new Panel();
            lbl_CardName = new Label();
            lbl_email = new Label();
            lbl_birthday = new Label();
            ptb_Avatar = new PictureBox();
            lbl_username = new Label();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ptb_Avatar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btn_vao_game
            // 
            btn_vao_game.BackColor = SystemColors.ControlLightLight;
            btn_vao_game.Font = new Font("Consolas", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_vao_game.ForeColor = Color.OrangeRed;
            btn_vao_game.Location = new Point(442, 679);
            btn_vao_game.Margin = new Padding(4);
            btn_vao_game.Name = "btn_vao_game";
            btn_vao_game.Size = new Size(325, 82);
            btn_vao_game.TabIndex = 0;
            btn_vao_game.Text = "Let's go!";
            btn_vao_game.UseVisualStyleBackColor = false;
            btn_vao_game.Click += btn_vao_game_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BackgroundImage = Properties.Resources.The_Sinh_Vien;
            panel1.BackgroundImageLayout = ImageLayout.Stretch;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(lbl_CardName);
            panel1.Controls.Add(lbl_email);
            panel1.Controls.Add(lbl_birthday);
            panel1.Controls.Add(ptb_Avatar);
            panel1.Controls.Add(lbl_username);
            panel1.Location = new Point(260, 193);
            panel1.Name = "panel1";
            panel1.Size = new Size(688, 379);
            panel1.TabIndex = 5;
            // 
            // lbl_CardName
            // 
            lbl_CardName.BackColor = Color.Transparent;
            lbl_CardName.Font = new Font("Consolas", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbl_CardName.ForeColor = Color.Green;
            lbl_CardName.Location = new Point(324, 95);
            lbl_CardName.Name = "lbl_CardName";
            lbl_CardName.Size = new Size(344, 35);
            lbl_CardName.TabIndex = 6;
            lbl_CardName.Text = "USER'S INFOMATION";
            lbl_CardName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbl_email
            // 
            lbl_email.BackColor = Color.Transparent;
            lbl_email.Font = new Font("Consolas", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_email.ForeColor = Color.Black;
            lbl_email.Location = new Point(324, 251);
            lbl_email.Name = "lbl_email";
            lbl_email.Size = new Size(344, 33);
            lbl_email.TabIndex = 4;
            lbl_email.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lbl_birthday
            // 
            lbl_birthday.BackColor = Color.Transparent;
            lbl_birthday.Font = new Font("Consolas", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_birthday.ForeColor = Color.Black;
            lbl_birthday.Location = new Point(324, 198);
            lbl_birthday.Name = "lbl_birthday";
            lbl_birthday.Size = new Size(344, 33);
            lbl_birthday.TabIndex = 2;
            lbl_birthday.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ptb_Avatar
            // 
            ptb_Avatar.BorderStyle = BorderStyle.FixedSingle;
            ptb_Avatar.Image = Properties.Resources.Avt_Player;
            ptb_Avatar.Location = new Point(3, 128);
            ptb_Avatar.Name = "ptb_Avatar";
            ptb_Avatar.Size = new Size(177, 207);
            ptb_Avatar.SizeMode = PictureBoxSizeMode.StretchImage;
            ptb_Avatar.TabIndex = 5;
            ptb_Avatar.TabStop = false;
            // 
            // lbl_username
            // 
            lbl_username.BackColor = Color.Transparent;
            lbl_username.Font = new Font("Consolas", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbl_username.ForeColor = Color.Black;
            lbl_username.Location = new Point(324, 144);
            lbl_username.Name = "lbl_username";
            lbl_username.Size = new Size(344, 36);
            lbl_username.TabIndex = 3;
            lbl_username.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.Background_Information;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1210, 828);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // Form_Thong_Tin
            // 
            AcceptButton = btn_vao_game;
            AutoScaleDimensions = new SizeF(13F, 26F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AliceBlue;
            BackgroundImageLayout = ImageLayout.None;
            ClientSize = new Size(1208, 828);
            Controls.Add(panel1);
            Controls.Add(btn_vao_game);
            Controls.Add(pictureBox1);
            Font = new Font("Times New Roman", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ForeColor = Color.DarkGoldenrod;
            Margin = new Padding(4);
            MaximizeBox = false;
            Name = "Form_Thong_Tin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Thông tin ";
            FormClosed += Form_Thong_Tin_FormClosed;
            Load += Form_Thong_Tin_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ptb_Avatar).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_birthday;
        private System.Windows.Forms.Label lbl_email;
        private System.Windows.Forms.Label lbl_username;
        private System.Windows.Forms.Button btn_vao_game;
        private Panel panel1;
        private PictureBox ptb_Avatar;
        private Label lbl_CardName;
        private PictureBox pictureBox1;
    }
}
