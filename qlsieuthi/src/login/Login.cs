using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using qlsieuthi.src.main.admin;
using qlsieuthi.src.main.ketoan;
using qlsieuthi.src.main.nhanvien;
using qlsieuthi.src.main.qlkho;

namespace qlsieuthi.LOGIN
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
            // Ẩn password khi nhập
            txtPass.PasswordChar = '*';
        }

        // Gọi lại class lib để kết nối MySQL
        connectdb mysql = new connectdb();

        private void btnLog_Click(object sender, EventArgs e)
        {
            // Lấy dữ liệu từ ô nhập
            String username = txtUser.Text.Trim();
            String pass = txtPass.Text.Trim();

            // Kiểm tra input không được để trống
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Bạn cần điền đầy đủ thông tin");
                return;
            }

            // Câu truy vấn kiểm tra đăng nhập
            string query = "SELECT Role FROM TAI_KHOAN WHERE MaNV = @IDNV AND Password = @Pass";

            try
            {
                // Kết nối MySQL
                using (MySqlConnection conn = mysql.sql()) // Sử dụng phương thức sql() từ class lib
                {
                    conn.Open();

                    // Sử dụng MySqlCommand (thay vì SqlCommand)
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDNV", username);
                        cmd.Parameters.AddWithValue("@Pass", pass);

                        // Thực hiện truy vấn
                        object xacminh = cmd.ExecuteScalar();
                        if (xacminh != null)
                        {
                            string ChucVu = xacminh.ToString();
                            MessageBox.Show($"Đăng nhập thành công, bạn đã đăng nhập dưới quyền {ChucVu}");

                            // kiểm tra quyền và đăng nhập sang form khác
                            if (ChucVu.Equals("admin", StringComparison.OrdinalIgnoreCase))
                            {
                                // Chuyên form admin
                                qlNhanVien fqlNhanVien = new qlNhanVien();
                                fqlNhanVien.Show();
                                this.Hide();
                            }
                            if (ChucVu.Equals("Kế toán", StringComparison.OrdinalIgnoreCase))
                            {
                                // Chuyên form kế toán
                                this.Hide();
                                thuchi ftc = new thuchi();
                                ftc.Show();
                            }
                            if (ChucVu.Equals("Quản lí kho", StringComparison.OrdinalIgnoreCase))
                            {
                                // Chuyên form ql kho
                                this.Hide();
                                BangQlkho fbqlkho = new BangQlkho();
                                fbqlkho.Show();
                            }
                            if (ChucVu.Equals("Nhân viên", StringComparison.OrdinalIgnoreCase))
                            {
                                // Chuyên form nhân viên
                                this.Hide();
                                HoaDon fdh = new HoaDon();
                                fdh.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message);
            }
        }

        private void htpass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.PasswordChar = htpass.Checked ? '\0' : '*'; // Hiển thị hoặc ẩn mật khẩu
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
