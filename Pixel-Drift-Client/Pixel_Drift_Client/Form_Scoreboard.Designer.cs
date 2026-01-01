namespace Pixel_Drift
{
    partial class Form_ScoreBoard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
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
            lbl_TitleBanner = new Guna.UI2.WinForms.Guna2Button();
            dgv_ScoreBoard = new Guna.UI2.WinForms.Guna2DataGridView();
            btn_Close = new Guna.UI2.WinForms.Guna2GradientButton();
            tb_username = new Guna.UI2.WinForms.Guna2TextBox();
            btn_search = new Guna.UI2.WinForms.Guna2Button();
            btn_refesh = new Guna.UI2.WinForms.Guna2Button();
            pictureBox1 = new PictureBox();
            pnl_GridContainer = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)dgv_ScoreBoard).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnl_GridContainer.SuspendLayout();
            SuspendLayout();
            // 
            // lbl_TitleBanner
            // 
            lbl_TitleBanner.BackColor = Color.Transparent;
            lbl_TitleBanner.BorderColor = Color.Cyan;
            lbl_TitleBanner.BorderThickness = 2;
            lbl_TitleBanner.CustomizableEdges = customizableEdges1;
            lbl_TitleBanner.DisabledState.BorderColor = Color.DarkGray;
            lbl_TitleBanner.DisabledState.CustomBorderColor = Color.DarkGray;
            lbl_TitleBanner.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            lbl_TitleBanner.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            lbl_TitleBanner.FillColor = Color.FromArgb(20, 20, 50);
            lbl_TitleBanner.Font = new Font("Segoe UI Black", 28F, FontStyle.Bold);
            lbl_TitleBanner.ForeColor = Color.Cyan;
            lbl_TitleBanner.HoverState.BorderColor = Color.Magenta;
            lbl_TitleBanner.HoverState.FillColor = Color.FromArgb(30, 30, 60);
            lbl_TitleBanner.Location = new Point(358, 11);
            lbl_TitleBanner.Margin = new Padding(3, 2, 3, 2);
            lbl_TitleBanner.Name = "lbl_TitleBanner";
            lbl_TitleBanner.ShadowDecoration.CustomizableEdges = customizableEdges2;
            lbl_TitleBanner.Size = new Size(529, 76);
            lbl_TitleBanner.TabIndex = 0;
            lbl_TitleBanner.Text = "🏆 SCORE BOARD 🏆";
            // 
            // dgv_ScoreBoard
            // 
            dgv_ScoreBoard.AllowUserToAddRows = false;
            dgv_ScoreBoard.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dgv_ScoreBoard.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgv_ScoreBoard.BackgroundColor = Color.WhiteSmoke;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.Navy;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.Navy;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgv_ScoreBoard.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgv_ScoreBoard.ColumnHeadersHeight = 40;
            dgv_ScoreBoard.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgv_ScoreBoard.DefaultCellStyle = dataGridViewCellStyle3;
            dgv_ScoreBoard.Dock = DockStyle.Fill;
            dgv_ScoreBoard.GridColor = Color.LightGray;
            dgv_ScoreBoard.Location = new Point(3, 2);
            dgv_ScoreBoard.Margin = new Padding(3, 2, 3, 2);
            dgv_ScoreBoard.Name = "dgv_ScoreBoard";
            dgv_ScoreBoard.ReadOnly = true;
            dgv_ScoreBoard.RowHeadersVisible = false;
            dgv_ScoreBoard.RowHeadersWidth = 51;
            dgv_ScoreBoard.RowTemplate.Height = 35;
            dgv_ScoreBoard.Size = new Size(638, 444);
            dgv_ScoreBoard.TabIndex = 1;
            dgv_ScoreBoard.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgv_ScoreBoard.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgv_ScoreBoard.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgv_ScoreBoard.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgv_ScoreBoard.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgv_ScoreBoard.ThemeStyle.BackColor = Color.WhiteSmoke;
            dgv_ScoreBoard.ThemeStyle.GridColor = Color.LightGray;
            dgv_ScoreBoard.ThemeStyle.HeaderStyle.BackColor = Color.Navy;
            dgv_ScoreBoard.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv_ScoreBoard.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            dgv_ScoreBoard.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgv_ScoreBoard.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgv_ScoreBoard.ThemeStyle.HeaderStyle.Height = 40;
            dgv_ScoreBoard.ThemeStyle.ReadOnly = true;
            dgv_ScoreBoard.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgv_ScoreBoard.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv_ScoreBoard.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 10F);
            dgv_ScoreBoard.ThemeStyle.RowsStyle.ForeColor = Color.Black;
            dgv_ScoreBoard.ThemeStyle.RowsStyle.Height = 35;
            dgv_ScoreBoard.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgv_ScoreBoard.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // btn_Close
            // 
            btn_Close.BackColor = Color.Transparent;
            btn_Close.BorderColor = Color.White;
            btn_Close.BorderThickness = 3;
            btn_Close.CustomizableEdges = customizableEdges3;
            btn_Close.DialogResult = DialogResult.Cancel;
            btn_Close.DisabledState.BorderColor = Color.DarkGray;
            btn_Close.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_Close.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_Close.DisabledState.FillColor2 = Color.FromArgb(169, 169, 169);
            btn_Close.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_Close.FillColor = Color.Firebrick;
            btn_Close.FillColor2 = Color.Salmon;
            btn_Close.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            btn_Close.ForeColor = Color.White;
            btn_Close.Location = new Point(296, 723);
            btn_Close.Margin = new Padding(3, 2, 3, 2);
            btn_Close.Name = "btn_Close";
            btn_Close.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btn_Close.Size = new Size(644, 95);
            btn_Close.TabIndex = 2;
            btn_Close.Text = "EXIT";
            btn_Close.Click += btn_Close_Click;
            // 
            // tb_username
            // 
            tb_username.BackColor = Color.Transparent;
            tb_username.BorderColor = Color.FromArgb(64, 64, 64);
            tb_username.Cursor = Cursors.IBeam;
            tb_username.CustomizableEdges = customizableEdges5;
            tb_username.DefaultText = "";
            tb_username.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            tb_username.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            tb_username.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            tb_username.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            tb_username.FillColor = Color.FromArgb(30, 30, 50);
            tb_username.FocusedState.BorderColor = Color.Cyan;
            tb_username.Font = new Font("Segoe UI", 12F);
            tb_username.ForeColor = Color.White;
            tb_username.HoverState.BorderColor = Color.Magenta;
            tb_username.Location = new Point(302, 101);
            tb_username.Margin = new Padding(5, 9, 5, 9);
            tb_username.Name = "tb_username";
            tb_username.PlaceholderForeColor = Color.Gray;
            tb_username.PlaceholderText = "Enter username...";
            tb_username.SelectedText = "";
            tb_username.ShadowDecoration.CustomizableEdges = customizableEdges6;
            tb_username.Size = new Size(330, 85);
            tb_username.TabIndex = 3;
            tb_username.KeyPress += txt_Search_KeyPress;
            // 
            // btn_search
            // 
            btn_search.BackColor = Color.Transparent;
            btn_search.BorderColor = Color.Cyan;
            btn_search.BorderThickness = 3;
            btn_search.CustomizableEdges = customizableEdges7;
            btn_search.DisabledState.BorderColor = Color.DarkGray;
            btn_search.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_search.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_search.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_search.FillColor = Color.FromArgb(40, 40, 80);
            btn_search.Font = new Font("Arial Rounded MT Bold", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_search.ForeColor = Color.White;
            btn_search.HoverState.FillColor = Color.DodgerBlue;
            btn_search.Location = new Point(793, 101);
            btn_search.Margin = new Padding(3, 2, 3, 2);
            btn_search.Name = "btn_search";
            btn_search.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btn_search.Size = new Size(144, 85);
            btn_search.TabIndex = 4;
            btn_search.Text = "🔍Search";
            btn_search.Click += btn_Search_Click;
            // 
            // btn_refesh
            // 
            btn_refesh.BackColor = Color.Transparent;
            btn_refesh.BorderColor = Color.LawnGreen;
            btn_refesh.BorderThickness = 3;
            btn_refesh.CustomizableEdges = customizableEdges9;
            btn_refesh.DisabledState.BorderColor = Color.DarkGray;
            btn_refesh.DisabledState.CustomBorderColor = Color.DarkGray;
            btn_refesh.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btn_refesh.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btn_refesh.FillColor = Color.FromArgb(40, 80, 40);
            btn_refesh.Font = new Font("Arial Rounded MT Bold", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btn_refesh.ForeColor = Color.White;
            btn_refesh.HoverState.FillColor = Color.ForestGreen;
            btn_refesh.Location = new Point(631, 101);
            btn_refesh.Margin = new Padding(3, 2, 3, 2);
            btn_refesh.Name = "btn_refesh";
            btn_refesh.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btn_refesh.Size = new Size(156, 85);
            btn_refesh.TabIndex = 5;
            btn_refesh.Text = "🔄 Refesh";
            btn_refesh.Click += btn_Refresh_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = Properties.Resources.Background_ScoreBoard;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(4, 5, 4, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1236, 886);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // pnl_GridContainer
            // 
            pnl_GridContainer.BackColor = Color.Transparent;
            pnl_GridContainer.BorderColor = Color.FromArgb(64, 64, 64);
            pnl_GridContainer.BorderRadius = 20;
            pnl_GridContainer.BorderThickness = 2;
            pnl_GridContainer.Controls.Add(dgv_ScoreBoard);
            pnl_GridContainer.CustomizableEdges = customizableEdges11;
            pnl_GridContainer.FillColor = Color.FromArgb(30, 30, 60);
            pnl_GridContainer.Location = new Point(296, 226);
            pnl_GridContainer.Margin = new Padding(3, 2, 3, 2);
            pnl_GridContainer.Name = "pnl_GridContainer";
            pnl_GridContainer.Padding = new Padding(3, 2, 3, 2);
            pnl_GridContainer.ShadowDecoration.CustomizableEdges = customizableEdges12;
            pnl_GridContainer.Size = new Size(644, 448);
            pnl_GridContainer.TabIndex = 8;
            // 
            // Form_ScoreBoard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(20, 20, 40);
            CancelButton = btn_Close;
            ClientSize = new Size(1236, 886);
            Controls.Add(pnl_GridContainer);
            Controls.Add(btn_refesh);
            Controls.Add(btn_search);
            Controls.Add(tb_username);
            Controls.Add(btn_Close);
            Controls.Add(lbl_TitleBanner);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "Form_ScoreBoard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bảng Xếp Hạng";
            ((System.ComponentModel.ISupportInitialize)dgv_ScoreBoard).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnl_GridContainer.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button lbl_TitleBanner;
        private Guna.UI2.WinForms.Guna2DataGridView dgv_ScoreBoard;
        private Guna.UI2.WinForms.Guna2GradientButton btn_Close;
        private Guna.UI2.WinForms.Guna2TextBox tb_username;
        private Guna.UI2.WinForms.Guna2Button btn_search;
        private Guna.UI2.WinForms.Guna2Button btn_refesh;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Panel pnl_GridContainer;
    }
}