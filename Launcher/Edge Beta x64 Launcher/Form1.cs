using System.IO;
using System.Windows.Forms;

namespace Edge_Beta_x64_Launcher
{
    public partial class Form1 : Form 
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Button1_Click(object sender, System.EventArgs e)
        {
            if (radioButton1.Checked)
            {
                File.WriteAllText(@"Edge Beta x64\Profile.txt", "--user-data-dir=\"profile\"");
                this.Close();
            }
            if (radioButton2.Checked)
            {
                File.WriteAllText(@"Edge Beta x64\Profile.txt", "--user-data-dir=\"Edge Beta x64\\profile\"");
                this.Close();
            }
            if (radioButton3.Checked)
            {
                File.WriteAllText(@"Edge Beta x64\Profile.txt", "");
                this.Close();
            }
        }
    }
}
