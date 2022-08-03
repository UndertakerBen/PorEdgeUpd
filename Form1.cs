using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        private static readonly string[] ring = new string[4] { "Canary", "Dev", "Beta", "Stable" };
        private static readonly string[] ring2 = new string[8] { "Canary", "Developer", "Beta", "Stable", "Canary", "Developer", "Beta", "Stable" };
        private static readonly string[] buildversion = new string[8];
        private static readonly string[] architektur = new string[2] { "X86", "X64" };
        private static readonly string[] architektur2 = new string[2] { "x86", "x64" };
        private static readonly string[] instOrdner = new string[9] { "Edge Canary x86", "Edge Dev x86", "Edge Beta x86", "Edge Stable x86", "Edge Canary x64", "Edge Dev x64", "Edge Beta x64", "Edge Stable x64", "Edge" };
        private static readonly string[] entpDir = new string[9] { "Canary86", "Dev86", "Beta86", "Stable86", "Canary64", "Dev64", "Beta64", "Stable64", "Single" };
        private static readonly string[] icon = new string[4] { "4", "8", "9", "0" };
        WebClient webClient;
        private readonly string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        private readonly string applicationPath = Application.StartupPath;
        private readonly ToolTip toolTip = new ToolTip();
        readonly string policyVMenu;
        readonly ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
        readonly ToolStripMenuItem SubPVMenu = new ToolStripMenuItem();
        

        public Form1()
        {
            buildversion[0] = GetEdgeVersion.EdgeVersion("Canary", "X86");
            buildversion[4] = GetEdgeVersion.EdgeVersion("Canary", "X64");
            buildversion[1] = GetEdgeVersion.EdgeVersion("Dev", "X86");
            buildversion[5] = GetEdgeVersion.EdgeVersion("Dev", "X64");
            buildversion[2] = GetEdgeVersion.EdgeVersion("Beta", "X86");
            buildversion[6] = GetEdgeVersion.EdgeVersion("Beta", "X64");
            buildversion[3] = GetEdgeVersion.EdgeVersion("Stable", "X86");
            buildversion[7] = GetEdgeVersion.EdgeVersion("Stable", "X64");

            InitializeComponent();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create("https://www.microsoft.com/en-us/edge/business/download");
                HttpWebResponse response2 = (HttpWebResponse)request2.GetResponse();
                if (response2.StatusCode == HttpStatusCode.OK)
                {
                    using (StreamReader reader = new StreamReader(response2.GetResponseStream()))
                    {
                        string texting = reader.ReadToEnd();
                        string text = texting.ToLower().Replace("&quot;", "\"");
                        reader.Close();
                        if (text.Contains("\"product\": \"policy"))
                        {
                            string[] splittext = text.Substring(text.IndexOf("\"product\": \"policy")).Replace("\"product\": \"policy", "|\"product\": \"policy").ToString().Split(new char[] { '|', '>' }, 3)[1].Replace("\"releaseid", "|\"releaseid").Split(new char[] { '|' });
                            for (int i = 0; i < splittext.GetLength(0); i++)
                            {
                                if (splittext[i].Contains("productversion") & splittext[i].Contains("artifactname\": \"zip\","))
                                {
                                    string productVersion = splittext[i].Substring(splittext[i].IndexOf("productversion\": \"")).Split(new char[] { '"' }, 4)[2];
                                    string productVShort = productVersion.Split(new char[] { '.' }, 2)[0];
                                    string productURL = splittext[i].Substring(splittext[i].IndexOf("\"location\":")).Split(new char[] { '"' }, 5)[3];
                                    if (policyVMenu != productVShort)
                                    {
                                        SubPVMenu = new ToolStripMenuItem(productVShort);
                                        SubPVMenu.Font = new Font("Segoe UI", 9F);
                                        policyTemplatesDownloadToolStripMenuItem.DropDownItems.Add(SubPVMenu);
                                        var tb = SubPVMenu.DropDownItems.Add(productVersion);
                                        tb.ToolTipText = productURL;
                                        tb.Click += new EventHandler(Download_Click);
                                        async void Download_Click(object sender, EventArgs e)
                                        {
                                            await DownloadADMX(productURL, productVersion, "zip");
                                        }

                                    }
                                    else if (policyVMenu == productVShort)
                                    {
                                        var tb2 = SubPVMenu.DropDownItems.Add(productVersion);
                                        tb2.ToolTipText = productURL;
                                        tb2.Click += new EventHandler(Download2_Click);
                                        async void Download2_Click(object sender, EventArgs e)
                                        {
                                            await DownloadADMX(productURL, productVersion, "zip");
                                        }
                                    }
                                    policyVMenu = productVShort;

                                }
                                else if (splittext[i].Contains("productversion") & splittext[i].Contains("artifactname\": \"cab\","))
                                {
                                    string productVersion = splittext[i].Substring(splittext[i].IndexOf("productversion\": \"")).Split(new char[] { '"' }, 4)[2];
                                    string productVShort = productVersion.Split(new char[] { '.' }, 2)[0];
                                    string productURL = splittext[i].Substring(splittext[i].IndexOf("\"location\":")).Split(new char[] { '"' }, 5)[3];
                                    if (policyVMenu != productVShort)
                                    {
                                        SubPVMenu = new ToolStripMenuItem(productVShort);
                                        SubPVMenu.Font = new Font("Segoe UI", 9F);
                                        policyTemplatesDownloadToolStripMenuItem.DropDownItems.Add(SubPVMenu);
                                        var tb = SubPVMenu.DropDownItems.Add(productVersion);
                                        tb.ToolTipText = productURL;
                                        tb.Click += new EventHandler(Download_Click);
                                        async void Download_Click(object sender, EventArgs e)
                                        {
                                            await DownloadADMX(productURL, productVersion, "cab");
                                        }

                                    }
                                    else if (policyVMenu == productVShort)
                                    {
                                        var tb2 = SubPVMenu.DropDownItems.Add(productVersion);
                                        tb2.ToolTipText = productURL;
                                        tb2.Click += new EventHandler(Download2_Click);
                                        async void Download2_Click(object sender, EventArgs e)
                                        {
                                            await DownloadADMX(productURL, productVersion, "cab");
                                        }
                                    }
                                    policyVMenu = productVShort;

                                }
                            }
                        
                        }
                        else if (text.Contains("\"product\":\"policy"))
                        {
                            string[] splittext = text.Substring(text.IndexOf("\"product\":\"policy")).Replace("\"product\":\"policy", "|\"product\":\"policy").ToString().Split(new char[] { '|', '>' }, 3)[1].Replace("\"releaseid", "|\"releaseid").Split(new char[] { '|' });
                            for (int i = 0; i < splittext.GetLength(0); i++)
                            {
                                if (splittext[i].Contains("productversion") & splittext[i].Contains("artifactname\":\"zip\","))
                                {
                                    string productVersion = splittext[i].Substring(splittext[i].IndexOf("productversion\":\"")).Split(new char[] { '"' }, 4)[2];
                                    string productVShort = productVersion.Split(new char[] { '.' }, 2)[0];
                                    string productURL = splittext[i].Substring(splittext[i].IndexOf("\"location\":")).Split(new char[] { '"' }, 5)[3];
                                    if (policyVMenu != productVShort)
                                    {
                                        SubPVMenu = new ToolStripMenuItem(productVShort);
                                        SubPVMenu.Font = new Font("Segoe UI", 9F);
                                        policyTemplatesDownloadToolStripMenuItem.DropDownItems.Add(SubPVMenu);
                                        var tb = SubPVMenu.DropDownItems.Add(productVersion);
                                        tb.ToolTipText = productURL;
                                        tb.Click += new EventHandler(Download_Click);
                                        async void Download_Click(object sender, EventArgs e)
                                        {
                                            await DownloadADMX(productURL, productVersion, "zip");
                                        }

                                    }
                                    else if (policyVMenu == productVShort)
                                    {
                                        var tb2 = SubPVMenu.DropDownItems.Add(productVersion);
                                        tb2.ToolTipText = productURL;
                                        tb2.Click += new EventHandler(Download2_Click);
                                        async void Download2_Click(object sender, EventArgs e)
                                        {
                                            await DownloadADMX(productURL, productVersion, "zip");
                                        }
                                    }
                                    policyVMenu = productVShort;

                                }
                                else if (splittext[i].Contains("productversion") & splittext[i].Contains("artifactname\":\"cab\","))
                                {
                                    string productVersion = splittext[i].Substring(splittext[i].IndexOf("productversion\":\"")).Split(new char[] { '"' }, 4)[2];
                                    string productVShort = productVersion.Split(new char[] { '.' }, 2)[0];
                                    string productURL = splittext[i].Substring(splittext[i].IndexOf("\"location\":")).Split(new char[] { '"' }, 5)[3];
                                    if (policyVMenu != productVShort)
                                    {
                                        SubPVMenu = new ToolStripMenuItem(productVShort);
                                        SubPVMenu.Font = new Font("Segoe UI", 9F);
                                        policyTemplatesDownloadToolStripMenuItem.DropDownItems.Add(SubPVMenu);
                                        var tb = SubPVMenu.DropDownItems.Add(productVersion);
                                        tb.ToolTipText = productURL;
                                        tb.Click += new EventHandler(Download_Click);
                                        async void Download_Click(object sender, EventArgs e)
                                        {
                                            await DownloadADMX(productURL, productVersion, "cab");
                                        }

                                    }
                                    else if (policyVMenu == productVShort)
                                    {
                                        var tb2 = SubPVMenu.DropDownItems.Add(productVersion);
                                        tb2.ToolTipText = productURL;
                                        tb2.Click += new EventHandler(Download2_Click);
                                        async void Download2_Click(object sender, EventArgs e)
                                        {
                                            await DownloadADMX(productURL, productVersion, "cab");
                                        }
                                    }
                                    policyVMenu = productVShort;

                                }
                            }
                        }
                        reader.Close();
                    }
                }
            }
            catch (WebException ex)
            {
                MessageBox.Show("Template\r\n" + ex.Message);
            }
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
            CheckUpdate();
            if ((buildversion[0] == null) || (buildversion[1] == null) || (buildversion[2] == null) || (buildversion[3] == null))
            {
                groupBox3.Enabled = false;
                button9.Enabled = false;
                checkBox1.Enabled = false;
            }
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.ProcessName.Equals("msedge"))
                {
                    MessageBox.Show(Langfile.Texts("MeassageRunning"), "Portable Edge (Chromium) Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
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
        public async Task DownloadFile(int a, int b, int c, int d)
        {
            GroupBox progressBox = new GroupBox
            {
                Location = new Point(groupBox3.Location.X, button10.Location.Y + button10.Size.Height + 5),
                Size = new Size(groupBox3.Width, 90),
                BackColor = Color.Lavender,
            };
            Label title = new Label
            {
                AutoSize = false,
                Location = new Point(5, 10),
                Size = new Size(progressBox.Size.Width-10, 25),
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
                Location = new Point(progressBox.Size.Width - 108, 35),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.BottomRight
            };
            ProgressBar progressBarneu = new ProgressBar
            {
                Location = new Point(8, 65),
                Size = new Size(progressBox.Size.Width - 18, 7)
            };
            progressBox.Controls.Add(title);
            progressBox.Controls.Add(downloadLabel);
            progressBox.Controls.Add(percLabel);
            progressBox.Controls.Add(progressBarneu);
            Controls.Add(progressBox);
            List<Task> list = new List<Task>();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://msedge.api.cdp.microsoft.com/api/v1.1/internal/contents/Browser/namespaces/Default/names/msedge-" + ring[a] + "-win-" + architektur[c] + "/versions/" + buildversion[a] + "/files?action=GenerateDownloadInfo&foregroundPriority=true");
                request.Host = "msedge.api.cdp.microsoft.com";
                request.UserAgent = "Microsoft Edge Update/1.3.139.59;winhttp";
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
                            downloadLabel.Text = $"{args.BytesReceived / 1024d / 1024d:0.00} MB's / {args.TotalBytesToReceive / 1024d / 1024d:0.00} MB's";
                            percLabel.Text = $"{args.ProgressPercentage}%";
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
                                downloadLabel.Text = Langfile.Texts("downUnpstart");
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
                                if (File.Exists(applicationPath + "\\Update\\" + entpDir[b] + "\\Chrome-bin\\msedge.exe"))
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
                                else if (File.Exists(applicationPath + "\\Update\\" + entpDir[b] + "\\chrome-bin\\" + buildversion[a] + "\\msedge.exe"))
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
                                    NewMethod4(architektur2[c], d, FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[b] + "\\chrome-bin\\" + buildversion[a] + "\\msedge.exe"), b);
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
                            downloadLabel.Text = Langfile.Texts("downUnpfine");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            await Task.WhenAll(list);
            await Task.Delay(2000);
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
                        button9.Text = Langfile.Texts("Button9UAll");
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
            MessageBox.Show(Langfile.Texts("MeassageVersion"), "Portabel Edge (Chromium) Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            if (File.Exists(@"Update\" + entpDir[b] + "\\chrome-bin" + "\\" + testm.FileVersion + "\\msedge.exe"))
            {
                Directory.Move(@"Update\" + entpDir[b] + "\\chrome-bin" + "\\" + testm.FileVersion, instOrdner[b] + "\\" + testm.FileVersion);
                File.Copy(instOrdner[b] + "\\" + testm.FileVersion + "\\msedge.exe", instOrdner[b] + "\\msedge.exe", true);
                File.Copy(instOrdner[b] + "\\" + testm.FileVersion + "\\msedge_proxy.exe", instOrdner[b] + "\\msedge_proxy.exe", true);
            }
            else if (File.Exists(@"Update\" + entpDir[b] + "\\Chrome-bin\\msedge.exe"))
            {
                Directory.Move(@"Update\" + entpDir[b] + "\\chrome-bin" + "\\" + testm.FileVersion, instOrdner[b] + "\\" + testm.FileVersion);
                File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\msedge.exe", instOrdner[b] + "\\msedge.exe", true);
                File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\msedge_proxy.exe", instOrdner[b] + "\\msedge_proxy.exe", true);
            }
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
            GroupBox groupBoxupdate = new GroupBox
            {
                Location = new Point(groupBox3.Location.X, button10.Location.Y + button10.Size.Height + 5),
                Size = new Size(groupBox3.Width, 90),
                BackColor = Color.Aqua
            };
            Label versionLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.BottomCenter,
                Dock = DockStyle.None,
                Location = new Point(2, 30),
                Size = new Size(groupBoxupdate.Width - 4, 25),
            };
            versionLabel.Font = new Font(versionLabel.Font.Name, 10F, FontStyle.Bold);
            Label infoLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.BottomCenter,
                Dock = DockStyle.None,
                Location = new Point(2, 10),
                Size = new Size(groupBoxupdate.Width - 4, 20),
            };
            infoLabel.Font = new Font(infoLabel.Font.Name, 8.75F);
            Label downLabel = new Label
            {
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Size = new Size(100, 23),
            };
            Button laterButton = new Button
            {
                Size = new Size(50, 23),
                BackColor = Color.FromArgb(224, 224, 224)
               
            };
            Button updateButton = new Button
            {
                Location = new Point(groupBoxupdate.Width - Width - 10, 60),
                Text = "Ja",
                Size = new Size(50, 23),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            updateButton.Location = new Point(groupBoxupdate.Width - updateButton.Width - 10, 60);
            laterButton.Location = new Point(updateButton.Location.X - laterButton.Width - 5, 60);
            downLabel.Location = new Point(laterButton.Location.X - downLabel.Width - 20, 60);
            groupBoxupdate.Controls.Add(updateButton);
            groupBoxupdate.Controls.Add(laterButton);
            groupBoxupdate.Controls.Add(downLabel);
            groupBoxupdate.Controls.Add(infoLabel);
            groupBoxupdate.Controls.Add(versionLabel);
            updateButton.Click += new EventHandler(UpdateButton_Click);
            laterButton.Click += new EventHandler(LaterButton_Click);
            infoLabel.Text = Langfile.Texts("infoLabel");
            laterButton.Text = Langfile.Texts("laterButton");
            updateButton.Text = Langfile.Texts("updateButton");
            downLabel.Text = Langfile.Texts("downLabel");
            void LaterButton_Click(object sender, EventArgs e)
            {
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
                    FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Portable Edge (Chromium) Updater.exe");
                    versionLabel.Text = testm.FileVersion + "  >>> " + version;
                    if (Convert.ToInt32(version.Replace(".", "")) > Convert.ToInt32(testm.FileVersion.Replace(".", "")))
                    {
                        Controls.Add(groupBoxupdate);
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
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                var request = (WebRequest)HttpWebRequest.Create("https://github.com/UndertakerBen/PorEdgeUpd/raw/master/Launcher/Version.txt");
                var response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var version = reader.ReadToEnd();
                    FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Bin\\Launcher\\Edge Launcher.exe");
                    if (Convert.ToInt32(version.Replace(".", "")) > Convert.ToInt32(testm.FileVersion.Replace(".", "")))
                    {
                        reader.Close();
                        try
                        {
                            using (WebClient myWebClient2 = new WebClient())
                            {
                                myWebClient2.DownloadFile("https://github.com/UndertakerBen/PorEdgeUpd/raw/master/Launcher/Launcher.7z", @"Launcher.7z");
                            }
                            string arguments = " x " + @"Launcher.7z" + " -o" + @"Bin\\Launcher" + " -y";
                            Process process = new Process();
                            process.StartInfo.FileName = @"Bin\7zr.exe";
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            process.StartInfo.Arguments = arguments;
                            process.Start();
                            process.WaitForExit();
                            File.Delete(@"Launcher.7z");
                            foreach (string launcher in instOrdner)
                            {
                                if (File.Exists(launcher + " Launcher.exe"))
                                {
                                    FileVersionInfo binLauncher = FileVersionInfo.GetVersionInfo(applicationPath + "\\Bin\\Launcher\\" + launcher + " Launcher.exe");
                                    FileVersionInfo istLauncher = FileVersionInfo.GetVersionInfo(applicationPath + "\\" + launcher + " Launcher.exe");
                                    if (Convert.ToDecimal(binLauncher.FileVersion) > Convert.ToDecimal(istLauncher.FileVersion))
                                    {
                                        File.Copy(@"bin\\Launcher\\" + launcher + " Launcher.exe", launcher + " Launcher.exe", true);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {

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
        public async Task DownloadADMX(string URL, string version, string extension)
        {
            GroupBox progressBox = new GroupBox
            {
                Location = new Point(groupBox3.Location.X, button10.Location.Y + button10.Size.Height + 5),
                Size = new Size(groupBox3.Width, 90),
                BackColor = Color.Lavender,
            };
            Label title = new Label
            {
                AutoSize = false,
                Location = new Point(5, 10),
                Size = new Size(progressBox.Size.Width - 10, 25),
                Text = "(" + version + ")MicrosoftEdgePolicyTemplates." + extension,
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
                Location = new Point(progressBox.Size.Width - 108, 35),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.BottomRight
            };
            ProgressBar progressBarneu = new ProgressBar
            {
                Location = new Point(8, 65),
                Size = new Size(progressBox.Size.Width - 18, 7)
            };
            progressBox.Controls.Add(title);
            progressBox.Controls.Add(downloadLabel);
            progressBox.Controls.Add(percLabel);
            progressBox.Controls.Add(progressBarneu);
            Controls.Add(progressBox);
            List<Task> list = new List<Task>();
            try
            {
                    WebClient myWebClient = new WebClient();
                    Uri uri = new Uri(URL);
                using (webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged += (o, args) =>
                    {
                        progressBarneu.Value = args.ProgressPercentage;
                        downloadLabel.Text = $"{args.BytesReceived / 1024d / 1024d:0.00} MB's / {args.TotalBytesToReceive / 1024d / 1024d:0.00} MB's";
                        percLabel.Text = $"{args.ProgressPercentage}%";
                    };
                    webClient.DownloadFileCompleted += (o, args) =>
                    {
                        if (args.Error != null)
                        {
                            var task = webClient.DownloadFileTaskAsync(uri, "ADMX Policy Templates\\(" + version + ")MicrosoftEdgePolicyTemplates." + extension);
                            list.Add(task);
                        }
                        if (args.Cancelled == true)
                        {
                            MessageBox.Show("Download has been canceled.");
                        }
                    };
                    try
                    {
                        if (!Directory.Exists(applicationPath + "\\ADMX Policy Templates"))
                        {
                            Directory.CreateDirectory(applicationPath + "\\ADMX Policy Templates");
                        }
                        var task = webClient.DownloadFileTaskAsync(uri, "ADMX Policy Templates\\(" + version + ")MicrosoftEdgePolicyTemplates." + extension);
                        list.Add(task);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            await Task.WhenAll(list);
            await Task.Delay(2000);
            Controls.Remove(progressBox);
        }
    }
    public static class GetEdgeVersion
    {
        public static string EdgeVersion(/*string version, */string ring, string arch)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string postData = "{\"targetingAttributes\":{\"Updater\":\"MicrosoftEdgeUpdate\",}}";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://msedge.api.cdp.microsoft.com/api/v1.1/contents/Browser/namespaces/Default/names/msedge-{ring.ToLower()}-win-{arch}/versions/latest?action=select");
                request.Method = "POST";
                request.UserAgent = "Microsoft Edge Update/1.3.129.35;winhttp";
                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (dataStream = request.GetResponse().GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    string[] URL = responseFromServer.Substring(responseFromServer.IndexOf("Version\":\"")).Split(new char[] { '"' });
                    reader.Close();
                    dataStream.Close();
                    return URL[2];
                }
            }
            catch (Exception)
            {
                return "No Response";
            }
            
        }
    }
}