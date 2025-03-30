using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using qlsieuthi.src.main.admin;
using qlsieuthi.src.main.qlkho.libqlkho;

namespace qlsieuthi.src.main.qlkho
{
    public partial class themhanghoa : Form
    {
        // Lib
        connectdb mysql = new connectdb();
        private LoadData dbLoad = new LoadData();
        private DataTable dgvhead = new DataTable();
        InputQLkho dulieu = new InputQLkho();
        public themhanghoa()
        {
            InitializeComponent();

            // Load dữ liệu mã loại hàng & mã nhà cung cấp
            dbLoad.LoadCBBMaLoaiHang(cbbMaLoai);
            dbLoad.LoadCBBMaNhaCC(cbbMaNCC);

            LoadDGV();
        }

        // Load datagridview
        public void LoadDGV()
        {
            dgvhead = new DataTable();

            dgvhead.Columns.Add("Mã hàng hóa"); //0
            dgvhead.Columns.Add("Tên hàng hóa"); //1
            dgvhead.Columns.Add("Mã loại hàng"); //2
            dgvhead.Columns.Add("Số lượng"); //3
            dgvhead.Columns.Add("Đơn giá (VND)"); //4
            dgvhead.Columns.Add("Đơn vị tính (chai/kg/...)"); //5
            dgvhead.Columns.Add("Mã nhà cung cấp hàng"); //6

            dgvThemHH.DataSource = dgvhead;
        }
        // Đặt các giá trị về mặc định
        void ReloadInput()
        {
            txtMaHH.Text = "Tạo tự động";
            txtTenHH.Text = "";
            txtDonGia.Text = "";
            UpDowSoLuong.Value = 0;
            txtDonViTinh.Text = "";
        }
        // Chỉ nhập số vào các ô SDT và Lương
        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }
        /*
         * THÊM ĐƠN HÀNG VÀO BẢNG TRƯỚC ĐÓ
         */
        private void btnThemVaoBang_Click(object sender, EventArgs e)
        {
            dulieu.TenHH = txtTenHH.Text.Trim();

            string MaLoaiHang = cbbMaLoai.Text.Trim();

            dulieu.SoLuong = UpDowSoLuong.Value.ToString();
            dulieu.DonGia = txtDonGia.Text.Trim();
            dulieu.DonViTinh = txtDonViTinh.Text.Trim();

            string MaNhaCC = cbbMaNCC.Text.Trim();

            // Gán mã hàng hóa = MaLoaiHang + (lấy chữ đầu tiên)Tên hàng hóa + chuỗi 4 số random
            string Shorted_TenHH = dulieu.TenHH.Substring(0,1).ToUpper();
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
            if(txtDonGia.Text.Trim() == "0")
            {
                MessageBox.Show("Đơn giá không thể = 0");
                txtDonGia.Focus();
                return;
            }
            if(UpDowSoLuong.Value < 0)
            {
                MessageBox.Show("Số lượng không thể bằng 0");
                UpDowSoLuong.Focus();
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
            DialogResult themvaobang = MessageBox.Show($"- Mã hàng hóa : {dulieu.MaHH}" +
                $"\n- Tên hàng hóa : {dulieu.TenHH}" +
                $"\n- Mã loại hàng : {MaLoaiHang}" +
                $"\n- Số lượng : {dulieu.SoLuong} {dulieu.DonViTinh}" +
                $"\n- Đơn giá : {formattedDonGia} VND"+
                $"\n- Mã nhà cung cấp : {MaNhaCC}","Xác nhận thêm"
                ,MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (themvaobang == DialogResult.Yes)
            {
                foreach (DataGridViewColumn column in dgvThemHH.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                dgvhead.Rows.Add(dulieu.MaHH , dulieu.TenHH , MaLoaiHang , dulieu.SoLuong , dongia.ToString("#,##0") , dulieu.DonViTinh , MaNhaCC);
                MessageBox.Show("Thêm thành công");

                ReloadInput();
            }
        }

        // Xóa hàng dữ liệu được chọn
        private void btnXoaKhoiBang_Click(object sender, EventArgs e)
        {
            // Format đơn giá
            decimal dongia;
            if (!decimal.TryParse(dulieu.DonGia, out dongia))
            {
                MessageBox.Show("Lỗi: Đơn giá không hợp lệ!");
                txtDonGia.Focus();
                return;
            }
            string formattedDonGia = dongia.ToString("#,##0");

            if (dgvThemHH.SelectedRows.Count > 0) // Kiểm tra có hàng được chọn không
            {
                DataGridViewRow selectedRow = dgvThemHH.SelectedRows[0]; 
                string MaLoaiHang = selectedRow.Cells[2].Value.ToString(); 
                string MaNhaCC = selectedRow.Cells[6].Value.ToString();

                DialogResult xacnhanxoa = MessageBox.Show($"- Mã hàng hóa : {dulieu.MaHH}" +
                    $"\n- Tên hàng hóa : {dulieu.TenHH}" +
                    $"\n- Mã loại hàng : {MaLoaiHang}" +
                    $"\n- Số lượng : {dulieu.SoLuong} {dulieu.DonViTinh}" +
                    $"\n- Đơn giá : {formattedDonGia} VND" +
                    $"\n- Mã nhà cung cấp : {MaNhaCC}" +
                    "\n\nLưu ý: Dữ liệu bị xóa sẽ không khôi phục được", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (xacnhanxoa == DialogResult.Yes)
                {
                    dgvThemHH.Rows.Remove(selectedRow); // Xóa hàng được chọn
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hàng để xóa!");
            }

        }

        /*
         * THÊM DANH SÁCH TỪ BẢNG VÀO CSDL
         */
        //===================//
        /*
         * Lưu ý:
         * Vì có tỉ lệ xác xuất bị trùng mã hàng hóa nên sẽ quét qua
         * bảng trước 1 lượt 
         * - Nếu bị trùng những dòng bị trùng sẽ có màu vàng và bắt sửa lại
         * - Nếu không bị trùng quét thành công
         * 
         * - Bước tiếp theo : 
         * + Sau khi quét thành công sẽ thêm vào CSDL
         */
        //===================//
        private void btnThemCSDL_Click(object sender, EventArgs e)
        {
            string querycheck = "SELECT MaHH FROM HANG_HOA WHERE MaHH = @MaHH";
            string queryadd = "INSERT INTO HANG_HOA (MaHH, TenHH, MaLoai, SoLuong, DonGia, DonViTinh, MaNCC) " +
                "VALUES (@MaHH, @TenHH, @MaLoai, @SoLuong, @DonGia, @DonViTinh, @MaNCC)";
            bool TrungLapDuLieu = false; // Mặc định trùng lặp là sai
            List<DataGridViewRow> ThemVaoSQL = new List<DataGridViewRow>();

            /*
             * Bước 1 quét dữ liệu trong bảng
             */
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmdcheck = new MySqlCommand(querycheck, conn))
                {
                    foreach(DataGridViewRow row in dgvThemHH.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            dulieu.MaHH = row.Cells[0].Value.ToString();
                            cmdcheck.Parameters.Clear();
                            cmdcheck.Parameters.AddWithValue("@MaHH", dulieu.MaHH);

                            object trunglap = cmdcheck.ExecuteScalar();
                            if (trunglap != null)
                            {
                                row.DefaultCellStyle.ForeColor = Color.Red; //Đánh dấu màu đỏ
                                TrungLapDuLieu = true; // Thông báo
                            }
                            else
                            {
                                row.DefaultCellStyle.ForeColor = Color.Green; //Đánh dấu màu xanh
                                ThemVaoSQL.Add(row);
                            }
                        }
                    }
                }
            }

            // Hàm con
            /*
             * Nếu có trùng lặp dừng ngay lập tức
             */
            if (TrungLapDuLieu)
            {
                MessageBox.Show("Mã hàng hóa này đã bị trùng lặp", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return; // Dừng lại
            }

            /*
             * Bước 2: Sau khi quét và không còn trùng lặp nữa 
             * thì sẽ lưu vào CSDL
             */
            int count = 0;
            DialogResult xacnhanthem = MessageBox.Show("Bạn chắc chắn muốn thêm vào CSDL", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(xacnhanthem == DialogResult.Yes)
            {
                using (MySqlConnection conn = mysql.sql())
                {
                    conn.Open();
                    using (MySqlCommand cmdThem = new MySqlCommand(queryadd, conn))
                    {
                        foreach (DataGridViewRow row in ThemVaoSQL)
                        {
                            try
                            {
                                cmdThem.Parameters.Clear();

                                string MaLoaiHang = row.Cells[2].Value?.ToString().Split('-')[0].Trim() ?? "";
                                string MaNhaCC = row.Cells[6].Value?.ToString().Split('-')[0].Trim() ?? "";

                                decimal donGia;
                                string donGiaStr = row.Cells[4].Value?.ToString().Replace(",", "").Trim();
                                if (!decimal.TryParse(donGiaStr, out donGia))
                                {
                                    MessageBox.Show($"Lỗi: Đơn giá '{donGiaStr}' không hợp lệ!");
                                    continue;
                                }

                                // Làm tròn về số nguyên nếu cần
                                donGia = Math.Round(donGia, 0);

                                cmdThem.Parameters.AddWithValue("@MaHH", row.Cells[0].Value?.ToString() ?? "");
                                cmdThem.Parameters.AddWithValue("@TenHH", row.Cells[1].Value?.ToString() ?? "");
                                cmdThem.Parameters.AddWithValue("@MaLoai", MaLoaiHang);
                                cmdThem.Parameters.AddWithValue("@SoLuong", row.Cells[3].Value?.ToString() ?? "");
                                cmdThem.Parameters.AddWithValue("@DonGia", donGia);
                                cmdThem.Parameters.AddWithValue("@DonViTinh", row.Cells[5].Value?.ToString() ?? "");
                                cmdThem.Parameters.AddWithValue("@MaNCC", MaNhaCC);

                                int themthanhcong = cmdThem.ExecuteNonQuery();

                                if (themthanhcong > 0)
                                {
                                    count++;
                                }
                            }
                            catch (Exception ex) 
                            {
                                MessageBox.Show($"Lỗi khi thêm vào CSDL {ex.Message}");
                                return;
                            }
                        }
                        MessageBox.Show($"Thêm thành công {count} vào CSDL");

                        dgvThemHH.DataSource = null;

                        dgvThemHH.Rows.Clear();
                        LoadDGV();
                        foreach (DataGridViewColumn column in dgvThemHH.Columns)
                        {
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        }
                    }
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            btn closeform = new btn();
            closeform.Xacnhandong(this);
        }
    }
}
