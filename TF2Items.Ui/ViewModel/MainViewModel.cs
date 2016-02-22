using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using TF2Items.Ui.ViewModel.Messenges;

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
        private Tf2WeaponListViewModel _weaponList;
        private Tf2AttributeListViewModel _attributeList;
        private WeaponDetailsViewModel _weaponDetails;

        public Tf2WeaponListViewModel WeaponList
        {
            get { return _weaponList; }
            set
            {
                _weaponList = value;
                RaisePropertyChanged(() => WeaponList);
            }
        }

        public Tf2AttributeListViewModel AttributeList
        {
            get { return _attributeList; }
            set
            {
                _attributeList = value;
                RaisePropertyChanged(() => AttributeList);
            }
        }

        public WeaponDetailsViewModel WeaponDetails
        {
            get { return _weaponDetails; }
            set
            {
                _weaponDetails = value;
                RaisePropertyChanged(() => WeaponDetails);
            }
        }

        public bool ShowWeaponDetails
        {
            get { return _weaponDetails != null && _weaponDetails.Model != null; }
        }

        public MainViewModel(Tf2WeaponListViewModel weaponList, Tf2AttributeListViewModel attributeList, WeaponDetailsViewModel weaponDetails)
        {
            _weaponList = weaponList;
            _attributeList = attributeList;
            _weaponDetails = weaponDetails;

            Messenger.Default.Register<SelectWeapon>(this, SelectWeapon);
        }

        private void SelectWeapon(SelectWeapon msg)
        {
            WeaponDetails.Model = msg.Weapon;
            RaisePropertyChanged(() => ShowWeaponDetails);
        }
    }
}