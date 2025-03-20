using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qlsieuthi.src.main.qlkho.libqlkho
{
    public class InputQLkho
    {
        public string MaHH { get; set; }
        public string TenHH { get; set; }
        public string DonGia { get; set; }
        public string SoLuong { get; set; }
        public string DonViTinh { get; set; }

        public InputQLkho(string mahh , string tenhh , string dongia , string soluong , string donvitinh)
        {
            MaHH = mahh;
            TenHH = tenhh;
            DonGia = dongia;
            SoLuong = soluong;
            DonViTinh = donvitinh;
        }

        // Đặt biến null cho các thuộc tính trên
        public InputQLkho()
        {
            MaHH = "";
            TenHH = "";
            DonGia = "";
            SoLuong = "";
            DonViTinh = "";
        }
    }

    public class TTKhoHang
    {
        // Bảng Thông Tin kho hàng
        public string MaKho { get; set; }
        public string MaHangHoa { get; set; }
        public string NgayNhap { get; set; }
        public string NgayXuat { get; set; }
        public string SoLuongHH { get; set; }

        public TTKhoHang(string makho, string mahanghoa, string ngaynhap, string ngayxuat, string soluonghh)
        {
            MaKho = makho;
            MaHangHoa = mahanghoa;
            NgayNhap = ngaynhap;
            NgayXuat = ngayxuat;
            SoLuongHH = soluonghh;
        }

        public TTKhoHang()
        {
            MaKho = "";
            MaHangHoa = "";
            NgayNhap = "";
            NgayXuat = "";
            SoLuongHH = "";
        }
    }
}
