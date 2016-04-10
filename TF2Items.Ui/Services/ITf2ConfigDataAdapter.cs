using System;
using System.IO;
using System.Linq;
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
        Result Save();
        Result Save(string saveTo);
    }

    class Tf2ConfigDataAdapter : ITf2ConfigDataAdapter
    {
        private readonly ITf2ItemsWeaponsParser _parser;
        private readonly IConfigWeaponService _service;
        private readonly IUserSettings _userSettings;
        private readonly ITf2ItemsWeaponsWriter _writer;

        public Tf2ConfigDataAdapter(ITf2ItemsWeaponsParser parser, IConfigWeaponService service, IUserSettings userSettings, ITf2ItemsWeaponsWriter writer)
        {
            if (parser == null)
                throw new ArgumentNullException("parser");
            if (service == null)
                throw new ArgumentNullException("service");
            if (userSettings == null)
                throw new ArgumentNullException("userSettings");
            if (writer == null)
                throw new ArgumentNullException("writer");
            _parser = parser;
            _service = service;
            _userSettings = userSettings;
            _writer = writer;
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


            ServerConfiguration configuration = _parser.Parse(fileName);
            if (configuration == null)
                return Result.Failed("no configuration present");

            WeaponCollection weapons = GetWeaponsForAnyUser(configuration);
            Result result = _service.Set(weapons);

            if (result.Success)
                _userSettings.LoadFrom = fileName;

            return result;
        }

        public Result Save()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            bool? locationChosen = dialog.ShowDialog();

            if (!locationChosen.HasValue)
                return Result.UserAborted();
            if (!locationChosen.Value)
                return Result.UserAborted();

            string fileName = dialog.FileName;
            return Save(fileName);
        }

        public Result Save(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return Result.UserAborted();

            ServerConfiguration configuration = GetServerConfiguration(fileName);

            //remove existing WeaponCollection so we can add our WeaponCollection later
            WeaponCollection existingWeapons = GetWeaponsForAnyUser(configuration);
            if (existingWeapons != null)
                configuration.WeaponCollections.Remove(existingWeapons);

            //get our WeaponCollection and add it to the ServerConfiguration
            WeaponCollection weapons = _service.Get();
            configuration.WeaponCollections.Add(weapons);

            NomalizeServerConfig(configuration);

            _writer.Write(configuration, fileName);

            _userSettings.SaveTo = fileName;
            return Result.Successfull();
        }

        private void NomalizeServerConfig(ServerConfiguration configuration)
        {
            //move WeaponCollection for "any User" to the back of the list
            WeaponCollection weapons = GetWeaponsForAnyUser(configuration);
            if (weapons != null)
            {
                configuration.WeaponCollections.Remove(weapons);
                configuration.WeaponCollections.Add(weapons);
            }

            //within each WeaponCollection move ConfigWeapon for "any Weapon" to the back of the list
            foreach (WeaponCollection collection in configuration.WeaponCollections)
            {
                ConfigWeapon anyWeapon = collection.Weapons.FirstOrDefault(w => w.Id.Id == null);
                if (anyWeapon == null)
                    continue;
                collection.Weapons.Remove(anyWeapon);
                collection.Weapons.Add(anyWeapon);
            }
        }

        private ServerConfiguration GetServerConfiguration(string fileName)
        {
            return File.Exists(fileName)
                ? (_parser.Parse(fileName)
                   ?? new ServerConfiguration())
                : new ServerConfiguration();
        }

        WeaponCollection GetWeaponsForAnyUser(ServerConfiguration configuration)
        {
            return configuration.WeaponCollections.FirstOrDefault(c => c.Users.IsAny());
        }
    }
}