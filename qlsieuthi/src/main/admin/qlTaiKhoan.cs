using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using qlsieuthi.src.main.admin.chuyenform;

namespace qlsieuthi.src.main.admin
{
    public partial class qlTaiKhoan : Form
    {
        public qlTaiKhoan()
        {
            InitializeComponent();
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            this.Hide();
            chuyenformadmin fc = new chuyenformadmin();
            fc.fQLNhanVien();
        }

        private void btnQLKhoHang_Click(object sender, EventArgs e)
        {
            this.Hide();
            chuyenformadmin fc = new chuyenformadmin();
            fc.fQLKhoHang();
        }
    }
}
