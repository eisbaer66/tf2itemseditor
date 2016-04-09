using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public class HeaderViewModel : ViewModelBase
    {
        private readonly IUserSettings _userSettings;
        private readonly ITf2ConfigDataAdapter _dataAdapter;

        public HeaderViewModel(IUserSettings userSettings, ITf2ConfigDataAdapter dataAdapter)
        {
            if (userSettings == null)
                throw new ArgumentNullException("userSettings");
            if (dataAdapter == null)
                throw new ArgumentNullException("dataAdapter");

            _userSettings = userSettings;
            _dataAdapter = dataAdapter;
            LoadCommand = new RelayCommand(Load);
            SaveCommand = new RelayCommand(Save);
        }

        public ICommand LoadCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public IUserSettings Settings
        {
            get { return _userSettings; }
        }

        private void Load()
        {
            Result result = _dataAdapter.Load();
            if (result.UserAbort)
                return;

            ToastMessage message = CreateToastMessage(result);
            MessengerInstance.Send(message);
        }

        private static ToastMessage CreateToastMessage(Result result)
        {
            if (result.Success)
            {
                return new ToastMessage
                       {
                           Caption = "configuration loaded",
                           Text = "The configuration was loaded successfully."
                       };
            }

            string text = string.Format("The selected file could not be loaded.\r\nReason: {0}", result.Reason);
            string caption = "failed loading configuration";
            return new ToastMessage
                   {
                       Caption = caption,
                       Text = text,
                   };
        }

        private void Save()
        {
            Result result = _dataAdapter.Save();
            if (result.UserAbort)
                return;

            ToastMessage toast = CreateSaveToast(result);
            MessengerInstance.Send(toast);
        }

        private ToastMessage CreateSaveToast(Result result)
        {
            if (result.Success)
            {
                return new ToastMessage
                {
                    Caption = "configuration saved",
                    Text = "The configuration was saved successfully."
                };
            }

            string text = string.Format("The selected file could not be saved.\r\nReason: {0}", result.Reason);
            string caption = "failed saving configuration";
            return new ToastMessage
                   {
                       Caption = caption,
                       Text = text,
                   };
        }
    }
}