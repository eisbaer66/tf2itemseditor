using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using TF2Items.Core;
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

        private ObservableCollection<Tf2WeaponViewModel> _weapons;

        public ObservableCollection<Tf2WeaponViewModel> Weapons
        {
            get { return _weapons; }
            set
            {
                _weapons = value;
                RaisePropertyChanged(() => Weapons);
            }
        }
        
        public RelayCommand ReadAllCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(ITf2WeaponService itemsGameService, Func<Tf2WeaponViewModel> getWeaponViewModel)
        {
            _itemsGameService = itemsGameService;
            _getWeaponViewModel = getWeaponViewModel;
            Weapons = new ObservableCollection<Tf2WeaponViewModel>();
            ReadAllCommand = new RelayCommand(GetWeapons);

            GetWeapons();

        }

        private void GetWeapons()
        {
            Weapons.Clear();
            foreach (Tf2Weapon weapon in _itemsGameService.Get())
            {
                Tf2WeaponViewModel weaponViewModel = _getWeaponViewModel();
                weaponViewModel.Model = weapon;
                Weapons.Add(weaponViewModel);
            }
        }
    }

    public class Tf2WeaponViewModel : ViewModelBase
    {
        private Tf2Weapon _model;

        public Tf2Weapon Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged(() => Model);
                RaisePropertyChanged(() => Name);
            }
        }

        public string Name
        {
            get { return _model.Name; }
        }

        public string Image
        {
            get { return @"C:\Users\Jan\Documents\GitHub\tf2itemseditor_vso\tmp\c_bet_rocketlauncher\c_bet_rocketlauncher_large.png"; }
        }
    }
}