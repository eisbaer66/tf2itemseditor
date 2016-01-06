using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TF2Items
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ISteamService steam = new SteamService();

            Application.Run(new MainWindow(steam));
        }
    }
}
