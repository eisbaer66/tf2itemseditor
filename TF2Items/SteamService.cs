using System.IO;
using System.Windows.Forms;
using TF2Items.Properties;

namespace TF2Items
{
    public interface ISteamService
    {
        bool EnsureSteamFolderIsSet();
        bool SetNewSteamFolder();
    }

    public class SteamService : ISteamService
    {
        public bool EnsureSteamFolderIsSet()
        {
            if (!string.IsNullOrEmpty(Settings.Default.SteamFolder))
                return true;
            return SetNewSteamFolder();
        }

        public bool SetNewSteamFolder()
        {

            string steamFolder;
            using (OpenFileDialog folderBrowserDialog1 = new OpenFileDialog
            {
                RestoreDirectory = true,
                CheckFileExists = true,
                FileName = "hl2.exe",
                Filter = "Team Fortress 2|hl2.exe",
            })
            {
                if (folderBrowserDialog1.ShowDialog() != DialogResult.OK)
                    return false;

                steamFolder = Path.GetDirectoryName(folderBrowserDialog1.FileName);
            }

            Settings.Default.SteamFolder = steamFolder;
            Settings.Default.Save();
            return true;
        }
    }
}