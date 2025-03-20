using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace qlsieuthi.src.main.qlkho.libqlkho
{
    public class LoadData
    {
        // Gọi lib mysql
        private lib mysql = new lib();

        // Load data : mã loại hàng
        public DataTable GetMaLoaiHang()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = mysql.sql())
            {
                try
                {
                    conn.Open();
                    string queryMaLoaiHang = "SELECT * FROM LOAI_HANG";
                    MySqlCommand cmd = new MySqlCommand(queryMaLoaiHang,conn);
                    MySqlDataReader read = cmd.ExecuteReader();
                    dt.Load(read);
                    read.Close();
                }
                catch
                {
                    MessageBox.Show("Không có dữ liệu tồn tại trên database");
                }
            }
            return dt;
        }
        public void LoadCBBMaLoaiHang(ComboBox comboBox)
        {
            DataTable dt = GetMaLoaiHang();

            if (dt.Rows.Count > 0)
            {
                // Thêm cột mới chứa chuỗi "MaLoai - TenLoai"
                dt.Columns.Add("DisplayText", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    row["DisplayText"] = row["MaLoai"].ToString() + " - " + row["TenLoai"].ToString();
                }

                comboBox.DataSource = dt;
                comboBox.DisplayMember = "DisplayText"; // Hiển thị "MaLoai - TenLoai"
                comboBox.ValueMember = "MaLoai"; // Giá trị thực tế là MaLoai
            }
        }

        // Load data : Nhà cung cấp
        public DataTable GetNhaCC()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = mysql.sql())
            {
                try
                {
                    conn.Open();
                    string queryMaNhaCC = "SELECT * FROM NHA_CUNG_CAP";
                    MySqlCommand cmd = new MySqlCommand(queryMaNhaCC, conn);
                    MySqlDataReader read = cmd.ExecuteReader();
                    dt.Load(read);
                    read.Close();
                }
                catch
                {
                    MessageBox.Show("Không có dữ liệu tồn tại trên database");
                }
            }
            return dt;
        }
        public void LoadCBBMaNhaCC(ComboBox comboBox)
        {
            DataTable dt = GetNhaCC();

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("DisplayText", typeof(string));

                foreach (DataRow row in dt.Rows)
                {
                    row["DisplayText"] = row["MaNCC"].ToString() + " - " + row["TenNCC"].ToString();
                }

                comboBox.DataSource = dt;
                comboBox.DisplayMember = "DisplayText";
                comboBox.ValueMember = "MaNCC";
            }
        }

        // Load Data : Tên Hàng Hóa
        public DataTable GetTenHH()
        {
            DataTable dt = new DataTable();
            using (MySqlConnection conn = mysql.sql())
            {
                try
                {
                    conn.Open();
                    string queryTenHH = "SELECT DISTINCT TenHH FROM HANG_HOA;";
                    MySqlCommand cmd = new MySqlCommand(queryTenHH, conn);
                    MySqlDataReader read = cmd.ExecuteReader();
                    dt.Load(read);
                    read.Close();
                }
                catch
                {
                    MessageBox.Show("Không có dữ liệu tồn tại trên database");
                }
            }
            return dt;
        }
        public void LoadCBBTenHH(ComboBox comboBox)
        {
            DataTable dt = GetTenHH();

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("DisplayText", typeof(string));

                comboBox.DataSource = dt;
                comboBox.DisplayMember = "TenHH";
                comboBox.SelectedIndex = 0;
            }
        }

        // Load data : Mã hàng hóa
        public DataTable GetMaHH()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = mysql.sql())
            {
                try
                {
                    conn.Open();
                    string queryMaHH = "SELECT MaHH FROM HANG_HOA WHERE TinhTrang = 'khongcokho'";
                    MySqlCommand cmd = new MySqlCommand(queryMaHH, conn);
                    MySqlDataReader read = cmd.ExecuteReader();
                    dt.Load(read);
                    read.Close();
                }
                catch
                {
                    MessageBox.Show("Không có dữ liệu tồn tại trên database");
                }
            }

            return dt;
        }

        public void LoadCBBMaHH(ComboBox comboBox)
        {
            DataTable dt = GetMaHH();

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("DisplayText", typeof(string));

                comboBox.DataSource = dt;
                comboBox.DisplayMember = "MaHH";
                comboBox.SelectedIndex = 0;
            }
        }

        // Load Data : Mã kho
        public DataTable GetMaKho()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = mysql.sql())
            {
                try
                {
                    conn.Open();
                    string queryMaHH = "SELECT MaKho FROM QUAN_LY_KHO;";
                    MySqlCommand cmd = new MySqlCommand(queryMaHH, conn);
                    MySqlDataReader read = cmd.ExecuteReader();
                    dt.Load(read);
                    read.Close();
                }
                catch
                {
                    MessageBox.Show("Không có dữ liệu tồn tại trên database");
                }
            }

            return dt;
        }

        public void LoadCBBMaKho(ComboBox comboBox)
        {
            DataTable dt = GetMaKho();

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("DisplayText", typeof(string));

                comboBox.DataSource = dt;
                comboBox.DisplayMember = "MaKho";
                comboBox.SelectedIndex = 0;
            }
        }
    }
}
