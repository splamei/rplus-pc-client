using Microsoft.Web.WebView2.Core;
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
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Web.WebView2.WinForms;
using Rhythm_Plus___Splamei_Client.Save_System;
using Newtonsoft.Json;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class Form1 : Form
    {
        public Splash splash;
        public Welcome welcome;
        public WebView2 webView2;
        public SaveManager saveManager = new SaveManager();

        private bool closeSplash = false;
        private bool forceClose = false;

        private bool showingError = false;
        private Settings settingsBox;

        public int myVerCode = 1008;

        public DiscordRpcClient client;
        public bool failedRpConnection = false;

        public DiscordRPC.Button playButton = new DiscordRPC.Button();

        public float currentAccuracy = 0.0f;
        public string currentScore = "";
        public double currentTime = 0.0f;
        public bool isAutoplay = false;

        public string resultAccuracy = "";
        public string resultRank = "";
        public string resultMaxCombo = "";
        public string resultScore = "";
        public string resultFC = "";

        public string selectedSongName = "";
        public string selectedSongAuthor = "";
        public string selectedSongCharter = "";
        public string selectedSongTitle = "";

        public DateTime startRP;

        public bool enabledRP = true;
        public bool showTitleOfMaps = true;
        public bool showStatsinRPC = true;
        public bool directLinkRP = false;
        public int discordRpRefresh = 5;

        public bool retainWinSize = true;
        public bool fullscreen = false;
        public bool firstBoot = false;

        public string showMenu = "Only in settings";

        public bool debugMode = false;

        public string prevDataRP = "";

        public bool failedToRemoveExtension = false;
        public bool enabledExtensions = false;
        public float zoom = 1;
        public bool loadedV2 = false;

        // Fullscreen stuff
        public bool f11Pressed = false;
        public Rectangle originalBounds;
        public FormWindowState originalWindowState;

        private string savePath;


        public Form1()
        {
            InitializeComponent();

            savePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Splamei",
                "Rhythm Plus - Splamei Client");
        }

        public void setUpRP()
        {
            client = new DiscordRpcClient("1331684607199936552");

            //Set the logger
            client.Logger = new ConsoleLogger() { Level = LogLevel.Warning };

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Logging.logString("Received Ready from user " + e.User.Username);
                failedRpConnection = false;
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Logging.logString("Received Update - " + e.Presence.Details);
            };

            client.OnConnectionFailed += (sender, e) =>
            {
                Logging.logString("Failed to connect to discord - " + e.Type.ToString());
                if (webView2 != null && !firstBoot)
                {
                    if ((!webView2.Source.ToString().StartsWith("https://rhythm-plus.com/game/") && !webView2.Source.ToString().StartsWith("https://v2.rhythm-plus.com/game/")) && !failedRpConnection)
                    {
                        failedRpConnection = true;
                        DialogResult result = MessageBox.Show("Something went wrong when connecting to Discord. This may be because Discord is not installed/open or something is blocking the connection.\n\nYou can abort the connection and disable Rich Precence, Retry the connection or Ignore the issue and disable Rich Precence for this session.", "Failed to connect to Discord", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
                        if (result == DialogResult.Abort)
                        {
                            saveManager.setString("enabledRP", "0");
                            saveManager.saveData();
                            enabledRP = false;

                            client.Dispose();
                        }
                        else if (result == DialogResult.Ignore)
                        {
                            enabledRP = false;
                        }
                        else { failedRpConnection = false; }
                    }
                }
                else if (firstBoot)
                {
                    saveManager.setString("enabledRP", "0");
                    saveManager.saveData();
                    enabledRP = false;

                    client.Dispose();

                    Logging.logString("Disabling RP since it's first boot");
                }
            };

            client.OnClose += (sender, e) =>
            {
                Logging.logString("Connection closed to discord - " + e.Reason);
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
                    bool forceUpdate = false;

                    if (uri.Equals("https://rhythm-plus.com") || uri.Equals("https://v2.rhythm-plus.com"))
                    {
                        point = "On the into screen";
                    }
                    else if (uri.StartsWith("https://rhythm-plus.com/menu/") || uri.StartsWith("https://v2.rhythm-plus.com/menu/"))
                    {
                        point = "Looking at songs";
                    }
                    else if (uri.Equals("https://rhythm-plus.com/studio/") || uri.StartsWith("https://rhythm-plus.com/editor/") || uri.Equals("https://v2.rhythm-plus.com/studio/") || uri.StartsWith("https://v2.rhythm-plus.com/editor/"))
                    {
                        point = "Creating a chart";
                    }
                    else if (uri.Equals("https://rhythm-plus.com/account/") || uri.Equals("https://v2.rhythm-plus.com/account/"))
                    {
                        point = "Changing settings";
                    }
                    else if (uri.Equals("https://rhythm-plus.com/tutorial/") || uri.Equals("https://v2.rhythm-plus.com/tutorial/"))
                    {
                        point = "Playing the tutorial";
                    }
                    else if (uri.StartsWith("https://rhythm-plus.com/result/") || uri.StartsWith("https://v2.rhythm-plus.com/result/"))
                    {
                        point = "Looking at results";
                        forceUpdate = true;
                    }
                    else if (uri.StartsWith("https://rhythm-plus.com/game-over/") || uri.StartsWith("https://v2.rhythm-plus.com/game-over/"))
                    {
                        point = "Failed a chart";
                    }
                    else if (uri.StartsWith("https://rhythm-plus.com/game/") || uri.StartsWith("https://v2.rhythm-plus.com/game/"))
                    {
                        string songName = webView21.CoreWebView2.DocumentTitle.Split(new string[] { " - Rhythm+ Music" }, StringSplitOptions.None)[0];
                        if (loadedV2)
                        {
                            songName = webView21.CoreWebView2.DocumentTitle.Split(new string[] { " - Rhythm Plus Music" }, StringSplitOptions.None)[0];
                        }

                        if (songName == "Game")
                        {
                            point = "Loading a song";
                        }
                        else
                        {
                            if (songName == selectedSongTitle)
                            {
                                point = $"Playing '{selectedSongName} -by- {selectedSongAuthor}' [{selectedSongCharter}]";
                            }
                            else
                            {
                                point = $"Playing '{songName}'";
                            }

                            forceUpdate = true;
                        }
                    }

                    if (directLinkRP)
                    {
                        playButton.Url = uri;
                    }

                    if (prevDataRP != point || forceUpdate)
                    {
                        string state = "";
                        if ((uri.StartsWith("https://rhythm-plus.com/game/") || uri.StartsWith("https://v2.rhythm-plus.com/game/")) && showStatsinRPC)
                        {
                            updateGameStatDetails();

                            Logging.logString($"Current Score: {currentScore} | Current Acc: {currentAccuracy}% | Current Time: {currentTime}%");
                            if (currentScore != "" && !isAutoplay)
                            {
                                string rank = "F";
                                if (currentAccuracy == 0)
                                {
                                    rank = "?";
                                }
                                else if (currentAccuracy > 99f)
                                {
                                    rank = "S+";
                                }
                                else if (currentAccuracy > 97f)
                                {
                                    rank = "S";
                                }
                                else if (currentAccuracy > 94f)
                                {
                                    rank = "A";
                                }
                                else if (currentAccuracy > 90f)
                                {
                                    rank = "B";
                                }
                                else if (currentAccuracy > 80f)
                                {
                                    rank = "C";
                                }
                                else if (currentAccuracy > 60f)
                                {
                                    rank = "D";
                                }

                                state = $" - Score: {currentScore} - Acc: {currentAccuracy}% - Rank: ~{rank} - Point: {currentTime}%";
                            }
                            else if (isAutoplay)
                            {
                                state = $" - [Playing in Autoplay] - Score: {currentScore}";
                            }
                        }
                        else if ((uri.StartsWith("https://rhythm-plus.com/result/") || uri.StartsWith("https://v2.rhythm-plus.com/result/")) && showStatsinRPC)
                        {
                            updateResultsDetails();

                            if (resultScore != "")
                            {
                                if (resultFC == "Full Combo")
                                {
                                    state = $" - [FC] - Score: {resultScore} - Acc: {resultAccuracy}% - Rank: {resultRank} - Max Combo: {resultMaxCombo}";
                                }
                                else
                                {
                                    state = $" - Score: {resultScore} - Acc: {resultAccuracy}% - Rank: {resultRank} - Max Combo: {resultMaxCombo}";
                                }
                            }
                        }

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
                            },
                            State = state
                        });

                        prevDataRP = point;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.logString("Error! - " + ex.ToString());
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();

            menuStrip1.Visible = false;

            splash = new Splash();
            splash.Show();

            var exists = System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Count() > 1;
            if (exists)
            {
                MessageBox.Show("You seem to have the client already running in a seperate instance. To open a new instance, close the current instance first.", "Rhythm Plus - Splamei Client", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                forceClose = true;
                this.Close();
                return;
            }

            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            File.WriteAllText(Path.Combine(savePath, "Client.log"), "(i) This log file is for debugging the client and should only really be used by programmers. You can still try to understand it if you want!\n\n--\n\n");

            Logging.logString("Rhythm Plus - Splamei Client");
            Logging.logString("Version " + Application.ProductVersion + "\n");

            Logging.logString("Processor Count: " + Environment.ProcessorCount);
            Logging.logString($"Mapped Memory: {Environment.WorkingSet} bytes");
            Logging.logString("System Page Size: " + Environment.SystemPageSize + " bytes");
            Logging.logString("Is x64: " + Environment.Is64BitOperatingSystem);
            Logging.logString("OS Version: " + Environment.OSVersion + "\n");

            Logging.logString("--\n");

            Logging.addStarter = true;

            saveManager.loadData();
            saveManager.upgradeData();

            if (!System.IO.Directory.Exists(savePath))
            {
                System.IO.Directory.CreateDirectory(savePath);
            }

            if (saveManager.dataExist("retainWindowSize"))
            {
                if (saveManager.getString("retainWindowSize") == "0")
                {
                    retainWinSize = false;
                }
            }
            else
            {
                saveManager.setString("retainWindowSize", "1");
            }

            if (saveManager.dataExist("debugMode"))
            {
                if (saveManager.getString("debugMode") == "1")
                {
                    debugMode = true;
                }
            }
            else
            {
                saveManager.setString("debugMode", "0");
            }

            if (saveManager.dataExist("playFullScreen"))
            {
                if (saveManager.getString("playFullScreen") == "1")
                {
                    fullscreen = true;
                }
            }
            else
            {
                saveManager.setString("playFullScreen", "0");
            }

            if (saveManager.dataExist("sizeState"))
            {
                if (saveManager.getString("sizeState") == "Max")
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                    try
                    {
                        this.Size = new System.Drawing.Size(int.Parse(saveManager.getString("sizeWidth")), int.Parse(saveManager.getString("sizeHeight")));
                        this.Location = new System.Drawing.Point(int.Parse(saveManager.getString("locationX")), int.Parse(saveManager.getString("locationY")));
                    }
                    catch (Exception ex)
                    {
                        this.WindowState = FormWindowState.Maximized;
                        this.Location = new System.Drawing.Point(0, 0);
                        Logging.logString(ex.ToString());

                        using (Error errorD = new Error())
                        {
                            errorD.errorDebug = ex.ToString();
                            errorD.ShowDialog();
                        }
                    }
                }
            }
            else
            {
                saveManager.setString("sizeState", "Norm");
                saveManager.setString("sizeWidth", "1210");
                saveManager.setString("sizeHeight", "705");
                saveManager.setString("locationX", "0");
                saveManager.setString("locationY", "0");
                this.WindowState = FormWindowState.Maximized;
            }

            if (saveManager.dataExist("enabledRP"))
            {
                if (saveManager.getString("enabledRP") == "0")
                {
                    enabledRP = false;
                }
            }
            else
            {
                saveManager.setString("enabledRP", "1");
            }

            if (saveManager.dataExist("showTitleOfMapsRP"))
            {
                if (saveManager.getString("showTitleOfMapsRP") == "0")
                {
                    showTitleOfMaps = false;
                }
            }
            else
            {
                saveManager.setString("showTitleOfMapsRP", "1");
            }

            if (saveManager.dataExist("dontShowStatsInRPC"))
            {
                if (saveManager.getString("dontShowStatsInRPC") == "1")
                {
                    showStatsinRPC = false;
                }
            }
            else
            {
                saveManager.setString("dontShowStatsInRPC", "0");
            }

            if (saveManager.dataExist("directLinkRP"))
            {
                if (saveManager.getString("directLinkRP") == "1")
                {
                    directLinkRP = true;
                }
            }
            else
            {
                saveManager.setString("directLinkRP", "0");
            }

            if (!saveManager.dataExist("enableExtensions"))
            {
                saveManager.setString("enableExtensions", "0");
            }

            if (saveManager.getString("enableExtensions") == "1")
            {
                enabledExtensions = true;

                //notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
                //notifyIcon1.BalloonTipText = "Extensions on the client are enabled. Turn them off if you experience any issues";
                //notifyIcon1.BalloonTipTitle = "Extensions enabled";

                //notifyIcon1.ShowBalloonTip(2);
            }

            if (saveManager.dataExist("discordRpRefresh"))
            {
                try
                {
                    discordRpRefresh = int.Parse(saveManager.getString("discordRpRefresh"));
                    timer3.Interval = discordRpRefresh * 1000;
                }
                catch (Exception ex)
                {
                    Logging.logString("Error! " + ex);
                    discordRpRefresh = 5;
                    saveManager.setString("discordRpRefresh", "5");

                    using (Error errorD = new Error())
                    {
                        errorD.errorDebug = ex.ToString();
                        errorD.shouldClose = false;
                        errorD.ShowDialog();
                    }
                }
            }
            else
            {
                saveManager.setString("discordRpRefresh", "5");
            }

            if (saveManager.dataExist("viewZoom"))
            {
                try
                {
                    zoom = float.Parse(saveManager.getString("viewZoom"));
                }
                catch (Exception ex)
                {
                    Logging.logString("Error! " + ex);
                    saveManager.setString("viewZoom", "1");

                    using (Error errorD = new Error())
                    {
                        errorD.errorDebug = ex.ToString();
                        errorD.shouldClose = false;
                        errorD.ShowDialog();
                    }
                }
            }
            else
            {
                saveManager.setString("viewZoom", "1");
            }

            if (saveManager.dataExist("showMenuIn"))
            {
                try
                {
                    showMenu = saveManager.getString("showMenuIn");
                }
                catch (Exception ex)
                {
                    Logging.logString("Error! " + ex);
                    saveManager.setString("showMenuIn", "Only in settings");

                    using (Error errorD = new Error())
                    {
                        errorD.errorDebug = ex.ToString();
                        errorD.shouldClose = false;
                        errorD.ShowDialog();
                    }
                }
            }
            else
            {
                saveManager.setString("showMenuIn", "Only in settings");
            }

            if (!Directory.Exists(Path.Combine(savePath, "Extensions")))
            {
                Directory.CreateDirectory(Path.Combine(savePath, "Extensions"));
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


                var webView2Environment = await CoreWebView2Environment.CreateAsync(null, savePath, options);

                this.Hide();

                await webView21.EnsureCoreWebView2Async(webView2Environment);

                this.Hide();

                if (!saveManager.dataExist("v2Page"))
                {
                    if (MessageBox.Show("The long awaited Rhythm Plus v2 has finally released! And you can now play it via the client!\n\nDo you want us to change the to use v2 instead of v1? (You can change this is settings)", "Rhythm Plus - Splamei Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        saveManager.setInt("v2Page", 1);
                    }
                    else
                    {
                        saveManager.setInt("v2Page", 0);
                    }
                    saveManager.saveData();
                }

                if (saveManager.getInt("v2Page") == 1)
                {
                    webView21.Source = new Uri("https://v2.rhythm-plus.com");
                    loadedV2 = true;
                }
                else
                {
                    webView21.Source = new Uri("https://rhythm-plus.com");
                }

                webView21.CoreWebView2.DocumentTitleChanged += titleChanged;
                webView21.CoreWebView2.ContextMenuRequested += webView2ContextMenuRequested;

                webView21.CoreWebView2.Settings.AreDevToolsEnabled = false;
                webView21.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
                webView21.CoreWebView2.Settings.IsBuiltInErrorPageEnabled = false;
                webView21.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
                webView21.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
                webView21.CoreWebView2.Settings.IsStatusBarEnabled = false;
                webView21.CoreWebView2.Settings.IsZoomControlEnabled = true;
                //webView21.Source = new Uri("https://google.com");

                webView21.CoreWebView2.Profile.PreferredColorScheme = CoreWebView2PreferredColorScheme.Auto;
                webView21.CoreWebView2.Profile.DefaultDownloadFolderPath = $"C:/Users/{Environment.UserName}/Downloads/";
                webView21.CoreWebView2.Profile.IsGeneralAutofillEnabled = false;
                webView21.CoreWebView2.Profile.IsPasswordAutosaveEnabled = false;

                webView21.CoreWebView2.ContainsFullScreenElementChanged += (obj, args) =>
                {
                    toggleFullscreen(!fullscreen);
                };

                webView21.ZoomFactor = zoom;
                webView2 = webView21;

                this.Hide();

                closeSplash = true;
            }
            catch (Exception ex)
            {
                Logging.logString(ex.ToString());

                using (Error errorD = new Error())
                {
                    errorD.errorDebug = ex.ToString();
                    errorD.ShowDialog();
                }
            }

            if (!saveManager.dataExist("played"))
            {
                if (welcome != null) { welcome.Close(); welcome.Dispose(); }

                firstBoot = true;
                welcome = new Welcome();
                welcome.Show();
                saveManager.setString("played", "Hello World!");
                //if (MessageBox.Show("Thank you for using the Splamei Client to play Rhythm Plus! It means alot to me and I hope you enjoy it!\r\n\r\nFeel free to join the SplameiPlay Discord for support on the client and use the GitHub repo to make your own client!\r\n\r\n\nIf you havn't, I would recommend you play the client through SplameiPlay for automatic update installs but you don't have to!", "Welcome to the new PC Rhythm Plus - Splamei Client!", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                //{
                //    saveManager.setString("played", "Hello World!");
                //}
            }
            else
            {
                try
                {
                    if (saveManager.dataExist("checkCode"))
                    {
                        if (Int32.Parse(saveManager.getString("checkCode")) < Int32.Parse(DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2")))
                        {
                            saveManager.setString("checkCode", DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2"));

                            checkVer();
                        }
                        else
                        {
                            Logging.logString("Last got ver and notices in the last 2 days. Not checking so returning the last saved value");
                        }
                    }
                    else
                    {
                        saveManager.setString("checkCode", DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2"));

                        checkVer();
                    }
                }
                catch (Exception ex)
                {
                    Logging.logString("Error getting notices or ver! - " + ex);

                    using (Error errorD = new Error())
                    {
                        errorD.errorDebug = ex.ToString();
                        errorD.ShowDialog();
                    }
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
                splash.Dispose();

                this.Opacity = 1f;

                this.Show();

                splash = null;

                notifyIcon1.Visible = true;
            }
            else if (!showingError && !e.IsSuccess)
            {
                showingError = true;

                Logging.logString("Error! " + e.WebErrorStatus);

                using (Error errorD = new Error())
                {
                    errorD.errorDebug = "WebView2 error status code - " + e.WebErrorStatus.ToString();
                    errorD.ShowDialog();
                }
            }
        }

        private void checkVer()
        {
            try
            {
                var task = MakeAsyncRequest("https://www.veemo.uk/net/r-plus/pc/ver", "text/html");

                if (!task.IsFaulted)
                {

                    if (Int32.Parse(task.Result) > myVerCode)
                    {
                        Logging.logString("New update!");
                        if (File.Exists("C:/Splamei/SplameiPlay/Launcher/Updater-data/location.dat"))
                        {
                            if (MessageBox.Show("Theres a new update to the client! Press 'Yes' to close close the client and open SplameiPlay to get the new update installed.\n\nIf you already launched the client through SplameiPlay today, you'll need to wait until tomorrow before the update will be installed. Sorry!", "New Update", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                using (var process = Process.Start("splameiplay://"))
                                {
                                    Application.Exit();
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
                                using (var process = Process.Start("https://github.com/splamei/rplus-pc-client/releases"))
                                {
                                    Application.Exit();
                                }
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
                    Logging.logString("Error getting ver");
                }
            }
            catch (Exception ex)
            {
                Logging.logString("Exception getting version - " + ex);
            }
        }

        private void checkNotices()
        {

            try
            {
                var task = MakeAsyncRequest("https://www.veemo.uk/net/r-plus/pc/client-notices", "text/html");

                if (!task.IsFaulted)
                {
                    string[] notices = task.Result.ToString().Split(';');

                    if (saveManager.dataExist("notice"))
                    {
                        if (notices[3] != saveManager.getString("notice"))
                        {
                            saveManager.setString("notice", notices[3]);

                            Logging.logString("New notice!");

                            if (notices[2].Contains("NONE"))
                            {
                                MessageBox.Show(notices[1], notices[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                if (MessageBox.Show(notices[1] + "\n\n\nDo you want to open the URL added for the notice?", notices[0], MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                {
                                    using (var process = Process.Start(notices[2]))
                                    {

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        saveManager.setString("notice", notices[3]);

                        Logging.logString("New notice!");

                        if (notices[2].Contains("NONE"))
                        {
                            MessageBox.Show(notices[1], notices[0], MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            if (MessageBox.Show(notices[1] + "\n\n\nDo you want to open the URL added for the notice?", notices[0], MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                using (var process = Process.Start(notices[2]))
                                {

                                }
                            }
                        }
                    }
                }
                else { Logging.logString("Error getting notices"); }
            }
            catch (Exception ex)
            {
                Logging.logString("Error decoding notices - " + ex);

                using (Error errorD = new Error())
                {
                    errorD.errorDebug = ex.ToString();
                    errorD.ShowDialog();
                }
            }
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
            if (!fullscreen)
            {
                if (this.Size.Height > 200 && this.Size.Width > 200)
                {
                    Logging.logString("Size changed!");
                    saveManager.setString("sizeWidth", this.Size.Width.ToString());
                    saveManager.setString("sizeHeight", this.Size.Height.ToString());
                }
                else
                {
                    Logging.logString("Not saving size since too small");
                }
            }
            else
            {
                Logging.logString("Not saving size since in fullscreen");
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

            if (!fullscreen)
            {
                Logging.logString("Location changed!");
                saveManager.setString("locationX", this.Location.X.ToString());
                saveManager.setString("locationY", this.Location.Y.ToString());
            }
            else
            {
                Logging.logString("Not saving location since in fullscreen");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
            {
                client.Dispose();
            }

            if (fileMenu != null)
            {
                fileMenu.Dispose();
            }

            if (welcome != null)
            {
                welcome.Dispose();
            }

            if (splash != null)
            {
                splash.Dispose();
            }

            if (webView21 != null)
            {
                webView21.Dispose();
            }

            if (forceClose)
            {
                try
                {
                    saveManager.setString("viewZoom", webView21.ZoomFactor.ToString());
                    saveManager.setInt("discordRpRefresh", discordRpRefresh);
                    saveManager.setString("showMenuIn", showMenu);

                    if (enabledExtensions)
                    {
                        saveManager.setInt("enableExtensions", 1);
                    }
                    else
                    {
                        saveManager.setInt("enableExtensions", 1);
                    }

                    saveManager.saveData();
                    Logging.logString("Saved zoom and data");
                }
                catch (Exception ex)
                {
                    Logging.logString("Failed to save zoom! - " + ex);
                }
            }
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

            using (HelpBox help = new HelpBox())
            {
                help.ShowDialog();
            }
        }

        private void aboutMenu_Click(object sender, EventArgs e)
        {
            //AboutBox1 about = new AboutBox1();
            //about.ShowDialog();

            using (AboutNew aboutNew = new AboutNew())
            {
                aboutNew.form = this;
                aboutNew.ShowDialog();
            }
        }

        private void settingsMenu_Click(object sender, EventArgs e)
        {
            using (settingsBox = new Settings())
            {
                settingsBox.form = this;

                settingsBox.ShowDialog();
            }
        }

        public void setSettings(bool LenableRP, bool LshowTitleMaps, bool LdirectLinkRP, bool LretainWinSize, int LdiscordRefesh)
        {
            enabledRP = LenableRP;
            showTitleOfMaps = LshowTitleMaps;
            directLinkRP = LdirectLinkRP;

            retainWinSize = LretainWinSize;

            discordRpRefresh = LdiscordRefesh;

            if (enabledRP)
            { saveManager.setString("enabledRP", "1"); }
            else
            { saveManager.setString("enabledRP", "0"); }

            if (showTitleOfMaps)
            { saveManager.setString("showTitleOfMapsRP", "1"); }
            else
            { saveManager.setString("showTitleOfMapsRP", "0"); }

            if (directLinkRP)
            { saveManager.setString("directLinkRP", "1"); }
            else
            { saveManager.setString("directLinkRP", "0"); }

            if (retainWinSize)
            { saveManager.setString("retainWindowSize", "1"); }
            else
            { saveManager.setString("retainWindowSize", "0"); }

            if (showStatsinRPC)
            { saveManager.setString("dontShowStatsInRPC", "0"); }
            else
            { saveManager.setString("dontShowStatsInRPC", "1"); }

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
            else if (webView21.CoreWebView2.DocumentTitle == "Rhythm Plus Music Game - Game Offline")
            {
                this.Text = "Game Offline - Rhythm Plus - Splamei Client";
            }
            else if (webView21.Source.ToString().StartsWith("https://rhythm-plus.com/game/"))
            {
                this.Text = $"'{selectedSongName} -by- {selectedSongAuthor}' [{selectedSongCharter}] - Rhythm Plus - Splamei Client";
            }
            else
            {
                this.Text = webView21.CoreWebView2.DocumentTitle.Replace("Rhythm+ Music Game", "Rhythm Plus - Splamei Client").Replace("Rhythm Plus Music Game", "Rhythm Plus - Splamei Client");
            }

            if (showMenu == "Only in settings")
            {
                if (webView21.CoreWebView2.DocumentTitle != null)
                {
                    if (webView21.CoreWebView2.DocumentTitle.Contains("Settings"))
                    {
                        menuStrip1.Visible = true;
                    }
                    else
                    {
                        menuStrip1.Visible = false;
                    }
                }
            }
            else if (showMenu == "Not in game")
            {
                if (webView21.Source != null)
                {
                    if (webView21.Source.ToString().Contains("rhythm-plus.com/game/"))
                    {
                        menuStrip1.Visible = false;
                    }
                    else
                    {
                        menuStrip1.Visible = true;
                    }
                }
            }
            else if (showMenu == "Not in fullscreen")
            {
                if (fullscreen)
                {
                    menuStrip1.Visible = false;
                }
                else
                {
                    menuStrip1.Visible = true;
                }
            }
            else
            {
                menuStrip1.Visible = true;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            //if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
            //{
            //    if (settingsBox == null)
            //    {
            //        settingsBox = new Settings();
            //        settingsBox.form = this;
            //
            //        settingsBox.ShowDialog();
            //    }
            //    else if (settingsBox.IsDisposed)
            //    {
            //        settingsBox = new Settings();
            //        settingsBox.form = this;
            //        settingsBox.ShowDialog();
            //    }
            //}

            if (Keyboard.IsKeyDown(Key.F11) && !f11Pressed && Form.ActiveForm != null)
            {
                f11Pressed = true;
                toggleFullscreen(!fullscreen);
            }
            else if (f11Pressed && Keyboard.IsKeyUp(Key.F11) && Form.ActiveForm != null)
            {
                f11Pressed = false;
            }
        }

        public void newUrlToGo(string url)
        {
            if (webView21.Source == new Uri(url))
            {
                using (MsgBox msgBox = new MsgBox())
                {
                    msgBox.setData(null, "Unable to navigate", "We can't go to that URL since it's the current URL you're on\n\nIf you really want to go to this URL, go to a different page then try\nagain", "Unable to navigate - Splamei Client", "OK", "");
                    msgBox.ShowDialog();
                }
            }
            else
            {
                loadedV2 = url.StartsWith("https://v2.rhythm-plus.com");
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
            using (URL_Select uRL_Select = new URL_Select())
            {
                uRL_Select.action = newUrlToGo;
                uRL_Select.ShowDialog();
            }
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
            if (saveManager.getString("enableExtensions") == "1")
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
                Logging.logString(ex.ToString());

                return false;
            }
        }

        public async void removeExtention(string id)
        {
            try
            {
                var coreWebView2 = webView21.CoreWebView2;

                var browserExtensions = await coreWebView2.Profile.GetBrowserExtensionsAsync();

                var extensionToRemove = browserExtensions.FirstOrDefault(ext => ext.Id == id);

                if (extensionToRemove != null)
                {
                    await extensionToRemove.RemoveAsync();
                    Logging.logString($"Extension {extensionToRemove.Name} has been removed successfully.");
                }
                else
                {
                    Logging.logString("Extension not found.");
                    failedToRemoveExtension = true;
                }
            }
            catch (Exception ex)
            {
                Logging.logString(ex.ToString());

                failedToRemoveExtension = true;
            }
        }

        public void refreshWebView()
        {
            webView21.CoreWebView2.Reload();
        }

        public void toggleFullscreen(bool toggle)
        {
            try
            {
                fullscreen = toggle;

                if (fullscreen)
                {
                    saveManager.setString("playFullScreen", "1");

                    originalBounds = this.Bounds;
                    originalWindowState = this.WindowState;

                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Normal;
                    this.WindowState = FormWindowState.Maximized;

                    if (showMenu == "Not in fullscreen")
                    {
                        menuStrip1.Visible = false;
                    }
                }
                else
                {
                    saveManager.setString("playFullScreen", "0");

                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Normal;
                    this.Bounds = originalBounds;
                    this.WindowState = originalWindowState;

                    this.Invalidate();
                    this.Refresh();

                    if (showMenu == "Not in fullscreen")
                    {
                        menuStrip1.Visible = true;
                    }

                    this.Size = new System.Drawing.Size(int.Parse(saveManager.getString("sizeWidth")), int.Parse(saveManager.getString("sizeHeight")));
                    this.Location = new System.Drawing.Point(int.Parse(saveManager.getString("locationX")), int.Parse(saveManager.getString("locationY")));
                }

                if (settingsBox != null)
                {
                    settingsBox.fullscreenChanged(fullscreen);
                }
            }
            catch (Exception ex)
            {
                Logging.logString("Failed to change fullscreen - " + ex);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (fullscreen)
            {
                toggleFullscreen(true);
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            Logging.logString("Autosaving data");
            saveManager.saveData();
        }

        private async void updateGameStatDetails()
        {
            try
            {
                if (loadedV2)
                {
                    string script = "";
                    string result = "";
                    string value = "";

                    try
                    {
                        script = "document.querySelector('div.score > div:nth-child(2)').innerText";
                        result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                        value = JsonConvert.DeserializeObject<string>(result);
                        if (value != null) { currentAccuracy = float.Parse(value.Replace("%", "")); }
                    }
                    catch (Exception ex)
                    {
                        Logging.logString("Failed to get accuracy! - " + ex);
                        currentAccuracy = 0f;
                    }

                    script = "document.querySelector('div.score > div.text-5xl')?.innerText";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    if (value != null) { currentScore = value; }

                    script = "document.querySelector('.top-progress')?.style.width || getComputedStyle(document.querySelector('.top-progress')).width;";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    if (value != null)
                    {
                        if (value.EndsWith("%"))
                        {
                            value = value.TrimEnd('%');
                        }
                        currentTime = Math.Floor(float.Parse(value));
                    }

                    script = "document.querySelector('div.score > div > span')?.innerText";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    if (value != null) { isAutoplay = (value == "Autoplay"); } else { isAutoplay = false; }
                    Console.WriteLine(value);
                }
                else
                {
                    string script = "document.querySelector('.score span')?.innerText";
                    string result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    string value = JsonConvert.DeserializeObject<string>(result);
                    currentAccuracy = float.Parse(value);

                    script = "document.querySelector('.score > span')?.innerText";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    currentScore = value;

                    script = "document.querySelector('.top-progress')?.style.width || getComputedStyle(document.querySelector('.top-progress')).width;";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    if (value.EndsWith("%"))
                    {
                        value = value.TrimEnd('%');
                    }
                    currentTime = Math.Floor(float.Parse(value));
                }
            }
            catch (Exception ex)
            {
                Logging.logString("Failed to get Game Stats! - " + ex);

                currentScore = "";
                currentAccuracy = 0f;
            }
        }

        private async void updateResultsDetails()
        {
            try
            {
                if (loadedV2)
                {
                    string script = "document.querySelector('.score')?.innerText";
                    string result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    string value = JsonConvert.DeserializeObject<string>(result);
                    resultRank = value;

                    script = "document.querySelector('div.percentage-display > div')?.innerText";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    resultAccuracy = value;

                    script = "document.querySelector('div.score-title > div')?.innerText";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    resultScore = value;

                    script = "document.querySelector('div.combo-container > div > div')?.innerText";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    resultMaxCombo = value;

                    try
                    {
                        script = "document.querySelector('div.combo-container > div.mark-chip.achievement-chip.combo-chip')?.innerText";
                        result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                        value = JsonConvert.DeserializeObject<string>(result);
                        resultFC = value;
                    }
                    catch { resultFC = ""; }
                }
                else
                {
                    string script = "document.querySelector('.score')?.innerText";
                    string result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    string value = JsonConvert.DeserializeObject<string>(result);
                    resultRank = value;

                    script = "document.querySelector('div:nth-child(3) > span')?.innerText";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    resultAccuracy = value;

                    script = "document.querySelector('div.rightScore.flex-grow > div:nth-child(1) > span')?.innerText";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    resultScore = value;

                    script = "document.querySelector('div.rightScore.flex-grow > div:nth-child(2) > span')?.innerText";
                    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                    value = JsonConvert.DeserializeObject<string>(result);
                    resultMaxCombo = value;

                    try
                    {
                        script = "document.querySelector('div.rightScore.flex-grow > div:nth-child(2) > div')?.innerText";
                        result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                        value = JsonConvert.DeserializeObject<string>(result);
                        resultFC = value;
                    }
                    catch { resultFC = ""; }
                }
            }
            catch (Exception ex)
            {
                Logging.logString("Failed to get results Stats! - " + ex);

                currentScore = "";
                currentAccuracy = 0f;
            }
        }

        private async void timer6_Tick(object sender, EventArgs e)
        {
            if (webView21 != null)
            {
                if (webView21.CoreWebView2 != null)
                {
                    if (loadedV2)
                    {
                        string script = "document.querySelector('div.flex-1 > div.mt-10 > div')?.innerText";
                        string result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                        string value = JsonConvert.DeserializeObject<string>(result);
                        if (!string.IsNullOrEmpty(value))
                        {
                            selectedSongName = value;

                            script = "document.querySelector('div.flex-1.self-end > div.mt-10 > div.opacity-60')?.innerText";
                            result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                            value = JsonConvert.DeserializeObject<string>(result);
                            selectedSongAuthor = value;

                            script = "document.querySelector('a > div > div > div.text-sm.leading-5 > div > div')?.innerText";
                            result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                            value = JsonConvert.DeserializeObject<string>(result);
                            //if (string.IsNullOrEmpty(value))
                            //{
                            //    script = "document.querySelector('div.pt-2.text-xs.text-white.text-opacity-25 > span > span')?.innerText";
                            //    result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                            //    value = JsonConvert.DeserializeObject<string>(result);
                                selectedSongCharter = value;
                            //}
                            //else { selectedSongCharter = value; }

                            script = "document.querySelector('div.text-xl > span.w-fit')?.innerText";
                            result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                            value = JsonConvert.DeserializeObject<string>(result);
                            selectedSongTitle = value;
                        }
                    }
                    else
                    {
                        string script = "document.querySelector('div.detail.py-5 > div:nth-child(1)')?.innerText";
                        string result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                        string value = JsonConvert.DeserializeObject<string>(result);
                        if (!string.IsNullOrEmpty(value))
                        {
                            selectedSongName = value;

                            script = "document.querySelector('div.detail.py-5 > div:nth-child(2)')?.innerText";
                            result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                            value = JsonConvert.DeserializeObject<string>(result);
                            selectedSongAuthor = value;

                            script = "document.querySelector('div.pt-2.text-xs.text-white.text-opacity-25 > span:nth-child(3) > span')?.innerText";
                            result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                            value = JsonConvert.DeserializeObject<string>(result);
                            if (string.IsNullOrEmpty(value))
                            {
                                script = "document.querySelector('div.pt-2.text-xs.text-white.text-opacity-25 > span > span')?.innerText";
                                result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                                value = JsonConvert.DeserializeObject<string>(result);
                                selectedSongCharter = value;
                            }
                            else { selectedSongCharter = value; }

                            script = "document.querySelector('div.detail.py-5 > div:nth-child(1)').childNodes[1].nodeValue.trim()\r\n";
                            result = await webView21.CoreWebView2.ExecuteScriptAsync(script);
                            value = JsonConvert.DeserializeObject<string>(result);
                            selectedSongTitle = value;
                        }
                    }
                }
            }
        }

        private void starOnGitHubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Process.Start("https://github.com/splamei/rplus-pc-client")) { }
        }
    }
}
