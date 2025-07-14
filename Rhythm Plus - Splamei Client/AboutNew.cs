using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class AboutNew : Form
    {
        private int timesPressed = 0;

        private List<string> messages = new List<string>();
        private List<int> times = new List<int>();

        public Form1 form;

        public AboutNew()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutNew_Load(object sender, EventArgs e)
        {
            label4.Text = "Version - " + Application.ProductVersion;

            messages.Add("CRASH");
            times.Add(32);
            messages.Add("Yeah. I lied again. But I will crash you game if you continue");
            times.Add(31);
            messages.Add("Ok now it has :3");
            times.Add(30);
            messages.Add("Well it has now!");
            times.Add(20);
            messages.Add("You thought it had ended didn't you");
            times.Add(18);
            messages.Add("Bro");
            times.Add(13);
            messages.Add("Anyway. Just use the client for what it's used for");
            times.Add(10);
            messages.Add("Or YouTube or whatever. You can't say you didn't find it yourself");
            times.Add(9);
            messages.Add("You got this from the GitHub code didn't you?");
            times.Add(8);
            messages.Add("Ok. I might of lied");
            times.Add(6);
            messages.Add("Why do you keep clicking? There's nothing special");
            times.Add(4);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timesPressed++;

            bool showingEgg = false;

            for (int i = 0; i < times.Count; i++)
            {
                if (timesPressed >= times[i])
                {
                    if (messages[i] == "CRASH")
                    {
                        form.Hide();
                        Thread.Sleep(60_000);
                        MessageBox.Show("An unknown error has occured. The client will now close", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    MessageBox.Show(messages[i], "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    showingEgg = true;
                    break;
                }
            }

            if (!showingEgg)
            {
                using (Egg egg = new Egg())
                {
                    egg.ShowDialog();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (Licences licences = new Licences())
            {
                licences.ShowDialog();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void AboutNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}
