using System;
using System.Windows.Forms;
using System.Media;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class MsgBox : Form
    {
        Action<int> actionToRun;

        public MsgBox()
        {
            InitializeComponent();
        }

        private void MsgBox_Load(object sender, EventArgs e)
        {
            SystemSounds.Asterisk.Play();
        }

        public void setData(Action<int> action, string title, string text, string formTitle, string button1Text, string button2Text)
        {
            this.Text = formTitle;
            label1.Text = title;
            label2.Text = text;
            button1.Text = button1Text;
            button2.Text = button2Text;
            actionToRun = action;

            if (button1Text == "")
            {
                button1.Visible = false;
            }

            if (button2Text == "")
            {
                button2.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (actionToRun != null)
            {
                actionToRun(1);
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (actionToRun != null)
            {
                actionToRun(2);
            }

            this.Close();
        }
    }
}
