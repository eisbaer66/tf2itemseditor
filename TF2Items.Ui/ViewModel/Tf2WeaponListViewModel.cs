using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AsyncMvvmMessenger;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using TF2Items.Core;
using TF2Items.Ui.Dispatch;
using TF2Items.Ui.Services;
using TF2Items.Ui.ViewModel.Messenges;

namespace TF2Items.Ui.ViewModel
{
    public class Tf2WeaponListViewModel :ViewModelBase
    {

        private readonly ITf2WeaponService _itemsGameService;
        private readonly Func<Tf2WeaponViewModel> _getWeaponViewModel;

        private IList<Tf2WeaponViewModel> _allWeapons;
        private SmartCollection<Tf2WeaponViewModel, WeaponIdentifier> _weapons;
        private string _filter;

        public Tf2WeaponListViewModel(ITf2WeaponService itemsGameService, Func<Tf2WeaponViewModel> getWeaponViewModel)
        {
            _itemsGameService = itemsGameService;
            _getWeaponViewModel = getWeaponViewModel;

            Weapons = new SmartCollection<Tf2WeaponViewModel, WeaponIdentifier>(vm => vm.Model.Id);
            ReadAllWeaponsCommand = new AsyncRelayCommand(GetWeapons);
            SelectWeaponCommand = new AsyncRelayCommand<Tf2WeaponViewModel>(SelectWeapon);
#if DEBUG
            if (IsInDesignMode)
            {
                Task.Run(() =>
                {
                    ReadAllWeaponsCommand.Execute(null);
                }).Wait();
            }
#endif
        }

        private async Task SelectWeapon(Tf2WeaponViewModel vm)
        {
            if (vm == null)
                return;
            if (vm.Model == null)
                return;
            Messenger.Default.Send(new SelectWeapon { Weapon = vm.Model });
        }

        public AsyncRelayCommand ReadAllWeaponsCommand { get; set; }
        public AsyncRelayCommand<Tf2WeaponViewModel> SelectWeaponCommand { get; set; }

        public SmartCollection<Tf2WeaponViewModel, WeaponIdentifier> Weapons
        {
            get { return _weapons; }
            set
            {
                _weapons = value;
                RaisePropertyChanged(() => Weapons);
            }
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                FilterWeapons();
                RaisePropertyChanged(() => Filter);
            }
        }

        private async Task GetWeapons()
        {
            IEnumerable<Tf2WeaponViewModel> viewModels = await GetWeaponViewModels();

            Application.Current.Dispatcher.Invoke(() => Weapons.SmartReset(viewModels, (o, n) => o.Model = n.Model));
        }

        private async Task<IEnumerable<Tf2WeaponViewModel>> GetWeaponViewModels()
        {
            _allWeapons = (await _itemsGameService.Get())
                .Select(w =>
                {
                    Tf2WeaponViewModel weaponViewModel = _getWeaponViewModel();
                    weaponViewModel.Model = w;
                    return weaponViewModel;
                })
                .ToList();
#if DEBUG
            if (IsInDesignMode)
            {
                Tf2WeaponViewModel tf2WeaponViewModel = _allWeapons.FirstOrDefault(w => w.Model.Id.Id == 35);
                SelectWeapon(tf2WeaponViewModel);
            }
#endif
            return _allWeapons;
        }

        private void FilterWeapons()
        {
            Func<Tf2WeaponViewModel, bool> matches = w => CultureInfo.InvariantCulture.CompareInfo.IndexOf(w.Name, _filter, CompareOptions.IgnoreCase) >= 0;
            List<Tf2WeaponViewModel> weaponsNotMatchingFilter = Weapons.Where(w => !matches(w)).ToList();
            Weapons.Remove(weaponsNotMatchingFilter);

            List<Tf2WeaponViewModel> weaponsMatchingFilter = _allWeapons.Where(w => matches(w) && !Weapons.Contains(w)).ToList();
            Weapons.AddRange(weaponsMatchingFilter);
        }
    }
}