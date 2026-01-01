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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Dang_Nhap));
            lb_pass = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lb_user = new Guna.UI2.WinForms.Guna2HtmlLabel();
            lb_dangnhap = new Guna.UI2.WinForms.Guna2HtmlLabel();
            tb_username = new Guna.UI2.WinForms.Guna2TextBox();
            tb_matkhau = new Guna.UI2.WinForms.Guna2TextBox();
            btn_dangnhap = new Guna.UI2.WinForms.Guna2Button();
            btn_quenmatkhau = new Guna.UI2.WinForms.Guna2Button();
            btn_quaylaidk = new Guna.UI2.WinForms.Guna2Button();
            Panel_Chua_Form = new Guna.UI2.WinForms.Guna2Panel();
            Panel_Chua_Form.SuspendLayout();
            SuspendLayout();
            // 
            // lb_pass
            // 
            lb_pass.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lb_pass.AutoSize = false;
            lb_pass.BackColor = Color.Transparent;
            lb_pass.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lb_pass.ForeColor = Color.FromArgb(128, 255, 255);
            lb_pass.Location = new Point(35, 226);
            lb_pass.Margin = new Padding(4, 5, 4, 5);
            lb_pass.Name = "lb_pass";
            lb_pass.Size = new Size(153, 66);
            lb_pass.TabIndex = 7;
            lb_pass.Text = "Password";
            // 
            // lb_user
            // 
            lb_user.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lb_user.AutoSize = false;
            lb_user.BackColor = Color.Transparent;
            lb_user.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lb_user.ForeColor = Color.FromArgb(128, 255, 255);
            lb_user.Location = new Point(35, 118);
            lb_user.Margin = new Padding(4, 5, 4, 5);
            lb_user.Name = "lb_user";
            lb_user.Size = new Size(222, 68);
            lb_user.TabIndex = 8;
            lb_user.Text = "Username";
            // 
            // lb_dangnhap
            // 
            lb_dangnhap.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lb_dangnhap.AutoSize = false;
            lb_dangnhap.BackColor = Color.Transparent;
            lb_dangnhap.Font = new Font("Arial Black", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lb_dangnhap.ForeColor = Color.FromArgb(0, 255, 255);
            lb_dangnhap.Location = new Point(198, 29);
            lb_dangnhap.Margin = new Padding(4, 5, 4, 5);
            lb_dangnhap.Name = "lb_dangnhap";
            lb_dangnhap.Size = new Size(211, 86);
            lb_dangnhap.TabIndex = 9;
            lb_dangnhap.Text = "LOGIN";
            // 
            // tb_username
            // 
            tb_username.BorderColor = Color.FromArgb(0, 192, 192);
            tb_username.BorderRadius = 8;
            tb_username.Cursor = Cursors.IBeam;
            tb_username.CustomizableEdges = customizableEdges1;
            tb_username.DefaultText = "";
            tb_username.FillColor = Color.FromArgb(20, 20, 40);
            tb_username.FocusedState.BorderColor = Color.Aqua;
            tb_username.Font = new Font("Segoe UI", 10F);
            tb_username.ForeColor = Color.WhiteSmoke;
            tb_username.HoverState.BorderColor = Color.FromArgb(0, 255, 255);
            tb_username.Location = new Point(35, 156);
            tb_username.Margin = new Padding(4, 8, 4, 8);
            tb_username.MaxLength = 50;
            tb_username.Name = "tb_username";
            tb_username.PlaceholderText = "Username";
            tb_username.SelectedText = "";
            tb_username.ShadowDecoration.CustomizableEdges = customizableEdges2;
            tb_username.Size = new Size(427, 55);
            tb_username.TabIndex = 3;
            // 
            // tb_matkhau
            // 
            tb_matkhau.BorderColor = Color.FromArgb(0, 192, 192);
            tb_matkhau.BorderRadius = 8;
            tb_matkhau.Cursor = Cursors.IBeam;
            tb_matkhau.CustomizableEdges = customizableEdges3;
            tb_matkhau.DefaultText = "";
            tb_matkhau.FillColor = Color.FromArgb(20, 20, 40);
            tb_matkhau.FocusedState.BorderColor = Color.Aqua;
            tb_matkhau.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_matkhau.ForeColor = Color.WhiteSmoke;
            tb_matkhau.HoverState.BorderColor = Color.FromArgb(0, 255, 255);
            tb_matkhau.Location = new Point(35, 264);
            tb_matkhau.Margin = new Padding(4, 5, 4, 5);
            tb_matkhau.MaxLength = 50;
            tb_matkhau.Name = "tb_matkhau";
            tb_matkhau.PasswordChar = '*';
            tb_matkhau.PlaceholderText = "Password";
            tb_matkhau.SelectedText = "";
            tb_matkhau.ShadowDecoration.CustomizableEdges = customizableEdges4;
            tb_matkhau.Size = new Size(427, 55);
            tb_matkhau.TabIndex = 4;
            // 
            // btn_dangnhap
            // 
            btn_dangnhap.BorderColor = Color.Orange;
            btn_dangnhap.BorderRadius = 15;
            btn_dangnhap.BorderThickness = 3;
            btn_dangnhap.CustomizableEdges = customizableEdges5;
            btn_dangnhap.DisabledState.BorderColor = Color.DarkGray;
            btn_dangnhap.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_dangnhap.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_dangnhap.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_dangnhap.FillColor = Color.FromArgb(255, 128, 0);
            btn_dangnhap.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_dangnhap.ForeColor = Color.White;
            btn_dangnhap.HoverState.FillColor = Color.FromArgb(255, 192, 128);
            btn_dangnhap.Location = new Point(89, 349);
            btn_dangnhap.Margin = new Padding(4, 5, 4, 5);
            btn_dangnhap.Name = "btn_dangnhap";
            btn_dangnhap.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btn_dangnhap.Size = new Size(320, 69);
            btn_dangnhap.TabIndex = 5;
            btn_dangnhap.Text = "Login";
            btn_dangnhap.Click += btn_dangnhap_Click;
            // 
            // btn_quenmatkhau
            // 
            btn_quenmatkhau.BackColor = Color.Transparent;
            btn_quenmatkhau.BorderColor = Color.Transparent;
            btn_quenmatkhau.CustomizableEdges = customizableEdges7;
            btn_quenmatkhau.DisabledState.BorderColor = Color.DarkGray;
            btn_quenmatkhau.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_quenmatkhau.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_quenmatkhau.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_quenmatkhau.FillColor = Color.Transparent;
            btn_quenmatkhau.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            btn_quenmatkhau.ForeColor = Color.FromArgb(128, 255, 0);
            btn_quenmatkhau.HoverState.ForeColor = Color.Lime;
            btn_quenmatkhau.Location = new Point(3, 448);
            btn_quenmatkhau.Margin = new Padding(3, 1, 3, 1);
            btn_quenmatkhau.Name = "btn_quenmatkhau";
            btn_quenmatkhau.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btn_quenmatkhau.Size = new Size(254, 39);
            btn_quenmatkhau.TabIndex = 0;
            btn_quenmatkhau.Text = "Forgot / Change password";
            btn_quenmatkhau.Click += btn_quenmatkhau_Click;
            // 
            // btn_quaylaidk
            // 
            btn_quaylaidk.BackColor = Color.Transparent;
            btn_quaylaidk.BorderColor = Color.Transparent;
            btn_quaylaidk.CustomizableEdges = customizableEdges9;
            btn_quaylaidk.DisabledState.BorderColor = Color.DarkGray;
            btn_quaylaidk.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_quaylaidk.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_quaylaidk.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_quaylaidk.FillColor = Color.Transparent;
            btn_quaylaidk.Font = new Font("Segoe UI", 9F, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, 0);
            btn_quaylaidk.ForeColor = Color.FromArgb(0, 192, 255);
            btn_quaylaidk.HoverState.ForeColor = Color.DeepSkyBlue;
            btn_quaylaidk.Location = new Point(242, 448);
            btn_quaylaidk.Margin = new Padding(3, 1, 3, 1);
            btn_quaylaidk.Name = "btn_quaylaidk";
            btn_quaylaidk.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btn_quaylaidk.Size = new Size(248, 39);
            btn_quaylaidk.TabIndex = 6;
            btn_quaylaidk.Text = "Don't have an account yet?";
            btn_quaylaidk.Click += btn_quaylaidk_Click;
            // 
            // Panel_Chua_Form
            // 
            Panel_Chua_Form.BackColor = Color.Transparent;
            Panel_Chua_Form.BorderColor = Color.Aquamarine;
            Panel_Chua_Form.BorderRadius = 15;
            Panel_Chua_Form.BorderThickness = 2;
            Panel_Chua_Form.Controls.Add(btn_quaylaidk);
            Panel_Chua_Form.Controls.Add(btn_quenmatkhau);
            Panel_Chua_Form.Controls.Add(btn_dangnhap);
            Panel_Chua_Form.Controls.Add(tb_matkhau);
            Panel_Chua_Form.Controls.Add(tb_username);
            Panel_Chua_Form.Controls.Add(lb_pass);
            Panel_Chua_Form.Controls.Add(lb_user);
            Panel_Chua_Form.Controls.Add(lb_dangnhap);
            Panel_Chua_Form.CustomizableEdges = customizableEdges11;
            Panel_Chua_Form.FillColor = Color.FromArgb(200, 0, 0, 0);
            Panel_Chua_Form.Location = new Point(163, 50);
            Panel_Chua_Form.Margin = new Padding(4, 5, 4, 5);
            Panel_Chua_Form.Name = "Panel_Chua_Form";
            Panel_Chua_Form.ShadowDecoration.CustomizableEdges = customizableEdges12;
            Panel_Chua_Form.Size = new Size(504, 518);
            Panel_Chua_Form.TabIndex = 0;
            // 
            // Form_Dang_Nhap
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 20, 30);
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(852, 617);
            Controls.Add(Panel_Chua_Form);
            ForeColor = Color.DarkGoldenrod;
            Margin = new Padding(3, 1, 3, 1);
            MaximizeBox = false;
            Name = "Form_Dang_Nhap";
            Text = "Đăng Nhập";
            Panel_Chua_Form.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2HtmlLabel lb_pass;
        private Guna.UI2.WinForms.Guna2HtmlLabel lb_user;
        private Guna.UI2.WinForms.Guna2HtmlLabel lb_dangnhap;
        private Guna.UI2.WinForms.Guna2TextBox tb_username;
        private Guna.UI2.WinForms.Guna2TextBox tb_matkhau;
        private Guna.UI2.WinForms.Guna2Button btn_dangnhap;
        private Guna.UI2.WinForms.Guna2Button btn_quenmatkhau;
        private Guna.UI2.WinForms.Guna2Button btn_quaylaidk;
        private Guna.UI2.WinForms.Guna2Panel Panel_Chua_Form;
    }
}
