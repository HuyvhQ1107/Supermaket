using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using qlsieuthi.LOGIN;
using qlsieuthi.src.main.admin;
using qlsieuthi.src.main.PhieuHang;
using qlsieuthi.src.main.qlkho;
using qlsieuthi.src.main.qlkho.ThongTinKhoHang;

namespace qlsieuthi
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PhieuHang());
        }
    }
}
