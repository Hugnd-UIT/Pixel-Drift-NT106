using System.Drawing;

namespace Pixel_Drift
{
    partial class Form_Dang_Ki
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Dang_Ki));
            this.label1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.label2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.label3 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.label4 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.label5 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.tb_emailsdt = new Guna.UI2.WinForms.Guna2TextBox();
            this.tb_xacnhanmk = new Guna.UI2.WinForms.Guna2TextBox();
            this.tb_matkhau = new Guna.UI2.WinForms.Guna2TextBox();
            this.tb_tendangnhap = new Guna.UI2.WinForms.Guna2TextBox();
            this.btn_xacnhan = new Guna.UI2.WinForms.Guna2Button();
            this.tb_BirthDay = new Guna.UI2.WinForms.Guna2TextBox();
            this.label6 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btn_backdn = new Guna.UI2.WinForms.Guna2Button();
            this.Panel_Chua_Form = new Guna.UI2.WinForms.Guna2Panel();
            this.Panel_Chua_Form.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Arial Black", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label1.Location = new System.Drawing.Point(290, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 76);
            this.label1.TabIndex = 11;
            this.label1.Text = "ĐĂNG KÍ";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label2.Location = new System.Drawing.Point(60, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 25);
            this.label2.TabIndex = 15;
            this.label2.Text = "Email *";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label3.Location = new System.Drawing.Point(60, 270);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 25);
            this.label3.TabIndex = 14;
            this.label3.Text = "Tên đăng nhập *";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label4.Location = new System.Drawing.Point(60, 350);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 25);
            this.label4.TabIndex = 13;
            this.label4.Text = "Mật khẩu *";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label5.Location = new System.Drawing.Point(60, 430);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(169, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "Xác nhận mật khẩu *";
            // 
            // tb_emailsdt
            // 
            this.tb_emailsdt.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tb_emailsdt.BorderRadius = 8;
            this.tb_emailsdt.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_emailsdt.DefaultText = "";
            this.tb_emailsdt.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.tb_emailsdt.FocusedState.BorderColor = System.Drawing.Color.Aqua;
            this.tb_emailsdt.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tb_emailsdt.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tb_emailsdt.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tb_emailsdt.Location = new System.Drawing.Point(60, 140);
            this.tb_emailsdt.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.tb_emailsdt.Name = "tb_emailsdt";
            this.tb_emailsdt.PlaceholderText = "Email/SĐT";
            this.tb_emailsdt.SelectedText = "";
            this.tb_emailsdt.Size = new System.Drawing.Size(744, 36);
            this.tb_emailsdt.TabIndex = 5;
            // 
            // tb_xacnhanmk
            // 
            this.tb_xacnhanmk.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tb_xacnhanmk.BorderRadius = 8;
            this.tb_xacnhanmk.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_xacnhanmk.DefaultText = "";
            this.tb_xacnhanmk.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.tb_xacnhanmk.FocusedState.BorderColor = System.Drawing.Color.Aqua;
            this.tb_xacnhanmk.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tb_xacnhanmk.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tb_xacnhanmk.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tb_xacnhanmk.Location = new System.Drawing.Point(60, 460);
            this.tb_xacnhanmk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.tb_xacnhanmk.Name = "tb_xacnhanmk";
            this.tb_xacnhanmk.PasswordChar = '*';
            this.tb_xacnhanmk.PlaceholderText = "Xác nhận mật khẩu";
            this.tb_xacnhanmk.SelectedText = "";
            this.tb_xacnhanmk.Size = new System.Drawing.Size(744, 36);
            this.tb_xacnhanmk.TabIndex = 8;
            // 
            // tb_matkhau
            // 
            this.tb_matkhau.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tb_matkhau.BorderRadius = 8;
            this.tb_matkhau.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_matkhau.DefaultText = "";
            this.tb_matkhau.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.tb_matkhau.FocusedState.BorderColor = System.Drawing.Color.Aqua;
            this.tb_matkhau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tb_matkhau.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tb_matkhau.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tb_matkhau.Location = new System.Drawing.Point(60, 380);
            this.tb_matkhau.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.tb_matkhau.Name = "tb_matkhau";
            this.tb_matkhau.PasswordChar = '*';
            this.tb_matkhau.PlaceholderText = "Mật khẩu";
            this.tb_matkhau.SelectedText = "";
            this.tb_matkhau.Size = new System.Drawing.Size(744, 36);
            this.tb_matkhau.TabIndex = 7;
            // 
            // tb_tendangnhap
            // 
            this.tb_tendangnhap.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tb_tendangnhap.BorderRadius = 8;
            this.tb_tendangnhap.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_tendangnhap.DefaultText = "";
            this.tb_tendangnhap.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.tb_tendangnhap.FocusedState.BorderColor = System.Drawing.Color.Aqua;
            this.tb_tendangnhap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tb_tendangnhap.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tb_tendangnhap.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tb_tendangnhap.Location = new System.Drawing.Point(60, 300);
            this.tb_tendangnhap.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.tb_tendangnhap.Name = "tb_tendangnhap";
            this.tb_tendangnhap.PlaceholderText = "Tên đăng nhập";
            this.tb_tendangnhap.SelectedText = "";
            this.tb_tendangnhap.Size = new System.Drawing.Size(744, 36);
            this.tb_tendangnhap.TabIndex = 6;
            // 
            // btn_xacnhan
            // 
            this.btn_xacnhan.BorderColor = System.Drawing.Color.Orange;
            this.btn_xacnhan.BorderRadius = 15;
            this.btn_xacnhan.BorderThickness = 3;
            this.btn_xacnhan.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_xacnhan.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_xacnhan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_xacnhan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_xacnhan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btn_xacnhan.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_xacnhan.ForeColor = System.Drawing.Color.White;
            this.btn_xacnhan.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.btn_xacnhan.Location = new System.Drawing.Point(200, 520);
            this.btn_xacnhan.Name = "btn_xacnhan";
            this.btn_xacnhan.Size = new System.Drawing.Size(464, 45);
            this.btn_xacnhan.TabIndex = 9;
            this.btn_xacnhan.Text = "Xác nhận";
            this.btn_xacnhan.Click += new System.EventHandler(this.btn_xacnhan_Click);
            // 
            // tb_BirthDay
            // 
            this.tb_BirthDay.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.tb_BirthDay.BorderRadius = 8;
            this.tb_BirthDay.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_BirthDay.DefaultText = "";
            this.tb_BirthDay.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.tb_BirthDay.FocusedState.BorderColor = System.Drawing.Color.Aqua;
            this.tb_BirthDay.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tb_BirthDay.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tb_BirthDay.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tb_BirthDay.Location = new System.Drawing.Point(60, 220);
            this.tb_BirthDay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.tb_BirthDay.Name = "tb_BirthDay";
            this.tb_BirthDay.PlaceholderText = "DD/MM/YYYY";
            this.tb_BirthDay.SelectedText = "";
            this.tb_BirthDay.Size = new System.Drawing.Size(744, 36);
            this.tb_BirthDay.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label6.Location = new System.Drawing.Point(60, 190);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 25);
            this.label6.TabIndex = 11;
            this.label6.Text = "Ngày sinh";
            // 
            // btn_backdn
            // 
            this.btn_backdn.BackColor = System.Drawing.Color.Transparent;
            this.btn_backdn.BorderColor = System.Drawing.Color.Transparent;
            this.btn_backdn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_backdn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_backdn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_backdn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_backdn.FillColor = System.Drawing.Color.Transparent;
            this.btn_backdn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_backdn.ForeColor = System.Drawing.Color.Chartreuse;
            this.btn_backdn.HoverState.ForeColor = System.Drawing.Color.Lime;
            this.btn_backdn.Location = new System.Drawing.Point(200, 580);
            this.btn_backdn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_backdn.Name = "btn_backdn";
            this.btn_backdn.Size = new System.Drawing.Size(464, 30);
            this.btn_backdn.TabIndex = 10;
            this.btn_backdn.Text = "Quay lại đăng nhập";
            this.btn_backdn.Click += new System.EventHandler(this.btn_backdn_Click);
            // 
            // Panel_Chua_Form
            // 
            this.Panel_Chua_Form.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Chua_Form.BorderColor = System.Drawing.Color.White;
            this.Panel_Chua_Form.BorderRadius = 15;
            this.Panel_Chua_Form.BorderThickness = 3;
            this.Panel_Chua_Form.Controls.Add(this.btn_backdn);
            this.Panel_Chua_Form.Controls.Add(this.label1);
            this.Panel_Chua_Form.Controls.Add(this.btn_xacnhan);
            this.Panel_Chua_Form.Controls.Add(this.tb_BirthDay);
            this.Panel_Chua_Form.Controls.Add(this.tb_xacnhanmk);
            this.Panel_Chua_Form.Controls.Add(this.tb_matkhau);
            this.Panel_Chua_Form.Controls.Add(this.tb_tendangnhap);
            this.Panel_Chua_Form.Controls.Add(this.tb_emailsdt);
            this.Panel_Chua_Form.Controls.Add(this.label6);
            this.Panel_Chua_Form.Controls.Add(this.label5);
            this.Panel_Chua_Form.Controls.Add(this.label4);
            this.Panel_Chua_Form.Controls.Add(this.label3);
            this.Panel_Chua_Form.Controls.Add(this.label2);
            this.Panel_Chua_Form.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Panel_Chua_Form.Location = new System.Drawing.Point(100, 100);
            this.Panel_Chua_Form.Name = "Panel_Chua_Form";
            this.Panel_Chua_Form.Size = new System.Drawing.Size(864, 612);
            this.Panel_Chua_Form.TabIndex = 0;
            // 
            // Form_Dang_Ki
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(50)))));
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1064, 765);
            this.Controls.Add(this.Panel_Chua_Form);
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.MaximizeBox = false;
            this.Name = "Form_Dang_Ki";
            this.Text = "Đăng Kí";
            this.Panel_Chua_Form.ResumeLayout(false);
            this.Panel_Chua_Form.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2HtmlLabel label1;
        private Guna.UI2.WinForms.Guna2HtmlLabel label2;
        private Guna.UI2.WinForms.Guna2HtmlLabel label3;
        private Guna.UI2.WinForms.Guna2HtmlLabel label4;
        private Guna.UI2.WinForms.Guna2HtmlLabel label5;
        private Guna.UI2.WinForms.Guna2TextBox tb_emailsdt;
        private Guna.UI2.WinForms.Guna2TextBox tb_xacnhanmk;
        private Guna.UI2.WinForms.Guna2TextBox tb_matkhau;
        private Guna.UI2.WinForms.Guna2TextBox tb_tendangnhap;
        private Guna.UI2.WinForms.Guna2Button btn_xacnhan;
        private Guna.UI2.WinForms.Guna2TextBox tb_BirthDay;
        private Guna.UI2.WinForms.Guna2HtmlLabel label6;
        private Guna.UI2.WinForms.Guna2Button btn_backdn;
        private Guna.UI2.WinForms.Guna2Panel Panel_Chua_Form;
    }
}
