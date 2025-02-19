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
    public partial class WaitDialog : Form
    {
        public int timer;

        public static Label status;

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
    }
}
