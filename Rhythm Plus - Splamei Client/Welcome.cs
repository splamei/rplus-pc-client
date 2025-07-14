using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using(Process.Start("https://github.com/splamei/rplus-pc-client")) { }
        }
    }
}
