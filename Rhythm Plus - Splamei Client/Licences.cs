using System.Diagnostics;
using System.Windows.Forms;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class Licences : Form
    {
        public Licences()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var process = Process.Start("https://github.com/Lachee/discord-rpc-csharp"))
            {

            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var process = Process.Start("https://www.nuget.org/packages/Microsoft.Web.WebView2/1.0.2957.106"))
            {

            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var process = Process.Start("https://www.nuget.org/packages/Newtonsoft.Json/13.0.1"))
            {

            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var process = Process.Start("https://www.nuget.org/packages/ILMerge/3.0.29"))
            {

            }
        }
    }
}
