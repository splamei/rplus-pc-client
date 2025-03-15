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
    public partial class MsgBox : Form
    {
        public MsgBox()
        {
            InitializeComponent();
        }

        private void MsgBox_Load(object sender, EventArgs e)
        {

        }

        public void setData(Action<int> action, string title, string text, string formTitle, string button1Text, string button2Text)
        {
            this.Text = formTitle;
            label1.Text = text;
            button1.Text = button1Text;
            button2.Text = button2Text;
            button1.Click += (sender, e) => { action(1); this.Close(); };
            button2.Click += (sender, e) => { action(2); this.Close(); };
        }
    }
}
