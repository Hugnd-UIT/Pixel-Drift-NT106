namespace Pixel_Drift
{
    partial class Form_ID
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
            this.tb_ID = new System.Windows.Forms.TextBox();
            this.btn_TimPhong = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_ID
            // 
            this.tb_ID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tb_ID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_ID.ForeColor = System.Drawing.Color.DarkCyan;
            this.tb_ID.Location = new System.Drawing.Point(144, 53);
            this.tb_ID.Margin = new System.Windows.Forms.Padding(4);
            this.tb_ID.Name = "tb_ID";
            this.tb_ID.Size = new System.Drawing.Size(227, 27);
            this.tb_ID.TabIndex = 1;
            // 
            // btn_TimPhong
            // 
            this.btn_TimPhong.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_TimPhong.Location = new System.Drawing.Point(205, 87);
            this.btn_TimPhong.Margin = new System.Windows.Forms.Padding(4);
            this.btn_TimPhong.Name = "btn_TimPhong";
            this.btn_TimPhong.Size = new System.Drawing.Size(100, 28);
            this.btn_TimPhong.TabIndex = 2;
            this.btn_TimPhong.Text = "Tìm Phòng";
            this.btn_TimPhong.UseVisualStyleBackColor = true;
            this.btn_TimPhong.Click += new System.EventHandler(this.btn_TimPhong_Click);
            // 
            // Form_ID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Pixel_Drift.Properties.Resources.Background_ID;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(514, 149);
            this.Controls.Add(this.btn_TimPhong);
            this.Controls.Add(this.tb_ID);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form_ID";
            this.Text = "ID";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_ID;
        private System.Windows.Forms.Button btn_TimPhong;
    }
}