using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Application = System.Windows.Application;

namespace WpfApp1
{
    public class wpfWindow
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application app = new Application();
            app.Run(MainWindow.SetupWindow());
        }

    }
}
