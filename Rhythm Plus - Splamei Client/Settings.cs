using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class Settings : Form
    {
        public Form1 form;

        public bool starting = true;

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = form.enabledRP;
            checkBox2.Checked = form.showTitleOfMaps;
            checkBox3.Checked = form.directLinkRP;

            checkBox4.Checked = form.retainWinSize;
            checkBox5.Checked = form.fullscreen;

            trackBar1.Value = form.discordRpRefresh;

            checkBox6.Checked = form.enabledExtensions;

            label5.Text = $"Discord RP Refresh Time ({trackBar1.Value}s)";

            starting = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (checkBox5.Checked)
            { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/playFullScreen.dat", "1"); }
            else
            { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/playFullScreen.dat", "0"); }

            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/discordRpRefresh.dat", trackBar1.Value.ToString());

            form.setSettings(checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked, trackBar1.Value);
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

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked && !starting)
            {
                form.toggleFullscreen(true);
                //if (MessageBox.Show($"If you put the client in full screen, you'll need to press 'Left Control + S' at the same time for at least a seccond to open settings and change settings and to go out of full screen.\n\nDo you still want to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                //{
                //    checkBox5.Checked = false;
                //}
            }
            else if (starting)
            {
                form.toggleFullscreen(false);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label5.Text = $"Discord RP Refresh Time ({trackBar1.Value}s)";
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (!starting)
            {
                if (checkBox6.Checked)
                {
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enableExtensions.dat", "1");

                    MessageBox.Show("By adding extensions to the client, you may allow the extension developer to access your Rhythm Plus account or game. Please be carefull with what extensions you add\n\nYou'll need to restart the client to apply this change", "Extensions warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enableExtensions.dat", "0"); }
            }
        }
    }
}
