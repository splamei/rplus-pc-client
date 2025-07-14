using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Rhythm_Plus___Splamei_Client
{
    public partial class Extentions: Form
    {
        public Form1 form;

        public Extentions()
        {
            InitializeComponent();
        }

        private void Extentions_Load(object sender, EventArgs e)
        {
            //checkedListBox1.Items.Clear();
        }

        public void loadExtentions(List<string> names, List<string> ids, List<bool> enabled)
        {
            Logging.logString(names[0]);

            for (int i = 0; i < names.Count; i++)
            {
                checkedListBox1.Items.Add($"{names[i]} (ID: {ids[i]})", false);
                //Logging.logString(names[i]);
            }

            //Logging.logString(checkedListBox1.Items[0].ToString());
            checkedListBox1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                if (openFileDialog1.FileName != "" && openFileDialog1.FileName.EndsWith("manifest.json"))
                {
                    if (MessageBox.Show("Do you want to continue to install the extension? By installing it, you allow the extension to modify and see data about Rhythm Plus which could get you banned or hacked. When installing, the unpacked folder will be moved to the extension directory.\n\nDo you wish to continue? Rhythm Plus will be reloaded to allow ther extention to be added.", "Add extension", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        string dirName = Directory.GetParent(openFileDialog1.FileName).Name;
                        if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Extensions/" + dirName))
                        {
                            Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Extensions/" + dirName, true);
                        }

                        CopyDirectory(Directory.GetParent(openFileDialog1.FileName).ToString(), Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Extensions/" + dirName, true);

                        if (form.addExtention(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Extensions/" + dirName))
                        {
                            MessageBox.Show("The extension has been added. To remove it, go into the extensions page and select it when in 'remove' mode.", "Added extension", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.Close();

                            form.WindowState = FormWindowState.Maximized;
                            form.Focus();
                        }
                        else
                        {
                            MessageBox.Show($"An error occured while adding the extension. We are sorry for the issue", "Error adding extension", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.logString(ex.ToString());

                MessageBox.Show($"An error occured while adding the extension. We are sorry for the issue\n\nError: {ex.Message}", "Error adding extension", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Extentions_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!checkedListBox1.CheckOnClick)
            {
                if (MessageBox.Show("Your about to enter extension removal mode. Whatever extensions you check will then be removed and must be readded. Do you wish to continue?", "Remove extension", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    button1.Enabled = false;
                    checkedListBox1.CheckOnClick = true;
                    checkedListBox1.Enabled = true;

                    label1.Text = "Check the extensions you\r\nwant to remove then press\r\n(-) again to remove them";
                }
            }
            else
            {
                try
                {
                    var selected = checkedListBox1.CheckedItems;

                    List<string> extension = new List<string>();

                    for (int i = 0; i < selected.Count; i++)
                    {
                        string[] id1 = selected[i].ToString().Split(new string[] { " (ID: " }, StringSplitOptions.None);
                        string[] id2 = id1[id1.Length - 1].ToString().Split(new string[] { ")" }, StringSplitOptions.None);

                        extension.Add(id1[0]);

                        Logging.logString($"Removing extension with ID: {id2[0]}");

                        form.removeExtention(id2[0]);
                    }

                    string[] directorys = Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Splamei/Rhythm Plus - Splamei Client/Extensions/");

                    foreach (string directory in directorys)
                    {
                        string fileData = File.ReadAllText(directory + "/manifest.json");

                        foreach (string extensionName in extension)
                        {
                            if (fileData.Contains($"\"name\": \"{extensionName}\","))
                            {
                                //Logging.logString("AAAAAAA " + directory);
                                Directory.Delete(directory, true);
                            }
                        }
                    }

                    if (form.failedToRemoveExtension)
                    {
                        MessageBox.Show("There was an error removing some of the extensions you selected. Please try again later", "Failed to remove Extension(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        this.Close();
                    }
                    else
                    {
                        if (MessageBox.Show("The extensions you wanted to remove have been removed.\n\nWould you like Rhythm Plus to be reloaded for you? You may loose your current progress if you do", "Removed Extension(s)", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        {
                            form.refreshWebView();
                        }

                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                    MessageBox.Show("There was an error removing some of the extensions you selected. Please try again later", "Failed to remove Extension(s)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
        }
    }
}
