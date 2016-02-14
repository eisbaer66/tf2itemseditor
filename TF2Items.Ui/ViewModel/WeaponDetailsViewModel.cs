using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using TF2Items.Core;
using TF2Items.Ui.Dispatch;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public class WeaponDetailsViewModel : ViewModelBase
    {
        private Tf2Weapon _model;
        private readonly IWeaponIconService _weaponIconService;
        private readonly ITf2WeaponService _attributeService;
        private readonly Func<WeaponDetailsAttributeViewModel> _getVm;
        private SmartCollection<WeaponDetailsAttributeViewModel, int?> _attributes;

        public WeaponDetailsViewModel(IWeaponIconService weaponIconService, ITf2WeaponService attributeService, Func<WeaponDetailsAttributeViewModel> getVm)
        {
            _weaponIconService = weaponIconService;
            _attributeService = attributeService;
            _getVm = getVm;

            Attributes = new SmartCollection<WeaponDetailsAttributeViewModel, int?>(vm => vm.Tf2Attribute.Id);
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

            IDictionary<string, Tf2Attribute> attributes = await _attributeService.GetAttributesAsClassDictionary();
            IEnumerable<WeaponDetailsAttributeViewModel> viewModels = Model.Attributes.Select(a =>
                                                                                   {
                                                                                       Tf2Attribute attribute = attributes[a.Class];

                                                                                       WeaponDetailsAttributeViewModel vm = _getVm();
                                                                                       vm.Model = a;
                                                                                       vm.Tf2Attribute = attribute;
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

        public SmartCollection<WeaponDetailsAttributeViewModel, int?> Attributes
        {
            get { return _attributes; }
            set
            {
                _attributes = value;
                RaisePropertyChanged(() => Attributes);
            }
        }
    }

    public class WeaponDetailsAttributeViewModel
    {
        public Tf2WeaponAttribute Model { get; set; }
        public Tf2Attribute Tf2Attribute { get; set; }

        public string Format
        {
            get
            {
                switch (Tf2Attribute.Format)
                {
                    case "value_is_percentage":
                        return "%";
                    case "value_is_inverted_percentage":
                        return "-%";
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
                            Debug.WriteLine(Tf2Attribute.Format);
                            return Tf2Attribute.Format.Replace("_", Environment.NewLine);
                        }
                }
            }
        }

    }
}