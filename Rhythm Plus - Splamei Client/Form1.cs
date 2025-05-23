﻿using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Input;
using System.Linq;
using System.Net;
using DiscordRPC;
using DiscordRPC.Logging;
using DiscordRPC.Message;
using System.Collections.Generic;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class Form1 : Form
    {

        public Splash splash;

        FormState formState = new FormState();

        private bool closeSplash = false;

        private bool showingError = false;
        private Settings settingsBox;

        public int myVerCode = 1005;

        public DiscordRpcClient client;

        public DiscordRPC.Button playButton = new DiscordRPC.Button();

        public DateTime startRP;

        public bool enabledRP = true;
        public bool showTitleOfMaps = true;
        public bool directLinkRP = false;
        public int discordRpRefresh = 5;

        public bool retainWinSize = true;
        public bool fullscreen = false;

        public bool debugMode = false;

        public string prevDataRP = "";

        public static ToolStripMenuItem discordRPstate;

        public bool failedToRemoveExtension = false;
        public bool enabledExtensions = false;

        public Form1()
        {
            InitializeComponent();
        }

        public void setUpRP()
        {
            client = new DiscordRpcClient("1331684607199936552");

            client.OnConnectionFailed += errorSettingRP;

            //Set the logger
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

            client.OnConnectionFailed += (sender, e) =>
            {
                Console.WriteLine("Failed to connect to discord - " + e.Type);
            };

            client.OnClose += (sender, e) =>
            {
                Console.WriteLine("Connection closed to discord - " + e.Reason);
            };

            //Connect to the RPC
            client.Initialize();

            startRP = DateTime.UtcNow;

            playButton = new DiscordRPC.Button();
            playButton.Label = "Play Rhythm Plus";
            playButton.Url = "https://rhythm-plus.com";

            if (directLinkRP)
            {
                playButton.Label = "Play with me";
            }

            setPresence();
        }

        public void setPresence()
        {
            try
            {
                if (webView21.Source != null)
                {
                    string point = "Playing Rhythm Plus";
                    string uri = webView21.Source.ToString();

                    if (uri.Equals("https://rhythm-plus.com"))
                    {
                        point = "On the into screen";
                    }
                    else if (uri.StartsWith("https://rhythm-plus.com/menu/"))
                    {
                        point = "Looking at songs";
                    }
                    else if (uri.Equals("https://rhythm-plus.com/studio/") || uri.StartsWith("https://rhythm-plus.com/editor/"))
                    {
                        point = "Creating a chart";
                    }
                    else if (uri.Equals("https://rhythm-plus.com/account/"))
                    {
                        point = "Changing settings";
                    }
                    else if (uri.Equals("https://rhythm-plus.com/tutorial/"))
                    {
                        point = "Playing the tutorial";
                    }
                    else if (uri.StartsWith("https://rhythm-plus.com/result/"))
                    {
                        point = "Looking at results";
                    }
                    else if (uri.StartsWith("https://rhythm-plus.com/game-over/"))
                    {
                        point = "Failed a chart";
                    }
                    else if (uri.StartsWith("https://rhythm-plus.com/game/"))
                    {
                        string songName = webView21.CoreWebView2.DocumentTitle.Split(new string[] { " - Rhythm+ Music" }, StringSplitOptions.None)[0];
                        if (songName == "Game")
                        {
                            point = "Loading a song";
                        }
                        else
                        {
                            point = $"Playing '{songName}'";
                        }
                    }

                    if (directLinkRP)
                    {
                        playButton.Url = uri;
                    }

                    if (prevDataRP != point)
                    {
                        client.SetPresence(new RichPresence()
                        {
                            Details = point,
                            //State = "Playing",
                            Timestamps = new Timestamps()
                            {
                                Start = startRP
                            },
                            Assets = new Assets()
                            {
                                LargeImageKey = "logo",
                                LargeImageText = "Rhythm Plus - Splamei Client",
                                SmallImageKey = "icon",
                                SmallImageText = "Client by Splamei"
                            },
                            Buttons = new DiscordRPC.Button[]
                            {
                                playButton
                            }
                        });

                        prevDataRP = point;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! - " + ex.ToString());
            }
        }

        private void errorSettingRP(object sender, ConnectionFailedMessage args)
        {
            enabledRP = false;

            Console.WriteLine("Error with RP! - " + args);

            client.Dispose();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();

            splash = new Splash();
            splash.Show();

            if (!System.IO.Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client"))
            {
                System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client");
            }

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/retainWindowSize.dat"))
            {
                if (File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/retainWindowSize.dat") == "0")
                {
                    retainWinSize = false;
                }
            }
            else
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/retainWindowSize.dat", "1");
            }

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/debugMode.dat"))
            {
                if (File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/debugMode.dat") == "1")
                {
                    debugMode = true;
                }
            }
            else
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/debugMode.dat", "0");
            }

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/playFullScreen.dat"))
            {
                if (File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/playFullScreen.dat") == "1")
                {
                    fullscreen = true;
                }
            }
            else
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/playFullScreen.dat", "0");
            }

            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeState.dat"))
            {
                if (System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeState.dat") == "Max")
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                    try
                    {
                        if (fullscreen)
                        {
                            formState.Maximize(this);
                            menuStrip1.Visible = false;
                            webView21.Dock = DockStyle.Fill;
                            timer4.Enabled = true;
                        }
                        else if (retainWinSize)
                        {
                            this.Size = new System.Drawing.Size(int.Parse(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeWidth.dat")), int.Parse(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeHeight.dat")));
                            this.Location = new System.Drawing.Point(int.Parse(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/locationX.dat")), int.Parse(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/locationY.dat")));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.WindowState = FormWindowState.Maximized;
                        this.Location = new System.Drawing.Point(0, 0);
                        Console.WriteLine(ex);

                        Error errorD = new Error();
                        errorD.errorDebug = ex.ToString();
                        errorD.ShowDialog();
                    }
                }
            }
            else
            {
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeState.dat", "Norm");
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeWidth.dat", "1210");
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeHeight.dat", "705");
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/locationX.dat", "0");
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/locationY.dat", "0");
                this.WindowState = FormWindowState.Maximized;
            }

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enabledRP.dat"))
            {
                if (File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enabledRP.dat") == "0")
                {
                    enabledRP = false;
                }
            }
            else
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enabledRP.dat", "1");
            }

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/showTitleOfMapsRP.dat"))
            {
                if (File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/showTitleOfMapsRP.dat") == "0")
                {
                    showTitleOfMaps = false;
                }
            }
            else
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/showTitleOfMapsRP.dat", "1");
            }

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/directLinkRP.dat"))
            {
                if (File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/directLinkRP.dat") == "1")
                {
                    directLinkRP = true;
                }
            }
            else
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/directLinkRP.dat", "0");
            }

            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enableExtensions.dat"))
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enableExtensions.dat", "0");
            }

            if (File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enableExtensions.dat") == "1")
            {
                enabledExtensions = true;

                //notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
                //notifyIcon1.BalloonTipText = "Extensions on the client are enabled. Turn them off if you experience any issues";
                //notifyIcon1.BalloonTipTitle = "Extensions enabled";

                //notifyIcon1.ShowBalloonTip(2);
            }

            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/discordRpRefresh.dat"))
            {
                try
                {
                    discordRpRefresh = int.Parse(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/discordRpRefresh.dat"));
                    timer3.Interval = discordRpRefresh * 1000;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error! " + ex);
                    discordRpRefresh = 5;
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/discordRpRefresh.dat", "5");

                    Error errorD = new Error();
                    errorD.errorDebug = ex.ToString();
                    errorD.shouldClose = false;
                    errorD.ShowDialog();
                }
            }
            else
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/discordRpRefresh.dat", "5");
            }

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Extensions"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Extensions");
            }

            if (enabledRP)
            {
                setUpRP();
            }

            try
            {
                var options = new CoreWebView2EnvironmentOptions
                {
                    AreBrowserExtensionsEnabled = enabledExtensions,
                    ScrollBarStyle = CoreWebView2ScrollbarStyle.FluentOverlay
                };


                var webView2Environment = await CoreWebView2Environment.CreateAsync(null, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client", options);

                this.Hide();

                await webView21.EnsureCoreWebView2Async(webView2Environment);

                this.Hide();

                webView21.Source = new Uri("https://rhythm-plus.com");

                webView21.CoreWebView2.DocumentTitleChanged += titleChanged;
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

                this.Hide();

                closeSplash = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                Error errorD = new Error();
                errorD.errorDebug = ex.ToString();
                errorD.ShowDialog();
            }

            if (!System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/played.dat"))
            {
                if (MessageBox.Show("Thank you for using the Splamei Client to play Rhythm Plus! It means alot to me and I hope you enjoy it!\r\n\r\nFeel free to join the SplameiPlay Discord for support on the client and use the GitHub repo to make your own client!\r\n\r\n\nIf you havn't, I would recommend you play the client through SplameiPlay for automatic update installs but you don't have to!", "Welcome to the new PC Rhythm Plus - Splamei Client!", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/played.dat", "Hello World!");
                }
            }
            else
            {
                try
                {
                    if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/checkCode.dat"))
                    {
                        if (Int32.Parse(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/checkCode.dat")) < Int32.Parse(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2")))
                        {
                            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/checkCode.dat", DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2"));

                            checkVer();
                        }
                        else
                        {
                            Console.WriteLine("[Net Client] Last got ver and notices in the last 2 days. Not checking so returning the last saved value");
                        }
                    }
                    else
                    {
                        System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/checkCode.dat", DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2"));

                        checkVer();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error getting notices or ver! - " + ex);

                    Error errorD = new Error();
                    errorD.errorDebug = ex.ToString();
                    errorD.ShowDialog();
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
            if (splash != null && e.IsSuccess && closeSplash)
            {
                splash.Close();

                this.Opacity = 1f;

                this.Show();

                splash = null;

                notifyIcon1.Visible = true;
            }
            else if (!showingError && !e.IsSuccess)
            {
                showingError = true;

                Console.WriteLine("Error! " + e.WebErrorStatus);

                Error errorD = new Error();
                errorD.errorDebug = "WebView2 error status code - " + e.WebErrorStatus.ToString();
                errorD.ShowDialog();
            }
        }

        private void checkVer()
        {
            var task = MakeAsyncRequest("https://www.veemo.uk/net/r-plus/pc/ver", "text/html");

            try
            {
                if (!task.IsFaulted)
                {

                    if (Int32.Parse(task.Result) > myVerCode)
                    {
                        Console.WriteLine("New update!");
                        if (File.Exists("C:/Splamei/SplameiPlay/Launcher/Updater-data/location.dat"))
                        {
                            if (MessageBox.Show("Theres a new update to the client! Press 'Yes' to close close the client and open SplameiPlay to get the new update installed.\n\nIf you already launched the client through SplameiPlay today, you'll need to wait until tomorrow before the update will be installed. Sorry!", "New Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                string location = File.ReadAllText("C:/Splamei/SplameiPlay/Launcher/Updater-data/location.dat");
                                if (location.StartsWith(@"C:\Splamei\SplameiPlay\"))
                                {
                                    Process.Start(location);
                                    Application.Exit();
                                }
                                else
                                {
                                    MessageBox.Show("We failed to launch SplameiPlay. Please check your installation and try again. You may need to reinstall the app.", "Rhythm Plus - Splamei Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                checkNotices();
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Theres a new update to the client! Press 'Yes' to close close the client and open the GitHub page to install the new update.\n\nYou don't neet to do this if your using SplameiPlay so just press 'No' and wait for it to realise the update exists", "New Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                Process.Start("https://github.com/splamei/rplus-pc-client/releases");
                                Application.Exit();
                            }
                            else
                            {
                                checkNotices();
                            }
                        }
                    }
                    else
                    {
                        checkNotices();
                    }
                }
                else
                {
                    Debug.WriteLine("Error getting ver");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception getting version - " + ex);
            }
        }

        private void checkNotices()
        {
            var task = MakeAsyncRequest("https://www.veemo.uk/net/r-plus/pc/client-notices", "text/html");

            if (!task.IsFaulted)
            {
                try
                {
                    string[] notices = task.Result.ToString().Split(';');

                    if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/notice.dat"))
                    {
                        if (notices[3] != System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/notice.dat"))
                        {
                            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/notice.dat", notices[3]);

                            Console.WriteLine("New notice!");

                            if (notices[2].Contains("NONE"))
                            {
                                MessageBox.Show(notices[1], notices[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                if (MessageBox.Show(notices[1] + "\n\n\nDo you want to open the URL added for the notice?", notices[0], MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    Process.Start(notices[2]);
                                }
                            }
                        }
                    }
                    else
                    {
                        System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/notice.dat", notices[3]);

                        Console.WriteLine("New notice!");

                        if (notices[2].Contains("NONE"))
                        {
                            MessageBox.Show(notices[1], notices[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            if (MessageBox.Show(notices[1] + "\n\n\nDo you want to open the URL added for the notice?", notices[0], MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                Process.Start(notices[2]);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error decoding notices - " + ex);

                    Error errorD = new Error();
                    errorD.errorDebug = ex.ToString();
                    errorD.ShowDialog();
                }
            }
            else { Debug.WriteLine("Error getting notices"); }
        }

        private void Maximise_Click(object sender, EventArgs e)
        {
            
        }

        // Define other methods and classes here
        public static Task<string> MakeAsyncRequest(string url, string contentType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = contentType;
            request.Method = WebRequestMethods.Http.Get;
            request.Timeout = 5000;
            request.Proxy = null;

            Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult =>
                {
                    try
                    {
                        return request.EndGetResponse(asyncResult);
                    }
                    catch (WebException ex)
                    {
                        if (ex.Response != null)
                        {
                            return ex.Response;
                        }
                        throw;
                    }
                },
                (object)null);

            return task.ContinueWith(t => ReadStreamFromResponse(t.Result));
        }

        private static string ReadStreamFromResponse(WebResponse response)
        {
            using (Stream responseStream = response.GetResponseStream())
            using (StreamReader sr = new StreamReader(responseStream))
            {
                //Need to return this response 
                string strContent = sr.ReadToEnd();
                return strContent;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (retainWinSize)
            {
                timer1.Stop();
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (this.Size.Height > 200 && this.Size.Width > 200)
            {
                Console.WriteLine("Size changed!");
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeWidth.dat", this.Size.Width.ToString());
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeHeight.dat", this.Size.Height.ToString());
            }
            else
            {
                Console.WriteLine("Not saving size since too small");
            }
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            timer2.Stop();
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            if (this.Location.X > 0 && this.Location.Y > 0)
            {
                Console.WriteLine("Location changed!");
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/locationX.dat", this.Location.X.ToString());
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/locationY.dat", this.Location.Y.ToString());
            }
            else { Console.WriteLine("Not saving location since off screen"); }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
            {
                client.Dispose();
            }

            webView21.Dispose();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (enabledRP)
            {
                setPresence();
            }
        }

        private void QuitMenu_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void getHelpMenu_Click(object sender, EventArgs e)
        {
            //DialogResult dialog = MessageBox.Show("Is the issue your having releating to the Rhythm Plus game and not the client? This will help us direct you to the best place to get help", "Get Help", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            //if (dialog == DialogResult.Yes)
            //{
            //    if (MessageBox.Show("The Rhythm Plus Comunity Discord link is now going to open to get help on the game. You will need a Discord account to join", "Get Help", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            //    {
            //        Process.Start("https://discord.gg/ZGhnKp4");
            //    }
            //}
            //else if (dialog == DialogResult.No)
            //{
            //    if (MessageBox.Show("The SplameiPlay Discord link is now going to open to get help on the client. You will need a Discord account to join", "Get Help", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            //    {
            //        Process.Start("https://discord.gg/g2KTP5X9At");
            //    }
            //}

            HelpBox help = new HelpBox();
            help.ShowDialog();
        }

        private void aboutMenu_Click(object sender, EventArgs e)
        {
            //AboutBox1 about = new AboutBox1();
            //about.ShowDialog();

            AboutNew aboutNew = new AboutNew();
            aboutNew.form = this;
            aboutNew.ShowDialog();
        }

        private void settingsMenu_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();
            settings.form = this;

            settings.ShowDialog();
        }

        public void setSettings(bool LenableRP, bool LshowTitleMaps, bool LdirectLinkRP, bool LretainWinSize, int LdiscordRefesh)
        {
            enabledRP = LenableRP;
            showTitleOfMaps = LshowTitleMaps;
            directLinkRP = LdirectLinkRP;

            retainWinSize = LretainWinSize;

            discordRpRefresh = LdiscordRefesh;

            if (enabledRP)
            { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enabledRP.dat", "1"); }
            else
            { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enabledRP.dat", "0"); }

            if (showTitleOfMaps)
            { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/showTitleOfMapsRP.dat", "1"); }
            else
            { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/showTitleOfMapsRP.dat", "0"); }

            if (directLinkRP)
            { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/directLinkRP.dat", "1"); }
            else
            { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/directLinkRP.dat", "0"); }

            if (retainWinSize)
            { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/retainWindowSize.dat", "1"); }
            else
            { File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/retainWindowSize.dat", "0"); }

            if (directLinkRP)
            {
                playButton.Label = "Play with me";
            }
        }

        public void titleChanged(object sender, object e)
        {
            if (webView21.CoreWebView2.DocumentTitle == "Rhythm+ Music Game" || webView21.CoreWebView2.DocumentTitle == "Rhythm Plus - Online Rhythm Game - Play, Create and Share Your Favorite Songs")
            {
                this.Text = "Rhythm Plus - Splamei Client";
            }
            else
            {
                this.Text = webView21.CoreWebView2.DocumentTitle.Replace("Rhythm+ Music Game", "Rhythm Plus - Splamei Client");
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
            {
                if (settingsBox == null)
                {
                    settingsBox = new Settings();
                    settingsBox.form = this;

                    settingsBox.ShowDialog();
                }
                else if (settingsBox.IsDisposed)
                {
                    settingsBox = new Settings();
                    settingsBox.form = this;
                    settingsBox.ShowDialog();
                }
            }
        }

        public void newUrlToGo(string url)
        {
            if (webView21.Source == new Uri(url))
            {
                MsgBox msgBox = new MsgBox();
                msgBox.setData(null, "Unable to navigate", "We can't go to that URL since it's the current URL you're on\n\nIf you really want to go to this URL, go to a different page then try\nagain", "Unable to navigate - Splamei Client", "OK", "");
                msgBox.ShowDialog();
            }
            else
            {
                webView21.Source = new Uri(url);
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            URL_Select uRL_Select = new URL_Select();
            uRL_Select.action = newUrlToGo;
            uRL_Select.ShowDialog();
        }

        private void copyLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)webView21.Source.ToString());

            //MsgBox msgBox = new MsgBox();
            //msgBox.setData(null, "Copied", "The URL has been copied to the clipboard", "Copied - Splamei Client", "OK", "");
            //msgBox.ShowDialog();

            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.BalloonTipTitle = "Copied";
            notifyIcon1.BalloonTipText = "The URL has been copied to the clipboard";
            notifyIcon1.ShowBalloonTip(500);
        }

        private async void extensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/enableExtensions.dat") == "1")
            {
                Extentions ext = new Extentions();

                var coreWebView2 = webView21.CoreWebView2;

                var browserExtensions = await coreWebView2.Profile.GetBrowserExtensionsAsync();

                List<string> names = new List<string>();
                List<string> ids = new List<string>();
                List<bool> enabled = new List<bool>();

                foreach (var extension in browserExtensions)
                {
                    //if (extension.Name != "Microsoft Clipboard Extension" && extension.Name != "Microsoft Edge PDF Viewer")
                    //{
                    names.Add(extension.Name);
                    ids.Add(extension.Id);
                    enabled.Add(extension.IsEnabled);
                    //}
                }

                failedToRemoveExtension = false;

                ext.loadExtentions(names, ids, enabled);
                ext.form = this;
                ext.Show();
            }
            else
            {
                MessageBox.Show("Extensions are disabled in settings. Enable them to add, view and remove extensions", "Extensions disabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public bool addExtention(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    webView21.CoreWebView2.Profile.AddBrowserExtensionAsync(path);
                    webView21.CoreWebView2.Reload();
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                return false;
            }
        }

        public async void removeExtention(string id)
        {
            try
            {
                var coreWebView2 = webView21.CoreWebView2;

                // Get the browser extensions asynchronously
                var browserExtensions = await coreWebView2.Profile.GetBrowserExtensionsAsync();

                // Specify the extension ID you want to remove
                string extensionIdToRemove = id; // Replace with the actual extension ID

                // Find the extension to remove
                var extensionToRemove = browserExtensions.FirstOrDefault(ext => ext.Id == extensionIdToRemove);

                if (extensionToRemove != null)
                {
                    // Remove the browser extension asynchronously
                    await extensionToRemove.RemoveAsync();
                    Console.WriteLine($"Extension {extensionToRemove.Name} has been removed successfully.");
                }
                else
                {
                    Console.WriteLine("Extension not found.");
                    failedToRemoveExtension = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);

                failedToRemoveExtension = true;
            }
        }

        public void refreshWebView()
        {
            webView21.CoreWebView2.Reload();
        }
    }
}
