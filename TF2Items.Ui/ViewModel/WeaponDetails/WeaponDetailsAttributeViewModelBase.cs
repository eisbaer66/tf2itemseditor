using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using TF2Items.Core;
using TF2Items.Ui.Services;
using TF2Items.Ui.ViewModel.Messenges;

namespace TF2Items.Ui.ViewModel
{
    public abstract class WeaponDetailsAttributeViewModelBase : ViewModelBase, IWeaponDetailsAttributeViewModel
    {
        private ConfigWeaponAttribute _weaponAttribute;
        private AttributeClass _class;
        private Tf2Attribute _attribute;
        private bool _editing;

        public virtual ConfigWeaponAttribute WeaponAttribute
        {
            get { return _weaponAttribute; }
            set
            {
                _weaponAttribute = value;
                RaisePropertyChanged(() => WeaponAttribute);
                Value = value.Value;
            }
        }

        public abstract string Value { get; set; }

        public AttributeClass Class
        {
            get { return _class; }
            set
            {
                _class = value;
                RaisePropertyChanged(() => Class);
            }
        }

        public Tf2Attribute Attribute
        {
            get { return _attribute; }
            set
            {
                _attribute = value;

                WeaponAttribute.Id = _attribute.Id;
                WeaponAttribute.Value = Value;

                RaisePropertyChanged(() => Attribute);
                RaisePropertyChanged(() => Image);
                RaisePropertyChanged(() => Format);
            }
        }

        public ICommand RemoveCommand { get; set; }
        public ICommand EditAttributeCommand { get; set; }
        public ICommand EndEditingCommand { get; set; }

        public bool Predefined { get { return _weaponAttribute.IsPredefined; } }

        public string Image
        {
            get
            {
                switch (Attribute.EffectType)
                {
                    case "positive":
                        return "pack://application:,,,/TF2Items.Ui;component/assets/icons/Pictogram_plus.png";
                    case "negative":
                        return "pack://application:,,,/TF2Items.Ui;component/assets/icons/Pictogram_minus.png";
                    default:
                        return "pack://application:,,,/TF2Items.Ui;component/assets/icons/Pictogram_neutral.png";
                }
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

        public WeaponDetailsAttributeViewModelBase(ITf2WeaponService service)
        {
            RemoveCommand = new RelayCommand(Remove, CanRemove);
            EditAttributeCommand = new RelayCommand(EditAttribute);
            EndEditingCommand = new RelayCommand(EndEditing);
#if DEBUG
            if (IsInDesignMode)
            {
                Task.Run(async () =>
                         {
                             IEnumerable<AttributeClass> attributeClasses = await service.GetAttributeClasses();
                             Class = attributeClasses.FirstOrDefault(c => c.Name.StartsWith("set_"));
                             WeaponAttribute = Class.GetDefaultWeaponAttribute();
                             Attribute = Class.Attributes[0];
                         }).Wait();
            }
#endif
        }

        private bool CanRemove()
        {
            return !Predefined;
        }

        private void Remove()
        {
            MessengerInstance.Send(new RemoveWeaponAttribute { Class = _class.Name });
        }

        private void EditAttribute()
        {
            MessengerInstance.Send(new EditWeaponAttribute { Class = _class.Name });
        }

        private void EndEditing()
        {
            Editing = false;
        }

        public bool Editing
        {
            get { return _editing; }
            set
            {
                _editing = value;
                RaisePropertyChanged(() => Editing);
                RaisePropertyChanged(() => IsFocused);
            }
        }

        public bool IsFocused
        {
            get { return _editing; }
        }
    }
}