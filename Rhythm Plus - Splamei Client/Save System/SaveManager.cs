using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Rhythm_Plus___Splamei_Client.Save_System
{
    public class SaveManager
    {
        private int mySaveVer = 1000;

        private int saveVersion = 1000;
        private Version assemblyVersion = new Version();

        private List<string> dataNames = new List<string>();
        private List<string> dataValues = new List<string>();

        private List<string> dataUpgradable = new List<string>()
        { "checkCode", "debugMode", "directLinkRP", "discordRpRefresh", "enabledRP", "enableExtensions", "locationX", "locationY",
            "notice", "played", "playFullScreen", "retainWindowSize", "showMenuIn", "showTitleOfMapsRP", "sizeHeight", "sizeHeight",
            "sizeWidth", "sizeState", "viewZoom" };

        public void loadData()
        {
            string savePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Splamei",
                "Rhythm Plus - Splamei Client",
                "data.sav");

            if (File.Exists(savePath))
            {
                string[] data = File.ReadAllLines(savePath);

                saveVersion = int.Parse(data[0]);
                assemblyVersion = new Version(data[1]);

                bool isReading = false;
                for (int i = 3; i < data.Length; i++)
                {
                    if (isReading)
                    {
                        dataValues.Add(data[i]);
                        isReading = false;
                    }
                    else
                    {
                        dataNames.Add(data[i]);
                        isReading = true;
                    }
                }

                if (dataNames.Count != dataValues.Count)
                {
                    using (Error error = new Error())
                    {
                        error.errorDebug = "Lists for save data names and values are not equal";
                        error.shouldClose = true;
                        error.ShowDialog();
                    }
                }
            }
            else
            {
                saveData();
            }
        }

        public void saveData()
        {
            string savePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Splamei",
                "Rhythm Plus - Splamei Client",
                "data.sav");

            List<string> data = new List<string>();

            data.Add(mySaveVer.ToString());
            data.Add(Assembly.GetEntryAssembly().GetName().Version.ToString() + "\n");

            for (int i = 0; i < dataNames.Count; i++)
            {
                data.Add(dataNames[i]);
                data.Add(dataValues[i]);
            }

            File.WriteAllLines(savePath, data);
        }

        public void upgradeData()
        {
            string savePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Splamei",
                "Rhythm Plus - Splamei Client");

            if (File.Exists(Path.Combine(savePath, "checkcode.dat")))
            {
                Logging.logString("Upgraded data!");

                foreach (string name in dataUpgradable)
                {
                    if (File.Exists(Path.Combine(savePath, $"{name}.dat")))
                    {
                        string result = File.ReadAllText(Path.Combine(savePath, $"{name}.dat"));

                        setString(name, result);

                        File.Delete(Path.Combine(savePath, $"{name}.dat"));
                    }
                }
            }
            else
            {
                Logging.logString("Not upgrading data");
            }
        }

        public bool dataExist(string name)
        {
            return dataNames.Contains(name);
        }

        public int getInt(string name)
        {
            try
            {
                if (dataNames.Contains(name))
                {
                    return int.Parse(dataValues[dataNames.IndexOf(name)]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logging.logString($"Failed to get int value '{name}'. Returning default - {ex}");
                return 0;
            }
        }

        public void setInt(string name, int value)
        {
            try
            {
                if (dataNames.Contains(name))
                {
                    dataValues[dataNames.IndexOf(name)] = value.ToString();
                }
                else
                {
                    dataNames.Add(name);
                    dataValues.Add(value.ToString());
                }
            }
            catch (Exception ex)
            {
                Logging.logString($"Failed to add int '{name}'! - {ex}");
            }
        }

        public float getFloat(string name)
        {
            try
            {
                if (dataNames.Contains(name))
                {
                    return float.Parse(dataValues[dataNames.IndexOf(name)]);
                }
                else
                {
                    return 0f;
                }
            }
            catch (Exception ex)
            {
                Logging.logString($"Failed to get float value '{name}'. Returning default - {ex}");
                return 0f;
            }
        }

        public void setFloat(string name, float value)
        {
            try
            {
                if (dataNames.Contains(name))
                {
                    dataValues[dataNames.IndexOf(name)] = value.ToString();
                }
                else
                {
                    dataNames.Add(name);
                    dataValues.Add(value.ToString());
                }
            }
            catch (Exception ex)
            {
                Logging.logString($"Failed to add float '{name}'! - {ex}");
            }
        }

        public string getString(string name)
        {
            try
            {
                if (dataNames.Contains(name))
                {
                    return dataValues[dataNames.IndexOf(name)];
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                Logging.logString($"Failed to get string value '{name}'. Returning default - {ex}");
                return "";
            }
        }

        public void setString(string name, string value)
        {
            try
            {
                if (dataNames.Contains(name))
                {
                    dataValues[dataNames.IndexOf(name)] = value;
                }
                else
                {
                    dataNames.Add(name);
                    dataValues.Add(value);
                }
            }
            catch (Exception ex)
            {
                Logging.logString($"Failed to add string '{name}'! - {ex}");
            }
        }
    }
}
