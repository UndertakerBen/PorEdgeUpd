using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Edge_Updater
{
    public partial class Form1 : Form
    {
        public static string[] ring = new string[4] { "Canary", "Dev", "Beta", "Stable" };
        public static string[] ring2 = new string[8] { "Canary", "Developer", "Beta", "Stable", "Canary", "Developer", "Beta", "Stable" };
        public static string[] buildversion = new string[8];
        public static string[] architektur = new string[2] { "X86", "X64" };
        public static string[] architektur2 = new string[2] { "x86", "x64" };
        public static string[] instOrdner = new string[9] { "Edge Canary x86", "Edge Dev x86", "Edge Beta x86", "Edge Stable x86", "Edge Canary x64", "Edge Dev x64", "Edge Beta x64", "Edge Stable x64", "Edge" };
        public static string[] entpDir = new string[9] { "Canary86", "Dev86", "Beta86", "Stable86", "Canary64", "Dev64", "Beta64", "Stable64", "Single" };
        public static string[] icon = new string[4] { "4", "8", "9", "0" };
        WebClient webClient;
        readonly string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        readonly string applicationPath = Application.StartupPath;
        readonly CultureInfo culture1 = CultureInfo.CurrentUICulture;
        readonly ToolTip toolTip = new ToolTip();
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i <= 3; i++)
            {
                WebRequest request = WebRequest.Create("https://msedge.api.cdp.microsoft.com/api/v1.1/contents/Browser/namespaces/Default/names/msedge-" + ring[i] + "-win-x64/versions/latest?action=select");
                request.Method = "POST";
                string postData = "{\"targetingAttributes\":{\"AppAp\":\"\",\"AppCohort\":\"\",\"AppLang\":\"de-de\",\"AppRollout\":\"1.0\",\"AppVersion\":\"\",\"IsMachine\":\"0\",\"OsArch\":\"x64\",\"OsPlatform\":\"win\",\"OsVersion\":\"\",\"Updater\":\"MicrosoftEdgeUpdate\",\"UpdaterVersion\":\"1.3.101.13\"}}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                using (dataStream = request.GetResponse().GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    string[] URL = responseFromServer.Substring(responseFromServer.IndexOf("Version\":\"")).Split(new char[] { '"' });
                    buildversion[i] = URL[2];
                    buildversion[i + 4] = URL[2];
                }
            }
            label2.Text = buildversion[0];
            label4.Text = buildversion[1];
            label6.Text = buildversion[2];
            label8.Text = buildversion[3];
            Refresh();
            button9.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;
            if (culture1.Name != "de-DE")
            {
                button10.Text = "Quit";
                button9.Text = "Install all";
                label10.Text = "Install all x86 and or x64";
                checkBox4.Text = "Ignore version check";
                checkBox1.Text = "Create a Folder for each version";
                checkBox5.Text = "Create a shortcut on the desktop";
            }
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
            CheckUpdate();
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
        public async Task DownloadFile(int a, int b, int c, int d)
        {
            GroupBox progressBox = new GroupBox
            {
                Location = new Point(10, 290),
                Size = new Size(359, 90),
                BackColor = Color.Lavender,
            };
            Label title = new Label
            {
                AutoSize = false,
                Location = new Point(5, 10),
                Size = new Size(349, 25),
                Text = "Edge Chromium " + ring2[a] + " " + buildversion[a] + " " + architektur2[c],
                TextAlign = ContentAlignment.BottomCenter
            };
            title.Font = new Font(title.Font.Name, 9.25F, FontStyle.Bold);
            Label downloadLabel = new Label
            {
                AutoSize = false,
                Location = new Point(8, 35),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.BottomLeft
            };
            Label percLabel = new Label
            {
                AutoSize = false,
                Location = new Point(253, 35),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.BottomRight
            };
            ProgressBar progressBarneu = new ProgressBar
            {
                Location = new Point(8, 65),
                Size = new Size(341, 7)
            };
            progressBox.Controls.Add(title);
            progressBox.Controls.Add(downloadLabel);
            progressBox.Controls.Add(percLabel);
            progressBox.Controls.Add(progressBarneu);
            Controls.Add(progressBox);
            Size = new Size(396, 432);
            List<Task> list = new List<Task>();
            WebRequest request = WebRequest.Create("https://msedge.api.cdp.microsoft.com/api/v1.1/internal/contents/Browser/namespaces/Default/names/msedge-" + ring[a] + "-win-" + architektur[c] + "/versions/" + buildversion[a] + "/files?action=GenerateDownloadInfo&foregroundPriority=true");
            request.Method = "POST";
            string postData = "{}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            using (dataStream = request.GetResponse().GetResponseStream())
            {
                string responseFromServer = new StreamReader(dataStream).ReadToEnd();
                string[] URL = responseFromServer.Substring(responseFromServer.IndexOf("MicrosoftEdge_" + architektur[c] + "_" + buildversion[a] + ".exe")).Split(new char[] { '"' });
                WebClient myWebClient = new WebClient();
                Uri uri = new Uri(URL[4]);

                using (webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged += (o, args) =>
                    {
                        Control[] buttons = Controls.Find("button" + d, true);
                        if (buttons.Length > 0)
                        {
                            Button button = (Button)buttons[0];
                            button.BackColor = Color.Orange;
                        }
                        progressBarneu.Value = args.ProgressPercentage;
                        downloadLabel.Text = string.Format("{0} MB's / {1} MB's",
                            (args.BytesReceived / 1024d / 1024d).ToString("0.00"),
                            (args.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
                        percLabel.Text = args.ProgressPercentage.ToString() + "%";
                    };
                    webClient.DownloadFileCompleted += (o, args) =>
                    {
                        if (args.Error != null)
                        {
                            var task = webClient.DownloadFileTaskAsync(uri, "MicrosoftEdge_" + architektur[c] + "_" + buildversion[a] + "_" + ring[a] + ".exe");
                            list.Add(task);
                        }
                        if (args.Cancelled == true)
                        {
                            MessageBox.Show("Download has been canceled.");
                        }
                        else
                        {
                            downloadLabel.Text = culture1.Name != "de-DE" ? "Unpacking" : "Entpacken";
                            string arguments = " x " + "MicrosoftEdge_" + architektur[c] + "_" + buildversion[a] + "_" + ring[a] + ".exe" + " -o" + @"Update\" + entpDir[b] + " -y";
                            Process process = new Process();
                            process.StartInfo.FileName = @"Bin\7zr.exe";
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            process.StartInfo.Arguments = arguments;
                            process.Start();
                            process.WaitForExit();
                            process.StartInfo.Arguments = " x " + @"Update\" + entpDir[b] + "\\MSEDGE.7z -o" + @"Update\" + entpDir[b] + " -y";
                            process.Start();
                            process.WaitForExit();
                            if ((File.Exists(@"Update\" + entpDir[d] + "\\chrome-bin\\msedge.exe")) && (File.Exists(instOrdner[b] + "\\updates\\Version.log")))
                            {
                                string[] instVersion = File.ReadAllText(instOrdner[b] + "\\updates\\Version.log").Split(new char[] { '|' });
                                FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[b] + "\\chrome-bin\\msedge.exe");
                                if (checkBox1.Checked)
                                {
                                    if (testm.FileVersion != instVersion[0])
                                    {
                                        if (Directory.Exists(instOrdner[b] + "\\" + instVersion[0]))
                                        {
                                            Directory.Delete(instOrdner[b] + "\\" + instVersion[0], true);
                                        }
                                        Thread.Sleep(2000);
                                        NewMethod4(architektur2[c], d, testm, b);
                                    }
                                    else if ((testm.FileVersion == instVersion[0]) && (checkBox4.Checked))
                                    {
                                        if (Directory.Exists(instOrdner[b] + "\\" + instVersion[0]))
                                        {
                                            Directory.Delete(instOrdner[b] + "\\" + instVersion[0], true);
                                        }
                                        Thread.Sleep(2000);
                                        NewMethod4(architektur2[c], d, testm, b);
                                    }
                                }
                                else if (!checkBox1.Checked)
                                {
                                    if (Directory.Exists(instOrdner[b] + "\\" + instVersion[0]))
                                    {
                                        Directory.Delete(instOrdner[b] + "\\" + instVersion[0], true);
                                    }
                                    Thread.Sleep(2000);
                                    NewMethod4(architektur2[c], d, testm, b);
                                }
                            }
                            else
                            {
                                if (!Directory.Exists(instOrdner[b]))
                                {
                                    Directory.CreateDirectory(instOrdner[b]);
                                }
                                else if (Directory.Exists(instOrdner[b]))
                                {
                                    if (File.Exists(instOrdner[b] + "\\msedge.exe") && (File.Exists(instOrdner[b] + "\\updates\\version.log")))
                                    {
                                        string[] instVersion = File.ReadAllText(instOrdner[b] + "\\updates\\Version.log").Split(new char[] { '|' });
                                        if (Directory.Exists(instOrdner[b] + "\\" + instVersion[0]))
                                        {
                                            Directory.Delete(instOrdner[b] + "\\" + instVersion[0], true);
                                        }
                                    }
                                }
                                NewMethod4(architektur2[c], d, FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[b] + "\\chrome-bin\\msedge.exe"), b);
                            }
                        }
                        if (checkBox5.Checked)
                        {
                            if (!File.Exists(deskDir + "\\" + instOrdner[b] + ".lnk"))
                            {
                                NewMethod5(a, b);
                            }
                        }
                        else if (File.Exists(deskDir + "\\" + instOrdner[b] + ".lnk") && (instOrdner[b] == "Edge"))
                        {
                            NewMethod5(a, b);
                        }
                        if (!File.Exists(@instOrdner[b] + " Launcher.exe"))
                        {
                            File.Copy(@"Bin\Launcher\" + instOrdner[b] + " Launcher.exe", @instOrdner[b] + " Launcher.exe");
                        }
                        File.Delete("MicrosoftEdge_" + architektur[c] + "_" + buildversion[a] + "_" + ring[a] + ".exe");
                        downloadLabel.Text = culture1.Name != "de-DE" ? "Unpacked" : "Entpackt";
                    };
                    try
                    {
                        var task = webClient.DownloadFileTaskAsync(uri, "MicrosoftEdge_" + architektur[c] + "_" + buildversion[a] + "_" + ring[a] + ".exe");
                        list.Add(task);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            await Task.WhenAll(list);
            await Task.Delay(2000);
            Size = new Size(396, 342);
            Controls.Remove(progressBox);
        }
        public void CheckButton()
        {
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
                        if (culture1.Name != "de-DE")
                        {
                            button9.Text = "Update all";
                        }
                        else
                        {
                            button9.Text = "Alle Updaten";
                        }
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
            if (culture1.Name != "de-DE")
            {
                MessageBox.Show("The same version is already installed", "Portabel Edge (Chromium) Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                MessageBox.Show("Die selbe Version ist bereits installiert", "Portabel Edge (Chromium) Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
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
            if (Directory.Exists(@"Update"))
            {
                Directory.Delete(@"Update", true);
            }
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
                if (File.ReadAllText(instOrdner[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] == buildversion[a])
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
                if ((instVersion[0] == buildversion[a]) && (instVersion[1] == ring2[a]) && (instVersion[2] == architektur2[b]))
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
                    if (File.ReadAllText(instOrdner[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] != buildversion[a])
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
        private void NewMethod4(string s, int a, FileVersionInfo testm, int b)
        {
            Directory.Move(@"Update\" + entpDir[b] + "\\chrome-bin" + "\\" + testm.FileVersion, instOrdner[b] + "\\" + testm.FileVersion);
            File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\msedge.exe", instOrdner[b] + "\\msedge.exe", true);
            File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\msedge_proxy.exe", instOrdner[b] + "\\msedge_proxy.exe", true);
            if (!Directory.Exists(instOrdner[b] + "\\updates"))
            {
                Directory.CreateDirectory(instOrdner[b] + "\\updates");
            }
            File.WriteAllText(instOrdner[b] + "\\updates\\Version.log", testm.FileVersion + "|" + ring2[(a - 1)] + "|" + s);
            Directory.Delete(@"Update\" + entpDir[b], true);
            if (checkBox1.Checked)
            {
                CheckButton();
            }
            else if (!checkBox1.Checked)
            {
                CheckButton2();
            }
        }
        private void NewMethod5(int c, int d)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(deskDir + "\\" + instOrdner[d] + ".lnk");
            link.IconLocation = applicationPath + "\\" + instOrdner[d] + "\\msedge.exe" + "," + icon[c];
            link.WorkingDirectory = applicationPath;
            link.TargetPath = applicationPath + "\\" + instOrdner[d] + " Launcher.exe";
            link.Save();
        }
        private void NewMethod6(string[] instVersion, int a, int b, int c)
        {
            Control[] buttons = Controls.Find("button" + a, true);
            Control[] buttons2 = Controls.Find("button" + b, true);
            if (instVersion[0] == buildversion[c])
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
            else if (instVersion[0] != buildversion[c])
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
        private void CheckUpdate()
        {
            Label versionLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.BottomCenter,
                Dock = DockStyle.None,
                Location = new Point(2, 30),
                Size = new Size(350, 25),
            };
            versionLabel.Font = new Font(versionLabel.Font.Name, 10F, FontStyle.Bold);
            Label infoLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.BottomCenter,
                Dock = DockStyle.None,
                Location = new Point(2, 10),
                Size = new Size(350, 20),
                Text = "Eine neue Version ist verfügbar"
            };
            infoLabel.Font = new Font(infoLabel.Font.Name, 8.75F);
            Label downLabel = new Label
            {
                Location = new Point(140, 60),
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Size = new Size(100, 23),
                Text = "Jetzt Updaten"
            };
            Button laterButton = new Button
            {
                Location = new Point(245, 60),
                Text = "Nein",
                Size = new Size(40, 23),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            Button updateButton = new Button
            {
                Location = new Point(290, 60),
                Text = "Ja",
                Size = new Size(40, 23),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            GroupBox groupBoxupdate = new GroupBox
            {
                Location = new Point(12, 290),
                Size = new Size(355, 90),
                BackColor = Color.Aqua
            };
            groupBoxupdate.Controls.Add(updateButton);
            groupBoxupdate.Controls.Add(laterButton);
            groupBoxupdate.Controls.Add(downLabel);
            groupBoxupdate.Controls.Add(infoLabel);
            groupBoxupdate.Controls.Add(versionLabel);
            updateButton.Click += new EventHandler(UpdateButton_Click);
            laterButton.Click += new EventHandler(LaterButton_Click);
            if (culture1.Name != "de-DE")
            {
                infoLabel.Text = "A new version is available";
                laterButton.Text = "No";
                updateButton.Text = "Yes";
                downLabel.Text = "Update now";
            }
            void LaterButton_Click(object sender, EventArgs e)
            {
                Size = new Size(396, 342);
                groupBoxupdate.Dispose();
                Controls.Remove(groupBoxupdate);
                groupBox3.Enabled = true;
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                var request = (WebRequest)HttpWebRequest.Create("https://github.com/UndertakerBen/PorEdgeUpd/raw/master/Version.txt");
                var response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var version = reader.ReadToEnd();
                    versionLabel.Text = version;
                    FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Portable Edge (Chromium) Updater.exe");
                    if (Convert.ToDecimal(version) > Convert.ToDecimal(testm.FileVersion))
                    {
                        Controls.Add(groupBoxupdate);
                        Size = new Size(396, 432);
                        groupBox3.Enabled = false;
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {

            }
            void UpdateButton_Click(object sender, EventArgs e)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request2 = (WebRequest)HttpWebRequest.Create("https://github.com/UndertakerBen/PorEdgeUpd/raw/master/Version.txt");
                var response2 = request2.GetResponse();
                using (StreamReader reader = new StreamReader(response2.GetResponseStream()))
                {
                    var version = reader.ReadToEnd();
                    reader.Close();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (WebClient myWebClient2 = new WebClient())
                    {
                        myWebClient2.DownloadFile($"https://github.com/UndertakerBen/PorEdgeUpd/releases/download/v{version}/Portable.Edge.Chromium.Updater.v{version}.7z", @"Portable.Edge.Chromium.Updater.v" + version + ".7z");
                    }
                    File.AppendAllText(@"Update.cmd", "@echo off" + "\n" +
                        "timeout /t 2 /nobreak" + "\n" +
                        "\"" + applicationPath + "\\Bin\\7zr.exe\" e \"" + applicationPath + "\\Portable.Edge.Chromium.Updater.v" + version + ".7z\" -o\"" + applicationPath + "\" \"Portable Edge (Chromium) Updater.exe\"" + " -y\n" +
                        "call cmd /c Start /b \"\" " + "\"" + applicationPath + "\\Portable Edge (Chromium) Updater.exe\"\n" +
                        "del /f /q \"" + applicationPath + "\\Portable.Edge.Chromium.Updater.v" + version + ".7z\"\n" +
                        "del /f /q \"" + applicationPath + "\\Update.cmd\" && exit\n" +
                        "exit\n");

                    string arguments = " /c call Update.cmd";
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = arguments;
                    process.Start();
                    Close();
                }
            }
        }
    }
}