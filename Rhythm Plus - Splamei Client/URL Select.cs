using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class URL_Select : Form
    {
        public Action<string> action;

        public URL_Select()
        {
            InitializeComponent();
        }

        private void URL_Select_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.StartsWith("https://rhythm-plus.com"))
            {
                action(textBox1.Text);
                this.Close();
            }
            else
            {
                textBox1.Text = "https://rhythm-plus.com";
                MessageBox.Show("The URL you provided isn't a valid Rhythm Plus URL. Please insert a URL starting with 'https://rhythm-plus.com'", "Invalid URL", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
