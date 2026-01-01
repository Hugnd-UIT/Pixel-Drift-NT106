using System.Drawing;
using System.Windows.Forms;

namespace Pixel_Drift
{
    partial class Form_Mo_Dau
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
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            btn_dang_ki = new Guna.UI2.WinForms.Guna2GradientButton();
            btn_dang_nhap = new Guna.UI2.WinForms.Guna2GradientButton();
            btn_thoat = new Guna.UI2.WinForms.Guna2GradientButton();
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            SuspendLayout();
            // 
            // btn_dang_ki
            // 
            btn_dang_ki.Anchor = AnchorStyles.None;
            btn_dang_ki.BorderThickness = 2;
            btn_dang_ki.CustomizableEdges = customizableEdges1;
            btn_dang_ki.DialogResult = DialogResult.OK;
            btn_dang_ki.DisabledState.BorderColor = Color.DarkGray;
            btn_dang_ki.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_dang_ki.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_dang_ki.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_dang_ki.FillColor = Color.FromArgb(0, 255, 128);
            btn_dang_ki.FillColor2 = Color.Cyan;
            btn_dang_ki.Font = new Font("Segoe UI Black", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_dang_ki.ForeColor = Color.White;
            btn_dang_ki.HoverState.BorderColor = Color.White;
            btn_dang_ki.HoverState.FillColor = Color.FromArgb(128, 255, 128);
            btn_dang_ki.HoverState.FillColor2 = Color.FromArgb(192, 255, 255);
            btn_dang_ki.ImageSize = new Size(30, 30);
            btn_dang_ki.Location = new Point(413, 337);
            btn_dang_ki.Margin = new Padding(4, 5, 4, 5);
            btn_dang_ki.Name = "btn_dang_ki";
            btn_dang_ki.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btn_dang_ki.Size = new Size(455, 106);
            btn_dang_ki.TabIndex = 0;
            btn_dang_ki.Text = "REGISTER";
            btn_dang_ki.Click += btn_dang_ki_Click;
            // 
            // btn_dang_nhap
            // 
            btn_dang_nhap.Anchor = AnchorStyles.None;
            btn_dang_nhap.BorderThickness = 2;
            btn_dang_nhap.CustomizableEdges = customizableEdges3;
            btn_dang_nhap.DisabledState.BorderColor = Color.DarkGray;
            btn_dang_nhap.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_dang_nhap.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_dang_nhap.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_dang_nhap.FillColor = Color.DodgerBlue;
            btn_dang_nhap.FillColor2 = Color.Magenta;
            btn_dang_nhap.Font = new Font("Segoe UI Black", 18F, FontStyle.Bold);
            btn_dang_nhap.ForeColor = Color.White;
            btn_dang_nhap.HoverState.BorderColor = Color.White;
            btn_dang_nhap.HoverState.FillColor = Color.FromArgb(128, 128, 255);
            btn_dang_nhap.HoverState.FillColor2 = Color.FromArgb(255, 128, 255);
            btn_dang_nhap.ImageSize = new Size(30, 30);
            btn_dang_nhap.Location = new Point(413, 453);
            btn_dang_nhap.Margin = new Padding(4, 5, 4, 5);
            btn_dang_nhap.Name = "btn_dang_nhap";
            btn_dang_nhap.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btn_dang_nhap.Size = new Size(455, 106);
            btn_dang_nhap.TabIndex = 3;
            btn_dang_nhap.Text = "LOGIN";
            btn_dang_nhap.Click += btn_dang_nhap_Click;
            // 
            // btn_thoat
            // 
            btn_thoat.Anchor = AnchorStyles.None;
            btn_thoat.BorderThickness = 2;
            btn_thoat.CustomizableEdges = customizableEdges5;
            btn_thoat.DisabledState.BorderColor = Color.DarkGray;
            btn_thoat.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_thoat.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_thoat.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_thoat.FillColor = Color.LightCoral;
            btn_thoat.FillColor2 = Color.OrangeRed;
            btn_thoat.Font = new Font("Segoe UI Black", 18F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_thoat.ForeColor = Color.White;
            btn_thoat.HoverState.BorderColor = Color.White;
            btn_thoat.HoverState.FillColor = Color.FromArgb(255, 128, 128);
            btn_thoat.HoverState.FillColor2 = Color.FromArgb(255, 192, 128);
            btn_thoat.ImageSize = new Size(30, 30);
            btn_thoat.Location = new Point(413, 569);
            btn_thoat.Margin = new Padding(4, 5, 4, 5);
            btn_thoat.Name = "btn_thoat";
            btn_thoat.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btn_thoat.Size = new Size(455, 106);
            btn_thoat.TabIndex = 2;
            btn_thoat.Text = "EXIT";
            btn_thoat.Click += btn_thoat_Click;
            // 
            // guna2BorderlessForm1
            // 
            guna2BorderlessForm1.ContainerControl = this;
            guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // Form_Mo_Dau
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            BackgroundImage = Properties.Resources.Background_Open;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1276, 746);
            Controls.Add(btn_dang_nhap);
            Controls.Add(btn_thoat);
            Controls.Add(btn_dang_ki);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form_Mo_Dau";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Pixel Drift";
            ResumeLayout(false);

        }

        #endregion
        private Guna.UI2.WinForms.Guna2GradientButton btn_dang_ki;
        private Guna.UI2.WinForms.Guna2GradientButton btn_dang_nhap;
        private Guna.UI2.WinForms.Guna2GradientButton btn_thoat;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
    }
}

