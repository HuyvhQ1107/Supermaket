using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace qlsieuthi.src.main.admin.libNhanVien
{
    public class Province
    {
        public string Name { get; set; }
        public List<District> Districts { get; set; } = new List<District>();
    }

    public class District
    {
        public string Name { get; set; }
        public List<Ward> Wards { get; set; } = new List<Ward>();
    }

    public class Ward
    {
        public string Name { get; set; }
    }

    public class api
    {
        private qlNhanVien _qlNhanVien;
        private List<Province> provinceList = new List<Province>();

        // Gọi hàm add vào form qlNhanVien
        public api(qlNhanVien qlNhanViens)
        {
            _qlNhanVien = qlNhanViens;
        }
        private qlKhoHang _qlKhoHang;
        // Gọi hàm add vào form qlKhoHang
        public api(qlKhoHang qlKhoHangs)
        {
            _qlKhoHang = qlKhoHangs;
        }
        

        // Load dữ liệu từ API vào ComboBox
        public async Task LoadDataCity()
        {
            string apiURL = "https://provinces.open-api.vn/api/?depth=3";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiURL);
                    response.EnsureSuccessStatusCode();
                    string jsonData = await response.Content.ReadAsStringAsync();

                    provinceList = JsonConvert.DeserializeObject<List<Province>>(jsonData);
                    provinceList.Insert(0, new Province { Name = "Chọn tỉnh/thành" });

                    ComboBox cbbTinhTP, cbbQuanHuyen, cbbPhuongXa ;

                    // Gọi vào các form
                    if (_qlNhanVien != null)
                    {
                        (cbbTinhTP, cbbQuanHuyen, cbbPhuongXa) = _qlNhanVien.GetLoadCbbDC();
                    }
                    else if (_qlKhoHang != null)
                    {
                        (cbbTinhTP, cbbQuanHuyen, cbbPhuongXa) = _qlKhoHang.GetLoadCbbDC();
                    }
                    else
                    {
                        return;
                    }


                    cbbTinhTP.DataSource = provinceList;
                    cbbTinhTP.DisplayMember = "Name";
                    cbbTinhTP.SelectedIndex = 0;

                    cbbTinhTP.SelectedIndexChanged += (s, e) =>
                    {
                        if (cbbTinhTP.SelectedIndex > 0)
                        {
                            List<District> districts = provinceList[cbbTinhTP.SelectedIndex].Districts;
                            LoadDistricts(cbbQuanHuyen, districts);
                        }
                        else
                        {
                            LoadDistricts(cbbQuanHuyen, new List<District>());
                        }
                    };

                    cbbQuanHuyen.SelectedIndexChanged += (s, e) =>
                    {
                        if (cbbQuanHuyen.SelectedIndex > 0)
                        {
                            District selectedDistrict = (District)cbbQuanHuyen.SelectedItem;
                            LoadWards(cbbPhuongXa, selectedDistrict.Wards);
                        }
                        else
                        {
                            LoadWards(cbbPhuongXa, new List<Ward>());
                        }
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi : " + ex.Message);
                }
            }
        }

        private void LoadDistricts(ComboBox cbb, List<District> districts)
        {
            districts.Insert(0, new District { Name = "Chọn quận/huyện" });
            cbb.DataSource = null;
            cbb.DataSource = districts;
            cbb.DisplayMember = "Name";
            cbb.SelectedIndex = 0;
        }

        private void LoadWards(ComboBox cbb, List<Ward> wards)
        {
            wards.Insert(0, new Ward { Name = "Chọn phường/xã" });
            cbb.DataSource = null;
            cbb.DataSource = wards;
            cbb.DisplayMember = "Name";
            cbb.SelectedIndex = 0;
        }

        public Province GetProvinceByName(string provinceName)
        {
            return provinceList.FirstOrDefault(p => p.Name == provinceName);
        }
    }
}