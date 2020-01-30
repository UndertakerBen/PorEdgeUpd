using System.IO;
using System.Windows.Forms;

namespace Edge_Launcher
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
                File.WriteAllText(@"Edge\Profile.txt", "--user-data-dir=\"profile\"");
                this.Close();
            }
            if (radioButton2.Checked)
            {
                File.WriteAllText(@"Edge\Profile.txt", "--user-data-dir=\"Edge\\profile\"");
                this.Close();
            }
            if (radioButton3.Checked)
            {
                File.WriteAllText(@"Edge\Profile.txt", "");
                this.Close();
            }
        }
    }
}
