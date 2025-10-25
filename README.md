
# Rhythm Plus - Splamei Client (PC)
## If your looking for the Android Client, find it [here](https://github.com/splamei/rplus-mobile-client)
Another Splamei custom client for the [Rhythm Plus music game](https://rhythm-plus.com) that now allows playing on Windows with some nice extra features!

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
- A 64-bit version of Windows 10 or 11
- .NET Framework 4.7.2 (Should be preinstalled)
- Also you need an active internet connection and to be able to access the Rhythm Plus website (rhythm-plus.com) and the Splamei Network (veemo.uk)

## Installation
It's best to play through SplameiPlay to have the client automatically updated when a new update is pushed. However, if you don't want to use SplameiPlay, you can use the portable version (the raw files) or installer (installs the files for ease of use)

### Recommended / Easy Method (SplameiPlay)

 1. Download and install [SplameiPlay](https://www.veemo.uk/splameiplay/download) if not installed. (You must be using 4.0 Bloom or later to have the client in the store)
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

Note: You **must** have the Discord Desktop app installed and running rather than the website app. If you launch the client and it failed to reach Discord (usually due to Discord being closed), DiscordRP will be disabled automatically. Settings for Rich Prescence can be changed in the setting window.
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
 - Replace or remove the update handling (and notice feature if you want)
 - Change the package name
 - If the client isn't for Rhythm Plus, please change the icons the client uses
 - Because this client is currently under the MIT Licence, you **must** acknowledge the copyright and MIT Licence
## Editing and building
This section presumes you have the .NET SDKs, Git and Visual Studio installed and a basic knowledge of them for building and editing of the code. You don't need Visual Studio but it's recommended to make it easier to manage the files.

Once you have Visual Studio and select 'Clone a repository' and paste the repository location (``https://github.com/splamei/rplus-pc-client.git``) in the required field. After pressing 'Clone', you should now have access to client's files for easy editing and building.
## Contributing
I would love for people to help the clients development so any little contribution would go a long way!
You could contribute by:
 - Reporting issues / feature requests on the [issue page](https://github.com/splamei/rplus-mobile-client/issues)
 - Making forks of the repo
 - Making pull request to add code or fix bugs
 - Staring or watching the repo
 - Sharing the repo!

## FAQ
### Q) What is SplameiPlay?
SplameiPlay is an indie store app for Windows allowing easy project (game / app) updating for Splamei and creators
### Q) Can older Windows versions run the client
Possibly. But the .NET Framework 4.7.2 needs to be installed and the Windows install needs to be able to run WebView2 which would likely not supported by Microsoft. So the chances you can get it running are very slim.
### Q) Can it run on ARM64 or 32-bit devices
No. No build for ARM64 and 32-bit has and, right now, will be made meaning you cannot play on those CPUs however, you may be able to build the client yourself to do this
### Q) I want to make my own client. Can I do that?
Yes! Just make sure you follow everything stated in the 'Making your own Client' section
### Q) Is this client official?
At the current moment, no
### Q) Wasn't this repo around before?
Yes, the repo has had to be remade several times for many issues. Don't worry! I think their all fixed now
## Socials
[YouTube](https://youtube.com/@splamei)
[Twitch](https://twitch.tv/splamei)
[Twitter](https://twitter.com/splamei)
[BlueSky](http://splamei.bsky.social/)
[Discord](https://discord.gg/g2KTP5X9At)
## Built with ❤️ in Visual Studio
