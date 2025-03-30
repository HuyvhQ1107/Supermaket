using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using qlsieuthi.src.main.ketoan.lib;

namespace qlsieuthi.src.main.ketoan
{
    public partial class thuchi : Form
    {
        connectdb mysql = new connectdb();
        private LoadDataKT dbkt = new LoadDataKT();
        public void cbb_ThuChi()
        {
            cbbLoaiGD.Items.Clear();
            cbbLoaiGD.Items.Add("Thu");
            cbbLoaiGD.Items.Add("Chi");

            cbbLoaiGD.SelectedIndex = -1;
        }
        public thuchi()
        {
            InitializeComponent();
            dbkt.LoadCBBMaHD(cbbMaHoaDon);
            dbkt.LoadCBBMaKho(cbbMaKho);
            cbb_ThuChi();
        }

        private void btnThemBang_Click(object sender, EventArgs e)
        {
            string MaGD = txtMaGD.Text.Trim();
            string LoaiGD = cbbLoaiGD.Text.Trim();
            string NgayGD = dtNgayGD.Value.ToString("dd/MM/yyyy");
            string MoTa = txtMoTa.Text.Trim();
            string MaHD = cbbMaHoaDon.Text.Trim();
            string MaKho = cbbMaKho.Text.Trim();

            // Kiểm tra dữ liệu
            if (string.IsNullOrEmpty(MaGD) || string.IsNullOrEmpty(LoaiGD))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã GD, Loại GD và Số Tiền!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra trùng MaGD trong dgv
            foreach (DataGridViewRow row in dgvThuChi.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["Mã giao dịch"].Value.ToString() == MaGD)
                {
                    MessageBox.Show($"Mã giao dịch {MaGD} đã tồn tại trong bảng!", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Load số tiền vảo bảng từ Mã hóa đơn từ bảng HOA_DON
            string soTienStr = "";

            if (!string.IsNullOrEmpty(MaHD))
            {
                using (MySqlConnection conn = mysql.sql())
                {
                    conn.Open();
                    string query = "SELECT TongTien FROM HOA_DON WHERE MaHD = @MaHD";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaHD", MaHD);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            soTienStr = result.ToString();
                        }
                        else
                        {
                            MessageBox.Show($"Không tìm thấy hóa đơn {MaHD} trong CSDL!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn Mã hóa đơn!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Parse số tiền
            if (!decimal.TryParse(soTienStr, out decimal soTien))
            {
                MessageBox.Show("Số tiền không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tạo bảng nếu chưa có
            DataTable dt;
            if (dgvThuChi.DataSource == null)
            {
                dt = new DataTable();
                dt.Columns.Add("Mã giao dịch");
                dt.Columns.Add("Loại GD");
                dt.Columns.Add("Ngày GD");
                dt.Columns.Add("Số Tiền", typeof(decimal));
                dt.Columns.Add("Mô Tả");
                dt.Columns.Add("Mã hóa đơn");
                dt.Columns.Add("Mã kho");
            }
            else
            {
                dt = (DataTable)dgvThuChi.DataSource;
            }

            // Thêm dòng mới
            DataRow newRow = dt.NewRow();
            newRow["Mã giao dịch"] = MaGD;
            newRow["Loại GD"] = LoaiGD;
            newRow["Ngày GD"] = NgayGD;
            newRow["Số Tiền"] = soTien;
            newRow["Mô Tả"] = MoTa;
            newRow["Mã hóa đơn"] = MaHD;
            newRow["Mã kho"] = MaKho;

            dt.Rows.Add(newRow);
            dgvThuChi.DataSource = dt;

            CanChinhCotThuChi();

            //MessageBox.Show("Đã thêm giao dịch vào bảng!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reset input
            txtMaGD.Clear();
            cbbLoaiGD.SelectedIndex = -1;
            txtMoTa.Clear();
            cbbMaHoaDon.SelectedIndex = -1;
            cbbMaKho.SelectedIndex = -1;
        }
        private void CanChinhCotThuChi()
        {
            dgvThuChi.Columns["Mã giao dịch"].Width = 110;
            dgvThuChi.Columns["Loại GD"].Width = 70;
            dgvThuChi.Columns["Ngày GD"].Width = 100;
            dgvThuChi.Columns["Số Tiền"].Width = 120;
            dgvThuChi.Columns["Mô Tả"].Width = 250;
            dgvThuChi.Columns["Mã hóa đơn"].Width = 110;
            dgvThuChi.Columns["Mã kho"].Width = 91;

            // Căn phải số tiền cho đẹp
            dgvThuChi.Columns["Số Tiền"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Định dạng tiền tệ
            dgvThuChi.Columns["Số Tiền"].DefaultCellStyle.Format = "#,##0 VND";
        }

        private void dgvThuChi_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu click ngoài vùng dữ liệu
            if (e.RowIndex < 0 || e.RowIndex >= dgvThuChi.Rows.Count) return;

            DataGridViewRow row = dgvThuChi.Rows[e.RowIndex];

            // Lấy và hiển thị dữ liệu lên các control
            txtMaGD.Text = row.Cells["Mã giao dịch"].Value?.ToString();
            cbbLoaiGD.Text = row.Cells["Loại GD"].Value?.ToString();
            txtMoTa.Text = row.Cells["Mô Tả"].Value?.ToString();
            cbbMaHoaDon.Text = row.Cells["Mã hóa đơn"].Value?.ToString();
            cbbMaKho.Text = row.Cells["Mã kho"].Value?.ToString();

            // Parse ngày
            if (DateTime.TryParse(row.Cells["Ngày GD"].Value?.ToString(), out DateTime ngayGD))
            {
                dtNgayGD.Value = ngayGD;
            }
            else
            {
                dtNgayGD.Value = DateTime.Now; // fallback nếu lỗi
            }
        }


        private void btnUpdateBang_Click(object sender, EventArgs e)
        {
            if (dgvThuChi.CurrentRow == null || dgvThuChi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn dòng cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvThuChi.CurrentRow;

            string maGD = txtMaGD.Text.Trim();
            string loaiGD = cbbLoaiGD.Text.Trim();
            string moTa = txtMoTa.Text.Trim();
            string maHD = cbbMaHoaDon.Text.Trim();
            string maKho = cbbMaKho.Text.Trim();
            DateTime ngayGD = dtNgayGD.Value;

            // Kiểm tra rỗng
            if (string.IsNullOrEmpty(maGD) || string.IsNullOrEmpty(loaiGD) || string.IsNullOrEmpty(maHD))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã GD, Loại GD và Mã hóa đơn!", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Lấy số tiền nếu cbb điều chỉnh
            decimal soTien = 0;
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                string query = "SELECT TongTien FROM HOA_DON WHERE MaHD = @MaHD";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaHD", maHD);
                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show($"Không tìm thấy hóa đơn {maHD}!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    soTien = Convert.ToDecimal(result);
                }
            }

            // Cập nhật lại dòng
            row.Cells["Mã giao dịch"].Value = maGD;
            row.Cells["Loại GD"].Value = loaiGD;
            row.Cells["Ngày GD"].Value = ngayGD.ToString("dd/MM/yyyy");
            row.Cells["Số Tiền"].Value = soTien;
            row.Cells["Mô Tả"].Value = moTa;
            row.Cells["Mã hóa đơn"].Value = maHD;
            row.Cells["Mã kho"].Value = maKho;

            MessageBox.Show("Đã cập nhật thành công dòng giao dịch!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnXoaDLBang_Click(object sender, EventArgs e)
        {
            if (dgvThuChi.CurrentRow == null || dgvThuChi.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dòng này không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            // Nếu dùng DataSource
            if (dgvThuChi.DataSource is DataTable dt)
            {
                dt.Rows.RemoveAt(dgvThuChi.CurrentRow.Index);
            }
            else
            {
                dgvThuChi.Rows.Remove(dgvThuChi.CurrentRow);
            }

            MessageBox.Show("Đã xóa dòng khỏi bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (dgvThuChi.Rows.Count <= 0)
            {
                MessageBox.Show("Không có dữ liệu để lưu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int soDongLuuThanhCong = 0;

            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();

                foreach (DataGridViewRow row in dgvThuChi.Rows)
                {
                    if (row.IsNewRow) continue;

                    string maGD = row.Cells["Mã giao dịch"].Value.ToString();
                    string loaiGD = row.Cells["Loại GD"].Value.ToString();
                    string moTa = row.Cells["Mô Tả"].Value?.ToString();
                    string maHD = row.Cells["Mã hóa đơn"].Value?.ToString();
                    string maKho = row.Cells["Mã kho"].Value?.ToString();

                    DateTime ngayGD;
                    if (!DateTime.TryParseExact(row.Cells["Ngày GD"].Value.ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngayGD))
                    {
                        MessageBox.Show($"Ngày giao dịch không hợp lệ tại Mã GD: {maGD}", "Lỗi ngày", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    if (!decimal.TryParse(row.Cells["Số Tiền"].Value.ToString(), out decimal soTien))
                    {
                        MessageBox.Show($"Số tiền không hợp lệ tại Mã GD: {maGD}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        continue;
                    }

                    // Kiểm tra trùng MaGD trong DB
                    string checkQuery = "SELECT COUNT(*) FROM THU_CHI WHERE MaGD = @MaGD";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@MaGD", maGD);
                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (exists > 0)
                        {
                            MessageBox.Show($"Mã giao dịch {maGD} đã tồn tại! Dừng lưu!", "Trùng mã", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // INSERT vào GIAO_DICH
                    string insertQuery = @"INSERT INTO THU_CHI (MaGD, LoaiGD, NgayGD, SoTien, MoTa, MaHD, MaKho)
                                   VALUES (@MaGD, @LoaiGD, @NgayGD, @SoTien, @MoTa, @MaHD, @MaKho)";
                    using (MySqlCommand cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaGD", maGD);
                        cmd.Parameters.AddWithValue("@LoaiGD", loaiGD);
                        cmd.Parameters.AddWithValue("@NgayGD", ngayGD);
                        cmd.Parameters.AddWithValue("@SoTien", soTien);
                        cmd.Parameters.AddWithValue("@MoTa", string.IsNullOrEmpty(moTa) ? (object)DBNull.Value : moTa);
                        cmd.Parameters.AddWithValue("@MaHD", string.IsNullOrEmpty(maHD) ? (object)DBNull.Value : maHD);
                        cmd.Parameters.AddWithValue("@MaKho", string.IsNullOrEmpty(maKho) ? (object)DBNull.Value : maKho);

                        cmd.ExecuteNonQuery();
                        soDongLuuThanhCong++;
                    }
                }
            }

            MessageBox.Show($"Đã lưu thành công {soDongLuuThanhCong} dòng vào bảng GIAO_DICH!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Xóa bảng nếu muốn
            if (dgvThuChi.DataSource is DataTable dt)
                dt.Rows.Clear();
            else
                dgvThuChi.Rows.Clear();
        }

    }
}
