﻿using Microsoft.Web.WebView2.Core;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Input;
using System.Net;
using DiscordRPC;
using DiscordRPC.Logging;
using DiscordRPC.Message;
using System.Security.Policy;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class Form1 : Form
    {

        public Splash splash;

        FormState formState = new FormState();

        private bool closeSplash = false;

        private bool showingError = false;

        public int myVerCode = 1000;

        public DiscordRpcClient client;

        public DiscordRPC.Button playButton = new DiscordRPC.Button();

        public DateTime start;

        public bool enabledRP = true;
        public bool showTitleOfMaps = true;
        public bool directLinkRP = false;
        public int discordRpRefresh = 5;

        public bool retainWinSize = true;
        public bool fullscreen = false;

        public string prevDataRP = "";
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

            //Connect to the RPC
            client.Initialize();

            start = DateTime.Now;

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
                if (webView21.Source.ToString() != null)
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
                        point = "Creating a map";
                    }
                    else if (uri.Equals("https://rhythm-plus.com/account/"))
                    {
                        point = "Changing settings";
                    }
                    else if (uri.Equals("https://rhythm-plus.com/tutorial/"))
                    {
                        point = "Playing the tutorial";
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
                            State = "Playing",
                            Party = new Party()
                            {
                                Max = 1,
                                Size = 1,
                                ID = "room123"
                            },
                            Timestamps = new Timestamps()
                            {
                                Start = start
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
                        MessageBox.Show($"Something went wrong while setting the window of the client up. It's been reset and we are sorry for the issue.\n\nError Code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    MessageBox.Show($"Something went wrong when running the client. The client will continue to run and the issue should now be resolved. We are sorry for the issue\n\nError Code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/discordRpRefresh.dat", "5");
            }

            setUpRP();

            try
            {
                var webView2Environment = await CoreWebView2Environment.CreateAsync(null, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client");

                this.Hide();

                await webView21.EnsureCoreWebView2Async(webView2Environment);

                this.Hide();

                webView21.Source = new Uri("https://rhythm-plus.com");

                webView21.CoreWebView2.DocumentTitleChanged += titleChanged;
                //webView21.Source = new Uri("https://google.com");

                this.Hide();

                closeSplash = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                if (MessageBox.Show($"Something went wrong when running the client. The client will now close. We are sorry for the issue\n\nError Code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Application.Exit();
                }
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

                    if (MessageBox.Show($"Something went wrong when running the client. The client will now close. We are sorry for the issue\n\nResult Code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    {
                        Application.Exit();
                    }
                }
            }
        }

        private void webView21_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (splash != null && e.IsSuccess && closeSplash)
            {
                splash.Close();

                this.Opacity = 1f;

                this.Show();

                splash = null;
            }
            else if (!showingError && !e.IsSuccess)
            {
                showingError = true;

                Console.WriteLine("Error! " + e.WebErrorStatus);

                if (MessageBox.Show($"Something went wrong when running the client. The client will now close. We are sorry for the issue\n\nResult Code: {e.WebErrorStatus}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }

        private void checkVer()
        {
            var task = MakeAsyncRequest("https://www.veemo.uk/net/r-plus/pc/ver", "text/html");
            Console.WriteLine("Got response of ", task.Result);

            if (Int32.Parse(task.Result) > myVerCode)
            {
                Console.WriteLine("New update!");
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
            else
            {
                checkNotices();
            }
        }

        private void checkNotices()
        {
            var task = MakeAsyncRequest("https://www.veemo.uk/net/r-plus/pc/notices", "text/html");

            try
            {
                string[] notices = task.Result.ToString().Split(';');

                if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/notice.dat"))
                {
                    if (notices[3] != System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/notice.dat"))
                    {
                        System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/notice.dat", notices[3]);

                        Console.WriteLine("New notice!");

                        if (notices[2] == "NONE" && notices[0] != "NONE")
                        {
                            MessageBox.Show(notices[1], notices[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (notices[0] != "NONE")
                        {
                            if (MessageBox.Show(notices[1] + "\n\n\nThis notice has a URL added to it. Do you want to open it?", notices[0], MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
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

                    if (notices[2] == "" && notices[0] != "NONE")
                    {
                        MessageBox.Show(notices[1], notices[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (notices[0] != "NONE")
                    {
                        if (MessageBox.Show(notices[1] + "\n\n\nThis notice has a URL added to it. Do you want to open it?", notices[0], MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            Process.Start(notices[2]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error decoding notices - " + ex);

                if (MessageBox.Show($"Something went wrong when running the client. The client will now close. We are sorry for the issue\n\nResult Code: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
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
            request.Timeout = 20000;
            request.Proxy = null;

            Task<WebResponse> task = Task.Factory.FromAsync(
                request.BeginGetResponse,
                asyncResult => request.EndGetResponse(asyncResult),
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
            Console.WriteLine("Size changed!");
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeWidth.dat", this.Size.Width.ToString());
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/sizeHeight.dat", this.Size.Height.ToString());
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            timer2.Stop();
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            Console.WriteLine("Location changed!");
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/locationX.dat", this.Location.X.ToString());
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/locationY.dat", this.Location.Y.ToString());
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Dispose();
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
                Settings settings = new Settings();
                settings.form = this;

                settings.ShowDialog();
            }
        }
    }
}