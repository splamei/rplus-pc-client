using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace Rhythm_Plus___Splamei_Client
{
    public static class Logging
    {
        public static bool addStarter = false;

        public static void logString(string message)
        {
            Debug.WriteLine(message);

            if (addStarter)
            {
                File.AppendAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Client.log", new List<string> { "- " + message });
            }
            else
            {
                File.AppendAllLines(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Client.log", new List<string> { message });
            }
        }
    }
}
