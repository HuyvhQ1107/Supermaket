using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qlsieuthi.src.main.admin.chuyenform
{
    public class chuyenformadmin
    {
        public void fQLKhoHang()
        {
            qlKhoHang fqlkhohang = new qlKhoHang();
            fqlkhohang.Show();
        }
        public void fQLNhanVien()
        {
            qlNhanVien fqlNhanVien = new qlNhanVien();
            fqlNhanVien.Show();
        }
        public void fQLTaiKhoan()
        {
            qlTaiKhoan fqlTaiKhoan = new qlTaiKhoan();
            fqlTaiKhoan.Show();
        }
    }
}
