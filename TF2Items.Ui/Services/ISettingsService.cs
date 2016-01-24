using System.IO;
using TF2Items.Core;

namespace TF2Items.Ui.Services
{
    public interface ISettingsService
    {
        string ItemsGameTxt { get; }
    }

    class SettingsService : ISettingsService
    {
        private readonly ISteamConfig _steamConfig;

        public SettingsService(ISteamConfig steamConfig)
        {
            _steamConfig = steamConfig;
        }

        public string ItemsGameTxt { get { return Path.Combine(_steamConfig.TeamFortress2Directory, "tf", "scripts", "items", "items_game.txt"); }}
    }
}