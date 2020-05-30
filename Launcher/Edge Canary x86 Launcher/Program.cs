using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace Edge_Canary_x86_Launcher
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
            string applicationPath = Application.StartupPath;
            if (File.Exists(applicationPath + "\\Edge Canary x86\\msedge.exe"))
            {
                var sb = new System.Text.StringBuilder();
                string[] CommandLineArgs = Environment.GetCommandLineArgs();
                for (int i = 1; i < CommandLineArgs.Length; i++)
                {
                    if (CommandLineArgs[i].Contains("="))
                    {
                        if (CommandLineArgs[i].Contains("LinkID"))
                        {
                            sb.Append(" " + CommandLineArgs[i]);
                        }
                        else if (CommandLineArgs[i].Contains("http"))
                        {
                            sb.Append(" \"" + CommandLineArgs[i] + "\"");
                        }
                        else
                        {
                            string[] test = CommandLineArgs[i].Split(new char[] { '=' }, 2);
                            sb.Append(" " + test[0] + "=\"" + test[1] + "\"");
                        }
                    }
                    else if (CommandLineArgs[i].Contains(".pdf"))
                    {
                        sb.Append(" \"" + CommandLineArgs[i] + "\"");
                    }
                    else
                    {
                        sb.Append(" " + CommandLineArgs[i]);
                    }
                }
                if (!File.Exists(applicationPath + "\\Edge Canary x86\\Profile.txt"))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                    String Arguments = File.ReadAllText(applicationPath + "\\Edge Canary x86\\Profile.txt") + sb.ToString();
                    if (Arguments.Contains("--user-data-dir="))
                    {
                        string[] Arguments2 = Arguments.Split(new char[] { '=' }, 2);
                        string Arguments3 = Arguments2[0] + "='\"" + applicationPath + "\\" + Arguments2[1].Remove(0, 1) + "'";
                        _ = Process.Start(applicationPath + "\\Edge Canary x86\\msedge.exe", Arguments3.Replace("'", ""));
                    }
                    else
                    {
                        _ = Process.Start(applicationPath + "\\Edge Canary x86\\msedge.exe", Arguments);
                    }
                }
                else
                {
                    String Arguments = File.ReadAllText(applicationPath + "\\Edge Canary x86\\Profile.txt") + sb.ToString();
                    if (Arguments.Contains("--user-data-dir="))
                    {
                        string[] Arguments2 = Arguments.Split(new char[] { '=' }, 2);
                        string Arguments3 = Arguments2[0] + "='\"" + applicationPath + "\\" + Arguments2[1].Remove(0, 1) + "'";
                        _ = Process.Start(applicationPath + "\\Edge Canary x86\\msedge.exe", Arguments3.Replace("'", ""));
                    }
                    else
                    {
                        _ = Process.Start(applicationPath + "\\Edge Canary x86\\msedge.exe", Arguments);
                    }
                }
            }
            else if (culture1.TwoLetterISOLanguageName == "de")
            {
                _ = MessageBox.Show("Edge (Chromium) ist nicht installiert", "Edge Canary x86 Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (culture1.TwoLetterISOLanguageName == "ru")
            {
                _ = MessageBox.Show("Microsoft Edge (Chromium) Portable не найден", "Edge Canary x86 Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                _ = MessageBox.Show("Edge (Chromium) is not installed", "Edge Canary x86 Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
