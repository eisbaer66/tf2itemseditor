using System.IO;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using TF2Items.Core;
using TF2Items.Ui.Dispatch;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public class Tf2WeaponViewModel : ViewModelBase
    {
        private readonly IWeaponIconService _iconService;
        private Tf2Weapon _model;

        public Tf2Weapon Model
        {
            get { return _model; }
            set
            {
                _model = value;
                Image = new RunNotifyTaskCompletion<string>(GetImage);

                RaisePropertyChanged(() => Model);
                RaisePropertyChanged(() => Name);
            }
        }

        private async Task<string> GetImage()
        {
            string iconPath = await _iconService.Get(_model);
            if (string.IsNullOrEmpty(iconPath))
                return "/TF2Items.Ui;component/assets/icons/error.png";
            return Path.Combine(Directory.GetCurrentDirectory(), iconPath);
        }

        public string Name
        {
            get { return _model.Name; }
        }

        public RunNotifyTaskCompletion<string> Image { get; private set; }

        public Tf2WeaponViewModel(IWeaponIconService iconService)
        {
            _iconService = iconService;
        }
    }
}