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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbl_TitleBanner = new Guna.UI2.WinForms.Guna2Button();
            this.dgv_ScoreBoard = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btn_Close = new Guna.UI2.WinForms.Guna2GradientButton();
            this.txt_Search = new Guna.UI2.WinForms.Guna2TextBox();
            this.btn_Search = new Guna.UI2.WinForms.Guna2Button();
            this.btn_Refresh = new Guna.UI2.WinForms.Guna2Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnl_GridContainer = new Guna.UI2.WinForms.Guna2Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ScoreBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnl_GridContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_TitleBanner
            // 
            this.lbl_TitleBanner.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TitleBanner.BorderColor = System.Drawing.Color.Cyan;
            this.lbl_TitleBanner.BorderRadius = 20;
            this.lbl_TitleBanner.BorderThickness = 2;
            this.lbl_TitleBanner.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.lbl_TitleBanner.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.lbl_TitleBanner.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.lbl_TitleBanner.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.lbl_TitleBanner.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(50)))));
            this.lbl_TitleBanner.Font = new System.Drawing.Font("Segoe UI Black", 28F, System.Drawing.FontStyle.Bold);
            this.lbl_TitleBanner.ForeColor = System.Drawing.Color.Cyan;
            this.lbl_TitleBanner.HoverState.BorderColor = System.Drawing.Color.Magenta;
            this.lbl_TitleBanner.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.lbl_TitleBanner.Location = new System.Drawing.Point(363, 12);
            this.lbl_TitleBanner.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lbl_TitleBanner.Name = "lbl_TitleBanner";
            this.lbl_TitleBanner.Size = new System.Drawing.Size(516, 65);
            this.lbl_TitleBanner.TabIndex = 0;
            this.lbl_TitleBanner.Text = "🏆 SCORE BOARD 🏆";
            // 
            // dgv_ScoreBoard
            // 
            this.dgv_ScoreBoard.AllowUserToAddRows = false;
            this.dgv_ScoreBoard.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgv_ScoreBoard.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_ScoreBoard.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_ScoreBoard.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_ScoreBoard.ColumnHeadersHeight = 40;
            this.dgv_ScoreBoard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_ScoreBoard.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_ScoreBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_ScoreBoard.GridColor = System.Drawing.Color.LightGray;
            this.dgv_ScoreBoard.Location = new System.Drawing.Point(3, 2);
            this.dgv_ScoreBoard.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgv_ScoreBoard.Name = "dgv_ScoreBoard";
            this.dgv_ScoreBoard.ReadOnly = true;
            this.dgv_ScoreBoard.RowHeadersVisible = false;
            this.dgv_ScoreBoard.RowHeadersWidth = 51;
            this.dgv_ScoreBoard.RowTemplate.Height = 35;
            this.dgv_ScoreBoard.Size = new System.Drawing.Size(638, 354);
            this.dgv_ScoreBoard.TabIndex = 1;
            this.dgv_ScoreBoard.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgv_ScoreBoard.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgv_ScoreBoard.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgv_ScoreBoard.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgv_ScoreBoard.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgv_ScoreBoard.ThemeStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgv_ScoreBoard.ThemeStyle.GridColor = System.Drawing.Color.LightGray;
            this.dgv_ScoreBoard.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.Navy;
            this.dgv_ScoreBoard.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_ScoreBoard.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.dgv_ScoreBoard.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgv_ScoreBoard.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgv_ScoreBoard.ThemeStyle.HeaderStyle.Height = 40;
            this.dgv_ScoreBoard.ThemeStyle.ReadOnly = true;
            this.dgv_ScoreBoard.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgv_ScoreBoard.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgv_ScoreBoard.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgv_ScoreBoard.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.Black;
            this.dgv_ScoreBoard.ThemeStyle.RowsStyle.Height = 35;
            this.dgv_ScoreBoard.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgv_ScoreBoard.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // btn_Close
            // 
            this.btn_Close.BackColor = System.Drawing.Color.Transparent;
            this.btn_Close.BorderColor = System.Drawing.Color.White;
            this.btn_Close.BorderRadius = 30;
            this.btn_Close.BorderThickness = 3;
            this.btn_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Close.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Close.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_Close.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_Close.DisabledState.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_Close.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_Close.FillColor = System.Drawing.Color.Crimson;
            this.btn_Close.FillColor2 = System.Drawing.Color.DarkRed;
            this.btn_Close.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.btn_Close.ForeColor = System.Drawing.Color.White;
            this.btn_Close.Location = new System.Drawing.Point(296, 581);
            this.btn_Close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Close.Name = "btn_Close";
            this.btn_Close.Size = new System.Drawing.Size(644, 76);
            this.btn_Close.TabIndex = 2;
            this.btn_Close.Text = "ĐÓNG";
            this.btn_Close.Click += new System.EventHandler(this.btn_Close_Click);
            // 
            // txt_Search
            // 
            this.txt_Search.BackColor = System.Drawing.Color.Transparent;
            this.txt_Search.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txt_Search.BorderRadius = 15;
            this.txt_Search.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_Search.DefaultText = "";
            this.txt_Search.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txt_Search.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txt_Search.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txt_Search.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txt_Search.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(50)))));
            this.txt_Search.FocusedState.BorderColor = System.Drawing.Color.Cyan;
            this.txt_Search.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txt_Search.ForeColor = System.Drawing.Color.White;
            this.txt_Search.HoverState.BorderColor = System.Drawing.Color.Magenta;
            this.txt_Search.Location = new System.Drawing.Point(319, 89);
            this.txt_Search.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.txt_Search.Name = "txt_Search";
            this.txt_Search.PlaceholderForeColor = System.Drawing.Color.Gray;
            this.txt_Search.PlaceholderText = "Nhập tên để tìm...";
            this.txt_Search.SelectedText = "";
            this.txt_Search.Size = new System.Drawing.Size(316, 50);
            this.txt_Search.TabIndex = 3;
            this.txt_Search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Search_KeyPress);
            // 
            // btn_Search
            // 
            this.btn_Search.BackColor = System.Drawing.Color.Transparent;
            this.btn_Search.BorderColor = System.Drawing.Color.Cyan;
            this.btn_Search.BorderRadius = 15;
            this.btn_Search.BorderThickness = 3;
            this.btn_Search.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Search.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_Search.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_Search.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_Search.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.btn_Search.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Search.ForeColor = System.Drawing.Color.White;
            this.btn_Search.HoverState.FillColor = System.Drawing.Color.DodgerBlue;
            this.btn_Search.Location = new System.Drawing.Point(793, 89);
            this.btn_Search.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(147, 53);
            this.btn_Search.TabIndex = 4;
            this.btn_Search.Text = "🔍Tìm kiếm";
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.BackColor = System.Drawing.Color.Transparent;
            this.btn_Refresh.BorderColor = System.Drawing.Color.LawnGreen;
            this.btn_Refresh.BorderRadius = 15;
            this.btn_Refresh.BorderThickness = 3;
            this.btn_Refresh.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btn_Refresh.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btn_Refresh.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btn_Refresh.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btn_Refresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(80)))), ((int)(((byte)(40)))));
            this.btn_Refresh.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btn_Refresh.ForeColor = System.Drawing.Color.White;
            this.btn_Refresh.HoverState.FillColor = System.Drawing.Color.ForestGreen;
            this.btn_Refresh.Location = new System.Drawing.Point(641, 89);
            this.btn_Refresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(147, 53);
            this.btn_Refresh.TabIndex = 5;
            this.btn_Refresh.Text = "🔄Làm mới";
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Pixel_Drift.Properties.Resources.Background_ScoreBoard;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1236, 709);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // pnl_GridContainer
            // 
            this.pnl_GridContainer.BackColor = System.Drawing.Color.Transparent;
            this.pnl_GridContainer.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnl_GridContainer.BorderRadius = 20;
            this.pnl_GridContainer.BorderThickness = 2;
            this.pnl_GridContainer.Controls.Add(this.dgv_ScoreBoard);
            this.pnl_GridContainer.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(60)))));
            this.pnl_GridContainer.Location = new System.Drawing.Point(296, 181);
            this.pnl_GridContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnl_GridContainer.Name = "pnl_GridContainer";
            this.pnl_GridContainer.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnl_GridContainer.Size = new System.Drawing.Size(644, 358);
            this.pnl_GridContainer.TabIndex = 8;
            // 
            // Form_ScoreBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            this.CancelButton = this.btn_Close;
            this.ClientSize = new System.Drawing.Size(1236, 709);
            this.Controls.Add(this.pnl_GridContainer);
            this.Controls.Add(this.btn_Refresh);
            this.Controls.Add(this.btn_Search);
            this.Controls.Add(this.txt_Search);
            this.Controls.Add(this.btn_Close);
            this.Controls.Add(this.lbl_TitleBanner);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "Form_ScoreBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bảng Xếp Hạng";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_ScoreBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnl_GridContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Button lbl_TitleBanner;
        private Guna.UI2.WinForms.Guna2DataGridView dgv_ScoreBoard;
        private Guna.UI2.WinForms.Guna2GradientButton btn_Close;
        private Guna.UI2.WinForms.Guna2TextBox txt_Search;
        private Guna.UI2.WinForms.Guna2Button btn_Search;
        private Guna.UI2.WinForms.Guna2Button btn_Refresh;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Panel pnl_GridContainer;
    }
}