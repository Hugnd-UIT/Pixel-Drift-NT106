using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pixel_Drift
{
    public partial class Form_Mo_Dau : Form
    {
        public Form_Mo_Dau()
        {
            InitializeComponent();
        }

        private void btn_dang_ki_Click(object sender, EventArgs e)
        {
            if (Client_Manager.Is_Connected && !string.IsNullOrEmpty(Form_Dang_Nhap.Current_Username))
            {
                MessageBox.Show("Bạn đã đăng nhập rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form_Dang_Ki Form = Application.OpenForms.OfType<Form_Dang_Ki>().FirstOrDefault();

            if (Form != null)
            {
                Form.Show();
            }
            else
            {
                Form = new Form_Dang_Ki();
                Form.Show();
            }
        }

        private void btn_thoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_dang_nhap_Click(object sender, EventArgs e)
        {
            if (Client_Manager.Is_Connected && !string.IsNullOrEmpty(Form_Dang_Nhap.Current_Username))
            {
                MessageBox.Show("Bạn đã đăng nhập rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form_Dang_Nhap Form = Application.OpenForms.OfType<Form_Dang_Nhap>().FirstOrDefault();

            if (Form != null)
            {
                Form.Show();
            }
            else
            {
                Form = new Form_Dang_Nhap();
                Form.Show();
            }
        }
    }
}