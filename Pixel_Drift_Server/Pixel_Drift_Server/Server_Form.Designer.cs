namespace Pixel_Drift_Server
{
    partial class ServerForm
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
            this.tb_hienthi = new System.Windows.Forms.TextBox();
            this.btn_Start_Server = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_hienthi
            // 
            this.tb_hienthi.Location = new System.Drawing.Point(9, 63);
            this.tb_hienthi.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tb_hienthi.Multiline = true;
            this.tb_hienthi.Name = "tb_hienthi";
            this.tb_hienthi.ReadOnly = true;
            this.tb_hienthi.Size = new System.Drawing.Size(626, 288);
            this.tb_hienthi.TabIndex = 10;
            // 
            // btn_Start_Server
            // 
            this.btn_Start_Server.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btn_Start_Server.Location = new System.Drawing.Point(9, 9);
            this.btn_Start_Server.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn_Start_Server.Name = "btn_Start_Server";
            this.btn_Start_Server.Size = new System.Drawing.Size(625, 48);
            this.btn_Start_Server.TabIndex = 8;
            this.btn_Start_Server.Text = "Start Server";
            this.btn_Start_Server.UseVisualStyleBackColor = true;
            this.btn_Start_Server.Click += new System.EventHandler(this.btn_Start_Server_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 362);
            this.Controls.Add(this.tb_hienthi);
            this.Controls.Add(this.btn_Start_Server);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "ServerForm";
            this.Text = "Server";
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_hienthi;
        private System.Windows.Forms.Button btn_Start_Server;
    }
}

