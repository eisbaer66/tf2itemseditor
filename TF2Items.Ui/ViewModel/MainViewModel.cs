using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GalaSoft.MvvmLight;
using TF2Items.Core;
using TF2Items.Ui.Dispatch;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly ITf2WeaponService _itemsGameService;
        private readonly Func<Tf2WeaponViewModel> _getWeaponViewModel;

        private IList<Tf2WeaponViewModel> _allWeapons;
        private SmartCollection<Tf2WeaponViewModel, WeaponIdentifier> _weapons;
        private string _filter;

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

        public AsyncRelayCommand ReadAllCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ITf2WeaponService itemsGameService, Func<Tf2WeaponViewModel> getWeaponViewModel)
        {
            _itemsGameService = itemsGameService;
            _getWeaponViewModel = getWeaponViewModel;
            Weapons = new SmartCollection<Tf2WeaponViewModel, WeaponIdentifier>(vm => vm.Model.Id);
            ReadAllCommand = new AsyncRelayCommand(GetWeapons);
#if DEBUG
            if (IsInDesignMode)
            {
                Task.Run(() => ReadAllCommand.Execute(null)).Wait();
            }
#endif
        }

        private void FilterWeapons()
        {
            Func<Tf2WeaponViewModel, bool> matches = w => CultureInfo.InvariantCulture.CompareInfo.IndexOf(w.Name, _filter, CompareOptions.IgnoreCase) >= 0;
            List<Tf2WeaponViewModel> weaponsNotMatchingFilter = Weapons.Where(w => !matches(w)).ToList();
            Weapons.Remove(weaponsNotMatchingFilter);

            List<Tf2WeaponViewModel> weaponsMatchingFilter = _allWeapons.Where(w => matches(w) && !Weapons.Contains(w)).ToList();
            Weapons.AddRange(weaponsMatchingFilter);
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
            return _allWeapons;
        }
    }
}