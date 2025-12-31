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
            this.txt_email = new System.Windows.Forms.TextBox();
            this.btn_guimahoa = new System.Windows.Forms.Button();
            this.btn_quaylai = new System.Windows.Forms.Button();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.SuspendLayout();
            // 
            // txt_email
            // 
            this.txt_email.BackColor = System.Drawing.Color.LightSlateGray;
            this.txt_email.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_email.Location = new System.Drawing.Point(268, 158);
            this.txt_email.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_email.Name = "txt_email";
            this.txt_email.Size = new System.Drawing.Size(512, 41);
            this.txt_email.TabIndex = 2;
            // 
            // btn_guimahoa
            // 
            this.btn_guimahoa.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_guimahoa.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_guimahoa.Location = new System.Drawing.Point(370, 231);
            this.btn_guimahoa.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_guimahoa.Name = "btn_guimahoa";
            this.btn_guimahoa.Size = new System.Drawing.Size(157, 44);
            this.btn_guimahoa.TabIndex = 3;
            this.btn_guimahoa.Text = "Gửi mã xác thực";
            this.btn_guimahoa.UseVisualStyleBackColor = false;
            this.btn_guimahoa.Click += new System.EventHandler(this.btn_quenmatkhau_Click);
            // 
            // btn_quaylai
            // 
            this.btn_quaylai.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btn_quaylai.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_quaylai.Location = new System.Drawing.Point(533, 231);
            this.btn_quaylai.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_quaylai.Name = "btn_quaylai";
            this.btn_quaylai.Size = new System.Drawing.Size(138, 44);
            this.btn_quaylai.TabIndex = 4;
            this.btn_quaylai.Text = "Quay lại";
            this.btn_quaylai.UseVisualStyleBackColor = false;
            this.btn_quaylai.Click += new System.EventHandler(this.btn_quaylai_Click);
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.AutoSize = false;
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.ForeColor = System.Drawing.Color.Orange;
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(324, 99);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(411, 33);
            this.guna2HtmlLabel1.TabIndex = 6;
            this.guna2HtmlLabel1.Text = "Vui lòng nhập email của bạn:";
            // 
            // Form_Quen_Mat_Khau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Pixel_Drift.Properties.Resources.Background_Quen_Mat_Khau;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1045, 559);
            this.Controls.Add(this.guna2HtmlLabel1);
            this.Controls.Add(this.btn_quaylai);
            this.Controls.Add(this.btn_guimahoa);
            this.Controls.Add(this.txt_email);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Form_Quen_Mat_Khau";
            this.Text = "Quên Mật Khẩu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_email;
        private System.Windows.Forms.Button btn_guimahoa;
        private System.Windows.Forms.Button btn_quaylai;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
    }
}