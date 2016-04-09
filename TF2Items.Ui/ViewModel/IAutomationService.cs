using System;
using GalaSoft.MvvmLight.Messaging;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public interface IAutomationService
    {
        void Startup();
    }

    class AutomationService : IAutomationService
    {
        private readonly UserSettings _settings;
        private readonly ITf2ConfigDataAdapter _configDataAdapter;
        private readonly IMessenger _messenger;

        public AutomationService(UserSettings settings, ITf2ConfigDataAdapter configDataAdapter, IMessenger messenger)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");
            if (configDataAdapter == null)
                throw new ArgumentNullException("configDataAdapter");
            if (messenger == null)
                throw new ArgumentNullException("messenger");
            _settings = settings;
            _configDataAdapter = configDataAdapter;
            _messenger = messenger;
        }

        public void Startup()
        {
            ReloadConfig();
        }

        private void ReloadConfig()
        {
            if (!_settings.ReloadOnStartup)
                return;

            Result result = _configDataAdapter.Load(_settings.ConfigPath);

            if (result.Success || result.UserAbort)
                return;

            ToastMessage toastMessage = new ToastMessage
                                        {
                                            Caption = "failed automatically loading configuration",
                                            Text = result.Reason,
                                        };
            _messenger.Send(toastMessage);
        }
    }
}