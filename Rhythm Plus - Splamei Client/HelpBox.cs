using Microsoft.Web.WebView2.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Policy;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class HelpBox : Form
    {
        public HelpBox()
        {
            InitializeComponent();
        }

        private async void HelpBox_Load(object sender, EventArgs e)
        {
            try
            {
                var webView2Environment2 = await CoreWebView2Environment.CreateAsync(null, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client");

                await webView21.EnsureCoreWebView2Async(webView2Environment2);

                webView21.Source = new Uri("https://veemo.uk/help");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (MessageBox.Show($"Something went wrong when running the client. The client will now close. We are sorry for the issue\n\nError Code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }

        private void webView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            this.Text = "Get Help -> Rhythm Plus - Splamei Client";

            this.UseWaitCursor = false;
        }
    }
}
