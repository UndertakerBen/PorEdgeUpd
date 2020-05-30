using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Edge_Updater
{
    public partial class Regfile
    {
        public static void RegCreate(string applicationPath, string instDir, int icon)
        {
            Microsoft.Win32.RegistryKey key;
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\MSEdgeHTM.PORTABLE");
            key.SetValue(default, "Microsoft Edge HTML Document");
            key.SetValue("AppUserModelId", "MSEdge.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\MSEdgeHTM.PORTABLE\\Application");
            key.SetValue("AppUserModelId", "MSEdge.PORTABLE");
            key.SetValue("ApplicationIcon", applicationPath + @"\" + instDir + @"\msedge.exe," + icon);
            key.SetValue("ApplicationName", "Microsoft " + instDir + @" Portable");
            key.SetValue("ApplicationDescription", "Im Web surfen");
            key.SetValue("ApplicationCompany", "Microsoft Corporation");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\MSEdgeHTM.PORTABLE\\DefaultIcon");
            key.SetValue(default, applicationPath + @"\" + instDir + @"\msedge.exe," + icon);
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\MSEdgeHTM.PORTABLE\\shell\\open\\command");
            key.SetValue(default, "\"" + applicationPath + @"\" + instDir + @" Launcher.exe"" \""%1\""");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\MSEdgePDF.PORTABLE");
            key.SetValue(default, "Microsoft Edge PDF Document");
            key.SetValue("AppUserModelId", "MSEdge.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\MSEdgePDF.PORTABLE\\Application");
            key.SetValue("AppUserModelId", "MSEdge.PORTABLE");
            key.SetValue("ApplicationIcon", applicationPath + @"\" + instDir + @"\msedge.exe,13");
            key.SetValue("ApplicationName", "Microsoft " + instDir + @" Portable");
            key.SetValue("ApplicationDescription", "Im Web surfen");
            key.SetValue("ApplicationCompany", "Microsoft Corporation");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\MSEdgePDF.PORTABLE\\DefaultIcon");
            key.SetValue(default, applicationPath + @"\" + instDir + @"\msedge.exe,13");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\MSEdgePDF.PORTABLE\\shell\\open\\command");
            key.SetValue(default, "\"" + applicationPath + @"\" + instDir + @" Launcher.exe"" \""%1\""");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\RegisteredApplications");
            key.SetValue("Microsoft Edge.PORTABLE", @"Software\Clients\StartMenuInternet\Microsoft Edge.PORTABLE\Capabilities");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE");
            key.SetValue(default, "Microsoft " + instDir + @" Portable");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\Capabilities");
            key.SetValue("ApplicationDescription", "Microsoft Edge ist ein Webbrowser, der Websites und Anwendungen blitzschnell ausführt.Er ist schnell, stabil und einfach zu verwenden.Browsen Sie sicherer im Web dank des integrierten Schadsoftware - und Phishing - Schutzes.");
            key.SetValue("ApplicationIcon", applicationPath + @"\" + instDir + @"\msedge.exe," + icon);
            key.SetValue("ApplicationName", "Microsoft " + instDir + @" Portable");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\Capabilities\\FileAssociations");
            key.SetValue(".htm", "MSEdgeHTM.PORTABLE");
            key.SetValue(".html", "MSEdgeHTM.PORTABLE");
            key.SetValue(".shtml", "MSEdgeHTM.PORTABLE");
            key.SetValue(".svg", "MSEdgeHTM.PORTABLE");
            key.SetValue(".xht", "MSEdgeHTM.PORTABLE");
            key.SetValue(".xhtml", "MSEdgeHTM.PORTABLE");
            key.SetValue(".webp", "MSEdgeHTM.PORTABLE");
            key.SetValue(".pdf", "MSEdgePDF.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\Capabilities\\Startmenu");
            key.SetValue("StartMenuInternet", "Microsoft Edge.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\Capabilities\\URLAssociations");
            key.SetValue("ftp", "MSEdgeHTM.PORTABLE");
            key.SetValue("http", "MSEdgeHTM.PORTABLE");
            key.SetValue("https", "MSEdgeHTM.PORTABLE");
            key.SetValue("irc", "MSEdgeHTM.PORTABLE");
            key.SetValue("mailto", "MSEdgeHTM.PORTABLE");
            key.SetValue("mms", "MSEdgeHTM.PORTABLE");
            key.SetValue("news", "MSEdgeHTM.PORTABLE");
            key.SetValue("nntp", "MSEdgeHTM.PORTABLE");
            key.SetValue("read", "MSEdgeHTM.PORTABLE");
            key.SetValue("sms", "MSEdgeHTM.PORTABLE");
            key.SetValue("smsto", "MSEdgeHTM.PORTABLE");
            key.SetValue("tel", "MSEdgeHTM.PORTABLE");
            key.SetValue("urn", "MSEdgeHTM.PORTABLE");
            key.SetValue("webcal", "MSEdgeHTM.PORTABLE");
            key.SetValue("microsoft-edge", "MSEdgeHTM.PORTABLE");
            key.SetValue("microsoft-edge-holographic", "MSEdgeHTM.PORTABLE");
            key.SetValue("ms-xbl-3d8b930f", "MSEdgeHTM.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\DefaultIcon");
            key.SetValue("ApplicationIcon", applicationPath + @"\" + instDir + @"\msedge.exe," + icon);
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\InstallInfo");
            key.SetValue("ReinstallCommand", "\"" + applicationPath + @"\\" + instDir + @" Launcher.exe"" --make-default-browser");
            key.SetValue("HideIconsCommand", "\"" + applicationPath + @"\\" + instDir + @" Launcher.exe"" --hide-icons");
            key.SetValue("ShowIconsCommand", "\"" + applicationPath + @"\\" + instDir + @" Launcher.exe"" --show-icons");
            key.SetValue("IconsVisible", 1, Microsoft.Win32.RegistryValueKind.DWord);
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\shell\\open\\command");
            key.SetValue(default, "\"" + applicationPath + @"\" + instDir + @" Launcher.exe"" \""%1\""");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.xhtml\\OpenWithProgids");
            key.SetValue("MSEdgeHTM.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.xht\\OpenWithProgids");
            key.SetValue("MSEdgeHTM.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.webp\\OpenWithProgids");
            key.SetValue("MSEdgeHTM.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.svg\\OpenWithProgids");
            key.SetValue("MSEdgeHTM.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.shtml\\OpenWithProgids");
            key.SetValue("MSEdgeHTM.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.pdf\\OpenWithProgids");
            key.SetValue("MSEdgePDF.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.html\\OpenWithProgids");
            key.SetValue("MSEdgeHTM.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\.htm\\OpenWithProgids");
            key.SetValue("MSEdgeHTM.PORTABLE", "");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\msedge.exe");
            key.SetValue(default, "\"" + applicationPath + @"\" + instDir + @" Launcher.exe"" \""%1\""");
            key.SetValue("Path", applicationPath);
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts");
            key.SetValue("MSEdgeHTM.PORTABLE_microsoft-edge", 0, Microsoft.Win32.RegistryValueKind.DWord);
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\https\\UserChoice");
            key.SetValue("ProgId", "MSEdgeHTM.PORTABLE");
            key.Close();
            key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\http\\UserChoice");
            key.SetValue("ProgId", "MSEdgeHTM.PORTABLE");
            key.Close();
            try
            {
                key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false);
                if (key.GetValue("ProductName").ToString().Contains("Windows 10"))
                {
                    key.Close();
                    Process process = new Process();
                    process.StartInfo.FileName = "ms-settings:defaultapps";
                    process.Start();
                }
                else
                {
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static void RegDel()
        {
            try
            {
                Microsoft.Win32.RegistryKey key;
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\FileExts\\.pdf\\UserChoice", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\Capabilities\\FileAssociations", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\shell\\open\\command", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\shell\\open", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\shell", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\DefaultIcon", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\InstallInfo", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\Capabilities\\Startmenu", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE\\Capabilities\\URLAssociations", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgeHTM.Portable\\shell\\open\\command", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgeHTM.Portable\\shell\\open", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgeHTM.Portable\\shell", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgeHTM.Portable\\DefaultIcon", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgeHTM.Portable\\Application", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgeHTM.Portable", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgePDF.Portable\\shell\\open\\command", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgePDF.Portable\\shell\\open", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgePDF.Portable\\shell", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgePDF.Portable\\DefaultIcon", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgePDF.Portable\\Application", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Classes\\MSEdgePDF.Portable", false);
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\msedge.exe", false);
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\RegisteredApplications", true);
                key.DeleteValue("Microsoft Edge.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.xhtml\\OpenWithProgids", true);
                key.DeleteValue("MSEdgeHTM.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.xht\\OpenWithProgids", true);
                key.DeleteValue("MSEdgeHTM.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.webp\\OpenWithProgids", true);
                key.DeleteValue("MSEdgeHTM.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.svg\\OpenWithProgids", true);
                key.DeleteValue("MSEdgeHTM.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.shtml\\OpenWithProgids", true);
                key.DeleteValue("MSEdgeHTM.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.pdf\\OpenWithProgids", true);
                key.DeleteValue("MSEdgeHTM.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.html\\OpenWithProgids", true);
                key.DeleteValue("MSEdgeHTM.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\.htm\\OpenWithProgids", true);
                key.DeleteValue("MSEdgeHTM.PORTABLE", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ApplicationAssociationToasts", true);
                key.DeleteValue("MSEdgeHTM.PORTABLE_microsoft-edge", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\https\\UserChoice", true);
                key.DeleteValue("Hash", false);
                key.DeleteValue("ProgId", false);
                key.Close();
                key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\Shell\\Associations\\UrlAssociations\\http\\UserChoice", true);                
                key.DeleteValue("Hash", false);
                key.DeleteValue("ProgId", false);
                key.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
    }
}
