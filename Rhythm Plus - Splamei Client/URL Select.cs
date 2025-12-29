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
            if (!textBox1.Text.StartsWith("https://") && !textBox1.Text.StartsWith("http://"))
            {
                textBox1.Text = "https://" + textBox1.Text;
            }

            if (textBox1.Text.StartsWith("https://rhythm-plus.com") || textBox1.Text.StartsWith("https://v2.rhythm-plus.com"))
            {
                action(textBox1.Text);
                this.Close();
            }
            else
            {
                using (MsgBox msgBox = new MsgBox())
                {
                    if (textBox1.Text.StartsWith("http:"))
                    {
                        msgBox.setData(null, "Unable to navigate", "We can't go to that URL since it's a http URL not a https URL.\n\nPlease replace http:// with https:// so you can keep\nyourself safe while playing!", "Unable to navigate - Splamei Client", "OK", "");
                    }
                    else
                    {
                        msgBox.setData(null, "Unable to navigate", "We can't go to that URL since it's not a Rhythm Plus URL\n\nPlease make sure the URL you're trying to go to starts with\nhttps://rhythm-plus.com or https://v2.rhythm-plus.com\nthen try again", "Unable to navigate - Splamei Client", "OK", "");
                    }
                    msgBox.ShowDialog();
                }

                textBox1.Text = "https://rhythm-plus.com";
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
