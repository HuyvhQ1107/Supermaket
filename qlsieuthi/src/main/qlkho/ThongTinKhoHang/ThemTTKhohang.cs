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
using qlsieuthi.src.main.qlkho.libqlkho;

namespace qlsieuthi.src.main.qlkho.ThongTinKhoHang
{
    public partial class ThemTTKhoHang : Form
    {
        //Lib
        private LoadData dbLoad = new LoadData();
        lib mysql = new lib();
        TTKhoHang dulieukhohang = new TTKhoHang();

        public ThemTTKhoHang()
        {
            InitializeComponent();

            // Load cbb
            dbLoad.LoadCBBMaHH(cbbMaHangHoa);
            dbLoad.LoadCBBMaKho(cbbMaKho);

            LoadDGVKhoHang();
        }
        // Load datagridview
        public void LoadDGVKhoHang()
        {
            DataTable dgvHeadLoad = new DataTable();

            dgvHeadLoad.Columns.Add("Mã kho hàng"); 
            dgvHeadLoad.Columns.Add("Mã hàng hóa");
            dgvHeadLoad.Columns.Add("Ngày nhập");
            dgvHeadLoad.Columns.Add("Ngày xuất");
            dgvHeadLoad.Columns.Add("Số lượng");

            dgvLoadQLKho.DataSource = dgvHeadLoad;

            ResizeDGVKhoHang();
        }
        void ResizeDGVKhoHang()
        {
            dgvLoadQLKho.Columns[0].Width = 100;
            dgvLoadQLKho.Columns[1].Width = 200;
            dgvLoadQLKho.Columns[2].Width = 170;
            dgvLoadQLKho.Columns[3].Width = 170;
            dgvLoadQLKho.Columns[4].Width = 105;

            dgvLoadQLKho.Columns[4].DefaultCellStyle.Format = "#,##0";
        }

        /*
         * LẤY DỮ LIỆU TỪ CBB Mã hàng hóa để give số lượng vào textbox
         */
        private void cbbMaHH_DropDownClosed(object sender, EventArgs e)
        {
            InputSoLuong();
        }
        void InputSoLuong()
        {
            string layMaHH = cbbMaHangHoa.Text;
            string QueryLaySoLuong = "SELECT SoLuong FROM HANG_HOA WHERE MaHH = @MaHH";

            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(QueryLaySoLuong , conn))
                {
                    cmd.Parameters.AddWithValue("@MaHH", layMaHH);

                    // Add vào textbox
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int soluong = reader.GetInt32("SoLuong");
                            txtSoLuong.Text = soluong.ToString();
                        }
                        else
                        {
                            txtSoLuong.Text = "0";
                        }
                    }
                }
            }
        }

        private void btnThemVaoBang_Click(object sender, EventArgs e)
        {
            dulieukhohang.MaKho = cbbMaKho.Text.Trim();
            dulieukhohang.NgayNhap = dtNgayNhap.Value.ToString("yyyy-MM-dd");
            dulieukhohang.MaHangHoa = cbbMaHangHoa.Text.Trim();
            dulieukhohang.SoLuongHH = txtSoLuong.Text.Trim();

            if (string.IsNullOrEmpty(dulieukhohang.MaKho))
            {
                MessageBox.Show("Lỗi: Không thể để trống mã kho");
                cbbMaKho.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dulieukhohang.SoLuongHH))
            {
                MessageBox.Show("Lỗi: Không thể để trống số lượng hàng hóa");
                txtSoLuong.Focus();
                return;
            }

            // Kiểm tra trùng dữ liệu trong DataGridView
            foreach (DataGridViewRow row in dgvLoadQLKho.Rows)
            {
                if (row.Cells["Mã kho hàng"].Value != null && row.Cells["Mã hàng hóa"].Value != null)
                {
                    string maKhoCu = row.Cells["Mã kho hàng"].Value.ToString();
                    string maHHCu = row.Cells["Mã hàng hóa"].Value.ToString();

                    if (dulieukhohang.MaKho == maKhoCu && dulieukhohang.MaHangHoa == maHHCu)
                    {

                        MessageBox.Show($"- Mã hàng hóa {dulieukhohang.MaHangHoa} đã tồn tại trong kho {dulieukhohang.MaKho}.\nVui lòng kiểm tra lại!",
                            "Lỗi trùng dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Không thêm vào bảng
                    }
                }
            }

            // Xác nhận thêm dữ liệu
            DialogResult xacnhanthem = MessageBox.Show($"- Mã kho hàng: {dulieukhohang.MaKho}" +
                $"\n- Mã hàng hóa: {dulieukhohang.MaHangHoa}" +
                $"\n- Ngày nhập: {dulieukhohang.NgayNhap}" +
                $"\n- Số lượng hàng hóa: {dulieukhohang.SoLuongHH}",
                "Xác nhận thêm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (xacnhanthem == DialogResult.OK)
            {
                // Lấy DataTable từ DataSource
                DataTable dt = (DataTable)dgvLoadQLKho.DataSource;

                // Nếu DataTable null, tạo mới
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("Mã kho hàng");
                    dt.Columns.Add("Mã hàng hóa");
                    dt.Columns.Add("Ngày nhập");
                    dt.Columns.Add("Ngày xuất");
                    dt.Columns.Add("Số lượng");

                    dgvLoadQLKho.DataSource = dt;
                }

                // Tạo dòng mới và thêm vào DataTable
                DataRow newRow = dt.NewRow();
                newRow["Mã kho hàng"] = dulieukhohang.MaKho;
                newRow["Mã hàng hóa"] = dulieukhohang.MaHangHoa;
                newRow["Ngày nhập"] = dulieukhohang.NgayNhap;
                newRow["Ngày xuất"] = "-";  // Để trống ngày xuất
                newRow["Số lượng"] = dulieukhohang.SoLuongHH;

                dt.Rows.Add(newRow);

                // Cập nhật DataGridView
                dgvLoadQLKho.DataSource = dt;

                // Điều chỉnh cột tự động co dãn
                foreach (DataGridViewColumn column in dgvLoadQLKho.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }

                MessageBox.Show("Thêm thành công!");
            }
        }

        /**/
        private void btnUpdateCSDL_Click(object sender, EventArgs e)
        {
            int count = 0;
            string QuerryUpCSDL = "INSERT INTO THONG_TIN_KHO_HANG (MaKho, MaHHKho, NgayNhap, Soluonghh) " +
                                         "VALUES (@MaKho, @MaHHKho, @NgayNhap, @Soluonghh); ";


            DialogResult xacnhan = MessageBox.Show("Bạn muốn thêm dữ liệu trong bảng vào CSDL", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (xacnhan == DialogResult.Yes)
            {
                using (MySqlConnection conn = mysql.sql())
                {
                    conn.Open();
                    using (MySqlCommand cmdUpdate = new MySqlCommand(QuerryUpCSDL, conn))
                    {
                    }
                }

                MessageBox.Show($"Thêm thành công {count} món hàng hóa");

                dgvLoadQLKho.DataSource = null;
                dgvLoadQLKho.Rows.Clear();
            }
        }


    }
}
