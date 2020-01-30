using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace Edge_Stable_x86_Launcher
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
            if (File.Exists(@"Edge Stable x86\msedge.exe"))
            {
                if (!File.Exists(@"Edge Stable x86\Profile.txt"))
                {
                    if (culture1.Name == "de-DE")
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form1());
                        String Arguments = File.ReadAllText(@"Edge Stable x86\Profile.txt");
                        Process.Start(@"Edge Stable x86\msedge.exe", Arguments);
                    }
                    else
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Form2());
                        String Arguments = File.ReadAllText(@"Edge Stable x86\Profile.txt");
                        Process.Start(@"Edge Stable x86\msedge.exe", Arguments);
                    }
                }
                else
                {
                    String Arguments = File.ReadAllText(@"Edge Stable x86\Profile.txt");
                    Process.Start(@"Edge Stable x86\msedge.exe", Arguments);
                }
            }
            else if (culture1.Name == "de-DE")
            {
                string message = "Edge Stable x86 ist nicht installiert";
                MessageBox.Show(message, "Edge Stable x86 Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                string message = "Edge Stable x86 is not installed";
                MessageBox.Show(message, "Edge Stable x86 Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
