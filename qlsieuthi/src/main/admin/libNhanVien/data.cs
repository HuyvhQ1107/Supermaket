using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qlsieuthi.src.main.admin.libNhanVien
{
    public static class data
    {
        public static List<string> GetDanhSachChucVu()
        {
            return new List<string> { "Admin", "Kế toán", "Quản lí kho", "Nhân viên" };
        }
    }
}
