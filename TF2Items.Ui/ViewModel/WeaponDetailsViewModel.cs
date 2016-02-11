using System.ComponentModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using TF2Items.Core;
using TF2Items.Ui.Dispatch;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public class WeaponDetailsViewModel : ViewModelBase
    {
        private Tf2Weapon _model;
        private readonly IWeaponIconService _weaponIconService;

        public WeaponDetailsViewModel(IWeaponIconService weaponIconService)
        {
            _weaponIconService = weaponIconService;
        }

        public Tf2Weapon Model
        {
            get { return _model; }
            set
            {
                _model = value;
                Image = new RunNotifyTaskCompletion<string>(GetImage);
                
                RaisePropertyChanged(() => Model);
                RaisePropertyChanged(() => Image);
            }
        }

        private async Task<string> GetImage()
        {
            return await _weaponIconService.Get(_model);
        }

        public RunNotifyTaskCompletion<string> Image { get; private set; }
    }
}