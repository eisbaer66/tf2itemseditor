using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
        private Tf2WeaponListViewModel _weaponList;
        private Tf2AttributeListViewModel _attributeList;

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

        public MainViewModel(Tf2WeaponListViewModel weaponList, Tf2AttributeListViewModel attributeList)
        {
            _weaponList = weaponList;
            _attributeList = attributeList;
        }
    }
}