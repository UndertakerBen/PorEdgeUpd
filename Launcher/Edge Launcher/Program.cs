using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace Edge_Launcher
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CultureInfo culture1 = CultureInfo.CurrentUICulture;
            if (File.Exists(@"Edge\msedge.exe"))
            {
                if (!File.Exists(@"Edge\Profile.txt"))
                {
                    if (culture1.Name == "de-DE")
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
                        String Arguments = File.ReadAllText(@"Edge\Profile.txt");
                        _ = Process.Start(@"Edge\msedge.exe", Arguments);
                    }
                    else
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form2());
                        String Arguments = File.ReadAllText(@"Edge\Profile.txt");
                        _ = Process.Start(@"Edge\msedge.exe", Arguments);
                    }
                    }
                else
                {
                    String Arguments = File.ReadAllText(@"Edge\Profile.txt");
                    _ = Process.Start(@"Edge\msedge.exe", Arguments);
                }
            }
            else if (culture1.Name == "de-DE")
            {
                string message = "Edge ist nicht installiert";
                _ = MessageBox.Show(message, "Edge Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string message = "Edge is not installed";
                _ = MessageBox.Show(message, "Edge Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
