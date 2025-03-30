using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using qlsieuthi.src.main.admin;
using qlsieuthi.src.main.nhanvien.lib;
using qlsieuthi.src.main.qlkho;
using qlsieuthi.src.main.qlkho.libqlkho;

namespace qlsieuthi.src.main.nhanvien
{
    public partial class HoaDon : Form
    {
        // Lib
        private LoadDataNV dbloadNV = new LoadDataNV();
        private LoadData dbLoadKho = new LoadData();

        // Đặt số lượng trong kho = 0
        private int SoluongTrongKho = 0;

        // Mysql
        connectdb mysql = new connectdb();

        void LoadMaHD()
        {
            Random random = new Random();
            int randomNumber = random.Next(1000, 10000); // tạo số từ 1000 đến 9999
            txtMaHD.Text = "HD" + "_" + randomNumber;
        }
        public HoaDon()
        {
            InitializeComponent();

            // Load cbb
            dbloadNV.LoadCBBMaKH(cbbMaKhachHang);
            dbloadNV.LoadCBBMaNV(cbbMaNhanVien);

            dbLoadKho.LoadCBBMaHH(cbbMaHangHoa);

            LoadMaHD();
        }

        // Load dữ liệu hàng hóa
        private void cbbcbbMaHangHoa_DropDownClosed(object sender, EventArgs e)
        {
            if (cbbMaHangHoa.SelectedItem == null) return;
            string CBBMaHH = cbbMaHangHoa.Text;
            string queryhanghoa = "SELECT SoLuong, DonGia FROM HANG_HOA WHERE MaHH = @MaHangHoa";

            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(queryhanghoa, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHangHoa", CBBMaHH);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            SoluongTrongKho = Convert.ToInt32(reader["SoLuong"]); // Lưu lại
                            txtSoLuong.Text = reader["SoLuong"].ToString();

                            decimal dongia = reader.GetDecimal("DonGia");
                            lbDonGia.Text = string.Format("{0:N0} VND", dongia);
                        }
                        else
                        {
                            txtSoLuong.Text = "";
                            lbDonGia.Text = "Không tìm thấy";
                        }
                    }
                }
            }
        }
        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtSoLuong.Text, out int soLuongNhap))
            {
                if (soLuongNhap > SoluongTrongKho)
                {
                    MessageBox.Show($"Chỉ còn {SoluongTrongKho} sản phẩm trong kho!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSoLuong.Text = SoluongTrongKho.ToString(); // tự động set lại về max
                }
                if (soLuongNhap == 0)
                {
                    txtSoLuong.Text = SoluongTrongKho.ToString(); // tự động set lại về max
                }
            }
        }

        private void btnThemBang_Click(object sender, EventArgs e)
        {
            string MaNV = cbbMaNhanVien.Text.Trim();
            string MaKH = cbbMaKhachHang.Text.Trim();

            string ngayLapHD = dtNgayLap.Value.ToString("dd/MM/yyyy");

            string MaHH = cbbMaHangHoa.Text.Trim();
            string SoLuong = txtSoLuong.Text.Trim();

            if (string.IsNullOrEmpty(MaNV))
            {
                MessageBox.Show("Lỗi: Điền thiếu dữ liệu");
                return;
            }
            if (string.IsNullOrEmpty(MaKH))
            {
                MessageBox.Show("Lỗi: Điền thiếu dữ liệu");
                return;
            }
            if (string.IsNullOrEmpty(MaHH))
            {
                MessageBox.Show("Lỗi: Điền thiếu dữ liệu");
                return;
            }
            if (string.IsNullOrEmpty(SoLuong))
            {
                MessageBox.Show("Lỗi: Điền thiếu dữ liệu");
                return;
            }

            // Kiểm tra trùng mã kho trong DataGridView
            foreach (DataGridViewRow row in dgvHoaDon.Rows)
            {
                if (row.Cells["Mã hóa đơn"].Value != null && row.Cells["Mã hóa đơn"].Value.ToString() == txtMaHD.Text)
                {
                    MessageBox.Show("Lỗi: Mã kho đã tồn tại!", "Trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LoadMaHD();
                    return;
                }
            }

            // Thêm vào bảng
            DataTable dt;
            if (dgvHoaDon.DataSource == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Mã hóa đơn");
                dt.Columns.Add("Mã khách hàng");
                dt.Columns.Add("Mã hàng hóa");
                dt.Columns.Add("Số Lượng");
                dt.Columns.Add("Mã nhân viên");
                dt.Columns.Add("Ngày lập");
                dt.Columns.Add("Tổng tiền");
            }
            else
            {
                dt = (DataTable)dgvHoaDon.DataSource;
            }

            // Biến đổi đơn giá
            string donGiaStr = lbDonGia.Text.Replace("VND", "").Trim().Replace(",", "");
            int donGia = int.Parse(donGiaStr);
            int soLuong = int.Parse(txtSoLuong.Text);
            int tongTien = soLuong * donGia;

            // Thêm dòng mới vào DataTable
            DataRow newRow = dt.NewRow();
            newRow["Mã hóa đơn"] = txtMaHD.Text.Trim();
            newRow["Mã khách hàng"] = MaKH;
            newRow["Mã hàng hóa"] = MaHH;
            newRow["Số Lượng"] = SoLuong;
            newRow["Mã nhân viên"] = MaNV;
            newRow["Ngày lập"] = ngayLapHD;
            newRow["Tổng tiền"] = tongTien;


            dt.Rows.Add(newRow);

            // Cập nhật DataSource của DataGridView
            dgvHoaDon.DataSource = dt;

            dgvHoaDon.Columns["Mã hóa đơn"].Width = 100;   // Đặt chiều rộng là 100px
            dgvHoaDon.Columns["Mã khách hàng"].Width = 110;  // Đặt chiều rộng là 150px
            dgvHoaDon.Columns["Mã hàng hóa"].Width = 150;
            dgvHoaDon.Columns["Số Lượng"].Width = 80;
            dgvHoaDon.Columns["Mã nhân viên"].Width = 100;
            dgvHoaDon.Columns["Ngày lập"].Width = 200;
            dgvHoaDon.Columns["Tổng tiền"].Width = 113;

            // Xóa các trường nhập để chuẩn bị thêm mới
            LoadMaHD();
            cbbMaNhanVien.SelectedIndex = -1;
            cbbMaKhachHang.SelectedIndex = -1;
            cbbMaHangHoa.SelectedIndex = -1;
            txtSoLuong.Text = "0";
            lbDonGia.Text = "...            VND";

            CapNhatTongSoTien();
            // Cập nhật số lượng
            string querryUpdateSoLuong = "UPDATE HANG_HOA SET SoLuong = SoLuong - @SoLuong WHERE MaHH = @MaHH";
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(querryUpdateSoLuong, conn))
                {
                    cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                    cmd.Parameters.AddWithValue("@MaHH", MaHH);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Cập nhật thành công, có thể load lại số lượng nếu cần
                    }
                    else
                    {
                        MessageBox.Show("Không cập nhật được số lượng hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void CapNhatTongSoTien()
        {
            int tong = 0;

            foreach (DataGridViewRow row in dgvHoaDon.Rows)
            {
                if (row.Cells["Tổng tiền"].Value != null)
                {
                    // Bắt lỗi nếu người dùng nhập tay hoặc lỗi dữ liệu
                    if (int.TryParse(row.Cells["Tổng tiền"].Value.ToString(), out int gia))
                    {
                        tong += gia;
                    }
                }
            }

            lbTongSoTien.Text = string.Format("{0:N0} VND", tong); // Hiển thị có dấu phẩy
        }

        // Nếu form bị đóng hoặc chuyển gọi hàm close
        private void HoaDon_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dgvHoaDon.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Dữ liệu chưa được lưu. Bạn có muốn hoàn tác số lượng hàng không?",
                    "Cảnh báo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    RollBack();
                }
                if (result == DialogResult.No)
                {
                    RollBack();
                }
            }
        }
        // Back lại dữ liệu
        private void RollBack()
        {
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();

                foreach (DataGridViewRow row in dgvHoaDon.Rows)
                {
                    if (row.IsNewRow) continue;

                    string maHH = row.Cells["Mã hàng hóa"].Value.ToString();
                    int soLuong = Convert.ToInt32(row.Cells["Số Lượng"].Value);

                    string query = "UPDATE HANG_HOA SET SoLuong = SoLuong + @SoLuong WHERE MaHH = @MaHH";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmd.Parameters.AddWithValue("@MaHH", maHH);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            MessageBox.Show("Đã khôi phục số lượng tồn kho!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);   
        }


        // Tính năng 2
        // Load từ bảng lên textbox để chỉnh sửa
        private void dgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu click vào dòng mới hoặc ngoài vùng dữ liệu
            if (e.RowIndex < 0 || e.RowIndex >= dgvHoaDon.Rows.Count) return;

            DataGridViewRow row = dgvHoaDon.Rows[e.RowIndex];

            // Gán giá trị lên các điều khiển tương ứng
            txtMaHD.Text = row.Cells["Mã hóa đơn"].Value.ToString();
            cbbMaKhachHang.Text = row.Cells["Mã khách hàng"].Value.ToString();
            cbbMaNhanVien.Text = row.Cells["Mã nhân viên"].Value.ToString();
            cbbMaHangHoa.Text = row.Cells["Mã hàng hóa"].Value.ToString();
            txtSoLuong.Text = row.Cells["Số Lượng"].Value.ToString();

            string ngayStr = row.Cells["Ngày lập"].Value.ToString();
            DateTime ngayLap;

            if (DateTime.TryParseExact(ngayStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayLap))
            {
                dtNgayLap.Value = ngayLap;
            }

            // Nếu có cột đơn giá/tổng tiền cần format lại
            if (row.Cells["Tổng tiền"].Value != null)
            {
                int tongTien = Convert.ToInt32(row.Cells["Tổng tiền"].Value);
                lbDonGia.Text = string.Format("{0:N0} VND", tongTien);
            }
        }

        private void btnUpdateBang_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null || dgvHoaDon.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn dòng cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvHoaDon.CurrentRow;

            // ✅ LẤY SỐ LƯỢNG CŨ TRƯỚC
            int soLuongCu = Convert.ToInt32(row.Cells["Số Lượng"].Value);

            // Lấy dữ liệu mới từ form
            string maHD = txtMaHD.Text.Trim();
            string maKH = cbbMaKhachHang.Text.Trim();
            string maHH = cbbMaHangHoa.Text.Trim();
            string maNV = cbbMaNhanVien.Text.Trim();
            string soLuongStr = txtSoLuong.Text.Trim();
            string donGiaStr = lbDonGia.Text.Replace("VND", "").Trim().Replace(",", "");
            string ngayLap = dtNgayLap.Value.ToString("dd/MM/yyyy");

            if (!int.TryParse(soLuongStr, out int soLuong) || !int.TryParse(donGiaStr, out int donGia))
            {
                MessageBox.Show("Số lượng hoặc đơn giá không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int tongTien = soLuong * donGia;

            // ✅ TÍNH CHÊNH LỆCH VÀ CẬP NHẬT TỒN KHO
            int chenhLech = soLuong - soLuongCu;

            if (chenhLech != 0)
            {
                using (MySqlConnection conn = mysql.sql())
                {
                    conn.Open();
                    string sqlUpdateSoLuong = "UPDATE HANG_HOA SET SoLuong = SoLuong - @ChenhLech WHERE MaHH = @MaHH";

                    using (MySqlCommand cmd = new MySqlCommand(sqlUpdateSoLuong, conn))
                    {
                        cmd.Parameters.AddWithValue("@ChenhLech", chenhLech); // có thể âm hoặc dương
                        cmd.Parameters.AddWithValue("@MaHH", maHH);
                        cmd.ExecuteNonQuery();
                    }
                }
            }

            // Cập nhật vào bảng
            row.Cells["Mã hóa đơn"].Value = maHD;
            row.Cells["Mã khách hàng"].Value = maKH;
            row.Cells["Mã hàng hóa"].Value = maHH;
            row.Cells["Số Lượng"].Value = soLuong;
            row.Cells["Mã nhân viên"].Value = maNV;
            row.Cells["Ngày lập"].Value = ngayLap;
            row.Cells["Tổng tiền"].Value = tongTien;

            CapNhatTongSoTien();

            MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaDLBang_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null || dgvHoaDon.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvHoaDon.CurrentRow;

            // Xác nhận trước khi xóa
            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa dòng này không?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No) return;

            // Lấy mã hàng hóa và số lượng để CỘNG LẠI TỒN KHO
            string maHH = row.Cells["Mã hàng hóa"].Value.ToString();
            int soLuong = Convert.ToInt32(row.Cells["Số Lượng"].Value);

            // Cộng lại số lượng trong DB
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                string query = "UPDATE HANG_HOA SET SoLuong = SoLuong + @SoLuong WHERE MaHH = @MaHH";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                    cmd.Parameters.AddWithValue("@MaHH", maHH);
                    cmd.ExecuteNonQuery();
                }
            }

            // Xóa dòng khỏi bảng
            dgvHoaDon.Rows.Remove(row);

            // Cập nhật lại tổng tiền
            CapNhatTongSoTien();

            MessageBox.Show("Đã xóa dòng và cập nhật tồn kho!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.Rows.Count <= 0)
            {
                MessageBox.Show("Không có dữ liệu để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soDonLuuThanhCong = 0;

            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();

                foreach (DataGridViewRow row in dgvHoaDon.Rows)
                {
                    if (row.IsNewRow) continue;

                    string maHD = row.Cells["Mã hóa đơn"].Value.ToString();
                    string maKH = row.Cells["Mã khách hàng"].Value.ToString();
                    string maNV = row.Cells["Mã nhân viên"].Value.ToString();
                    string maHH = row.Cells["Mã hàng hóa"].Value.ToString();
                    int soLuong = Convert.ToInt32(row.Cells["Số Lượng"].Value);
                    int tongTien = Convert.ToInt32(row.Cells["Tổng tiền"].Value);

                    DateTime ngayLap;
                    if (!DateTime.TryParseExact(row.Cells["Ngày lập"].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayLap))
                    {
                        MessageBox.Show($"Lỗi định dạng ngày ở hóa đơn {maHD}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    // Kiểm tra trùng mã hóa đơn trong DB
                    string checkQuery = "SELECT COUNT(*) FROM HOA_DON WHERE MaHD = @MaHD";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaHD", maHD);
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show($"Mã hóa đơn {maHD} đã tồn tại trong CSDL. Dừng lưu!", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; 
                        }
                    }

                    string query = @"INSERT INTO HOA_DON (MaHD, MaKH, MaNV, MaHH, SoLuong, NgayLap, TongTien) 
                             VALUES (@MaHD, @MaKH, @MaNV, @MaHH, @SoLuong, @NgayLap, @TongTien)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", maHD);
                        cmd.Parameters.AddWithValue("@MaKH", maKH);
                        cmd.Parameters.AddWithValue("@MaNV", maNV);
                        cmd.Parameters.AddWithValue("@MaHH", maHH);
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmd.Parameters.AddWithValue("@NgayLap", ngayLap);
                        cmd.Parameters.AddWithValue("@TongTien", tongTien);

                        cmd.ExecuteNonQuery();
                        soDonLuuThanhCong++;
                    }

                    // Update điểm tích lũy khách hàng
                    string queryUpdateDiem = "UPDATE KHACH_HANG SET DiemTichLuy = DiemTichLuy + 3 WHERE MaKH = @MaKH";
                    using (MySqlCommand diemCmd = new MySqlCommand(queryUpdateDiem, conn))
                    {
                        diemCmd.Parameters.AddWithValue("@MaKH", maKH);
                        diemCmd.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show($"Đã lưu thành công {soDonLuuThanhCong} hóa đơn vào CSDL.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Reload lại dgv
            dgvHoaDon.DataSource = null;
            dgvHoaDon.Rows.Clear();
            dgvHoaDon.Columns.Clear();
        }

        private void btnTaoTTKhachHang_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.Rows.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có hóa đơn chưa lưu. Bạn có muốn lưu trước khi chuyển trang?",
                    "Cảnh báo",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Cancel) return;
                if (result == DialogResult.Yes)
                {
                    btnLuu_Click(null, null); 
                }
            }

            this.Hide();
            TaoKhachHang fTaoKhachHang = new TaoKhachHang();
            fTaoKhachHang.Show();
        }
    }
}
