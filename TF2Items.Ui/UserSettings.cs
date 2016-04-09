using System.ComponentModel;
using System.Configuration;
using GalaSoft.MvvmLight;
using TF2Items.Ui.Properties;

namespace TF2Items.Ui
{
    public interface IUserSettings :INotifyPropertyChanged
    {
        bool ReloadOnStartup { get; set; }
        string ConfigPath { get; set; }
        bool ReuseLastSaveLocation { get; set; }
    }

    public class UserSettings : ObservableObject, IUserSettings
    {
        private readonly ApplicationSettingsBase _settings;

        public UserSettings()
            :this(Settings.Default)
        { }
        public UserSettings(ApplicationSettingsBase settings)
        {
            _settings = settings;
        }

        public bool ReloadOnStartup
        {
            get { return (bool)_settings["ReloadConfigOnStartup"]; }
            set
            {
                _settings["ReloadConfigOnStartup"] = value;
                RaisePropertyChanged(() => ReloadOnStartup);
            }
        }

        public string ConfigPath
        {
            get { return (string)_settings["ConfigPath"]; }
            set
            {
                _settings["ConfigPath"] = value;
                RaisePropertyChanged(() => ConfigPath);
            }
        }

        public bool ReuseLastSaveLocation
        {
            get { return (bool)_settings["ReuseLastSaveLocation"]; }
            set
            {
                _settings["ReuseLastSaveLocation"] = value;
                RaisePropertyChanged(() => ReuseLastSaveLocation);
            }
        }
    }
}