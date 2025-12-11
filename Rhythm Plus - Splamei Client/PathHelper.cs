using System;
using System.IO;

namespace Rhythm_Plus___Splamei_Client
{
    internal static class PathHelper
    {
        internal static bool isSafe(string path)
        {
            var fullPath = Path.GetFullPath(path);
            if (fullPath == null) return false;
            if (fullPath.StartsWith(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Splamei",
                    "Rhythm Plus - Splamei Client")))
            {
                return true;
            }

            return false;
        }
    }
}
