using System;
using System.Windows.Forms;
using Rhythm_Plus___Splamei_Client.Save_System;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class Settings : Form
    {
        public Form1 form;
        public SaveManager saveManager = new SaveManager();

        public bool starting = true;

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            saveManager.loadData();

            checkBox1.Checked = form.enabledRP;
            checkBox2.Checked = form.showTitleOfMaps;
            checkBox3.Checked = form.directLinkRP;
            checkBox7.Checked = form.showStatsinRPC;

            checkBox4.Checked = form.retainWinSize;
            checkBox5.Checked = form.fullscreen;

            trackBar1.Value = form.discordRpRefresh;

            checkBox6.Checked = form.enabledExtensions;

            checkBox8.Checked = saveManager.getInt("v2Page") == 1;

            if (form.showMenu == "Not in game")
            {
                comboBox1.SelectedIndex = 1;
            }
            else if (form.showMenu == "Not in fullscreen")
            {
                comboBox1.SelectedIndex = 2;
            }
            else if (form.showMenu == "Always")
            {
                comboBox1.SelectedIndex = 3;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }

            trackBar2.Value = int.Parse(Math.Round(form.webView2.ZoomFactor * 10).ToString());
            label9.Text = $"Current Zoom ({Math.Round((trackBar2.Value / 10f) * 100)}%)";

            label5.Text = $"Discord RP Refresh Time ({trackBar1.Value}s)";

            starting = false;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveManager.setString("discordRpRefresh", trackBar1.Value.ToString());

            if (comboBox1.SelectedIndex == 1)
            {
                form.showMenu = "Not in game";
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                form.showMenu = "Not in fullscreen";
                Console.WriteLine("not in full screen");
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                form.showMenu = "Always";
            }
            else
            {
                form.showMenu = "Only in settings";
            }
            saveManager.setString("showMenuIn", form.showMenu);

            form.showStatsinRPC = checkBox7.Checked;

            if (checkBox8.Checked)
            {
                saveManager.setString("v2Page", "1");
            }
            else
            {
                saveManager.setString("v2Page", "0");
            }

            form.setSettings(checkBox1.Checked, checkBox2.Checked, checkBox3.Checked, checkBox4.Checked, trackBar1.Value);
            saveManager.saveData();
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
            else if (!starting)
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
                    saveManager.setString("enableExtensions", "1");

                    MessageBox.Show("By adding extensions to the client, you may allow the extension developer to access your Rhythm Plus account or game. Please be carefull with what extensions you add\n\nYou'll need to restart the client to apply this change", "Extensions warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                { saveManager.setString("enableExtensions", "0"); }
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            try
            {
                form.webView2.ZoomFactor = trackBar2.Value / 10f;
                label9.Text = $"Current Zoom ({Math.Round((trackBar2.Value / 10f) * 100)}%)";
            }
            catch (Exception ex)
            {
                Logging.logString($"Failed to set webView zoom of {trackBar2.Value / 10f}! - " + ex);
            }
        }

        public void fullscreenChanged(bool state)
        {
            starting = true;
            checkBox5.Checked = state;
            starting = false;
        }
    }
}
