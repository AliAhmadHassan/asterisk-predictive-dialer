using System;
using System.Windows.Forms;
using Silver.RoboDiscagem.Exceptions;

namespace Silver.RoboDiscagem
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(GlobalException.GlobalExceptionListner);
            Application.Run(new Dashboard());
        }
    }
}
