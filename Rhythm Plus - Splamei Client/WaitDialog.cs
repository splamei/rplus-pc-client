using System;
using System.Windows.Forms;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class WaitDialog : Form
    {
        public int timer;

        public static Label status;

        private Action canceledAction = null;

        public WaitDialog()
        {
            InitializeComponent();

            status = label1;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer += 1;
            if (timer >= 5)
            {
                this.Text = "Rhythm Plus - Splamei Client (Busy for " + TimeSpan.FromSeconds(timer) + ")";
            }
        }

        public static void updateStatus(string text)
        {
            status.Text = text;
        }

        public void updateText(string text)
        {
            updateStatus(text);
        }

        private void WaitDialog_Load(object sender, EventArgs e)
        {

        }

        public void allowCanceling(Action action, bool canCancel)
        {
            button1.Enabled = canCancel;

            canceledAction = action;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (canceledAction != null) { canceledAction(); this.Close(); }
        }
    }
}
