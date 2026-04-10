using System;
using System.Windows.Forms;
using KutuphaneOtomasyonu.Forms;

namespace KutuphaneOtomasyonu
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AnaForm());
        }
    }
}
