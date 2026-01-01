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
            this.btn_vao_game = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_CardName = new System.Windows.Forms.Label();
            this.ptb_Avatar = new System.Windows.Forms.PictureBox();
            this.lbl_username = new System.Windows.Forms.Label();
            this.lbl_email = new System.Windows.Forms.Label();
            this.lbl_birthday = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Avatar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_vao_game
            // 
            this.btn_vao_game.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btn_vao_game.Font = new System.Drawing.Font("Times New Roman", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_vao_game.ForeColor = System.Drawing.Color.OrangeRed;
            this.btn_vao_game.Location = new System.Drawing.Point(318, 501);
            this.btn_vao_game.Margin = new System.Windows.Forms.Padding(4);
            this.btn_vao_game.Name = "btn_vao_game";
            this.btn_vao_game.Size = new System.Drawing.Size(233, 63);
            this.btn_vao_game.TabIndex = 0;
            this.btn_vao_game.Text = "Lets go!";
            this.btn_vao_game.UseVisualStyleBackColor = false;
            this.btn_vao_game.Click += new System.EventHandler(this.btn_vao_game_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BackgroundImage = global::Pixel_Drift.Properties.Resources.The_Sinh_Vien;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lbl_CardName);
            this.panel1.Controls.Add(this.ptb_Avatar);
            this.panel1.Controls.Add(this.lbl_username);
            this.panel1.Controls.Add(this.lbl_email);
            this.panel1.Controls.Add(this.lbl_birthday);
            this.panel1.Location = new System.Drawing.Point(184, 149);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(498, 275);
            this.panel1.TabIndex = 5;
            // 
            // lbl_CardName
            // 
            this.lbl_CardName.BackColor = System.Drawing.Color.Transparent;
            this.lbl_CardName.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_CardName.ForeColor = System.Drawing.Color.Green;
            this.lbl_CardName.Location = new System.Drawing.Point(258, 69);
            this.lbl_CardName.Name = "lbl_CardName";
            this.lbl_CardName.Size = new System.Drawing.Size(162, 35);
            this.lbl_CardName.TabIndex = 6;
            this.lbl_CardName.Text = "USER IN4";
            this.lbl_CardName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ptb_Avatar
            // 
            this.ptb_Avatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ptb_Avatar.Image = global::Pixel_Drift.Properties.Resources.Avt_Player;
            this.ptb_Avatar.Location = new System.Drawing.Point(3, 94);
            this.ptb_Avatar.Name = "ptb_Avatar";
            this.ptb_Avatar.Size = new System.Drawing.Size(137, 150);
            this.ptb_Avatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptb_Avatar.TabIndex = 5;
            this.ptb_Avatar.TabStop = false;
            // 
            // lbl_username
            // 
            this.lbl_username.BackColor = System.Drawing.Color.Transparent;
            this.lbl_username.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_username.ForeColor = System.Drawing.Color.Black;
            this.lbl_username.Location = new System.Drawing.Point(230, 104);
            this.lbl_username.Name = "lbl_username";
            this.lbl_username.Size = new System.Drawing.Size(226, 36);
            this.lbl_username.TabIndex = 3;
            this.lbl_username.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_email
            // 
            this.lbl_email.BackColor = System.Drawing.Color.Transparent;
            this.lbl_email.Font = new System.Drawing.Font("Times New Roman", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_email.ForeColor = System.Drawing.Color.Black;
            this.lbl_email.Location = new System.Drawing.Point(201, 173);
            this.lbl_email.Name = "lbl_email";
            this.lbl_email.Size = new System.Drawing.Size(292, 33);
            this.lbl_email.TabIndex = 4;
            this.lbl_email.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_birthday
            // 
            this.lbl_birthday.BackColor = System.Drawing.Color.Transparent;
            this.lbl_birthday.Font = new System.Drawing.Font("Times New Roman", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_birthday.ForeColor = System.Drawing.Color.Black;
            this.lbl_birthday.Location = new System.Drawing.Point(230, 140);
            this.lbl_birthday.Name = "lbl_birthday";
            this.lbl_birthday.Size = new System.Drawing.Size(226, 33);
            this.lbl_birthday.TabIndex = 2;
            this.lbl_birthday.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Pixel_Drift.Properties.Resources.Background_Information;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(870, 613);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // Form_Thong_Tin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 26F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(870, 613);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btn_vao_game);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Times New Roman", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form_Thong_Tin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Thong_Tin_FormClosed);
            this.Load += new System.EventHandler(this.Form_Thong_Tin_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptb_Avatar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

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
