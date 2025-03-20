using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using qlsieuthi.src.main.qlkho.libqlkho;

namespace qlsieuthi.src.main.qlkho
{
    public partial class BangQlkho : Form
    {
        // Load lib
        private LoadData dbLoad = new LoadData();
        lib mysql = new lib();
        public ComboBox GetLoadCbb()
        {
            return cbbTrangThai;
        }
        // Load dgv
        void RenameDGV()
        {
            dgvLoadHH.Columns[0].HeaderText = "STT";
            dgvLoadHH.Columns[1].HeaderText = "Mã hàng hóa";
            dgvLoadHH.Columns[2].HeaderText = "Tên hàng hóa";
            dgvLoadHH.Columns[3].HeaderText = "Mã loại hàng hóa";
            dgvLoadHH.Columns[4].HeaderText = "Số lượng";
            dgvLoadHH.Columns[5].HeaderText = "Đơn giá";
            dgvLoadHH.Columns[6].HeaderText = "Đơn vị tính";
            dgvLoadHH.Columns[7].HeaderText = "Mã nhà cung cấp";

            dgvLoadHH.Columns[0].Width = 50;
            dgvLoadHH.Columns[1].Width = 150;
            dgvLoadHH.Columns[2].Width = 100;
            dgvLoadHH.Columns[3].Width = 130;
            dgvLoadHH.Columns[4].Width = 80;
            dgvLoadHH.Columns[5].Width = 80;
            dgvLoadHH.Columns[6].Width = 85;
            dgvLoadHH.Columns[7].Width = 110;

            dgvLoadHH.Columns[8].Visible = false;

            dgvLoadHH.Columns[5].DefaultCellStyle.Format = "#,##0 VND";
            dgvLoadHH.Columns[4].DefaultCellStyle.Format = "#,##0";

        }
        public BangQlkho()
        {
            InitializeComponent();

            // Load cbb
            dbLoad.LoadCBBMaLoaiHang(cbbLoaiHang);
            dbLoad.LoadCBBMaNhaCC(cbbMaNhaCC);

            cbbQlKho cbbTrangThai = new cbbQlKho(this);
            cbbTrangThai.AddItemsCbbTrangThai();

            // Load các textbox
            LoadTongSoLuong();

            // Load dgv
            LoadDGVHH();
        }


        /*
         * BẢNG QLKHO
         * 
         * Tính năng :
         * - Xem tát cả các mặt hàng có trong kho
         * - Xem dưới dạng bộ lọc ( theo mã nhà cung cấp , Số lượng (còn,hết) , theo mã loại hàng )
         * - Xuất ra màn hình số lượng ở trong 
         * - Nếu dưới 100 báo gần hết hàng
         * VD : Số lượng hàng mà nhà cung cấp này còn là bao nhiêu
         */
        void LoadTongSoLuong()
        {
            string QueryLoadTongSL = "SELECT SUM(SoLuong) AS TongSoLuong FROM HANG_HOA";
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(QueryLoadTongSL, conn))
                {
                    object dem = cmd.ExecuteScalar(); // Lấy giá trị của dòng đầu tiên
                    if (dem != DBNull.Value)
                    {
                        txtSoLuong.Text = dem.ToString();
                    }
                    else
                    {
                        txtSoLuong.Text = "0";
                    }
                }
            }
        }
        void LoadDGVHH()
        {
            string QueryLoadBang = "SELECT * FROM HANG_HOA";
            using (MySqlConnection con = mysql.sql())
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand(QueryLoadBang, con))
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dgvLoadHH.DataSource = dt;

                        dgvLoadHH.Refresh();
                        RenameDGV();
                    }
                }
            }
        }


        // Lọc dựa trên sự lựa chọn vào combobox
        private void cbbNhaCC_DropDownClosed(object sender, EventArgs e)
        {
            LocTheoNhaCC();
        }
        private void cbbMaLoaiHang_DropDownClosed(object sender, EventArgs e)
        {
            LocTheoMaLoai();

        }
        private void cbbTrangThai_DropDownClosed(object sender, EventArgs e)
        {
            LocTheoTrangThai();
        }
        // Hàm biến lọc
        void LocTheoNhaCC()
        {
            // Tách biến chỉ lấy trước dấu -
            string LayMaloai = cbbMaNhaCC.SelectedValue.ToString();
            string MaNhaCC = cbbMaNhaCC.SelectedValue.ToString().Split('-')[0].Trim();
            // Gọi truy vấn
            string LocTheoNhaCC = "SELECT * FROM HANG_HOA WHERE MaNCC = @MaNCC";


            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd =  new MySqlCommand(LocTheoNhaCC, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNCC", MaNhaCC);

                    using (MySqlDataAdapter apdater = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        apdater.Fill(dt);

                        dgvLoadHH.DataSource = dt;

                        dgvLoadHH.Refresh();
                        RenameDGV();
                    }
                }
            }
        }

        void LocTheoMaLoai()
        {
            // Tách biến chỉ lấy trước dấu -
            string LayMaLH = cbbLoaiHang.SelectedValue.ToString();
            string MaLoaiHang = cbbLoaiHang.SelectedValue.ToString().Split('-')[0].Trim();
            // Gọi truy vấn
            string LocTheoMaHang = "SELECT * FROM HANG_HOA WHERE MaLoai = @MaLoai";


            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(LocTheoMaHang, conn))
                {
                    cmd.Parameters.AddWithValue("@MaLoai", MaLoaiHang);

                    using (MySqlDataAdapter apdater = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        apdater.Fill(dt);

                        dgvLoadHH.DataSource = dt;

                        dgvLoadHH.Refresh();
                        RenameDGV();
                    }
                }
            }
        }

        void LocTheoTrangThai()
        {
            string TrangThai = cbbTrangThai.Text;
            string LocTheoTrangThai = "";

            if (TrangThai == "Đang còn")
            {
                LocTheoTrangThai = "SELECT * FROM HANG_HOA WHERE SoLuong > 100";
            }
            if (TrangThai == "Sắp hết")
            {
                LocTheoTrangThai = "SELECT * FROM HANG_HOA WHERE SoLuong <= 100";
            }
            if (TrangThai == "Đã hết")
            {
                LocTheoTrangThai = "SELECT * FROM HANG_HOA WHERE SoLuong = 0";
            }

            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(LocTheoTrangThai, conn))
                {
                    using (MySqlDataAdapter apdater = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        apdater.Fill(dt);

                        dgvLoadHH.DataSource = dt;

                        dgvLoadHH.Refresh();
                        RenameDGV();
                    }
                }
            }
        }

        private void btnBangHH_Click(object sender, EventArgs e)
        {

        }
    }
}
