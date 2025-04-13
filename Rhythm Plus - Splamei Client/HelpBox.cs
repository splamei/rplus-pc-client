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
        private WaitDialog waitDialog;

        public HelpBox()
        {
            InitializeComponent();
        }

        private async void HelpBox_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Help Webview"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Help Webview");
            }

            try
            {
                var webView2Environment2 = await CoreWebView2Environment.CreateAsync(null, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Help Webview");

                await webView21.EnsureCoreWebView2Async(webView2Environment2);

                webView21.Source = new Uri("https://veemo.uk/help");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (MessageBox.Show($"Something went wrong when running the client. The help page will now close. We are sorry for the issue\n\nError Code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    waitDialog.Close();
                    this.Close();
                }
            }
        }

        private void webView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            this.Text = "Get Help -> Rhythm Plus - Splamei Client";

            this.UseWaitCursor = false;

            if (waitDialog != null)
            {
                waitDialog.Close();
                waitDialog.Dispose();
            }
        }

        private void HelpBox_Shown(object sender, EventArgs e)
        {
            waitDialog = new WaitDialog();
            waitDialog.updateText("Please wait while the help page loads");
            waitDialog.ShowDialog();
        }

        private void HelpBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            webView21.Dispose();
        }
    }
}
