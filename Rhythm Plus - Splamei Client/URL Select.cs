using System;
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

                using (MsgBox msgBox = new MsgBox())
                {
                    msgBox.setData(null, "Unable to navigate", "We can't go to that URL since it's not a Rhythm Plus URL\n\nPlease make sure the URL you're trying to go to starts with\nhttps://rhythm-plus.com then try again", "Unable to navigate - Splamei Client", "OK", "");
                    msgBox.ShowDialog();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
