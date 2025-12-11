using Microsoft.Web.WebView2.Core;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class HelpBox : Form
    {
        private WaitDialog waitDialog;

        private string webViewPath = "";

        public HelpBox()
        {
            InitializeComponent();
        }

        private async void HelpBox_Load(object sender, EventArgs e)
        {
            webViewPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Splamei",
                "Rhythm Plus - Splamei Client",
                "Help Webview");

            if (!Directory.Exists(webViewPath))
            {
                Directory.CreateDirectory(webViewPath);
            }

            try
            {
                var options = new CoreWebView2EnvironmentOptions
                {
                    AreBrowserExtensionsEnabled = false,
                    ScrollBarStyle = CoreWebView2ScrollbarStyle.FluentOverlay
                };

                var webView2Environment2 = await CoreWebView2Environment.CreateAsync(null, webViewPath, options);

                await webView21.EnsureCoreWebView2Async(webView2Environment2);

                webView21.Source = new Uri("https://veemo.uk/help");

                webView21.CoreWebView2.ContextMenuRequested += webView2ContextMenuRequested;

                webView21.CoreWebView2.Settings.AreDevToolsEnabled = false;
                webView21.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
                webView21.CoreWebView2.Settings.IsBuiltInErrorPageEnabled = false;
                webView21.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
                webView21.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
                webView21.CoreWebView2.Settings.IsStatusBarEnabled = false;
                //webView21.Source = new Uri("https://google.com");

                webView21.CoreWebView2.Profile.PreferredColorScheme = CoreWebView2PreferredColorScheme.Auto;
                webView21.CoreWebView2.Profile.DefaultDownloadFolderPath = $"C:/Users/{Environment.UserName}/Downloads/";
                webView21.CoreWebView2.Profile.IsGeneralAutofillEnabled = false;
                webView21.CoreWebView2.Profile.IsPasswordAutosaveEnabled = false;
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

        private void webView2ContextMenuRequested(object sender, CoreWebView2ContextMenuRequestedEventArgs e)
        {
            e.Handled = true;

            //contextMenuStrip2.Show(this, System.Windows.Forms.Cursor.Position);
        }

        private void webView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            this.Text = "Get Help -> Rhythm Plus - Splamei Client";

            this.UseWaitCursor = false;

            if (waitDialog != null)
            {
                waitDialog.Close();
            }
        }

        private void HelpBox_Shown(object sender, EventArgs e)
        {
            using (waitDialog = new WaitDialog())
            {
                waitDialog.updateText("Please wait while the help page loads");
                waitDialog.allowCanceling(cancelLoading, true);
                waitDialog.ShowDialog();
            }
        }

        private void HelpBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            webView21.Dispose();
            waitDialog.Dispose();
        }

        private void cancelLoading()
        {
            webView21.Stop();
            this.Close();
        }
    }
}
