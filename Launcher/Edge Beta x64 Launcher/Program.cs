using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace Edge_Beta_x64_Launcher
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
            if (File.Exists(@"Edge Beta x64\msedge.exe"))
            {
                if (!File.Exists(@"Edge Beta x64\Profile.txt"))
                {
                    if (culture1.Name == "de-DE")
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
                        String Arguments = File.ReadAllText(@"Edge Beta x64\Profile.txt");
                        Process.Start(@"Edge Beta x64\msedge.exe", Arguments);
                    }
                    else
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form2());
                        String Arguments = File.ReadAllText(@"Edge Beta x64\Profile.txt");
                        Process.Start(@"Edge Beta x64\msedge.exe", Arguments);
                    }
                }
                else
                {
                    String Arguments = File.ReadAllText(@"Edge Beta x64\Profile.txt");
                    Process.Start(@"Edge Beta x64\msedge.exe", Arguments);
                }
            }
            else if (culture1.Name == "de-DE")
            {
                string message = "Edge Beta x64 ist nicht installiert";
                MessageBox.Show(message, "Edge Beta x64 Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string message = "Edge Beta x64 is not installed";
                MessageBox.Show(message, "Edge Beta x64 Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
