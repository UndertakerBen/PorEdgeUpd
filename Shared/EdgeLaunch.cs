using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Windows.Forms;

[assembly: SupportedOSPlatform("windows6.1")]

namespace PortableEdge
{
    internal static class EdgeLaunch
    {
        public static ProcessStartInfo CreateStartInfo(string root, string folder, string profile, string[] arguments)
        {
            string browserDirectory = Path.Combine(root, folder);
            var start = new ProcessStartInfo(Path.Combine(browserDirectory, "msedge.exe"))
            {
                UseShellExecute = false,
                WorkingDirectory = browserDirectory
            };
            const string prefix = "--user-data-dir=";
            profile = profile.Trim();
            if (profile.Length > 0)
            {
                if (!profile.StartsWith(prefix, StringComparison.Ordinal))
                    throw new InvalidDataException("Profile.txt must contain --user-data-dir=\"path\" or be empty.");
                string path = profile.Substring(prefix.Length).Trim();
                if (path.StartsWith('"') && path.EndsWith('"'))
                    path = path.Substring(1, path.Length - 2);
                if (string.IsNullOrWhiteSpace(path) || path.Contains('"'))
                    throw new InvalidDataException("Invalid profile path in Profile.txt.");
                start.ArgumentList.Add(prefix + Path.GetFullPath(path, root));
            }
            foreach (string argument in arguments)
                start.ArgumentList.Add(argument);
            return start;
        }

        public static void Run(string folder, Func<Form> profileDialog, string[] arguments)
        {
            UiLanguage.UseEnglish();
            try
            {
                string root = AppContext.BaseDirectory;
                string profileFile = Path.Combine(root, folder, "Profile.txt");
                if (!File.Exists(Path.Combine(root, folder, "msedge.exe")))
                    throw new FileNotFoundException("Microsoft Edge Portable is not installed.");
                if (!File.Exists(profileFile))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    using (Form dialog = profileDialog())
                        dialog.ShowDialog();
                    if (!File.Exists(profileFile))
                        return; // The user closed the profile picker without choosing a profile.
                }
                SandboxAccess.Ensure(Path.Combine(root, folder));
                using (Process browser = Process.Start(CreateStartInfo(root, folder, File.ReadAllText(profileFile), arguments))) { }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, folder + " Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
