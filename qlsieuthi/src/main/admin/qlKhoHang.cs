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
using qlsieuthi.src.main.admin.chuyenform;
using qlsieuthi.src.main.admin.libNhanVien;
using qlsieuthi.src.main.admin.libqlKhoHang;
using qlsieuthi.src.main.qlkho.libqlkho;

namespace qlsieuthi.src.main.admin
{
    public partial class qlKhoHang : Form
    {
        // Gọi thư viện lib
        public (ComboBox, ComboBox, ComboBox) GetLoadCbbDC()
        {
            if (cbbTinhTP == null || cbbQuanHuyen == null || cbbPhuongXa == null || cbbTinhTPho == null || cbbQHuyen == null || cbbPXa == null)
            {
                MessageBox.Show("ComboBox chưa được khởi tạo!");
            }
            return (cbbTinhTP, cbbQuanHuyen, cbbPhuongXa);
        }
        public (ComboBox, ComboBox, ComboBox) GetLoadCbbDC_second()
        {
            if (cbbTinhTPho == null || cbbQHuyen == null || cbbPXa == null)
            {
                MessageBox.Show("ComboBox chưa được khởi tạo!");
            }
            return (cbbTinhTPho, cbbQHuyen, cbbPXa);
        }
        connectdb mysql = new connectdb();
        private LoadDataNhanVien dbLoad = new LoadDataNhanVien();

