using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using qlsieuthi.src.main.admin;

namespace qlsieuthi.src.main.qlkho.libqlkho
{
    public class cbbQlKho
    {

        // Load Trạng Thái của kho ( còn hoặc hết )
        private BangQlkho _BangQlkho;

        public cbbQlKho(BangQlkho bangQlkho)
        {
            _BangQlkho = bangQlkho;
        }
        public void AddItemsCbbTrangThai()
        {
            ComboBox cbb = _BangQlkho.GetLoadCbb();
            if (cbb != null)
            {
                cbb.Items.Clear();
                cbb.Items.Add("Đang còn");
                cbb.Items.Add("Sắp hết");
                cbb.Items.Add("Đã hết");
            }
        }
    }
}
