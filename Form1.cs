using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
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
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                NewMethod(0, 0, 0, 5, 18, 17, 1);
            }
            else if (!checkBox1.Checked)
            {
                NewMethod1(0, 0, 1);
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                NewMethod(1, 1, 0, 6, 20, 19, 2);
            }
            if (!checkBox1.Checked)
            {
                NewMethod1(1, 0, 2);
            }
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                NewMethod(2, 2, 0, 7, 22, 21, 3);
            }
            else if (!checkBox1.Checked)
            {
                NewMethod1(2, 0, 3);
            }
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                NewMethod(3, 3, 0, 8, 24, 23, 4);
            }
            else if (!checkBox1.Checked)
            {
                NewMethod1(3, 0, 4);
            }
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                NewMethod(0, 4, 1, 4, 15, 16, 5);
            }
            else if (!checkBox1.Checked)
            {
                NewMethod1(0, 1, 5);
            }
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                NewMethod(1, 5, 1, 3, 13, 14, 6);
            }
            else if (!checkBox1.Checked)
            {
                NewMethod1(1, 1, 6);
            }
        }
        private void Button7_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                NewMethod(2, 6, 1, 1, 11, 12, 7);
            }
            else if (!checkBox1.Checked)
            {
                NewMethod1(2, 1, 7);
            }
        }
        private void Button8_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                NewMethod(3, 7, 1, 2, 27, 28, 8);
            }
            else if (!checkBox1.Checked)
            {
                NewMethod1(3, 1, 8);
            }
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            if ((!Directory.Exists(@"Edge Canary x86")) && (!Directory.Exists(@"Edge Dev x86")) && (!Directory.Exists(@"Edge Beta x86")) && (!Directory.Exists(@"Edge Stable x86")))
            {
                if (checkBox2.Checked)
                {
                    DownloadFile(0, 0, 0, 5, 18, 17, 1);
                    DownloadFile(1, 1, 0, 6, 20, 19, 2);
                    DownloadFile(2, 2, 0, 7, 22, 21, 3);
                    DownloadFile(3, 3, 0, 8, 24, 23, 4);
                    checkBox2.Enabled = false;
                }
            }
            NewMethod2(0, 0, 0, 5, 18, 17, 1);
            NewMethod2(1, 1, 0, 6, 20, 19, 2);
            NewMethod2(2, 2, 0, 7, 22, 21, 3);
            NewMethod2(3, 3, 0, 8, 24, 23, 4);
            if (IntPtr.Size == 8)
            {
                if ((!Directory.Exists(@"Edge Canary x64")) && (!Directory.Exists(@"Edge Dev x64")) && (!Directory.Exists(@"Edge Beta x64")) && (!Directory.Exists(@"Edge Stable x64")))
                {
                    if (checkBox3.Checked)
                    {
                        DownloadFile(0, 4, 1, 4, 15, 16, 5);
                        DownloadFile(1, 5, 1, 3, 13, 14, 6);
                        DownloadFile(2, 6, 1, 1, 11, 12, 7);
                        DownloadFile(3, 7, 1, 2, 27, 28, 8);
                        checkBox3.Enabled = false;
                    }
                }
                NewMethod2(0, 4, 1, 4, 15, 16, 5);
                NewMethod2(1, 5, 1, 3, 13, 14, 6);
                NewMethod2(2, 6, 1, 1, 11, 12, 7);
                NewMethod2(3, 7, 1, 2, 27, 28, 8);
            }
        }
        public void DownloadFile(int i, int f, int e, int a, int b, int c, int d)
        {
            WebRequest request = WebRequest.Create("https://msedge.api.cdp.microsoft.com/api/v1.1/internal/contents/Browser/namespaces/Default/names/msedge-" + ring[i] + "-win-" + architektur[e] + "/versions/" + buildversion[i] + "/files?action=GenerateDownloadInfo&foregroundPriority=true");
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
                string[] URL = responseFromServer.Substring(responseFromServer.IndexOf("MicrosoftEdge_" + architektur[e] + "_" + buildversion[i] + ".exe")).Split(new char[] { '"' });
                WebClient myWebClient = new WebClient();
                Uri uri = new Uri(URL[4]);

                using (webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    try
                    {
                        webClient.DownloadFileAsync(uri, "MicrosoftEdge_" + architektur[e] + "_" + buildversion[i] + "_" + ring[i] + ".exe", a + "|" + b + "|" + c + "|" + d + "|" + "MicrosoftEdge_" + architektur[e] + "_" + buildversion[i] + "_" + ring[i] + ".exe" + "|" + architektur2[e] + "|" + i + "|" + f);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string[] i = e.UserState.ToString().Split(new char[] { '|' });
            Control[] progressBars = Controls.Find("progressBar" + i[0], true);
            Control[] buttons = Controls.Find("button" + i[3], true);
            Control[] label1 = Controls.Find("label" + i[1], true);
            Control[] label2 = Controls.Find("label" + i[2], true);
            if (buttons.Length > 0)
            {
                Button button = (Button)buttons[0];
                button.BackColor = Color.Orange;
            }
            if (progressBars.Length > 0)
            {
                ProgressBar progressBar = (ProgressBar)progressBars[0];
                progressBar.Visible = true;
                progressBar.Value = e.ProgressPercentage;
            }
            if (label1.Length > 0)
            {
                Label label = (Label)label1[0];
                label.Visible = true;
                label.Text = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
            }
            if (label2.Length > 0)
            {
                Label label3 = (Label)label2[0];
                label3.Visible = true;
                label3.Text = e.ProgressPercentage.ToString() + "%";
            }
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            string[] i = e.UserState.ToString().Split(new char[] { '|' });
            int b = int.Parse(i[1]);
            int d = int.Parse(i[7]);
            Control[] labels = Controls.Find("label" + b, true);
            Label label = (Label)labels[0];
            if (e.Cancelled == true)
            {
                MessageBox.Show("Download has been canceled.");
            }
            else
            {
                if (labels.Length > 0)
                {
                    label.Text = culture1.Name != "de-DE" ? "Unpacking" : "Entpacken";
                    string arguments = " x " + i[4] + " -o" + @"Update\" + entpDir[d] + " -y";
                    Process process = new Process();
                    process.StartInfo.FileName = @"Bin\7zr.exe";
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                    process.StartInfo.Arguments = arguments;
                    process.Start();
                    process.WaitForExit();
                    process.StartInfo.Arguments = " x " + @"Update\" + entpDir[d] + "\\MSEDGE.7z -o" + @"Update\" + entpDir[d] + " -y";
                    process.Start();
                    process.WaitForExit();
                    int a = int.Parse(i[3]);
                    if ((File.Exists(@"Update\" + entpDir[d] + "\\chrome-bin\\msedge.exe")) && (File.Exists(instOrdner[d] + "\\updates\\Version.log")))
                    {
                        string[] instVersion = File.ReadAllText(instOrdner[d] + "\\updates\\Version.log").Split(new char[] { '|' });
                        FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[d] + "\\chrome-bin\\msedge.exe");
                        if (checkBox1.Checked)
                        {
                            if (testm.FileVersion != instVersion[0])
                            {
                                if (Directory.Exists(instOrdner[d] + "\\" + instVersion[0]))
                                {
                                    Directory.Delete(instOrdner[d] + "\\" + instVersion[0], true);
                                }
                                Thread.Sleep(2000);
                                NewMethod4(i, a, testm, d);
                            }
                            else if ((testm.FileVersion == instVersion[0]) && (checkBox4.Checked))
                            {
                                if (Directory.Exists(instOrdner[d] + "\\" + instVersion[0]))
                                {
                                    Directory.Delete(instOrdner[d] + "\\" + instVersion[0], true);
                                }
                                Thread.Sleep(2000);
                                NewMethod4(i, a, testm, d);
                            }
                        }
                        else if (!checkBox1.Checked)
                        {
                            if (Directory.Exists(instOrdner[d] + "\\" + instVersion[0]))
                            {
                                Directory.Delete(instOrdner[d] + "\\" + instVersion[0], true);
                            }
                            Thread.Sleep(2000);
                            NewMethod4(i, a, testm, d);
                        }
                    }
                    else
                    {
                        if (!Directory.Exists(instOrdner[d]))
                        {
                            Directory.CreateDirectory(instOrdner[d]);
                        }
                        NewMethod4(i, a, FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[d] + "\\chrome-bin\\msedge.exe"), d);
                    }
                }
            }
            int c = int.Parse(i[6]);
            if (checkBox5.Checked)
            {
                if (!File.Exists(deskDir + "\\" + instOrdner[d] + ".lnk"))
                {
                    NewMethod5(c, d);
                }
            }
            else if (File.Exists(deskDir + "\\" + instOrdner[d] + ".lnk") && (instOrdner[d] == "Edge"))
            {
                NewMethod5(c, d);
            }
            if (!File.Exists(@instOrdner[d] + " Launcher.exe"))
            {
                File.Copy(@"Bin\Launcher\" + instOrdner[d] + " Launcher.exe", @instOrdner[d] + " Launcher.exe");
            }
            File.Delete(i[4]);
            label.Text = culture1.Name != "de-DE" ? "Unpacked" : "Entpackt";
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
        private void NewMethod(int a, int b, int c, int d, int e, int f, int g)
        {
            if (File.Exists(@instOrdner[b] + "\\updates\\Version.log"))
            {
                if (File.ReadAllText(instOrdner[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] == buildversion[a])
                {
                    if (checkBox4.Checked)
                    {
                        DownloadFile(a, b, c, d, e, f, g);
                    }
                    else
                    {
                        Message1();
                    }
                }
                else
                {
                    DownloadFile(a, b, c, d, e, f, g);
                }
            }
            else
            {
                DownloadFile(a, b, c, d, e, f, g);
            }
        }
        private void NewMethod1(int a, int b, int c)
        {
            if (File.Exists(@"Edge\updates\Version.log"))
            {
                string[] instVersion = File.ReadAllText(@"Edge\updates\Version.log").Split(new char[] { '|' });
                if ((instVersion[0] == buildversion[a]) && (instVersion[1] == ring2[a]) && (instVersion[2] == architektur2[b]))
                {
                    if (checkBox4.Checked)
                    {
                        DownloadFile(a, 8, b, 5, 18, 17, c);
                    }
                    else
                    {
                        Message1();
                    }
                }
                else
                {
                    DownloadFile(a, 8, b, 5, 18, 17, c);
                }
            }
            else
            {
                DownloadFile(a, 8, b, 5, 18, 17, c);
            }
        }
        private void NewMethod2(int a, int b, int c, int d, int e, int f, int g)
        {
            if (Directory.Exists(instOrdner[b]))
            {
                if (File.Exists(instOrdner[b] + "\\updates\\Version.log"))
                {
                    if (File.ReadAllText(instOrdner[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] != buildversion[a])
                    {
                        DownloadFile(a, b, c, d, e, f, g);
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
        private void NewMethod4(string[] i2, int a, FileVersionInfo testm, int b)
        {
            Directory.Move(@"Update\" + entpDir[b] + "\\chrome-bin" + "\\" + testm.FileVersion, instOrdner[b] + "\\" + testm.FileVersion);
            File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\msedge.exe", instOrdner[b] + "\\msedge.exe", true);
            File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\msedge_proxy.exe", instOrdner[b] + "\\msedge_proxy.exe", true);
            if (!Directory.Exists(instOrdner[b] + "\\updates"))
            {
                Directory.CreateDirectory(instOrdner[b] + "\\updates");
            }
            File.WriteAllText(instOrdner[b] + "\\updates\\Version.log", testm.FileVersion + "|" + ring2[(a - 1)] + "|" + i2[5]);
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
            }
            else
            {
                toolTip.SetToolTip(button, String.Empty);
            }
        }
    }
}