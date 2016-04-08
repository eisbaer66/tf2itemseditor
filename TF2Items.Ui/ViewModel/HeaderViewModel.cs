using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using TF2Items.Core;
using TF2Items.Parsers;

namespace TF2Items.Ui.ViewModel
{
    public class HeaderViewModel : ViewModelBase
    {
        private readonly ITf2ItemsWeaponsParser _parser;
        private readonly IConfigWeaponService _service;

        public HeaderViewModel(ITf2ItemsWeaponsParser parser, IConfigWeaponService service)
        {
            if (parser == null)
                throw new ArgumentNullException("parser");
            if (service == null)
                throw new ArgumentNullException("service");

            _parser = parser;
            _service = service;
            LoadCommand = new RelayCommand(Load);
            SaveCommand = new RelayCommand(Save, () => false);
        }

        public ICommand LoadCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        private void Load()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            bool? fileChosen = dialog.ShowDialog();

            if (!fileChosen.HasValue)
                return;
            if (!fileChosen.Value)
                return;

            string fileName = dialog.FileName;

            ServerConfiguration configuration = _parser.Parse(fileName);
            Result result = _service.Set(configuration);

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
            throw new System.NotImplementedException();
        }
    }
}