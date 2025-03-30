using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using qlsieuthi.src.main.qlkho.libqlkho;

namespace qlsieuthi.src.main.qlkho
{
    public partial class SuaHangHoa : Form
    {
        // Gọi lib
        connectdb mysql = new connectdb();
        private LoadData dbLoad = new LoadData();
        InputQLkho dulieu = new InputQLkho();

        void RenameDGV()
        {
            dgvBangHH.Columns[0].HeaderText = "STT";
            dgvBangHH.Columns[1].HeaderText = "Mã hàng hóa";
            dgvBangHH.Columns[2].HeaderText = "Tên hàng hóa";
            dgvBangHH.Columns[3].HeaderText = "Mã loại hàng hóa";
            dgvBangHH.Columns[4].HeaderText = "Số lượng";
            dgvBangHH.Columns[5].HeaderText = "Đơn giá";
            dgvBangHH.Columns[6].HeaderText = "Đơn vị tính";
            dgvBangHH.Columns[7].HeaderText = "Mã nhà cung cấp";

            dgvBangHH.Columns[0].Width = 50;
            dgvBangHH.Columns[1].Width = 150;
            dgvBangHH.Columns[2].Width = 100;
            dgvBangHH.Columns[3].Width = 130;
            dgvBangHH.Columns[4].Width = 80;
            dgvBangHH.Columns[5].Width = 80;
            dgvBangHH.Columns[6].Width = 85;
            dgvBangHH.Columns[7].Width = 70;

            dgvBangHH.Columns[4].DefaultCellStyle.Format = "#,##0";
            dgvBangHH.Columns[5].DefaultCellStyle.Format = "#,##0 VND";

            dgvBangHH.Columns[8].Visible = false;
        }
        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }
        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }
        public SuaHangHoa()
        {
            InitializeComponent();

            dbLoad.LoadCBBTenHH(cbbLocTenHH);
        }

        /*
         * TÍNH NĂNG 
         * - Lựa chọn hàng hóa có trong bảng 
         * - Đưa lên các box , cbb
         * - Sửa thông tin
         */
        // Lọc ở cbb
        private void cbbLocTenHH_DropDownClosed(object sender, EventArgs e)
        {
            LocTheoTenHH();
        }
        void LocTheoTenHH()
        {
            string LayTenHH = cbbLocTenHH.Text;
            string QueryLocTenHH = "SELECT * FROM HANG_HOA WHERE TenHH = @TenHH";

            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(QueryLocTenHH, conn))
                {
                    cmd.Parameters.AddWithValue("@TenHH", LayTenHH);

                    using (MySqlDataAdapter apdater = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        apdater.Fill(dt);

                        dgvBangHH.DataSource = dt;

                        dgvBangHH.Refresh();
                        RenameDGV();
                    }
                }
            }
        }
        // Sự kiện khi người dùng click vào bảng chọn 
        // dòng để sửa dữ liệu
        private int HangTruocDaChon = -1; // Lưu chỉ số hàng trước đó
        private void dgvLoadHH_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                // Load các dữ liệu vào cbb
                dbLoad.LoadCBBMaLoaiHang(cbbMaLoai);
                dbLoad.LoadCBBMaNhaCC(cbbMaNCC);


                DataGridViewRow row = dgvBangHH.Rows[e.RowIndex];

                // Hiển thị ô lên textbox
                txtIDHH.Text = row.Cells[0].Value?.ToString();
                txtMaHH.Text = row.Cells[1].Value?.ToString();
                txtTenHH.Text = row.Cells[2].Value?.ToString();
                txtSoLuong.Text = row.Cells[4].Value?.ToString();
                txtDonGia.Text = row.Cells[5].Value?.ToString();
                txtDonViTinh.Text = row.Cells[6].Value?.ToString();

                // Hiển thị trên cbb
                string MaLoai = row.Cells[3].Value?.ToString();
                if(!string.IsNullOrEmpty(MaLoai))
                {
                    cbbMaLoai.SelectedValue = MaLoai;
                }

                string MaNhaCC = row.Cells[7].Value?.ToString();
                if (!string.IsNullOrEmpty(MaNhaCC))
                {
                    cbbMaNCC.SelectedValue = MaNhaCC;
                }

                if (HangTruocDaChon >= 0)
                {
                    dgvBangHH.Rows[HangTruocDaChon].DefaultCellStyle.ForeColor = Color.Black;
                }

                //row.Selected = true;
                row.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                row.DefaultCellStyle.SelectionForeColor = Color.Black;

                HangTruocDaChon = e.RowIndex;
            }
        }

        // Thực hiện các btn

        private void btnThemCSDL_Click(object sender, EventArgs e)
        {
            string IDHH = txtIDHH.Text.Trim();
            dulieu.TenHH = txtTenHH.Text.Trim();

            string MaLoaiHang = cbbMaLoai.Text.Trim();

            dulieu.SoLuong = txtSoLuong.Text.Trim();
            dulieu.DonGia = txtDonGia.Text.Trim();
            dulieu.DonViTinh = txtDonViTinh.Text.Trim();

            string MaNhaCC = cbbMaNCC.Text.Trim();

            // Gán mã hàng hóa = MaLoaiHang + (lấy chữ đầu tiên)Tên hàng hóa + chuỗi 4 số random
            string Shorted_TenHH = dulieu.TenHH.Substring(0, 1).ToUpper();
            string MaNgauNhien = new Random().Next(1000, 9999).ToString();

            dulieu.MaHH = MaLoaiHang + "_" + Shorted_TenHH + MaNgauNhien;
            txtMaHH.Text = dulieu.MaHH;

            // Xử dụng if để tránh mắc lỗi
            if (string.IsNullOrEmpty(dulieu.TenHH))
            {
                MessageBox.Show("Lỗi : Tên hàng hóa không được để trống");
                txtTenHH.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieu.DonGia))
            {
                MessageBox.Show("Lỗi : Đơn giá không được để trống");
                txtDonGia.Focus();
                return;
            }
            if (txtDonGia.Text.Trim() == "0")
            {
                MessageBox.Show("Đơn giá không thể = 0");
                txtDonGia.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieu.SoLuong))
            {
                MessageBox.Show("Lỗi : Không thể để trống ô số lượng");
                txtSoLuong.Focus();
                return;
            }
            // Format đơn giá
            decimal dongia;
            if (!decimal.TryParse(dulieu.DonGia, out dongia))
            {
                MessageBox.Show("Lỗi: Đơn giá không hợp lệ!");
                txtDonGia.Focus();
                return;
            }

            string formattedDonGia = dongia.ToString("#,##0");

            // Update lên csdl
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                // query
                string QuerryUpdate = "UPDATE HANG_HOA SET " +
                    "MaHH = @MaHH, " +
                    "TenHH = @TenHH, " +
                    "MaLoai = @MaLoai, " +
                    "SoLuong = @SoLuong, " +
                    "DonGia = @DonGia, " +
                    "DonViTinh = @DonViTinh, " +
                    "MaNCC = @MaNCC " +
                    "WHERE ID = @ID";
                //MessageBox.Show(QuerryUpdate);
                using (MySqlCommand cmd = new MySqlCommand(QuerryUpdate, conn))
                {
                    // Loại bỏ các phần không cần thiết
                    string MaLoaiHang2 = cbbMaLoai.Text.Split('-')[0].Trim();
                    string MaNhaCC2 = cbbMaNCC.Text.Split('-')[0].Trim();

                    decimal donGia;
                    string donGiaStr = txtDonGia.Text.Trim();
                    if (!decimal.TryParse(donGiaStr, out donGia))
                    {
                        MessageBox.Show($"Lỗi: Đơn giá '{donGiaStr}' không hợp lệ!");
                        return;
                    }

                    // Thực hiện lệnh
                    cmd.Parameters.AddWithValue("@MaHH", dulieu.MaHH);
                    cmd.Parameters.AddWithValue("@TenHH", dulieu.TenHH);
                    cmd.Parameters.AddWithValue("@MaLoai", MaLoaiHang2);
                    cmd.Parameters.AddWithValue("@SoLuong", dulieu.SoLuong);
                    cmd.Parameters.AddWithValue("@DonGia", donGia);
                    cmd.Parameters.AddWithValue("@DonViTinh", dulieu.DonViTinh);
                    cmd.Parameters.AddWithValue("@MaNCC", MaNhaCC2);
                    cmd.Parameters.AddWithValue("@ID", IDHH);

                    int CapNhat = cmd.ExecuteNonQuery();

                    DialogResult xacnhanThem = MessageBox.Show($"- ID : {IDHH}" +
                        $"\n- Mã hàng hóa : {dulieu.MaHH}" +
                        $"\n- Mã loại hàng : {MaLoaiHang}" +
                        $"\n- Số lượng : {dulieu.SoLuong} {dulieu.DonViTinh}" +
                        $"\n- Đơn giá : {formattedDonGia} VND" +
                        $"\n- Mã nhà cung cấp : {MaNhaCC}" , 
                        "Xác nhận chỉnh sửa", MessageBoxButtons.YesNo , MessageBoxIcon.Question);
                    if (xacnhanThem == DialogResult.Yes)
                    {
                        if ( CapNhat > 0)
                        {
                            MessageBox.Show("Cập nhật thành công");
                            LocTheoTenHH();
                        }
                    }
                }
            }
        }
    }
}
