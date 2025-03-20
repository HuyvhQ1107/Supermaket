using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using qlsieuthi.src.main.admin.libNhanVien;
using qlsieuthi.src.main.admin.libqlKhoHang;

namespace qlsieuthi.src.main.admin
{
    public partial class qlKhoHang : Form
    {
        // Gọi thư viện lib
        public (ComboBox, ComboBox, ComboBox) GetLoadCbbDC()
        {
            if (cbbTinhTP == null || cbbQuanHuyen == null || cbbPhuongXa == null)
            {
                MessageBox.Show("ComboBox chưa được khởi tạo!");
            }
            return (cbbTinhTP, cbbQuanHuyen, cbbPhuongXa);
        }
        lib mysql = new lib();

        public qlKhoHang()
        {
            InitializeComponent();

            // Load api CBB địa chỉ
            api apiloader = new api(this);
            apiloader.LoadDataCity();

            
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }
        /*
         * TAB CONTROL 1 : QUẢN LÍ LOẠI HÀNG VÀ NHÀ CUNG CẤP
         * 
         * Tính năng : 
         * - Thêm nhà cung cấp và loại hàng vào csdl
         */
        private void btnThemNhaCC_Click(object sender, EventArgs e)
        {
            InputData dulieu = new InputData("","","","","","","","","","","");
            dulieu.MaNhaCC = txtMaNhaCC.Text;
            dulieu.TenNhaCC = txtTenNhaCC.Text;
            dulieu.SDTNhaCC = txtSDT.Text;
            dulieu.Email = txtEmail.Text;

            // Các biến có trong địa chỉ
            dulieu.TinhTP = cbbTinhTP.Text;
            dulieu.QuanHuyen = cbbQuanHuyen.Text;
            dulieu.PhuongXa = cbbPhuongXa.Text;
            dulieu.DiaChiHT = txtDC.Text;
            dulieu.DiaChi = $"{dulieu.DiaChiHT} , {dulieu.PhuongXa} , {dulieu.QuanHuyen} , {dulieu.TinhTP}";

            // Check xem người dùng có để rỗng không
            if (string.IsNullOrEmpty(dulieu.MaNhaCC))
            {
                MessageBox.Show("Lỗi: Mã nhà cung cấp không được để trống");
                txtMaNhaCC.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieu.TenNhaCC))
            {
                MessageBox.Show("Lỗi: Tên nhà cung cấp không được để trống");
                txtTenNhaCC.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieu.SDTNhaCC))
            {
                MessageBox.Show("Lỗi: Số điện thoại nhà cung cấp không được để trống");
                txtTenNhaCC.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieu.Email))
            {
                MessageBox.Show("Lỗi: Email nhà cung cấp không được để trống");
                txtTenNhaCC.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieu.TinhTP))
            {
                MessageBox.Show("Bạn chưa chọn Tỉnh/Thành Phố");
                cbbTinhTP.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieu.QuanHuyen))
            {
                MessageBox.Show("Bạn chưa chọn Quận/Huyện");
                cbbQuanHuyen.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieu.PhuongXa))
            {
                MessageBox.Show("Bạn chưa chọn Phường/Xã");
                cbbPhuongXa.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieu.DiaChiHT))
            {
                MessageBox.Show("Lỗi : Bạn chưa điền địa chỉ hiện tại");
                txtDC.Focus();
                return;
            }
            // query
            string query = "INSERT INTO NHA_CUNG_CAP (MaNCC, TenNCC, SDT, Email, DiaChi) " +
                "VALUES (@MaNCC, @TenNCC, @SDT, @Email, @DC)";
            // Kết nối đến sql
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaNCC", dulieu.MaNhaCC);
                        cmd.Parameters.AddWithValue("@TenNCC", dulieu.TenNhaCC);
                        cmd.Parameters.AddWithValue("@SDT", dulieu.SDTNhaCC);
                        cmd.Parameters.AddWithValue("@Email", dulieu.Email);
                        cmd.Parameters.AddWithValue("@DC", dulieu.DiaChi);

                        int add = cmd.ExecuteNonQuery();

                        //Xác nhận thêm
                        DialogResult xacnhanADD = MessageBox.Show($"Mã nhà cung cấp : {dulieu.MaNhaCC}" +
                            $"\nTên nhà cung cấp : {dulieu.TenNhaCC}" +
                            $"\nSDT : {dulieu.SDTNhaCC}" +
                            $"\nEmail : {dulieu.Email}" +
                            $"\nĐịa chỉ : {dulieu.DiaChi}", "Thông báo thêm",
                            MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (xacnhanADD == DialogResult.Yes)
                        {
                            if (add > 0)
                            {
                                MessageBox.Show("Thêm dữ liệu thành công");

                                // Xóa dữ liệu ở các ô
                                txtMaNhaCC.Text = "";
                                txtTenNhaCC.Text = "";
                                txtSDT.Text = "";
                                txtEmail.Text = "";
                                txtDC.Text = "";
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show($"Mã Nhà cung cấp ' {dulieu.MaNhaCC} ' đã tồn tại");
                    txtMaNhaCC.Focus();
                    return;
                }
            }
        }
        private void btnADDLoaiHang_Click(object sender, EventArgs e)
        {
            InputData dulieu = new InputData("", "", "", "", "", "", "", "", "", "", "");
            dulieu.MaLoaiHang = txtMaLoaiHang.Text;
            dulieu.TenLoaiHang = txtTenLoaiHang.Text;

            if (string.IsNullOrEmpty(dulieu.MaLoaiHang))
            {
                MessageBox.Show("Lỗi : Mã loại hàng hóa không thể để trống");
                txtMaLoaiHang.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieu.TenLoaiHang))
            {
                MessageBox.Show("Lỗi : Tên loại hàng hóa không thể để trống");
                txtTenLoaiHang.Focus();
                return;
            }

            // query
            string query = "INSERT INTO LOAI_HANG (MaLoai, TenLoai) VALUES (@MaLoaiHang, @TenLoaiHang)";
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                try
                {
                    using (MySqlCommand cmd = new MySqlCommand(query,conn))
                    {
                        cmd.Parameters.AddWithValue("@MaLoaiHang", dulieu.MaLoaiHang);
                        cmd.Parameters.AddWithValue("@TenLoaiHang", dulieu.TenLoaiHang);

                        int add = cmd.ExecuteNonQuery();
                        DialogResult addLoaiHang = MessageBox.Show($"- Mã loại hàng: {dulieu.MaLoaiHang}" +
                            $"\n- Tên loại hàng: {dulieu.TenLoaiHang}", "Thông báo thêm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (addLoaiHang == DialogResult.Yes)
                        {
                            if (add > 0)
                            {
                                MessageBox.Show("Thêm dữ liệu thành công");

                                txtMaLoaiHang.Text = "";
                                txtTenLoaiHang.Text = "";
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show($"Mã loại hàng '{dulieu.MaLoaiHang}' đã tồn tại");
                    txtMaLoaiHang.Focus();
                    return;
                }
            }
        }
    }
}
