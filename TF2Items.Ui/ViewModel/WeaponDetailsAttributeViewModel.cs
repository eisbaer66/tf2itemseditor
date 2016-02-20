using System;
using System.Diagnostics;
using System.Globalization;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using TF2Items.Core;
using TF2Items.Ui.ViewModel.Messenges;

namespace TF2Items.Ui.ViewModel
{
    public class WeaponDetailsAttributeViewModel :ViewModelBase
    {
        private AttributeClass _model;
        private Tf2Attribute _attribute;
        private Tf2WeaponAttribute _weaponAttribute;
        private float _value;

        public WeaponDetailsAttributeViewModel()
        {
            RemoveCommand = new RelayCommand(Remove, CanRemove);
        }

        private bool CanRemove()
        {
            return !Predefined;
        }

        private void Remove()
        {
            MessengerInstance.Send(new RemoveWeaponAttribute{Class = _model.Name});
        }

        public RelayCommand RemoveCommand { get; set; }

        public AttributeClass Model
        {
            get { return _model; }
            set
            {
                _model = value;
                RaisePropertyChanged(() => Model);

                WeaponAttribute = value.GetDefaultWeaponAttribute();
                Value = _weaponAttribute.Value;
            }
        }

        public Tf2WeaponAttribute WeaponAttribute
        {
            get { return _weaponAttribute; }
            set
            {
                _weaponAttribute = value;
                RaisePropertyChanged(() => WeaponAttribute);
            }
        }

        public string Value
        {
            get { return _value.ToString(CultureInfo.InvariantCulture); }
            set
            {
                float f;
                if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out f))
                    return;
                _value = f;
                RaisePropertyChanged(() => Value);

                Attribute = _model.Get(f);
            }
        }

        public string ValuePretty
        {
            get { return _model.Format(_value); }
            set
            {
                float f = _model.Deformat(value, _value);
                if (f == _value)
                    return;
                
                _value = f;

                RaisePropertyChanged(() => Value);

                Attribute = _model.Get(_value);
            }
        }

        public Tf2Attribute Attribute
        {
            get { return _attribute; }
            set
            {
                _attribute = value;
                RaisePropertyChanged(() => Attribute);
                RaisePropertyChanged(() => Format);
            }
        }

        public string Image
        {
            get
            {
                return Attribute.EffectType == "positive"
                    ? "pack://application:,,,/TF2Items.Ui;component/assets/icons/Pictogram_plus.png"
                    : "pack://application:,,,/TF2Items.Ui;component/assets/icons/Pictogram_minus.png";
            }
        }

        public string Format
        {
            get
            {
                switch (Attribute.Format)
                {
                    case "value_is_percentage":
                        return "%";
                    case "value_is_inverted_percentage":
                        return "%";
                    case "value_is_additive_percentage":
                        return "+%";
                    case "value_is_additive":
                        return "+";
                    case "value_is_or":
                        return "OR";
                    case "value_is_particle_index":
                        return "OR";
                    default:
                    {
                        Debug.WriteLine(Attribute.Format);
                        return Attribute.Format.Replace("_", Environment.NewLine);
                    }
                }
            }
        }

        public bool Predefined { get; set; }
    }
}