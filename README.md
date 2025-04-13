# Rhythm Plus - Splamei Client (PC)
## If your looking for the Android Client, find it [here](https://github.com/splamei/rplus-mobile-client)
Another Splamei custom client for the [Rhythm Plus game](https://rhythm-plus.com) that now allows playing on Windows instead of just on Android!
Rhythm Plus is a web-based vertical scrolling rhythm game (VSRG), you can make, play, and share any songs from and with anyone! The game can also be played on PC and other platforms with a working web-browser.
## Features

 - Native to Windows
 - Easy to use
 - Extension support
 - Discord Rich Presence
 - Notices for new info on Rhythm Plus
 - Alerts on new updates

## Compatibility
- WebView2 needs to be installed. All Windows 11 and most Windows 10 installs have it pre-installed
- Windows 10+ for support
- Also you need an active internet connection and to be able to access the Rhythm Plus website (rhythm-plus.com) and the Splamei Network (veemo.uk)

## Installation
It's best to play through SplameiPlay to have the client automatically updated when a new update is pushed. However, if you don't want to use SplameiPlay, you will need to use the installer but you will have to update the client yourself.

### Recommended / Easy Method (SplameiPlay)

 1. Download and install [SplameiPlay](https://www.veemo.uk/splameiplay-download) if not installed. (You must be using 4.0 Bloom or later to have the client in the store)
 2. Once you have SplameiPlay open, navigate to the 'Store' page and find the client in the 'Splamei Official' section
 3. When on the project page, press 'Download' and the latest version will be downloaded and installed. After that, you can find the project on the 'Home' page. You can also press the 'Fav' button to have it show higher up
 4. Now, to play, just open SplameiPlay, open the project page and press 'Play'. Any new versions will be automatically installed
### Installer (Worse) Method
This method is NOT recommended as you will need to update the client manually by downloading the latest installer (or exe). You should only do this if you can't or don't want to use SplameiPlay
1. Download the latest release from the [GitHub release page.](https://github.com/splamei/rplus-pc-client/releases) Download the installer to have it playable through the start menu or the portable exe if you do not want to install the client
2. If your using the portable app, open the main .exe file. Otherwise install the app with the installer then you can open it from the start menu

To update, follow the steps above to install / use the latest version
## Discord Rich Prescence
The Windows client supports integration with Discord through it's rich presence feature. This means you can show to your friends and to users in the servers you joined that your playing Rhythm Plus and, if set, share the name of the song you are playing.

Note: You **must** have the Discord Desktop app installed rather than the website app. If you launch the client and Discord was not open, DiscordRP will be disabled automatically. Settings for Rich Prescence can be changed in the setting window.
## Custom Extensions
If you wish, you can enable extensions within Settings to adjust the Rhythm Plus game. By default, extensions are disable. Below is how to add extensions:
1. Open Settings through 'File > Settings'
2. Check the box that says 'Enable Extensions' and accept the warning
3. Open the extension menu on the main window
4. Press (+) to add an extension and find the unpacked extension and select it's `manifest.json` file

One done, the extension should be added. A reload of the client might be needed for the extension to fully apply. Please note that extension buttons will not be accessible at the current moment. To remove, an extension, enter the extension window and select (-) and check any extension to be removed.

Please do not delete any folders for extensions as you will not be able to remove them unless you re-add the extension or retore the original folder and files.
## Making your own client
If your using this code to make your own custom client whether that be for Rhythm Plus or another web app, I ask for you to please do the following:
 - Credit this client as the base (whether it be in the repo README or the app itself)
 - Replace or remove the update handling (and notice feature if you want)
 - Change the package name
 - If the client isn't for Rhythm Plus, please change the icons the client uses
 - Because this client is currently under the MIT Licence, you **must** acknowledge my copyright ( (c) 2024 Splamei )
## Contributing
I would love for people to help the clients development so any little contribution would go a long way!
You could contribute by:
 - Reporting issues / feature requests on the [issue page](https://github.com/splamei/rplus-mobile-client/issues)
 - Making forks of the repo
 - Making pull request to add code or fix bugs
 - Staring or watching the repo
 - Sharing the repo!
## Splamei Network Notice
The Splamei Network is used to provide the following features to the client:
 - Checking for updates and alerting if so
 - Getting any notices and alerting if there's any new

The latest version of the client uses the 1.1 Splamei Network. This version does not save any data about the GET requests used.

## FAQ
### Q) What is SplameiPlay?
SplameiPlay is an indie store app for Windows allowing easy project (game / app) updating for Splamei and creators
### Q) Can older Windows versions run the client
Possibly. But the .NET Framework (4.7.2) needs to be installed and the Windows install needs to be able to run WebView2 which is likley not supported by Microsoft.
### Q) Can it run on ARM64 devices
No. No build for ARM64 has and, right now, will be made meaning you cannot play on ARM based CPUs
### Q) I want to make my own client. Can I do that?
Yes! Just make sure you follow everything stated in the 'Making your own Client' section
### Q) Is this client official?
At the current moment, no
## Socials
[YouTube](https://youtube.com/@splamei)
[Twitch](https://twitch.tv/splamei)
[Twitter](https://twitter.com/splamei)
[BlueSky](http://splamei.bsky.social/)
[Discord](https://discord.gg/g2KTP5X9At)
## Built with ❤️ in Visual Studio
