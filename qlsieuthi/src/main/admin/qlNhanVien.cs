using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;


// Thêm lib
using qlsieuthi.src.main.admin.libNhanVien;

namespace qlsieuthi.src.main.admin
{
    public partial class qlNhanVien : Form
    {
        // Khai báo các nút để add vào lib
        public ComboBox GetLoadCbb()
        {
            return cbbChucVu;
        }
        public (ComboBox, ComboBox, ComboBox) GetLoadCbbDC()
        {
            return (cbbTinhTP, cbbQuanHuyen, cbbPhuongXa);
        }
        /*
         * Tab control 2
         */
        // Khai báo các nút để add vào lib
        public ComboBox GetLoadCbbLoc()
        {
            return cbbBoLoc;
        }


        // Gọi hàm kết nối csdl
        lib mysql = new lib();


        DataTable dtNhanVien = new DataTable();
        public qlNhanVien()
        {
            InitializeComponent();
            // Load cbb
            cbb cbbLoadChucvu = new cbb(this);
            cbbLoadChucvu.AddItemsCbbChucVu();

            api apiLoader = new api(this);
            apiLoader.LoadDataCity();
            // Load DataTable

            dtNhanVien.Columns.Add("Mã nhân viên", typeof(string));
            dtNhanVien.Columns.Add("Họ Tên", typeof(string));
            dtNhanVien.Columns.Add("Chức vụ", typeof(string));
            dtNhanVien.Columns.Add("Số điện thoại", typeof(string));
            dtNhanVien.Columns.Add("Ngày sinh", typeof(DateTime));
            dtNhanVien.Columns.Add("Địa chỉ", typeof(string));
            dtNhanVien.Columns.Add("Lương", typeof(decimal));

            dgvThongTin.DataSource = dtNhanVien;

            foreach (DataGridViewColumn column in dgvThongTin.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            // TAB 2
            LoadTextBox();
            cbb cbbBoLoc = new cbb(this);
            cbbBoLoc.BoLoc();

            // TAB 3
            lockTAB3();
        }
        // Chỉ nhập số vào các ô SDT và Lương
        private void txtLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }
        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Chặn ký tự không phải số
            }
        }

        /*
         * TÍNH NĂNG
         * 1. Thêm tạm thời vào bảng
         * 2. Nếu có sai thông tin có thể chỉnh sửa dc
         * 3. Lưu vào cơ sở dữ liệu
         * 4. Thoát
         */
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Khai báo dữ liệu và lấy dữ liệu nhập
            string MaNV = txtIDNhanVien.Text.Trim();
            string HoTen = txtHoTen.Text.Trim();
            string ChucVu = cbbChucVu.Text.Trim();
            string SDT = txtSDT.Text.Trim();
            DateTime NgaySinh = dtNgaySinh.Value;
            string TinhThanhPho = cbbTinhTP.Text.Trim();
            string QuanHuyen = cbbQuanHuyen.Text.Trim();
            string PhuongXa = cbbPhuongXa.Text.Trim();
            string SoNha = txtDiaChi.Text.Trim();
            string Luong = txtLuong.Text.Trim();

            // Gộp biến 4 biến trên thành
            String DiaChi = $"{SoNha} , {PhuongXa} , {QuanHuyen} , {TinhThanhPho}";

            // Kiểm tra xem người dùng đã nhập dữ liệu chưa
            if (string.IsNullOrWhiteSpace(MaNV))
            {
                MessageBox.Show("Lỗi : Mã nhân viên không thể để trống !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIDNhanVien.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(HoTen))
            {
                MessageBox.Show("Lỗi : Họ tên không thể để trống !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHoTen.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(ChucVu))
            {
                MessageBox.Show("Lỗi : Vui lòng chọn chức vụ !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(SDT))
            {
                MessageBox.Show("Lỗi : Số điện thoại không thể để trống !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(Luong))
            {
                MessageBox.Show("Lỗi : Lương không thể để trống !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLuong.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(TinhThanhPho))
            {
                MessageBox.Show("Lỗi : Vui lòng chọn Tỉnh / Thành phố !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(QuanHuyen))
            {
                MessageBox.Show("Lỗi : Vui lòng chọn Quận/Huyện", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(PhuongXa))
            {
                MessageBox.Show("Lỗi : Vui lòng chọn Phường/Xã !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(SoNha))
            {
                MessageBox.Show("Lỗi : Địa chỉ không thể để trống !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDiaChi.Focus();
                return;
            }

            foreach (DataGridViewColumn column in dgvThongTin.Columns)
            {
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            // Load vào bảng
            dtNhanVien.Rows.Add(MaNV, HoTen, ChucVu, SDT, NgaySinh, DiaChi, Luong);

            // Định dạng cột "Ngày sinh" để chỉ hiển thị ngày, không có giờ
            dgvThongTin.Columns["Ngày sinh"].DefaultCellStyle.Format = "dd/MM/yyyy";

            // Xóa nhập ở các ô dữ liệu
            ReloadInput();
        }
        private void btnADD_Click(object sender, EventArgs e)
        {
            // Câu truy vấn
            string query = "INSERT INTO NHAN_VIEN (MaNV, HoTen, ChucVu, SDT, NgaySinh, DiaChi, Luong) " +
                   "VALUES (@MaNV, @HoTen, @ChucVu, @SDT, @NgaySinh, @DiaChi, @Luong)";
            using (MySqlConnection conn = mysql.sql())
            {
                // Mở truy vấn
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    // Quét từng dòng trong bảng
                    foreach (DataGridViewRow row in dgvThongTin.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            try
                            {
                                cmd.Parameters.Clear();

                                cmd.Parameters.AddWithValue("@MANV", row.Cells[0].Value);
                                cmd.Parameters.AddWithValue("@HoTen", row.Cells[1].Value);
                                cmd.Parameters.AddWithValue("@ChucVu", row.Cells[2].Value);
                                cmd.Parameters.AddWithValue("@SDT", row.Cells[3].Value);
                                cmd.Parameters.AddWithValue("@NgaySinh", Convert.ToDateTime(row.Cells[4].Value));
                                cmd.Parameters.AddWithValue("@DiaChi", row.Cells[5].Value);
                                cmd.Parameters.AddWithValue("@Luong", row.Cells[6].Value);

                                int themthanhcong = cmd.ExecuteNonQuery();

                                if(themthanhcong > 0)
                                {
                                    MessageBox.Show("Cập nhật dữ liệu thành công", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    ReloadInput();
                                    RefreshDgv();
                                }
                            }
                            catch
                            {
                                MessageBox.Show($"Mã nhân viên {row.Cells[0].Value} đã tồn tại",
                                    "Lỗi trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            btn btnclose = new btn();
            btnclose.Xacnhandong(this);
        }


        // Làm mới ô nhập và Bảng thông tin
        void ReloadInput()
        {
            txtIDNhanVien.Text = "";
            txtHoTen.Text = "";
            cbbChucVu.Items.Clear();
            txtSDT.Text = "";
            txtDiaChi.Text = "";
            txtLuong.Text = "";
            cbbTinhTP.DataSource = null;
            cbbQuanHuyen.DataSource = null;
            cbbPhuongXa.DataSource = null;
        }
        void RefreshDgv()
        {
            dgvThongTin.DataSource = null;
            dgvThongTin.Rows.Clear();
            dgvThongTin.Refresh();
        }



        /*
         * TAB 2 : Hiển thị danh sách nhân viên
         * 
         * Tính năng :
         * - Hiển thị danh sách nhân viên ở các textbox gồm Admin , Kế Toán , Quản lí kho , Nhân Viên
         * - Hiển thị bộ lọc theo chức vụ ở trên
         */
        // Ẩn textbox không cho nhập
        void AnTextBoxTab2()
        {
            txtAdmin.Enabled = false;
            txtKeToan.Enabled = false;
            txtQLKho.Enabled = false;
            txtNhanVien.Enabled = false;
        }
        void LoadDgvTT()
        {
            dgvNhanVien.Columns[0].HeaderText = "STT";
            dgvNhanVien.Columns[1].HeaderText = "Mã NV";
            dgvNhanVien.Columns[2].HeaderText = "Họ tên";
            dgvNhanVien.Columns[3].HeaderText = "Chức vụ";
            dgvNhanVien.Columns[4].HeaderText = "Số điện thoại";
            dgvNhanVien.Columns[5].HeaderText = "Ngày sinh";
            dgvNhanVien.Columns[6].HeaderText = "Địa chỉ";
            dgvNhanVien.Columns[7].HeaderText = "Lương";
        }
        // Load các textbox
        void LoadTextBox()
        {
            AnTextBoxTab2();
            // Truy vấn vào csdl để hiển thị số lượng
            // Textbox Admin
            string chucvuAdmin = "Admin";
            string querrysoluongAdmin = "SELECT COUNT(*) FROM NHAN_VIEN WHERE ChucVu = @ChucVu";
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(querrysoluongAdmin, conn))
                {
                    cmd.Parameters.AddWithValue("@ChucVu", chucvuAdmin);
                    int soLuongAD = Convert.ToInt32(cmd.ExecuteScalar());

                    // gắn vào textbox
                    txtAdmin.Text = soLuongAD.ToString();
                }
            }
            // TextBox Kế toán
            string chucvuKeToan = "Kế toán";
            string querrysoluongKeToan = "SELECT COUNT(*) FROM NHAN_VIEN WHERE ChucVu = @ChucVu";
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(querrysoluongKeToan, conn))
                {
                    cmd.Parameters.AddWithValue("@ChucVu", chucvuKeToan);
                    int soLuongAD = Convert.ToInt32(cmd.ExecuteScalar());

                    // gắn vào textbox
                    txtKeToan.Text = soLuongAD.ToString();
                }
            }
            // TextBox Kế toán
            string chucvuQLKho = "Quản lí kho";
            string querrysoluongQLKho = "SELECT COUNT(*) FROM NHAN_VIEN WHERE ChucVu = @ChucVu";
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(querrysoluongQLKho, conn))
                {
                    cmd.Parameters.AddWithValue("@ChucVu", chucvuQLKho);
                    int soLuongAD = Convert.ToInt32(cmd.ExecuteScalar());

                    // gắn vào textbox
                    txtQLKho.Text = soLuongAD.ToString();
                }
            }
            // TextBox Kế toán
            string chucvuNhanVien = "Nhân viên";
            string querrysoluongNhanVien = "SELECT COUNT(*) FROM NHAN_VIEN WHERE ChucVu = @ChucVu";
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(querrysoluongNhanVien, conn))
                {
                    cmd.Parameters.AddWithValue("@ChucVu", chucvuNhanVien);
                    int soLuongAD = Convert.ToInt32(cmd.ExecuteScalar());

                    // gắn vào textbox
                    txtNhanVien.Text = soLuongAD.ToString();
                }
            }
        }

        private void btnLoc_Click(object sender, EventArgs e)
        {
            string BoLoc = cbbBoLoc.Text.Trim(); // Lấy giá trị từ combobox lọc

            string query = "SELECT * FROM NHAN_VIEN WHERE ChucVu = @ChucVu"; // Lấy tất cả thông tin nhân viên
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ChucVu", BoLoc);

                    btn loc = new btn();
                    if(loc.Xacnhansearch() == true)
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt); // Đổ dữ liệu vào DataTable

                            dgvNhanVien.DataSource = dt; // Load vào DataGridView
                            LoadDgvTT();

                            foreach (DataGridViewColumn column in dgvNhanVien.Columns)
                            {
                                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                            }
                            
                            MessageBox.Show("Tìm kiếm thành công","Thông báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }

        /*
         * TAB3
         * 
         * Tính năng :
         * 1. Tìm kiếm thông tin qua số điện thoại
         * 2. Chỉnh sửa thông tin khách hàng
         * 
         */
        void lockTAB3()
        {
            txtEditMNV.Enabled = false;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string SearchMNV = txtSearchMNV.Text.Trim();

            string querrySearchMNV = "SELECT * FROM NHAN_VIEN WHERE MaNV = @MaNV";
            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(querrySearchMNV, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", SearchMNV);

                    using (MySqlDataAdapter dataAdapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        dataAdapter.Fill(dt);

                        dgvEditTT.DataSource = dt;
                        LoadDgvEdit();

                        foreach (DataGridViewColumn column in dgvEditTT.Columns)
                        {
                            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                        }
                    }
                }
            }
        }
        private void btnEDIT_Click(object sender, EventArgs e)
        {
            if (dgvEditTT.SelectedRows.Count > 0)
            {
                try
                {
                    // Lấy các id xuống
                    string IDNV = dgvEditTT.SelectedRows[0].Cells[1].Value.ToString();
                    string HoTen = dgvEditTT.SelectedRows[0].Cells[2].Value.ToString();
                    string ChucVu = dgvEditTT.SelectedRows[0].Cells[3].Value.ToString();
                    string SDT = dgvEditTT.SelectedRows[0].Cells[4].Value.ToString();

                    DateTime NgaySinh = Convert.ToDateTime(dgvEditTT.SelectedRows[0].Cells[5].Value);

                    string DiaChi = dgvEditTT.SelectedRows[0].Cells[6].Value.ToString();
                    int Luong = Convert.ToInt32(dgvEditTT.Rows[0].Cells[7].Value);

                    // Hiển thị vào khu chỉnh sửa
                    txtEditMNV.Text = IDNV.ToString();
                    txtEditHoTen.Text = HoTen.ToString();

                    // Gọi lại từ file data.cs
                    List<string> listchucvu = data.GetDanhSachChucVu();
                    cbbeditChucVu.SelectedItem = ChucVu;
                    if (!cbbeditChucVu.Items.Contains(ChucVu))
                    {
                        cbbeditChucVu.Items.Add(ChucVu);
                        cbbeditChucVu.SelectedItem = ChucVu;
                    }
                    // Được lựa chọn lại list cbb
                    cbbeditChucVu.Items.Clear();
                    cbbeditChucVu.Items.AddRange(listchucvu.ToArray());
                    cbbeditChucVu.SelectedItem = ChucVu;

                    txtEditSDT.Text = SDT.ToString();
                    dtEditNgaySinh.Value = NgaySinh;

                    // Tách địa chỉ từ ô địa chỉ ra thành 4 thành phần 
                    string[] tachdiachi = DiaChi.Split(new string[] { ", " }, StringSplitOptions.None);

                    if (tachdiachi.Length >= 4)
                    {
                        txtEditDiaChi.Text = tachdiachi[0];
                        // gán vào cbb
                        if (!cbbEditPhuongXa.Items.Contains(tachdiachi[1]))
                        {
                            cbbEditPhuongXa.Items.Add($"{tachdiachi[1]}");
                            cbbEditPhuongXa.SelectedItem = tachdiachi[1];
                        }
                        if (!cbbEditQuanHuyen.Items.Contains(tachdiachi[2]))
                        {
                            cbbEditQuanHuyen.Items.Add($"{tachdiachi[2]}");
                            cbbEditQuanHuyen.SelectedItem = tachdiachi[2];
                        }
                        if (!cbbeditTinhThanhPho.Items.Contains(tachdiachi[3]))
                        {
                            cbbeditTinhThanhPho.Items.Add($"{tachdiachi[3]}");
                            cbbeditTinhThanhPho.SelectedItem = tachdiachi[3];
                        }
                    }

                    txtEditLuong.Text = Luong.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi hiển thị dữ liệu: " + ex.Message);
                }
            }
        }
        private void btnEditCapNhat_Click(object sender, EventArgs e)
        {
            string MaNV = txtEditMNV.Text.Trim();
            string HoTen = txtEditHoTen.Text.Trim();
            string ChucVu = cbbeditChucVu.Text.Trim();
            string SDT = txtEditSDT.Text.Trim();
            DateTime NgaySinh = dtEditNgaySinh.Value;

            string TinhThanhPho = cbbeditTinhThanhPho.Text.Trim();
            string QuanHuyen = cbbEditQuanHuyen.Text.Trim();
            string PhuongXa = cbbEditPhuongXa.Text.Trim();
            string DC = txtEditDiaChi.Text.Trim();

            string Diachi = $"{DC} , {PhuongXa} , {QuanHuyen} , {TinhThanhPho}";

            string Luong = txtEditLuong.Text.Trim();

            if (string.IsNullOrWhiteSpace(MaNV))
            {
                MessageBox.Show("Lỗi : Mã nhân viên không thể để trống !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtIDNhanVien.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(HoTen))
            {
                MessageBox.Show("Lỗi : Họ tên không thể để trống !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHoTen.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(ChucVu))
            {
                MessageBox.Show("Lỗi : Vui lòng chọn chức vụ !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(SDT))
            {
                MessageBox.Show("Lỗi : Số điện thoại không thể để trống !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSDT.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(Luong))
            {
                MessageBox.Show("Lỗi : Lương không thể để trống !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLuong.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(TinhThanhPho))
            {
                MessageBox.Show("Lỗi : Vui lòng chọn Tỉnh / Thành phố !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(QuanHuyen))
            {
                MessageBox.Show("Lỗi : Vui lòng chọn Quận/Huyện", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(PhuongXa))
            {
                MessageBox.Show("Lỗi : Vui lòng chọn Phường/Xã !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(DC))
            {
                MessageBox.Show("Lỗi : Địa chỉ không thể để trống !!!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDiaChi.Focus();
                return;
            }

            using (MySqlConnection conn = mysql.sql())
            {
                conn.Open();
                string query = "UPDATE NHAN_VIEN SET " +
                    "HoTen = @HoTen, " +
                    "ChucVu = @ChucVu, " +
                    "SDT = @SDT, " +
                    "NgaySinh = @NgaySinh, " +
                    "DiaChi = @DiaChi, " +
                    "Luong = @Luong " +
                    "WHERE MaNV = @MaNV";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MaNV", MaNV);
                    cmd.Parameters.AddWithValue("@HoTen", HoTen);
                    cmd.Parameters.AddWithValue("@ChucVu", ChucVu);
                    cmd.Parameters.AddWithValue("@SDT", SDT);
                    cmd.Parameters.AddWithValue("@NgaySinh", NgaySinh);
                    cmd.Parameters.AddWithValue("@DiaChi", Diachi);
                    cmd.Parameters.AddWithValue("@Luong", Luong);

                    int update = cmd.ExecuteNonQuery();

                    // Gọi hàm xác nhận
                    btn xacnhanthem = new btn();
                    if (xacnhanthem.Xacnhanthem() == true)
                    {
                        if (update > 0)
                        {
                            MessageBox.Show("Cập nhật thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Xóa hết thông tin và để trống
                            ReloadEditInput();
                            Reloaddgv();
                        }
                    }
                }
            }
        }
        private void btnEditThoat_Click(object sender, EventArgs e)
        {
            btn closeeditform = new btn();
            closeeditform.Xacnhandong(this);
        }
        // Hàm thư viện
        void ReloadEditInput()
        {
            txtEditMNV.Text = "";
            txtEditHoTen.Text = "";
            cbbeditChucVu.Items.Clear();
            txtEditSDT.Text = "";
            txtEditDiaChi.Text = "";
            txtEditLuong.Text = "";
            cbbeditTinhThanhPho.DataSource = null;
            cbbEditQuanHuyen.DataSource = null;
            cbbEditPhuongXa.DataSource = null;

            txtSearchMNV.Text = "";
        }
        void Reloaddgv()
        {
            dgvEditTT.DataSource = null;
            dgvEditTT.Rows.Clear();
            dgvEditTT.Refresh();
        }
        void LoadDgvEdit()
        {
            dgvEditTT.Columns[0].HeaderText = "STT";
            dgvEditTT.Columns[1].HeaderText = "Mã NV";
            dgvEditTT.Columns[2].HeaderText = "Họ tên";
            dgvEditTT.Columns[3].HeaderText = "Chức vụ";
            dgvEditTT.Columns[4].HeaderText = "Số điện thoại";
            dgvEditTT.Columns[5].HeaderText = "Ngày sinh";
            dgvEditTT.Columns[6].HeaderText = "Địa chỉ";
            dgvEditTT.Columns[7].HeaderText = "Lương";
        }


        /*
         * 
         * CÁC NÚT CÓ TẠI PAGE QL NHÂN VIÊN
         * 
         */
        private void btnMain_Click(object sender, EventArgs e)
        {
            btn ADMainForm = new btn();
            ADMainForm.ADMain(this);
        }
        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            btn qlNhanVienForm = new btn();
            qlNhanVienForm.QLNhanVien(this);
        }

        private void btnQLTaiKhoan_Click(object sender, EventArgs e)
        {
            btn qlTaiKhoanForm = new btn();
            qlTaiKhoanForm.QLTaiKhoan(this);
        }

        private void btnQLKhoHang_Click(object sender, EventArgs e)
        {
            btn qlKhoHangForm = new btn();
            qlKhoHangForm.QLKhoHang(this);
        }
    }
}
