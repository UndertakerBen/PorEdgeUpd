using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edge_Updater
{
    public partial class Form1 : Form
    {
        private static readonly string[] ring = new string[4] { "Canary", "Dev", "Beta", "Stable" };
        private static readonly string[] ring2 = new string[8] { "Canary", "Developer", "Beta", "Stable", "Canary", "Developer", "Beta", "Stable" };
        private static readonly string[] buildversion = new string[8];
        private static readonly string[] architektur = new string[2] { "X86", "X64" };
        private static readonly string[] architektur2 = new string[2] { "x86", "x64" };
        private static readonly string[] instOrdner = new string[9] { "Edge Canary x86", "Edge Dev x86", "Edge Beta x86", "Edge Stable x86", "Edge Canary x64", "Edge Dev x64", "Edge Beta x64", "Edge Stable x64", "Edge" };
        private static readonly string[] icon = new string[4] { "4", "8", "9", "0" };
        private bool operationInProgress;
        private readonly string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        private readonly string applicationPath = Application.StartupPath;
        private readonly ToolTip toolTip = new ToolTip();
        private readonly CancellationTokenSource lifetime = new CancellationTokenSource();


        

        public Form1()
        {
            InitializeComponent();
            Shown += async (_, _) => await LoadMetadataAsync();
            FormClosing += (_, e) =>
            {
                if (operationInProgress)
                {
                    lifetime.Cancel();
                    e.Cancel = true;
                }
            };
            label2.Text = buildversion[0];
            label4.Text = buildversion[1];
            label6.Text = buildversion[2];
            label8.Text = buildversion[3];
            Refresh();
            button9.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            if (IntPtr.Size != 8)
            {
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                checkBox3.Visible = false;
            }
            if (IntPtr.Size == 8)
            {
                if (File.Exists(@"Edge Canary x64\msedge.exe") || File.Exists(@"Edge Dev x64\msedge.exe") || File.Exists(@"Edge Beta x64\msedge.exe") || File.Exists(@"Edge Stable x64\msedge.exe"))
                {
                    checkBox3.Enabled = false;
                }
                if (File.Exists(@"Edge Canary x86\msedge.exe") || File.Exists(@"Edge Dev x86\msedge.exe") || File.Exists(@"Edge Beta x86\msedge.exe") || File.Exists(@"Edge Stable x86\msedge.exe"))
                {
                    checkBox2.Enabled = false;
                }
                if (File.Exists(@"Edge Canary x86\msedge.exe") || File.Exists(@"Edge Dev x86\msedge.exe") || File.Exists(@"Edge Beta x86\msedge.exe") || File.Exists(@"Edge Stable x86\msedge.exe") || File.Exists(@"Edge Canary x64\msedge.exe") || File.Exists(@"Edge Dev x64\msedge.exe") || File.Exists(@"Edge Beta x64\msedge.exe") || File.Exists(@"Edge Stable x64\msedge.exe"))
                {
                    checkBox1.Checked = true;
                    CheckButton();
                }
                else if (!checkBox1.Checked)
                {
                    checkBox2.Enabled = false;
                    checkBox3.Enabled = false;
                    button9.Enabled = false;
                    button9.BackColor = Color.FromArgb(244, 244, 244);
                    if (File.Exists(@"Edge\msedge.exe"))
                    {
                        CheckButton2();
                    }
                }
            }
            if (IntPtr.Size != 8)
            {
                if (File.Exists(@"Edge Canary x86\msedge.exe") || File.Exists(@"Edge Dev x86\msedge.exe") || File.Exists(@"Edge Beta x86\msedge.exe") || File.Exists(@"Edge Stable x86\msedge.exe"))
                {
                    checkBox1.Checked = true;
                    checkBox2.Enabled = false;
                    CheckButton();
                }
                else if (!checkBox1.Checked)
                {
                    checkBox2.Enabled = false;
                    button9.Enabled = false;
                    button9.BackColor = Color.FromArgb(244, 244, 244);
                    if (File.Exists(@"Edge\msedge.exe"))
                    {
                        CheckButton2();
                    }
                }
            }

            if ((buildversion[0] == null) || (buildversion[1] == null) || (buildversion[2] == null) || (buildversion[3] == null))
            {
                groupBox3.Enabled = false;
                button9.Enabled = false;
                checkBox1.Enabled = false;
            }
        }
        private async void Button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(0, 0, 0, 1);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(0, 0, 1);
            }
        }
        private async void Button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(1, 1, 0, 2);
            }
            if (!checkBox1.Checked)
            {
                await NewMethod1(1, 0, 2);
            }
        }
        private async void Button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(2, 2, 0, 3);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(2, 0, 3);
            }
        }
        private async void Button4_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(3, 3, 0, 4);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(3, 0, 4);
            }
        }
        private async void Button5_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(0, 4, 1, 5);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(0, 1, 5);
            }
        }
        private async void Button6_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(1, 5, 1, 6);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(1, 1, 6);
            }
        }
        private async void Button7_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(2, 6, 1,  7);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(2, 1, 7);
            }
        }
        private async void Button8_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                await NewMethod(3, 7, 1, 8);
            }
            else if (!checkBox1.Checked)
            {
                await NewMethod1(3, 1, 8);
            }
        }
        private async void Button9_Click(object sender, EventArgs e)
        {
            await Testing();
        }
        private async Task Testing()
        {
            if ((!Directory.Exists(@"Edge Canary x86")) && (!Directory.Exists(@"Edge Dev x86")) && (!Directory.Exists(@"Edge Beta x86")) && (!Directory.Exists(@"Edge Stable x86")))
            {
                if (checkBox2.Checked)
                {
                    await DownloadFile(0, 0, 0, 1);
                    await DownloadFile(1, 1, 0, 2);
                    await DownloadFile(2, 2, 0, 3);
                    await DownloadFile(3, 3, 0, 4);
                    checkBox2.Enabled = false;
                }
            }
            await NewMethod2(0, 0, 0, 1);
            await NewMethod2(1, 1, 0, 2);
            await NewMethod2(2, 2, 0, 3);
            await NewMethod2(3, 3, 0, 4);
            if (IntPtr.Size == 8)
            {
                if ((!Directory.Exists(@"Edge Canary x64")) && (!Directory.Exists(@"Edge Dev x64")) && (!Directory.Exists(@"Edge Beta x64")) && (!Directory.Exists(@"Edge Stable x64")))
                {
                    if (checkBox3.Checked)
                    {
                        await DownloadFile(0, 4, 1, 5);
                        await DownloadFile(1, 5, 1, 6);
                        await DownloadFile(2, 6, 1, 7);
                        await DownloadFile(3, 7, 1, 8);
                        checkBox3.Enabled = false;
                    }
                }
                await NewMethod2(0, 4, 1, 5);
                await NewMethod2(1, 5, 1, 6);
                await NewMethod2(2, 6, 1, 7);
                await NewMethod2(3, 7, 1, 8);
            }
        }
        private async Task LoadMetadataAsync()
        {
            groupBox3.Enabled = false;
            var failures = new List<string>();
            await Task.WhenAll(Enumerable.Range(0, 8).Select(async index =>
            {
                try { buildversion[index] = await EdgeUpdateClient.GetVersionAsync(ring[index % 4], architektur[index / 4]); }
                catch (Exception ex) { failures.Add(ring[index % 4] + " " + architektur[index / 4] + ": " + ex.Message); }
            }));
            if (IsDisposed) return;
            label2.Text = buildversion[0] ?? buildversion[4] ?? "Unavailable";
            label4.Text = buildversion[1] ?? buildversion[5] ?? "Unavailable";
            label6.Text = buildversion[2] ?? buildversion[6] ?? "Unavailable";
            label8.Text = buildversion[3] ?? buildversion[7] ?? "Unavailable";
            groupBox3.Enabled = true;
            checkBox1.Enabled = true;
            RefreshVersionButtons();
            if (failures.Count > 0)
                MessageBox.Show("Some Edge versions could not be retrieved. Reopen the updater to retry.\n\n" + string.Join("\n", failures), Text);
            try
            {
                var policies = await EdgeUpdateClient.GetPoliciesAsync();
                if (IsDisposed) return;
                foreach (var policy in policies)
                {
                    var item = policyTemplatesDownloadToolStripMenuItem.DropDownItems.Add(policy.Version + " (" + policy.Extension + ")");
                    item.Click += async (_, _) => await DownloadADMX(policy);
                }
            }
            catch (Exception ex)
            {
                if (!IsDisposed) policyTemplatesDownloadToolStripMenuItem.ToolTipText = ex.Message;
            }
        }

        private void RefreshVersionButtons()
        {
            if (checkBox1.Checked) CheckButton(); else CheckButton2();
            for (int index = 0; index < 8; index++)
                if (Controls.Find("button" + (index + 1), true).FirstOrDefault() is Button button)
                {
                    button.Enabled = buildversion[index] != null;
                    toolTip.SetToolTip(button, buildversion[index] ?? "Version unavailable");
                }
        }

        public async Task DownloadFile(int a, int b, int c, int d)
        {
            if (operationInProgress || lifetime.IsCancellationRequested || IsDisposed) return;
            string version = buildversion[a + c * 4];
            if (version == null) return;
            string destination = Path.Combine(applicationPath, instOrdner[b]);
            string stage = Path.Combine(applicationPath, "Update", Guid.NewGuid().ToString("N"));
            string launcherName = instOrdner[b] + " Launcher.exe";
            string launcher = Path.Combine(applicationPath, "Bin", "Launcher", launcherName);
            string extractor = Path.Combine(applicationPath, "Bin", "7zr.exe");
            try
            {
                if (!File.Exists(launcher)) throw new FileNotFoundException("Launcher missing. Use a complete published package.", launcher);
                if (!File.Exists(extractor)) throw new FileNotFoundException("Place the official 7zr.exe in the Bin folder.", extractor);
                foreach (Process process in Process.GetProcessesByName("msedge"))
                    using (process)
                    {
                        string executable;
                        try { executable = process.MainModule?.FileName; }
                        catch (System.ComponentModel.Win32Exception) { continue; }
                        catch (InvalidOperationException) { continue; }
                        if (executable != null && executable.StartsWith(destination + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                            throw new IOException("Close this portable Edge installation before updating.");
                    }
                operationInProgress = true;
                groupBox3.Enabled = false;
                groupBox1.Enabled = false;
                button9.Enabled = false;
                Directory.CreateDirectory(stage);
                var package = await EdgeUpdateClient.GetPackageAsync(ring[a], architektur[c], version, lifetime.Token);
                string installer = Path.Combine(stage, package.FileId);
                var progress = new Progress<(long Received, long? Total)>(value =>
                {
                    Text = "Downloading Edge " + version + " — " + (value.Received / 1048576) + " MB";
                });
                await EdgeUpdateClient.DownloadAsync(package.Url, installer, Convert.FromBase64String(package.Hashes["Sha256"]), progress, lifetime.Token);
                Text = "Extracting Edge " + version;
                await EdgeInstaller.ExtractAsync(extractor, installer, stage, lifetime.Token);
                await EdgeInstaller.ExtractAsync(extractor, Path.Combine(stage, "MSEDGE.7z"), stage, lifetime.Token);
                EdgeInstaller.Install(Path.Combine(stage, "Chrome-bin"), destination, version, ring2[a], architektur2[c]);
                File.Copy(launcher, Path.Combine(applicationPath, launcherName), true);
                if (checkBox5.Checked) NewMethod5(a, b);
            }
            catch (OperationCanceledException) when (lifetime.IsCancellationRequested) { }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Edge update failed", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally
            {
                try { if (Directory.Exists(stage)) Directory.Delete(stage, true); }
                catch (IOException) { /* A scanner may still have the temporary download open. */ }
                catch (UnauthorizedAccessException) { /* Leave only this operation's staging files for manual cleanup. */ }
                operationInProgress = false;
                groupBox1.Enabled = true;
                groupBox3.Enabled = true;
                Text = "Portable Edge (Chromium) Updater";
                RefreshVersionButtons();
                if (lifetime.IsCancellationRequested) Close();
            }
        }
        public void CheckButton()
        {
            button9.Enabled = checkBox2.Checked || checkBox3.Checked;
            NewMethod3();
            for (int i = 0; i <= 7; i++)
            {
                if (File.Exists(@instOrdner[i] + "\\updates\\Version.log"))
                {
                    Control[] buttons = Controls.Find("button" + (i + 1), true);
                    string[] instVersion = File.ReadAllText(@instOrdner[i] + "\\updates\\Version.log").Split(new char[] { '|' });
                    if (buildversion[i] == instVersion[0])
                    {
                        if (buttons.Length > 0)
                        {
                            Button button = (Button)buttons[0];
                            button.BackColor = Color.Green;
                        }
                    }
                    else if (buildversion[i] != instVersion[0])
                    {
                        button9.Text = "Update all";
                        button9.Enabled = true;
                        button9.BackColor = Color.FromArgb(224, 224, 224);
                        if (buttons.Length > 0)
                        {
                            Button button = (Button)buttons[0];
                            button.BackColor = Color.Red;
                        }
                    }
                }
            }
        }
        public void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox3.Enabled = !File.Exists(@"Edge Canary x64\msedge.exe") && !File.Exists(@"Edge Dev x64\msedge.exe") && !File.Exists(@"Edge Beta x64\msedge.exe") && !File.Exists(@"Edge Stable x64\msedge.exe");
                checkBox2.Enabled = !File.Exists(@"Edge Canary x86\msedge.exe") && !File.Exists(@"Edge Dev x86\msedge.exe") && !File.Exists(@"Edge Beta x86\msedge.exe") && !File.Exists(@"Edge Stable x86\msedge.exe");
                if (button9.Enabled)
                {
                    button9.BackColor = Color.FromArgb(224, 224, 224);
                }
                CheckButton();
            }
            if (!checkBox1.Checked)
            {
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
                button9.Enabled = false;
                button9.BackColor = Color.FromArgb(244, 244, 244);
                CheckButton2();
            }
        }
        public void CheckButton2()
        {
            NewMethod3();
            if (File.Exists(@"Edge\updates\Version.log"))
            {
                string[] instVersion = File.ReadAllText(@"Edge\updates\Version.log").Split(new char[] { '|' });
                switch (instVersion[1])
                {
                    case "Canary":
                        NewMethod6(instVersion, 1, 5, 0);
                        break;
                    case "Developer":
                        NewMethod6(instVersion, 2, 6, 1);
                        break;
                    case "Beta":
                        NewMethod6(instVersion, 3, 7, 2);
                        break;
                    case "Stable":
                        NewMethod6(instVersion, 4, 8, 3);
                        break;
                }
            }
        }
        private void Button1_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(0, "x86");
        }
        private void Button2_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(1, "x86");
        }
        private void Button3_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(2, "x86");
        }
        private void Button4_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(3, "x86");
        }
        private void Button5_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(4, "x64");
        }
        private void Button6_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(5, "x64");
        }
        private void Button7_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(6, "x64");
        }
        private void Button8_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(7, "x64");
        }
        public void Message1()
        {
            MessageBox.Show("The same version is already installed.", "Portable Edge (Chromium) Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                button9.Enabled = true;
                button9.BackColor = Color.FromArgb(224, 224, 224);
            }
            else if ((!checkBox2.Checked) && (!checkBox3.Checked))
            {
                button9.Enabled = false;
                button9.BackColor = Color.FromArgb(244, 244, 244);
            }
        }
        private void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                button9.Enabled = true;
                button9.BackColor = Color.FromArgb(224, 224, 224);
            }
            else if ((!checkBox2.Checked) && (!checkBox3.Checked))
            {
                button9.Enabled = false;
                button9.BackColor = Color.FromArgb(244, 244, 244);
            }
        }
        private void Button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            lifetime.Cancel();
        }
        private void Button9_EnabledChanged(object sender, EventArgs e)
        {
            if (!button9.Enabled)
            {
                button9.BackColor = Color.FromArgb(244, 244, 244);
            }
        }
        private async Task NewMethod(int a, int b, int c, int d)
        {
            if (File.Exists(@instOrdner[b] + "\\updates\\Version.log"))
            {
                if (File.ReadAllText(instOrdner[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] == buildversion[a + (b >= 4 ? 4 : 0)])
                {
                    if (checkBox4.Checked)
                    {
                        await DownloadFile(a, b, c, d);
                    }
                    else
                    {
                        Message1();
                    }
                }
                else
                {
                    await DownloadFile(a, b, c, d);
                }
            }
            else
            {
                await DownloadFile(a, b, c, d);
            }
        }
        private async Task NewMethod1(int a, int b, int c)
        {
            if (File.Exists(@"Edge\updates\Version.log"))
            {
                string[] instVersion = File.ReadAllText(@"Edge\updates\Version.log").Split(new char[] { '|' });
                if ((instVersion[0] == buildversion[a + b * 4]) && (instVersion[1] == ring2[a]) && (instVersion[2] == architektur2[b]))
                {
                    if (checkBox4.Checked)
                    {
                        await DownloadFile(a, 8, b, c);
                    }
                    else
                    {
                        Message1();
                    }
                }
                else
                {
                    await DownloadFile(a, 8, b, c);
                }
            }
            else
            {
                await DownloadFile(a, 8, b, c);
            }
        }
        private async Task NewMethod2(int a, int b, int c, int d)
        {
            if (Directory.Exists(instOrdner[b]))
            {
                if (File.Exists(instOrdner[b] + "\\updates\\Version.log"))
                {
                    if (File.ReadAllText(instOrdner[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] != buildversion[a + (b >= 4 ? 4 : 0)])
                    {
                        await DownloadFile(a, b, c, d);
                    }
                }
            }
        }
        private void NewMethod3()
        {
            for (int i = 1; i <= 8; i++)
            {
                Control[] buttons = Controls.Find("button" + i, true);
                if (buttons.Length > 0)
                {
                    Button button = (Button)buttons[0];
                    button.BackColor = Color.FromArgb(224, 224, 224);
                }
            }
        }
        private void NewMethod5(int c, int d)
        {
            dynamic shell = Activator.CreateInstance(Type.GetTypeFromProgID("WScript.Shell", throwOnError: true));
            dynamic link = shell.CreateShortcut(deskDir + "\\" + instOrdner[d] + ".lnk");
            link.IconLocation = applicationPath + "\\" + instOrdner[d] + "\\msedge.exe" + "," + icon[c];
            link.WorkingDirectory = applicationPath;
            link.TargetPath = applicationPath + "\\" + instOrdner[d] + " Launcher.exe";
            link.Save();
        }
        private void NewMethod6(string[] instVersion, int a, int b, int c)
        {
            Control[] buttons = Controls.Find("button" + a, true);
            Control[] buttons2 = Controls.Find("button" + b, true);
            int versionIndex = c + (instVersion[2] == "x64" ? 4 : 0);
            if (instVersion[0] == buildversion[versionIndex])
            {
                if (instVersion[2] == "x86")
                {
                    if (buttons.Length > 0)
                    {
                        Button button = (Button)buttons[0];
                        button.BackColor = Color.Green;
                    }
                }
                else if (instVersion[2] == "x64")
                {
                    if (buttons2.Length > 0)
                    {
                        Button button = (Button)buttons2[0];
                        button.BackColor = Color.Green;
                    }
                }
            }
            else if (instVersion[0] != buildversion[versionIndex])
            {
                if (instVersion[2] == "x86")
                {
                    if (buttons.Length > 0)
                    {
                        Button button = (Button)buttons[0];
                        button.BackColor = Color.Red;
                    }
                }
                else if (instVersion[2] == "x64")
                {
                    if (buttons2.Length > 0)
                    {
                        Button button = (Button)buttons2[0];
                        button.BackColor = Color.Red;
                    }
                }
            }
        }
        private void NewMethod7(int a, string arch)
        {
            Control[] buttons = Controls.Find("button" + (a + 1), true);
            Button button = (Button)buttons[0];
            if (!checkBox1.Checked)
            {
                if (File.Exists(@"Edge\updates\Version.log"))
                {
                    NewMethod8(a, arch, button, File.ReadAllText(@"Edge\updates\Version.log").Split(new char[] { '|' }));
                }
            }
            if (checkBox1.Checked)
            {
                if (File.Exists(instOrdner[a] + "\\updates\\Version.log"))
                {
                    NewMethod8(a, arch, button, File.ReadAllText(instOrdner[a] + "\\updates\\Version.log").Split(new char[] { '|' }));
                }
            }
        }
        private void NewMethod8(int a, string arch, Button button, string[] instVersion)
        {
            if ((instVersion[1] == ring2[a]) && (instVersion[2] == arch))
            {
                toolTip.SetToolTip(button, instVersion[0]);
                toolTip.IsBalloon = true;
            }
            else
            {
                toolTip.SetToolTip(button, String.Empty);
            }
        }
        private void VersionsInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileVersionInfo updVersion = FileVersionInfo.GetVersionInfo(applicationPath + "\\Portable Edge (Chromium) Updater.exe");
            FileVersionInfo launcherVersion = FileVersionInfo.GetVersionInfo(applicationPath + "\\Bin\\Launcher\\Edge Launcher.exe");
            MessageBox.Show("Updater Version - " + updVersion.FileVersion + "\nLauncher Version - " + launcherVersion.FileVersion, "Version Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void RegistrierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instOrdner[8], 0);
        }
        private void RegistrierenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instOrdner[3], 0);
        }
        private void RegistrierenToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instOrdner[7], 0);
        }
        private void RegistrierenToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instOrdner[2], 9);
        }
        private void RegistrierenToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instOrdner[6], 9);
        }
        private void RegisrierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instOrdner[1], 8);
        }
        private void RegistrierenToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instOrdner[5], 8);
        }
        private void RegistrierenToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instOrdner[0], 4);
        }
        private void RegistrierenToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Regfile.RegCreate(applicationPath, instOrdner[4], 4);
        }
        private void EntfernenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem.Enabled = true;
            Regfile.RegDel();
        }
        private void EnfernenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem1.Enabled = true;
            Regfile.RegDel();
        }
        private void EntfernenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem2.Enabled = true;
            Regfile.RegDel();
        }
        private void EnfernenToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem3.Enabled = true;
            Regfile.RegDel();
        }
        private void EnfernenToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem4.Enabled = true;
            Regfile.RegDel();
        }
        private void EnfernenToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            regisrierenToolStripMenuItem.Enabled = true;
            Regfile.RegDel();
        }
        private void EnfernenToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem5.Enabled = true;
            Regfile.RegDel();
        }
        private void EnfernenToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem6.Enabled = true;
            Regfile.RegDel();
        }
        private void EnfernenToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            registrierenToolStripMenuItem7.Enabled = true;
            Regfile.RegDel();
        }
        private void ExtrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Win32.RegistryKey key;
                if (Microsoft.Win32.Registry.GetValue("HKEY_Current_User\\Software\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE", default, null) != null)
                {
                    key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\Clients\\StartMenuInternet\\Microsoft Edge.PORTABLE", false);
                    switch (key.GetValue(default).ToString())
                    {
                        case "Microsoft Edge Portable":
                            key.Close();
                            registrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            break;
                        case "Microsoft Edge Stable x86 Portable":
                            key.Close();
                            registrierenToolStripMenuItem1.Enabled = false;
                            edgeChromiumAlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            break;
                        case "Microsoft Edge Stable x64 Portable":
                            key.Close();
                            registrierenToolStripMenuItem2.Enabled = false;
                            edgeChromiumAlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            break;
                        case "Microsoft Edge Beta x86 Portable":
                            key.Close();
                            registrierenToolStripMenuItem3.Enabled = false;
                            edgeChromiumAlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            break;
                        case "Microsoft Edge Beta x64 Portable":
                            key.Close();
                            registrierenToolStripMenuItem4.Enabled = false;
                            edgeChromiumAlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            break;
                        case "Microsoft Edge Dev x86 Portable":
                            key.Close();
                            regisrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumAlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            break;
                        case "Microsoft Edge Dev x64 Portable":
                            key.Close();
                            registrierenToolStripMenuItem5.Enabled = false;
                            edgeChromiumAlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            break;
                        case "Microsoft Edge Canary x86 Portable":
                            key.Close();
                            registrierenToolStripMenuItem6.Enabled = false;
                            edgeChromiumAlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            break;
                        case "Microsoft Edge Canary x64 Portable":
                            key.Close();
                            registrierenToolStripMenuItem7.Enabled = false;
                            edgeChromiumAlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumStableX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumBetaX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumDeveloperX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            edgeChromiumCanaryX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                            break;
                    }
                }
                else
                {
                    if (Directory.Exists(@"Edge"))
                    {
                        edgeChromiumAlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        edgeChromiumAlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists(@"Edge Stable x86"))
                    {
                        edgeChromiumStableX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        edgeChromiumStableX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists(@"Edge Stable x64"))
                    {
                        edgeChromiumStableX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        edgeChromiumStableX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists(@"Edge Beta x86"))
                    {
                        edgeChromiumBetaX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        edgeChromiumBetaX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists(@"Edge Beta x64"))
                    {
                        edgeChromiumBetaX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        edgeChromiumBetaX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists(@"Edge Dev x86"))
                    {
                        edgeChromiumDeveloperX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        edgeChromiumDeveloperX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists(@"Edge Dev x64"))
                    {
                        edgeChromiumDeveloperX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        edgeChromiumDeveloperX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists(@"Edge Canary x86"))
                    {
                        edgeChromiumCanaryX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        edgeChromiumCanaryX86AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                    }
                    if (Directory.Exists(@"Edge Canary x64"))
                    {
                        edgeChromiumCanaryX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = true;
                    }
                    else
                    {
                        edgeChromiumCanaryX64AlsStandardBrowserRegistrierenToolStripMenuItem.Enabled = false;
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }
        private async Task DownloadADMX(PolicyPackage policy)
        {
            if (operationInProgress) return;
            operationInProgress = true;
            groupBox3.Enabled = false;
            groupBox1.Enabled = false;
            button9.Enabled = false;
            string temporary = null;
            try
            {
                string directory = Path.Combine(applicationPath, "ADMX Policy Templates");
                Directory.CreateDirectory(directory);
                string target = Path.Combine(directory, "(" + policy.Version + ")MicrosoftEdgePolicyTemplates." + policy.Extension);
                temporary = target + ".download";
                await EdgeUpdateClient.DownloadAsync(policy.Url, temporary, Convert.FromHexString(policy.Sha256), null, lifetime.Token);
                File.Move(temporary, target, true);
            }
            catch (OperationCanceledException) when (lifetime.IsCancellationRequested) { }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Policy download failed"); }
            finally
            {
                if (temporary != null && File.Exists(temporary)) File.Delete(temporary);
                operationInProgress = false;
                groupBox3.Enabled = true;
                groupBox1.Enabled = true;
                RefreshVersionButtons();
                if (lifetime.IsCancellationRequested) Close();
            }
        }
    }
}
