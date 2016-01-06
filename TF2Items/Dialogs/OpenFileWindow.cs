using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using TF2Items.Properties;


namespace TF2Items.Dialogs
{
    public partial class OpenFileWindow : Form
    {
        private string _filePath = "";
        private string _filename = "";
        private string _appFilePath = "";
        private readonly ISteamService _steam;

        public OpenFileWindow(ISteamService steam)
        {
            _steam = steam;

            InitializeComponent();
        }

        private void OpenFileDialog_Load(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
            radioManual.Checked = false;
            radioSteamapps.Checked = false;
            switch(Settings.Default.OpenFileSetting)
            {
                case 0:
                    //old option to load file from a .gcf
                    //now selects steamapps option instead
                    radioSteamapps.Checked = true;
                    btnNext.Enabled = true;
                    break;
                case 1:
                    radioSteamapps.Checked = true;
                    btnNext.Enabled = true;
                    break;
                case 2:
                    radioManual.Checked = true;
                    btnNext.Enabled = true;
                    break;
            }
            

        }

        public string ShowWindow(string filename, string path)
        {
            labelOpen.Text = "Open " + filename;
            _filename = filename;
            _appFilePath = path;
            Settings.Default.Upgrade();
            ShowDialog();
            Settings.Default.Save();
            return _filePath;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            if (radioSteamapps.Checked) 
            {
                filePath = GetFilePathFromSteamAppsFolder();
            }
            else if (radioManual.Checked)
            {
                filePath = GetFilePathManually();
            }

            if (string.IsNullOrEmpty(filePath))
            {
                _filePath = string.Empty;
                return;
            }

            _filePath = filePath;
            Close();
        }

        private string GetFilePathFromSteamAppsFolder()
        {
            Settings.Default.OpenFileSetting = 1;
            if (!_steam.EnsureSteamFolderIsSet())
                return null;

            string filePath = Settings.Default.SteamFolder + _appFilePath + _filename;
            if (!File.Exists(filePath))
            {
                Cursor = Cursors.Arrow;
                MessageBox.Show("Couldn't find " + _filename + "!\r\nPlease make sure it is present in " + filePath,
                    "TF2 Items Editor",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }

            return filePath;
        }

        private string GetFilePathManually()
        {
            Settings.Default.OpenFileSetting = 2;

            openFileDialog1.Filter = _filename.Split('.')[1] + " files|*." + _filename.Split('.')[1];
            return openFileDialog1.ShowDialog() == DialogResult.OK 
                ? openFileDialog1.FileName 
                : string.Empty;
        }

        private void radioSteamapps_CheckedChanged(object sender, EventArgs e)
        {
            btnNext.Enabled = true;
        }

        private void radioManual_CheckedChanged(object sender, EventArgs e)
        {
            btnNext.Enabled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _filePath = "";
            Close();
        }
    }
}
