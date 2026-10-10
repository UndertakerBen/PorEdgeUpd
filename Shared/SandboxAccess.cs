using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace PortableEdge
{
    internal static class SandboxAccess
    {
        private static readonly string[] PackageSids = { "S-1-15-2-1", "S-1-15-2-2" };

        // Chromium's AppContainer/LPAC children need RX access to browser binaries.
        // Never inherit these rules into the installation root: it can contain a profile.
        public static void Ensure(string browserDirectory)
        {
            var root = new DirectoryInfo(Path.GetFullPath(browserDirectory));
            if (!File.Exists(Path.Combine(root.FullName, "msedge.exe")))
                throw new FileNotFoundException("The selected directory is not an Edge installation.");
            GrantReadExecute(root);
            foreach (FileInfo file in root.GetFiles())
            {
                if (file.Extension.Equals(".exe", StringComparison.OrdinalIgnoreCase) ||
                    file.Extension.Equals(".dll", StringComparison.OrdinalIgnoreCase))
                    GrantReadExecute(file);
            }
            foreach (DirectoryInfo version in root.GetDirectories())
            {
                if (Version.TryParse(version.Name, out _) && File.Exists(Path.Combine(version.FullName, "msedge.dll")))
                    GrantTree(version);
            }
        }

        private static void GrantTree(DirectoryInfo directory)
        {
            RejectReparsePoint(directory);
            GrantReadExecute(directory);
            foreach (FileSystemInfo child in directory.GetFileSystemInfos())
            {
                if (child is DirectoryInfo nested)
                    GrantTree(nested);
                else
                    GrantReadExecute(child);
            }
        }

        private static void RejectReparsePoint(FileSystemInfo entry)
        {
            if ((entry.Attributes & FileAttributes.ReparsePoint) != 0)
                throw new IOException("Cannot repair sandbox access through a symbolic link: " + entry.FullName);
        }

        private static void GrantReadExecute(FileSystemInfo entry)
        {
            RejectReparsePoint(entry);
            FileSystemSecurity security = entry is DirectoryInfo directory
                ? directory.GetAccessControl(AccessControlSections.Access)
                : ((FileInfo)entry).GetAccessControl(AccessControlSections.Access);
            bool changed = false;
            foreach (string value in PackageSids)
            {
                var sid = new SecurityIdentifier(value);
                bool present = false;
                foreach (FileSystemAccessRule rule in security.GetAccessRules(true, true, typeof(SecurityIdentifier)))
                {
                    if (rule.IdentityReference.Equals(sid) && rule.AccessControlType == AccessControlType.Allow &&
                        (rule.PropagationFlags & PropagationFlags.InheritOnly) == 0 &&
                        (rule.FileSystemRights & FileSystemRights.ReadAndExecute) == FileSystemRights.ReadAndExecute)
                        present = true;
                }
                if (!present)
                {
                    security.AddAccessRule(new FileSystemAccessRule(sid, FileSystemRights.ReadAndExecute, AccessControlType.Allow));
                    changed = true;
                }
            }
            if (!changed)
                return;
            if (entry is DirectoryInfo folder)
                folder.SetAccessControl((DirectorySecurity)security);
            else
                ((FileInfo)entry).SetAccessControl((FileSecurity)security);
        }
    }
}
