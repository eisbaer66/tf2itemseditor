using System.ComponentModel;
using System.Configuration;
using GalaSoft.MvvmLight;
using TF2Items.Ui.Properties;

namespace TF2Items.Ui
{
    public interface IUserSettings :INotifyPropertyChanged
    {
        bool ReloadOnStartup { get; set; }
        string LoadFrom { get; set; }
        bool ReuseLastSaveLocation { get; set; }
        string SaveTo { get; set; }
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

        public string LoadFrom
        {
            get { return (string)_settings["LoadFrom"]; }
            set
            {
                _settings["LoadFrom"] = value;
                RaisePropertyChanged(() => LoadFrom);
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

        public string SaveTo
        {
            get { return (string)_settings["SaveTo"]; }
            set
            {
                _settings["SaveTo"] = value;
                RaisePropertyChanged(() => SaveTo);
            }
        }
    }
}