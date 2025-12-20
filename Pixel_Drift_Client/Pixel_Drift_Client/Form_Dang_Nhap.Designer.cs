using System.Drawing;
using System.Windows.Forms;

namespace Pixel_Drift
{
    partial class Form_Dang_Nhap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Dang_Nhap));
            this.lb_pass = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lb_user = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lb_dangnhap = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.textBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            this.textBox2 = new Guna.UI2.WinForms.Guna2TextBox();
            this.btn_vaogame = new Guna.UI2.WinForms.Guna2Button();
            this.btn_quenmatkhau = new Guna.UI2.WinForms.Guna2Button();
            this.btn_backdk = new Guna.UI2.WinForms.Guna2Button();
            this.Panel_Chua_Form = new Guna.UI2.WinForms.Guna2Panel();
            this.Panel_Chua_Form.SuspendLayout();
            this.SuspendLayout();
            // 
            // lb_pass
            // 
            this.lb_pass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_pass.AutoSize = false;
            this.lb_pass.BackColor = System.Drawing.Color.Transparent;
            this.lb_pass.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_pass.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lb_pass.Location = new System.Drawing.Point(35, 181);
            this.lb_pass.Margin = new System.Windows.Forms.Padding(4);
            this.lb_pass.Name = "lb_pass";
            this.lb_pass.Size = new System.Drawing.Size(153, 32);
            this.lb_pass.TabIndex = 7;
            this.lb_pass.Text = "Mật khẩu";
            // 
            // lb_user
            // 
            this.lb_user.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_user.AutoSize = false;
            this.lb_user.BackColor = System.Drawing.Color.Transparent;
            this.lb_user.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_user.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lb_user.Location = new System.Drawing.Point(35, 94);
            this.lb_user.Margin = new System.Windows.Forms.Padding(4);
            this.lb_user.Name = "lb_user";
            this.lb_user.Size = new System.Drawing.Size(222, 33);
            this.lb_user.TabIndex = 8;
            this.lb_user.Text = "Tên đăng nhập";
            // 
            // lb_dangnhap
            // 
            this.lb_dangnhap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_dangnhap.AutoSize = false;
            this.lb_dangnhap.BackColor = System.Drawing.Color.Transparent;
            this.lb_dangnhap.Font = new System.Drawing.Font("Arial Black", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_dangnhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lb_dangnhap.Location = new System.Drawing.Point(198, 23);
            this.lb_dangnhap.Margin = new System.Windows.Forms.Padding(4);
            this.lb_dangnhap.Name = "lb_dangnhap";
            this.lb_dangnhap.Size = new System.Drawing.Size(211, 48);
            this.lb_dangnhap.TabIndex = 9;
            this.lb_dangnhap.Text = "LOGIN";
            // 
            // textBox1
            // 
            this.textBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBox1.BorderRadius = 8;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox1.DefaultText = "";
            this.textBox1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.textBox1.FocusedState.BorderColor = System.Drawing.Color.Aqua;
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.textBox1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textBox1.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox1.Location = new System.Drawing.Point(35, 125);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.PlaceholderText = "Nhập tên đăng nhập";
            this.textBox1.SelectedText = "";
            this.textBox1.Size = new System.Drawing.Size(427, 44);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.textBox2.BorderRadius = 8;
            this.textBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.textBox2.DefaultText = "";
            this.textBox2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.textBox2.FocusedState.BorderColor = System.Drawing.Color.Aqua;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.textBox2.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.textBox2.Location = new System.Drawing.Point(35, 211);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Name = "textBox2";
            this.textBox2.PasswordChar = '*';
            this.textBox2.PlaceholderText = "Nhập mật khẩu";
            this.textBox2.SelectedText = "";
            this.textBox2.Size = new System.Drawing.Size(427, 44);
            this.textBox2.TabIndex = 4;
            // 
            // btn_vaogame
            // 
            this.btn_vaogame.BorderColor = System.Drawing.Color.Orange;
            this.btn_vaogame.BorderRadius = 15;
            this.btn_vaogame.BorderThickness = 3;
            this.btn_vaogame.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_vaogame.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_vaogame.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_vaogame.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_vaogame.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_vaogame.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_vaogame.ForeColor = System.Drawing.Color.White;
            this.btn_vaogame.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn_vaogame.Location = new System.Drawing.Point(89, 279);
            this.btn_vaogame.Margin = new System.Windows.Forms.Padding(4);
            this.btn_vaogame.Name = "btn_vaogame";
            this.btn_vaogame.Size = new System.Drawing.Size(320, 55);
            this.btn_vaogame.TabIndex = 5;
            this.btn_vaogame.Text = "ĐĂNG NHẬP";
            this.btn_vaogame.Click += new System.EventHandler(this.btn_vaogame_Click);
            // 
            // btn_quenmatkhau
            // 
            this.btn_quenmatkhau.BackColor = System.Drawing.Color.Transparent;
            this.btn_quenmatkhau.BorderColor = System.Drawing.Color.Transparent;
            this.btn_quenmatkhau.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_quenmatkhau.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_quenmatkhau.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_quenmatkhau.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_quenmatkhau.FillColor = System.Drawing.Color.Transparent;
            this.btn_quenmatkhau.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_quenmatkhau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this.btn_quenmatkhau.HoverState.ForeColor = System.Drawing.Color.Lime;
            this.btn_quenmatkhau.Location = new System.Drawing.Point(9, 347);
            this.btn_quenmatkhau.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btn_quenmatkhau.Name = "btn_quenmatkhau";
            this.btn_quenmatkhau.Size = new System.Drawing.Size(213, 31);
            this.btn_quenmatkhau.TabIndex = 0;
            this.btn_quenmatkhau.Text = "Quên / Đổi mật khẩu";
            this.btn_quenmatkhau.Click += new System.EventHandler(this.btn_quenmatkhau_Click);
            // 
            // btn_backdk
            // 
            this.btn_backdk.BackColor = System.Drawing.Color.Transparent;
            this.btn_backdk.BorderColor = System.Drawing.Color.Transparent;
            this.btn_backdk.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_backdk.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_backdk.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_backdk.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_backdk.FillColor = System.Drawing.Color.Transparent;
            this.btn_backdk.Font = new System.Drawing.Font("Segoe UI", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_backdk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btn_backdk.HoverState.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_backdk.Location = new System.Drawing.Point(222, 347);
            this.btn_backdk.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.btn_backdk.Name = "btn_backdk";
            this.btn_backdk.Size = new System.Drawing.Size(267, 31);
            this.btn_backdk.TabIndex = 6;
            this.btn_backdk.Text = "Chưa có tài khoản? Đăng kí";
            this.btn_backdk.Click += new System.EventHandler(this.btn_backdk_Click);
            // 
            // Panel_Chua_Form
            // 
            this.Panel_Chua_Form.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Chua_Form.BorderColor = System.Drawing.Color.Aquamarine;
            this.Panel_Chua_Form.BorderRadius = 15;
            this.Panel_Chua_Form.BorderThickness = 2;
            this.Panel_Chua_Form.Controls.Add(this.btn_backdk);
            this.Panel_Chua_Form.Controls.Add(this.btn_quenmatkhau);
            this.Panel_Chua_Form.Controls.Add(this.btn_vaogame);
            this.Panel_Chua_Form.Controls.Add(this.textBox2);
            this.Panel_Chua_Form.Controls.Add(this.textBox1);
            this.Panel_Chua_Form.Controls.Add(this.lb_pass);
            this.Panel_Chua_Form.Controls.Add(this.lb_user);
            this.Panel_Chua_Form.Controls.Add(this.lb_dangnhap);
            this.Panel_Chua_Form.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Panel_Chua_Form.Location = new System.Drawing.Point(84, 77);
            this.Panel_Chua_Form.Margin = new System.Windows.Forms.Padding(4);
            this.Panel_Chua_Form.Name = "Panel_Chua_Form";
            this.Panel_Chua_Form.Size = new System.Drawing.Size(504, 393);
            this.Panel_Chua_Form.TabIndex = 0;
            // 
            // Form_Dang_Nhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(20)))), ((int)(((byte)(30)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(667, 554);
            this.Controls.Add(this.Panel_Chua_Form);
            this.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.MaximizeBox = false;
            this.Name = "Form_Dang_Nhap";
            this.Text = "Đăng Nhập";
            this.Panel_Chua_Form.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2HtmlLabel lb_pass;
        private Guna.UI2.WinForms.Guna2HtmlLabel lb_user;
        private Guna.UI2.WinForms.Guna2HtmlLabel lb_dangnhap;
        private Guna.UI2.WinForms.Guna2TextBox textBox1;
        private Guna.UI2.WinForms.Guna2TextBox textBox2;
        private Guna.UI2.WinForms.Guna2Button btn_vaogame;
        private Guna.UI2.WinForms.Guna2Button btn_quenmatkhau;
        private Guna.UI2.WinForms.Guna2Button btn_backdk;
        private Guna.UI2.WinForms.Guna2Panel Panel_Chua_Form;
    }
}
