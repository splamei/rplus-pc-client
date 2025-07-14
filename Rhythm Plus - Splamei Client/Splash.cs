using System;
using System.Windows.Forms;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Splash_Load(object sender, EventArgs e)
        {
            label2.Text = $"Version {Application.ProductVersion}";
        }
    }
}
