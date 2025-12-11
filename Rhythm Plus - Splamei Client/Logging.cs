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
            var savePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Splamei",
                "Rhythm Plus - Splamei Client",
                "Client.log");

            if (!PathHelper.isSafe(savePath))
            {
                throw new UnauthorizedAccessException("Attempted to access or read a path outside of the bounds of the client's save directorys");
            }

            Debug.WriteLine(message);

            DateTime time = DateTime.Now;

            if (addStarter)
            {
                File.AppendAllLines(savePath, new List<string> { $"- {time.ToString("dd/MM/yyyy HH:mm")} - {message}"});
            }
            else
            {
                File.AppendAllLines(savePath, new List<string> { message });
            }
        }
    }
}
