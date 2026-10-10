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
            PortableEdge.UiLanguage.UseEnglish();
            System.IO.Directory.SetCurrentDirectory(Application.StartupPath);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Edge_Updater.Form1());
        }
    }
}
