using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using qlsieuthi.LOGIN;
using System.Windows.Forms;

namespace qlsieuthi.src.main.qlkho.libqlkho
{
    public class btn
    {
        public void Xacnhandong(Form currentForm)
        {
            DialogResult closeForm = MessageBox.Show("Xác nhận đóng phần mềm", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (closeForm == DialogResult.Yes)
            {
                currentForm.Hide();
                login flogin = new login();
                flogin.Show();
            }
        }
    }
}
