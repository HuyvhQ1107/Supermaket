using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace qlsieuthi.src.main.nhanvien.lib
{
    public class LoadDataNV
    {
        connectdb mysql = new connectdb();
        // Load Data : Mã kho
        public DataTable GetMaNhanVien()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = mysql.sql())
            {
                try
                {
                    conn.Open();
                    string queryMaHH = "SELECT MaNV FROM NHAN_VIEN;";
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

        public void LoadCBBMaNV(ComboBox comboBox)
        {
            DataTable dt = GetMaNhanVien();

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("DisplayText", typeof(string));

                comboBox.DataSource = dt;
                comboBox.DisplayMember = "MaNV";
                comboBox.SelectedIndex = -1;
            }
        }

        // Load cbb Mã khách hàng
        public DataTable GetMaKhachHang()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = mysql.sql())
            {
                try
                {
                    conn.Open();
                    string queryMaHH = "SELECT MaKH FROM KHACH_HANG;";
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

        public void LoadCBBMaKH(ComboBox comboBox)
        {
            DataTable dt = GetMaKhachHang();

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("DisplayText", typeof(string));

                comboBox.DataSource = dt;
                comboBox.DisplayMember = "MaKH";
                comboBox.SelectedIndex = -1;
            }
        }
    }
}
