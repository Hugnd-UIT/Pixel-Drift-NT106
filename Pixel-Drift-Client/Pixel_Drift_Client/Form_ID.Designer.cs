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
            tb_ID = new TextBox();
            btn_TimPhong = new Button();
            SuspendLayout();
            // 
            // tb_ID
            // 
            tb_ID.BorderStyle = BorderStyle.None;
            tb_ID.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tb_ID.ForeColor = Color.DarkCyan;
            tb_ID.Location = new Point(144, 66);
            tb_ID.Margin = new Padding(4, 5, 4, 5);
            tb_ID.MaxLength = 50;
            tb_ID.Name = "tb_ID";
            tb_ID.Size = new Size(227, 27);
            tb_ID.TabIndex = 1;
            // 
            // btn_TimPhong
            // 
            btn_TimPhong.Font = new Font("Consolas", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_TimPhong.Location = new Point(205, 109);
            btn_TimPhong.Margin = new Padding(4, 5, 4, 5);
            btn_TimPhong.Name = "btn_TimPhong";
            btn_TimPhong.Size = new Size(100, 35);
            btn_TimPhong.TabIndex = 2;
            btn_TimPhong.Text = "Find room";
            btn_TimPhong.UseVisualStyleBackColor = true;
            btn_TimPhong.Click += btn_TimPhong_Click;
            // 
            // Form_ID
            // 
            AcceptButton = btn_TimPhong;
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.Background_ID;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(514, 186);
            Controls.Add(btn_TimPhong);
            Controls.Add(tb_ID);
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "Form_ID";
            Text = "ID";
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tb_ID;
        private System.Windows.Forms.Button btn_TimPhong;
    }
}