using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using TF2Items.Core;
using TF2Items.Ui.Dispatch;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public class WeaponDetailsViewModel : ViewModelBase, IDropTarget
    {
        private Tf2Weapon _model;
        private readonly IWeaponIconService _weaponIconService;
        private readonly ITf2WeaponService _attributeService;
        private readonly Func<WeaponDetailsAttributeViewModel> _getVm;
        private SmartCollection<WeaponDetailsAttributeViewModel, string> _attributes;
        private IDictionary<string, AttributeClass> _attributeClasses;

        public WeaponDetailsViewModel(IWeaponIconService weaponIconService, ITf2WeaponService attributeService, Func<WeaponDetailsAttributeViewModel> getVm)
        {
            _weaponIconService = weaponIconService;
            _attributeService = attributeService;
            _getVm = getVm;

            Attributes = new SmartCollection<WeaponDetailsAttributeViewModel, string>(vm => vm.Model.Name);
            ReadAttributesCommand = new AsyncRelayCommand(GetAttributes);
#if DEBUG
            if (IsInDesignMode)
            {
                Task.Run(() =>
                {
                    ReadAttributesCommand.Execute(null);
                }).Wait();
            }
#endif
        }

        public Tf2Weapon Model
        {
            get { return _model; }
            set
            {
                _model = value;
                Image = new RunNotifyTaskCompletion<string>(GetImage);
                ReadAttributesCommand.Execute(null);
                
                RaisePropertyChanged(() => Model);
                RaisePropertyChanged(() => Image);
            }
        }

        private async Task GetAttributes()
        {
            if (Model == null)
                return;

            _attributeClasses = await _attributeService.GetAttributeClassesAsDictionary();
            IEnumerable<WeaponDetailsAttributeViewModel> viewModels = Model.Attributes.Select(a =>
                                                                                   {
                                                                                       AttributeClass attribute = _attributeClasses[a.Class];

                                                                                       WeaponDetailsAttributeViewModel vm = _getVm();
                                                                                       vm.Model = attribute;
                                                                                       return vm;
                                                                                   });

            Application.Current.Dispatcher.Invoke(() => Attributes.SmartReset(viewModels, (o, n) => {}));
        }

        private async Task<string> GetImage()
        {
            return await _weaponIconService.Get(_model);
        }

        public AsyncRelayCommand ReadAttributesCommand { get; set; }

        public RunNotifyTaskCompletion<string> Image { get; private set; }

        public SmartCollection<WeaponDetailsAttributeViewModel, string> Attributes
        {
            get { return _attributes; }
            set
            {
                _attributes = value;
                RaisePropertyChanged(() => Attributes);
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (_attributeClasses == null)
                return;
            Tf2AttributeViewModel tf2AttributeViewModel = dropInfo.Data as Tf2AttributeViewModel;
            if (tf2AttributeViewModel == null)
                return;

            bool attributeAlreadyExists = Attributes.Any(vm => vm.Model.Name == tf2AttributeViewModel.Model.Class);
            if (attributeAlreadyExists)
                return;

            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (_attributeClasses == null)
                return;
            Tf2AttributeViewModel tf2AttributeViewModel = dropInfo.Data as Tf2AttributeViewModel;
            if (tf2AttributeViewModel == null)
                return;

            WeaponDetailsAttributeViewModel vm = _getVm();
            vm.Model = tf2AttributeViewModel.Class;
            Attributes.Add(vm);
        }
    }

    public class WeaponDetailsAttributeViewModel :ViewModelBase
    {
        private AttributeClass _model;
        private Tf2Attribute _attribute;
        private Tf2WeaponAttribute _weaponAttribute;
        private string _value;

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
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged(() => Value);

                float f;
                if (float.TryParse(value, out f))
                    Attribute = _model.Get(f);
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

    }
}