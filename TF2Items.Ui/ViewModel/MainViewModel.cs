using System;
using System.Threading.Tasks;
using AsyncMvvmMessenger;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using TF2Items.Core;
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
        private static readonly ILog Log = LogManager.GetLogger(typeof(MainViewModel));
        private readonly IConfigWeaponService _configWeaponService;
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
            get { return _weaponDetails != null && _weaponDetails.Tf2Weapon != null; }
        }

        public MainViewModel(Tf2WeaponListViewModel weaponList, Tf2AttributeListViewModel attributeList, WeaponDetailsViewModel weaponDetails, IConfigWeaponService configWeaponService)
        {
            _weaponList = weaponList;
            _attributeList = attributeList;
            _weaponDetails = weaponDetails;
            _configWeaponService = configWeaponService;

            Messenger.Default.Register<SelectWeapon>(this, SelectWeapon);
        }

        private async void SelectWeapon(SelectWeapon msg)
        {
            try
            {
                ConfigWeapon configWeapon = await _configWeaponService.GetConfigWeaponFor(msg.Weapon);

                WeaponDetails.UpdateModels(msg.Weapon, configWeapon);
                RaisePropertyChanged(() => ShowWeaponDetails);
            }
            catch (Exception e)
            {
                Log.Error("Error while selecting weapon", e);
            }
        }
    }
}