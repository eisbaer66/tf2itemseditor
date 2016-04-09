using System;
using Microsoft.Win32;
using TF2Items.Core;
using TF2Items.Parsers;
using TF2Items.Ui.ViewModel;

namespace TF2Items.Ui.Services
{
    public interface ITf2ConfigDataAdapter
    {
        Result Load();
        Result Load(string configPath);
    }

    class Tf2ConfigDataAdapter : ITf2ConfigDataAdapter
    {
        private readonly ITf2ItemsWeaponsParser _parser;
        private readonly IConfigWeaponService _service;
        private readonly IUserSettings _userSettings;

        public Tf2ConfigDataAdapter(ITf2ItemsWeaponsParser parser, IConfigWeaponService service, IUserSettings userSettings)
        {
            if (parser == null)
                throw new ArgumentNullException("parser");
            if (service == null)
                throw new ArgumentNullException("service");
            if (userSettings == null)
                throw new ArgumentNullException("userSettings");
            _parser = parser;
            _service = service;
            _userSettings = userSettings;
        }

        public Result Load()
        {

            OpenFileDialog dialog = new OpenFileDialog();
            bool? fileChosen = dialog.ShowDialog();

            if (!fileChosen.HasValue)
                return Result.UserAborted();
            if (!fileChosen.Value)
                return Result.UserAborted();

            string fileName = dialog.FileName;

            return Load(fileName);
        }

        public Result Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return Result.UserAborted();

            SaveFileNameToUserSetting(fileName);

            ServerConfiguration configuration = _parser.Parse(fileName);
            Result result = _service.Set(configuration);

            return result;
        }

        private void SaveFileNameToUserSetting(string fileName)
        {
            _userSettings.ConfigPath = fileName;
        }
    }
}