namespace Pixel_Drift
{
    partial class Form_Quen_Mat_Khau
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
            txt_email = new TextBox();
            btn_guimahoa = new Button();
            btn_quaylai = new Button();
            guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            SuspendLayout();
            // 
            // txt_email
            // 
            txt_email.BackColor = Color.GhostWhite;
            txt_email.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_email.Location = new Point(269, 198);
            txt_email.Margin = new Padding(3, 2, 3, 2);
            txt_email.MaxLength = 100;
            txt_email.Name = "txt_email";
            txt_email.Size = new Size(508, 53);
            txt_email.TabIndex = 2;
            // 
            // btn_guimahoa
            // 
            btn_guimahoa.BackColor = Color.WhiteSmoke;
            btn_guimahoa.Font = new Font("Consolas", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_guimahoa.Location = new Point(377, 285);
            btn_guimahoa.Margin = new Padding(3, 2, 3, 2);
            btn_guimahoa.Name = "btn_guimahoa";
            btn_guimahoa.Size = new Size(151, 55);
            btn_guimahoa.TabIndex = 3;
            btn_guimahoa.Text = "Send token";
            btn_guimahoa.UseVisualStyleBackColor = false;
            btn_guimahoa.Click += btn_quenmatkhau_Click;
            // 
            // btn_quaylai
            // 
            btn_quaylai.BackColor = Color.WhiteSmoke;
            btn_quaylai.Font = new Font("Consolas", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_quaylai.Location = new Point(534, 285);
            btn_quaylai.Margin = new Padding(3, 2, 3, 2);
            btn_quaylai.Name = "btn_quaylai";
            btn_quaylai.Size = new Size(136, 55);
            btn_quaylai.TabIndex = 4;
            btn_quaylai.Text = "Back";
            btn_quaylai.UseVisualStyleBackColor = false;
            btn_quaylai.Click += btn_quaylai_Click;
            // 
            // guna2HtmlLabel1
            // 
            guna2HtmlLabel1.AutoSize = false;
            guna2HtmlLabel1.BackColor = Color.Transparent;
            guna2HtmlLabel1.Font = new Font("Consolas", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2HtmlLabel1.ForeColor = Color.Orange;
            guna2HtmlLabel1.Location = new Point(269, 111);
            guna2HtmlLabel1.Margin = new Padding(3, 4, 3, 4);
            guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            guna2HtmlLabel1.Size = new Size(528, 55);
            guna2HtmlLabel1.TabIndex = 6;
            guna2HtmlLabel1.Text = "Please enter your email address:";
            // 
            // Form_Quen_Mat_Khau
            // 
            AcceptButton = btn_guimahoa;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Background_Quen_Mat_Khau;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1045, 699);
            Controls.Add(guna2HtmlLabel1);
            Controls.Add(btn_quaylai);
            Controls.Add(btn_guimahoa);
            Controls.Add(txt_email);
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "Form_Quen_Mat_Khau";
            Text = "Quên Mật Khẩu";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.Button btn_guimahoa;
        private System.Windows.Forms.Button btn_quaylai;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
    }
}