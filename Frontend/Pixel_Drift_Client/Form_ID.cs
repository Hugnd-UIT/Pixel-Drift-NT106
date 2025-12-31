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
    public partial class Form_ID : Form
    {
        public string Room_ID { get; private set; }

        public Form_ID()
        {
            InitializeComponent();
        }

        private void btn_TimPhong_Click(object sender, EventArgs e)
        {
            string Input = tb_ID.Text.Trim();

            if (string.IsNullOrEmpty(Input))
            {
                MessageBox.Show("Vui lòng nhập mã phòng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Room_ID = Input;

            this.DialogResult = DialogResult.OK;

            this.Close();
        }
    }
}