using System;
using System.Windows.Forms;
using TF2Items.Properties;


namespace TF2Items.Dialogs
{
    public partial class SettingsWindow : Form
    {
        private readonly ISteamService _steam;

        public SettingsWindow(ISteamService steam)
        {
            _steam = steam;

            InitializeComponent();
        }

        private void ChangeSteamappsFolder()
        {
            _steam.SetNewSteamFolder();
        }

        private void btnEditSteamappsFolder_Click(object sender, EventArgs e)
        {
            ChangeSteamappsFolder();
            txtSteamappsFolder.Text = Settings.Default.SteamFolder;
        }

        private void SettingsWindow_Load(object sender, EventArgs e)
        {
            Settings.Default.Upgrade();
            Settings.Default.Reload();
            txtSteamappsFolder.Text = Settings.Default.SteamFolder;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
