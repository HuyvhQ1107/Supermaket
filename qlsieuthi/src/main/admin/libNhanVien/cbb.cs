using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qlsieuthi.src.main.admin.libNhanVien
{
    public class cbb
    {

        // Cbb Load chức vụ
        private qlNhanVien _qlnhanvien;
        public cbb(qlNhanVien qlNhanViens)
        {
            _qlnhanvien = qlNhanViens;
        }
        public void AddItemsCbbChucVu()
        {
            ComboBox cbb = _qlnhanvien.GetLoadCbb();
            if (cbb != null )
            {
                cbb.Items.Clear();
                cbb.Items.Add("Admin");
                cbb.Items.Add("Kế toán");
                cbb.Items.Add("Quản lí kho");
                cbb.Items.Add("Nhân viên");
            }
        }
        public void BoLoc()
        {
            ComboBox cbb = _qlnhanvien.GetLoadCbbLoc();
            if (cbb != null)
            {
                cbb.Items.Clear();
                cbb.Items.Add("Admin");
                cbb.Items.Add("Kế toán");
                cbb.Items.Add("Quản lí kho");
                cbb.Items.Add("Nhân viên");
            }
        }
    }
}
