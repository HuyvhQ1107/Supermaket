using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qlsieuthi.src.main.admin.libqlKhoHang
{
    public class InputData
    {
        // Constractor các biến dữ liệu
        public string MaNhaCC {  get; set; }
        public string TenNhaCC {  get; set; }
        public string SDTNhaCC {  get; set; }
        public string Email {  get; set; }
        public string DiaChi {  get; set; }
        public string TinhTP {  get; set; }
        public string QuanHuyen {  get; set; }
        public string PhuongXa {  get; set; }
        public string DiaChiHT {  get; set; }

        // Constractor các biến dữ liệu cho mặt hàng
        public string MaLoaiHang { get; set; }
        public string TenLoaiHang { get; set; }

        public InputData(string maNhaCC , string tenNhaCC , string sdtNhaCC , string EmailNhaCC , string diachiNhaCC , String tinhtp , string quanhuyen , string phuongxa , string diachiht , string maloaihang , string tenloaihang)
        {
            MaNhaCC = maNhaCC;
            TenNhaCC = tenNhaCC;
            SDTNhaCC = sdtNhaCC;
            Email = EmailNhaCC;
            DiaChi = diachiNhaCC; // Biến tổng của địa chỉ

            // Các biến địa chỉ
            TinhTP = tinhtp;
            QuanHuyen = quanhuyen;
            PhuongXa = phuongxa;
            DiaChiHT = diachiht;

            // Loại hàng hóa
            MaLoaiHang = maloaihang;
            TenLoaiHang = tenloaihang;
        }
    }
}
