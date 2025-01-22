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
    public partial class Settings : Form
    {
        public Form1 form;

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = form.enabledRP;
            checkBox2.Checked = form.showTitleOfMaps;
            checkBox3.Checked = form.directLinkRP;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            form.setSettings(checkBox1.Checked, checkBox2.Checked, checkBox3.Checked);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
            }
            else
            {
                checkBox2.Enabled = false;
                checkBox3.Enabled = false;
            }
        }
    }
}
