using System;
using System.Windows.Forms;
using System.Media;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class Error : Form
    {
        public string errorDebug = "";
        public bool shouldClose = true;

        public Error()
        {
            InitializeComponent();
        }

        private void Error_Load(object sender, EventArgs e)
        {
            SystemSounds.Hand.Play();

            if (errorDebug == "")
            {
                button2.Enabled = false;
                button3.Enabled = false;
                label3.Visible = true;
            }

            if (!shouldClose)
            {
                label2.Text = "An error occured while running the client.\r\nThe client will continue to run. We're\r\nsorry for the issue.";
            }
        }

        private void Error_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (shouldClose)
            {
                Environment.Exit(1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Error infomation:\n\n" + errorDebug, "Error - Debug Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)errorDebug);

            MessageBox.Show("The error has been added to the clipboard", "Rhythm Plus - Splamei Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
