using System;
using System.Windows.Forms;

namespace Edge_Updater
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Console.WriteLine(GlobalVar.GlobalString);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Edge_Updater.Form1());
        }
    }
}
