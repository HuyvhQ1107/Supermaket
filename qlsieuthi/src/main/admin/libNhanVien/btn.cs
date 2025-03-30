using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using qlsieuthi.LOGIN;

namespace qlsieuthi.src.main.admin.libNhanVien
{
    public class btn
    {
        public void Xacnhandong(Form currentForm)
        {
            DialogResult closeForm = MessageBox.Show("Xác nhận đóng phần mềm","Cảnh báo",MessageBoxButtons.YesNo , MessageBoxIcon.Question);
            if(closeForm == DialogResult.Yes)
            {
                currentForm.Hide();
                login flogin = new login();
                flogin.Show();
            }
        }
        public bool Xacnhansearch()
        {
            DialogResult add = MessageBox.Show("Xác nhận tìm kiếm", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (add == DialogResult.OK)
            {
                return true;
            }
            return false;
        }
        public bool Xacnhanthem()
        {
            DialogResult add = MessageBox.Show("Xác nhận thêm/sửa dữ liệu","Thông báo",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
            if(add == DialogResult.OK)
            {
                return true;
            }
            return false;
        }

        /*
         * Button chính
         */
        public void ADMain(Form currentForm)
        {
            currentForm.Hide();
           
        }
        public void QLNhanVien(Form currentForm)
        {
            currentForm.Hide();
            qlNhanVien fqlNhanViens = new qlNhanVien();
            fqlNhanViens.Show();
        }
        public void QLTaiKhoan(Form currentForm)
        {
            currentForm.Hide();
            qlTaiKhoan fqlTaiKhoan = new qlTaiKhoan();
            fqlTaiKhoan.Show();
        }
        public void QLKhoHang(Form currentForm)
        {
            currentForm.Hide();
            qlKhoHang fqlKhoHang = new qlKhoHang();
            fqlKhoHang.Show();
        }
    }
}
