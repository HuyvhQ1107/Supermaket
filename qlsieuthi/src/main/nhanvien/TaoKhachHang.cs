using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using qlsieuthi.src.main.nhanvien.lib;

namespace qlsieuthi.src.main.nhanvien
{
    public partial class TaoKhachHang : Form
    {
        connectdb mysql = new connectdb();
        public (ComboBox, ComboBox, ComboBox) GetLoadCbbDC()
        {
            if (cbbTinhTP == null || cbbQuanHuyen == null || cbbPhuongXa == null)
            {
                MessageBox.Show("ComboBox chưa được khởi tạo!");
            }
            return (cbbTinhTP, cbbQuanHuyen, cbbPhuongXa);
        }
        public TaoKhachHang()
        {
            InitializeComponent();

            // Load api CBB địa chỉ
            api apiloader = new api(this);
            apiloader.LoadDataCity();
        }

        private void btnTaoTTKhachHang_Click_1(object sender, EventArgs e)
        {
            string MaKH = txtMaKH.Text.Trim();
            string SDT = txtSDT.Text.Trim();
            string HoTen = txtHoTen.Text.Trim();
            string mail = txtEmail.Text.Trim();
            string DC = txtDiaChi.Text.Trim();

            string TinhTP = cbbTinhTP.Text.Trim();
            string QuanHuyen = cbbQuanHuyen.Text.Trim();
            string PhuongXa = cbbPhuongXa.Text.Trim();
            string NgaySinh = dtNgaySinh.Value.ToString("yyyy-MM-dd");
            string Diachi = $"{DC}, {PhuongXa}, {QuanHuyen}, {TinhTP}";
            string DiemTichLuy = "0";
            txtDiemTichLuy.Text = DiemTichLuy;

            // Kiểm tra các trường không được bỏ trống
            if (string.IsNullOrEmpty(MaKH) || string.IsNullOrEmpty(SDT) || string.IsNullOrEmpty(HoTen)
                || string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(DC)
                || string.IsNullOrEmpty(TinhTP) || string.IsNullOrEmpty(QuanHuyen) || string.IsNullOrEmpty(PhuongXa))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin khách hàng!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra số điện thoại chỉ chứa số
            if (!SDT.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại chỉ được chứa chữ số!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra định dạng email đơn giản
            if (!Regex.IsMatch(mail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không hợp lệ!", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string queryADD = @"INSERT INTO KHACH_HANG (MaKH, HoTen, SDT, Email, NgaySinh, DiaChi) 
                        VALUES (@MaKH, @Hoten, @SDT, @Email, @NgaySinh, @DiaChi)";

            try
            {
                using (MySqlConnection conn = mysql.sql())
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(queryADD, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaKH", MaKH);
                        cmd.Parameters.AddWithValue("@Hoten", HoTen);
                        cmd.Parameters.AddWithValue("@SDT", SDT);
                        cmd.Parameters.AddWithValue("@Email", mail);
                        cmd.Parameters.AddWithValue("@NgaySinh", NgaySinh);
                        cmd.Parameters.AddWithValue("@DiaChi", Diachi);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Clear
                txtMaKH.Clear();
                txtSDT.Clear();
                txtHoTen.Clear();
                txtEmail.Clear();
                txtDiaChi.Clear();
                cbbTinhTP.SelectedIndex = -1;
                cbbQuanHuyen.SelectedIndex = -1;
                cbbPhuongXa.SelectedIndex = -1;
                dtNgaySinh.Value = DateTime.Now;
                txtDiemTichLuy.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            this.Hide();
            HoaDon fhoadon = new HoaDon();
            fhoadon.Show();
        }
    }
}
