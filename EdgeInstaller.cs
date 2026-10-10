using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using PortableEdge;

namespace Edge_Updater
{
    internal static class EdgeInstaller
    {
        public static async Task ExtractAsync(string extractor, string archive, string destination, CancellationToken cancellationToken)
        {
            if (!File.Exists(extractor))
                throw new FileNotFoundException("Place the official 7zr.exe in the Bin folder before downloading Edge.", extractor);
            var start = new ProcessStartInfo(extractor) { UseShellExecute = false, CreateNoWindow = true };
            foreach (string argument in new[] { "x", archive, "-o" + destination, "-y" })
                start.ArgumentList.Add(argument);
            using var process = Process.Start(start);
            try { await process.WaitForExitAsync(cancellationToken); }
            catch (OperationCanceledException) { process.Kill(true); await process.WaitForExitAsync(); throw; }
            if (process.ExitCode != 0)
                throw new InvalidDataException("7-Zip could not extract the Edge installer (exit code " + process.ExitCode + ").");
        }

        public static void Install(string extractedRoot, string destination, string version, string channel, string architecture)
        {
            if (!Version.TryParse(version, out _)) throw new InvalidDataException("Invalid Edge version.");
            string sourceVersion = Path.Combine(extractedRoot, version);
            string sourceExe = File.Exists(Path.Combine(extractedRoot, "msedge.exe"))
                ? Path.Combine(extractedRoot, "msedge.exe") : Path.Combine(sourceVersion, "msedge.exe");
            if (!File.Exists(sourceExe) || !File.Exists(Path.Combine(sourceVersion, "msedge.dll")) ||
                !File.Exists(Path.Combine(sourceVersion, "icudtl.dat")))
                throw new InvalidDataException("The extracted Edge installation is incomplete. Existing files were kept.");
            if (FileVersionInfo.GetVersionInfo(sourceExe).FileVersion != version)
                throw new InvalidDataException("The extracted browser version does not match the requested version.");

            Directory.CreateDirectory(destination);
            string targetVersion = Path.Combine(destination, version);
            string backup = Path.Combine(destination, ".rollback-" + Guid.NewGuid().ToString("N"));
            string versionLog = Path.Combine(destination, "updates", "Version.log");
            var original = new Dictionary<string, byte[]>();
            var replacements = new Dictionary<string, byte[]>();
            foreach (string name in new[] { "msedge.exe", "msedge_proxy.exe" })
            {
                string source = Path.Combine(Path.GetDirectoryName(sourceExe), name);
                if (!File.Exists(source)) continue;
                string target = Path.Combine(destination, name);
                replacements[target] = File.ReadAllBytes(source);
                original[target] = File.Exists(target) ? File.ReadAllBytes(target) : null;
            }
            original[versionLog] = File.Exists(versionLog) ? File.ReadAllBytes(versionLog) : null;
            bool backedUp = false;
            bool moved = false;
            var written = new List<string>();
            try
            {
                if (Directory.Exists(targetVersion)) { Directory.Move(targetVersion, backup); backedUp = true; }
                Directory.Move(sourceVersion, targetVersion);
                moved = true;
                foreach (var replacement in replacements)
                {
                    WriteAtomic(replacement.Key, replacement.Value);
                    written.Add(replacement.Key);
                }
                SandboxAccess.Ensure(destination);
                Directory.CreateDirectory(Path.GetDirectoryName(versionLog));
                WriteAtomic(versionLog, System.Text.Encoding.UTF8.GetBytes(version + "|" + channel + "|" + architecture));
                written.Add(versionLog);
            }
            catch
            {
                foreach (string path in written)
                    if (original[path] == null) File.Delete(path); else WriteAtomic(path, original[path]);
                if (moved) Directory.Move(targetVersion, sourceVersion);
                if (backedUp) Directory.Move(backup, targetVersion);
                throw;
            }
            // Previous versions are deliberately retained; they are never deleted before a successful install.
            if (backedUp) Directory.Delete(backup, true);
        }

        private static void WriteAtomic(string path, byte[] content)
        {
            string temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
            try { File.WriteAllBytes(temporary, content); File.Move(temporary, path, true); }
            finally { if (File.Exists(temporary)) File.Delete(temporary); }
        }
    }
}