        public qlKhoHang()
        {
            InitializeComponent();

            // Load api CBB địa chỉ
            api apiloader = new api(this);
            apiloader.LoadDataCity();


            // Load cbb
            dbLoad.LoadCBBMaNV(cbbMaNhanVien);
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



        /*
         *  Quản lí kho
         */
        private void btnThemVaoBang_Click(object sender, EventArgs e)
        {
            string makho = txtIDMaKho.Text.Trim();
            string tenkho = txtTenKho.Text.Trim();
            string MaNhanVien = cbbMaNhanVien.Text.Trim();
            txtSoLuong.Text = "0";
            string TinhTPho = cbbTinhTPho.Text.Trim();
            string QHuyen = cbbQHuyen.Text.Trim();
            string PXa = cbbPXa.Text.Trim();
            string DC = txtDiaChi2.Text.Trim();

            string DiaChi = $"{DC} , {PXa} , {QHuyen} , {TinhTPho}";

            if (string.IsNullOrEmpty(makho))
            {
                MessageBox.Show("Lỗi: Không thể để trống mã kho");
                txtIDMaKho.Focus();
                return;
            }
            if (string.IsNullOrEmpty(tenkho))
            {
                MessageBox.Show("Lỗi: Không thể để trống tên kho");
                txtTenKho.Focus();
                return;
            }
            if (string.IsNullOrEmpty(DC))
            {
                MessageBox.Show("Lỗi: Không thể để trống Địa chỉ");
                txtDC.Focus();
                return;
            }

            // Kiểm tra trùng mã kho trong DataGridView
            foreach (DataGridViewRow row in dgvQLKho.Rows)
            {
                if (row.Cells["Mã kho"].Value != null && row.Cells["Mã kho"].Value.ToString() == makho)
                {
                    MessageBox.Show("Lỗi: Mã kho đã tồn tại!", "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Thêm vào bảng
            DataTable dt;
            if (dgvQLKho.DataSource == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Mã kho");
                dt.Columns.Add("Tên kho");
                dt.Columns.Add("Mã nhân viên quản lí");
                dt.Columns.Add("Địa chỉ");
            }
            else
            {
                dt = (DataTable)dgvQLKho.DataSource;
            }

            // Thêm dòng mới vào DataTable
            DataRow newRow = dt.NewRow();
            newRow["Mã kho"] = makho;
            newRow["Tên kho"] = tenkho;
            newRow["Mã nhân viên quản lí"] = MaNhanVien;
            newRow["Địa chỉ"] = DiaChi;

            dt.Rows.Add(newRow);

            // Cập nhật DataSource của DataGridView
            dgvQLKho.DataSource = dt;

            dgvQLKho.Columns["Mã kho"].Width = 100;   // Đặt chiều rộng là 100px
            dgvQLKho.Columns["Tên kho"].Width = 150;  // Đặt chiều rộng là 150px
            dgvQLKho.Columns["Mã nhân viên quản lí"].Width = 200;
            dgvQLKho.Columns["Địa chỉ"].Width = 400;


            // Xóa các trường nhập để chuẩn bị thêm mới
            txtIDMaKho.Clear();
            txtTenKho.Clear();
            cbbMaNhanVien.SelectedIndex = -1;
            txtDiaChi2.Clear();
            cbbTinhTP.SelectedIndex = -1;
            cbbQHuyen.SelectedIndex = -1;
            cbbPXa.SelectedIndex = -1;
        }

        private void btnXoaKhoiBang_Click(object sender, EventArgs e)
        {
            if (dgvQLKho.SelectedRows.Count > 0) // Kiểm tra có dòng nào được chọn không
            {
                DialogResult xacNhan = MessageBox.Show("Bạn có chắc chắn muốn xóa dòng này?", "Xác nhận xóa",
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (xacNhan == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dgvQLKho.SelectedRows)
                    {
                        if (!row.IsNewRow) // Kiểm tra dòng không phải dòng trống mới của DataGridView
                        {
                            dgvQLKho.Rows.Remove(row);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một dòng để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            int count = 0;
            string QuerryUpCSDL = "INSERT INTO QUAN_LY_KHO (MaKho, TenKho, DiaChi, MaNV_QuanLy) " +
                                         "VALUES (@MaKho, @TenKho, @DiaChi, @MaNV_QuanLy); ";


            DialogResult xacnhan = MessageBox.Show("Bạn muốn thêm dữ liệu trong bảng vào CSDL", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (xacnhan == DialogResult.Yes)
            {
                using (MySqlConnection conn = mysql.sql())
                {
                    conn.Open();
                    using (MySqlCommand cmdUpdate = new MySqlCommand(QuerryUpCSDL, conn))
                    {
                        foreach (DataGridViewRow row in dgvQLKho.Rows)
                        {
                            try
                            {
                                if (row.Cells["Mã kho"].Value == null) continue; // Bỏ qua dòng trống

                                cmdUpdate.Parameters.Clear(); // Xóa tham số cũ trước khi thêm mới
                                cmdUpdate.Parameters.AddWithValue("@MaKho", row.Cells["Mã kho"].Value.ToString());
                                cmdUpdate.Parameters.AddWithValue("@TenKho", row.Cells["Tên kho"].Value.ToString());
                                cmdUpdate.Parameters.AddWithValue("@DiaChi", row.Cells["Địa chỉ"].Value.ToString());
                                cmdUpdate.Parameters.AddWithValue("@MaNV_QuanLy", row.Cells["Mã nhân viên quản lí"].Value.ToString());

                                count += cmdUpdate.ExecuteNonQuery(); // Thực thi truy vấn
                            }
                            catch (MySqlException ex)
                            {
                                // Kiểm tra lỗi Duplicate Entry
                                if (ex.Number == 1062) // Lỗi trùng khóa chính
                                {
                                    // Lấy mã kho bị trùng từ thông báo lỗi
                                    string duplicatedKey = "";
                                    Match match = Regex.Match(ex.Message, @"'(.+?)' for key"); // Lấy giá trị bị trùng
                                    if (match.Success)
                                    {
                                        duplicatedKey = match.Groups[1].Value;
                                    }

                                    MessageBox.Show($"Lỗi: Mã kho '{duplicatedKey}' đã tồn tại trong cơ sở dữ liệu!",
                                                    "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    count = 0;
                                    break;
                                }
                                else
                                {
                                    // Nếu là lỗi khác, hiển thị chi tiết lỗi
                                    MessageBox.Show("Lỗi khi thêm dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }

                if (count > 0) // Chỉ hiển thị khi có dữ liệu được thêm
                {
                    MessageBox.Show($"Thêm thành công {count} kho hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvQLKho.DataSource = null;
                    dgvQLKho.Rows.Clear();
                }
            }
        }

        private void btnQLTaiKhoan_Click(object sender, EventArgs e)
        {
            this.Hide();
            chuyenformadmin fc = new chuyenformadmin();
            fc.fQLNhanVien();
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            this.Hide();
            chuyenformadmin fc = new chuyenformadmin();
            fc.fQLTaiKhoan();
        }
    }
}
