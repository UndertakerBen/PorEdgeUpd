using System.IO;
using System.Windows.Forms;

namespace Edge_Dev_x86_Launcher
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        private void Button1_Click(object sender, System.EventArgs e)
        {
            if (radioButton1.Checked)
            {
                File.WriteAllText(@"Edge Dev x86\Profile.txt", "--user-data-dir=\"profile\"");
                this.Close();
            }
            if (radioButton2.Checked)
            {
                File.WriteAllText(@"Edge Dev x86\Profile.txt", "--user-data-dir=\"Edge Dev x86\\profile\"");
                this.Close();
            }
            if (radioButton3.Checked)
            {
                File.WriteAllText(@"Edge Dev x86\Profile.txt", "");
                this.Close();
            }
        }
    }
}
