using System;
using System.Diagnostics;
using System.Globalization;
using GalaSoft.MvvmLight.Command;
using TF2Items.Core;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    [DebuggerDisplay("{Class.Name}: {Value}")]
    public class WeaponDetailsNumericalAttributeViewModel : WeaponDetailsAttributeViewModelBase
    {
        private float _value;
        public override string Value
        {
            get { return _value.ToString(CultureInfo.InvariantCulture); }
            set
            {
                float f;
                if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out f))
                    return;

                _value = f;
                RaisePropertyChanged(() => Value);
                RaisePropertyChanged(() => ValuePretty);

                Attribute = Class.Get(f, WeaponAttribute.Id);
            }
        }

        public string ValuePretty
        {
            get { return Class.Format(_value); }
            set
            {
                float f = Class.Deformat(value, _value);
                if (f == _value)
                    return;
                
                _value = f;
                RaisePropertyChanged(() => Value);
                RaisePropertyChanged(() => ValuePretty);

                Attribute = Class.Get(_value, Attribute.Name);
            }
        }

        public WeaponDetailsNumericalAttributeViewModel(ITf2WeaponService service)
            : base(service)
        {}
    }

    public class WeaponDetailsSetAttributeOptionViewModel
    {
        private Action<Tf2Attribute> _select;

        public RelayCommand SelectCommand { get; set; }
        public Tf2Attribute Attribute { get; set; }

        public WeaponDetailsSetAttributeOptionViewModel()
        {
            SelectCommand = new RelayCommand(() => { _select(Attribute); });
        }

        public static WeaponDetailsSetAttributeOptionViewModel From(Tf2Attribute attribute, Action<Tf2Attribute> select)
        {
            return new WeaponDetailsSetAttributeOptionViewModel
                   {
                       _select = @select,
                       Attribute = attribute,
                   };
        }
    }
}