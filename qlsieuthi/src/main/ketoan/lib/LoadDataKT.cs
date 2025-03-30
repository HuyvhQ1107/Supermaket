using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using qlsieuthi.src.main.admin;

namespace qlsieuthi.src.main.ketoan.lib
{
    public class LoadDataKT
    {
        connectdb mysql = new connectdb();
        // Load Data : Mã kho
        public DataTable GetMaHD()
        {
            DataTable dt = new DataTable();

            using (MySqlConnection conn = mysql.sql())
            {
                try
                {
                    conn.Open();
                    string queryMaHH = "SELECT MaHD FROM HOA_DON;";
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

        public void LoadCBBMaHD(ComboBox comboBox)
        {
            DataTable dt = GetMaHD();

            if (dt.Rows.Count > 0)
            {
                dt.Columns.Add("DisplayText", typeof(string));

                comboBox.DataSource = dt;
                comboBox.DisplayMember = "MaHD";
                comboBox.SelectedIndex = -1;
            }
        }

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
                comboBox.SelectedIndex = -1;
            }
        }
    }
}
