using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using TF2Items.Core;
using TF2Items.Ui.Dispatch;
using TF2Items.Ui.Services;
using TF2Items.Ui.ViewModel.Messenges;

namespace TF2Items.Ui.ViewModel
{
    public class WeaponDetailsViewModel : ViewModelBase, IDropTarget
    {
        private readonly IWeaponIconService _weaponIconService;
        private readonly ITf2WeaponService _attributeService;
        private readonly Func<WeaponDetailsNumericalAttributeViewModel> _getVm;
        private Tf2Weapon _tf2Weapon;
        private ConfigWeapon _configWeapon;
        private SmartCollection<IWeaponDetailsAttributeViewModel, string> _attributes;
        private IDictionary<string, AttributeClass> _attributeClasses;
        private IDictionary<int, AttributeClass> _attributeClassesByAttributeId;
        private IList<IWeaponDetailsAttributeViewModel> _tf2AttributeViewModels;
        private IList<IWeaponDetailsAttributeViewModel> _configAttributeViewModels;
        private readonly Func<WeaponDetailsSetAttributeViewModel> _getSetVm;
        private readonly IConfigWeaponService _weaponService;

        public WeaponDetailsViewModel(IWeaponIconService weaponIconService, ITf2WeaponService attributeService, Func<WeaponDetailsNumericalAttributeViewModel> getVm, Func<WeaponDetailsSetAttributeViewModel> getSetVm, IConfigWeaponService weaponService)
        {
            if (weaponIconService == null)
                throw new ArgumentNullException("weaponIconService");
            if (attributeService == null)
                throw new ArgumentNullException("attributeService");
            if (getVm == null)
                throw new ArgumentNullException("getVm");
            if (getSetVm == null)
                throw new ArgumentNullException("getSetVm");
            if (weaponService == null)
                throw new ArgumentNullException("weaponService");

            _weaponIconService = weaponIconService;
            _attributeService = attributeService;
            _getVm = getVm;
            _getSetVm = getSetVm;
            _weaponService = weaponService;

            Attributes = new SmartCollection<IWeaponDetailsAttributeViewModel, string>(vm => vm.Class.Name);
            ReadAttributesCommand = new AsyncRelayCommand(ReadAttributeClasses);
            ResetWeaponCommand = new AsyncRelayCommand(ResetWeapon);
            ResetAllWeaponsCommand = new AsyncRelayCommand(ResetAllWeapons);
            MessengerInstance.Register<RemoveWeaponAttribute>(this, RemoveAttribute);
            MessengerInstance.Register<EditWeaponAttribute>(this, EditWeaponAttribute);

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

        public AsyncRelayCommand ReadAttributesCommand { get; set; }
        public AsyncRelayCommand ResetWeaponCommand { get; set; }
        public AsyncRelayCommand ResetAllWeaponsCommand { get; set; }

        public RunNotifyTaskCompletion<string> Image { get; private set; }

        public SmartCollection<IWeaponDetailsAttributeViewModel, string> Attributes
        {
            get { return _attributes; }
            set
            {
                _attributes = value;
                RaisePropertyChanged(() => Attributes);
            }
        }

        public Tf2Weapon Tf2Weapon
        {
            get { return _tf2Weapon; }
            set
            {
                _tf2Weapon = value;
                Image = new RunNotifyTaskCompletion<string>(GetImage);

                RaisePropertyChanged(() => Tf2Weapon);
                RaisePropertyChanged(() => Image);
            }
        }

        public ConfigWeapon ConfigWeapon
        {
            get { return _configWeapon; }
            set
            {
                _configWeapon = value;

                RaisePropertyChanged(() => ConfigWeapon);
            }
        }

        public void UpdateModels(Tf2Weapon weapon, ConfigWeapon configWeapon)
        {
            Tf2Weapon = weapon;
            ConfigWeapon = configWeapon;
            UpdateAttributes();
        }

        public void EditAttribute(IWeaponDetailsAttributeViewModel attribute)
        {
            foreach (IWeaponDetailsAttributeViewModel vm in Attributes)
            {
                vm.Editing = vm.Class.Name == attribute.Class.Name;
            }
        }

        private async Task ReadAttributeClasses()
        {
            _attributeClasses = await _attributeService.GetAttributeClassesAsDictionary();
            _attributeClassesByAttributeId = await _attributeService.GetAttributeClassesAsAttributeIdDictionary();
        }

        private void UpdateAttributes()
        {
            if (Tf2Weapon == null)
                return;
            if (ConfigWeapon == null)
                return;
            if (_attributeClasses == null)
                return;

            _configAttributeViewModels = null;

            UpdateTf2Attributes();
            UpdateConfigAttributes();
        }

        private void UpdateConfigAttributes()
        {
            _configAttributeViewModels = ConfigWeapon.Attributes
                                                     .Select(a =>
                                                             {
                                                                 IWeaponDetailsAttributeViewModel vm = Get(a, a.Id);
                                                                 return vm;
                                                             })
                                                     .ToList();
#if DEBUG
            if (IsInDesignMode)
            {
                bool lastValue = false;
                foreach (IWeaponDetailsAttributeViewModel vm in _configAttributeViewModels)
                {
                    lastValue = !lastValue;
                    vm.Editing = lastValue;
                }
            }
#endif
            UpdateAttributeList();
        }

        private void UpdateTf2Attributes()
        {
            _tf2AttributeViewModels = Tf2Weapon.Attributes
                                           .Select(a =>
                                           {
                                               AttributeClass attributeClass = _attributeClasses[a.Class];

                                               Tf2Attribute attribute = attributeClass.Get(a.Value, a.Name);
                                               ConfigWeaponAttribute configattribute = new ConfigWeaponAttribute(attribute.Id, a.Value);
                                               configattribute.IsPredefined = true;
                                               IWeaponDetailsAttributeViewModel vm = Get(configattribute, attribute.Id);
                                               return vm;
                                           })
                                           .ToList();
            UpdateAttributeList();
        }


        public IWeaponDetailsAttributeViewModel Get(ConfigWeaponAttribute configWeaponAttribute, int attributeId)
        {
            AttributeClass attributeClass = _attributeClassesByAttributeId[attributeId];

            IWeaponDetailsAttributeViewModel vm;
            switch (attributeClass.EditMode)
            {
                case AttributeEditing.Numerical:
                    vm = _getVm();
                    break;
                case AttributeEditing.Set:
                    vm = _getSetVm();
                    break;
                default:
                    vm = _getVm();
                    break;
            }
            vm.Class = attributeClass;
            vm.WeaponAttribute = configWeaponAttribute;
            return vm;
        }

        private void UpdateAttributeList()
        {
            if (_tf2AttributeViewModels == null)
                return;
            if (_configAttributeViewModels == null)
                return;

            IEnumerable<IWeaponDetailsAttributeViewModel> viewModels = _tf2AttributeViewModels ?? new WeaponDetailsNumericalAttributeViewModel[0];
            if (_configAttributeViewModels == null)
            {
                Application.Current.Dispatcher.Invoke(() => Attributes.SmartReset(viewModels, (o, n) => { }));
                return;
            }

            CompareResult<IWeaponDetailsAttributeViewModel> compare = viewModels.Compare(_configAttributeViewModels, model => model.Attribute.Id);
            viewModels = compare.RemovedItems
                                .Concat(compare.UpdatedItems.Select(u => u.NewItem))
                                .Concat(compare.NewItems);

            Application.Current.Dispatcher.Invoke(() => Attributes.SmartReset(viewModels, (o, n) => { }));
        }

        private async Task ResetWeapon()
        {
            _configWeapon.Attributes.Clear();
            UpdateConfigAttributes();
        }

        private async Task ResetAllWeapons()
        {
            _weaponService.Set(new WeaponCollection());

            await ResetWeapon();
        }

        private async Task<string> GetImage()
        {
            return await _weaponIconService.Get(_tf2Weapon);
        }

        private void EditWeaponAttribute(EditWeaponAttribute msg)
        {
            IWeaponDetailsAttributeViewModel vm = Attributes.FirstOrDefault(a => a.Class.Name == msg.Class);
            EditAttribute(vm);
        }

        private void RemoveAttribute(RemoveWeaponAttribute msg)
        {
            IWeaponDetailsAttributeViewModel vm = Attributes.FirstOrDefault(a => a.Class.Name == msg.Class);
            if (vm == null)
                return;

            ConfigWeaponAttribute existingAttribute = _configWeapon.Attributes.FirstOrDefault(a => a.Id == vm.Attribute.Id);
            _configWeapon.Attributes.Remove(existingAttribute);
            UpdateConfigAttributes();
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (_attributeClasses == null)
                return;
            Tf2AttributeClassViewModel tf2AttributeClassViewModel = dropInfo.Data as Tf2AttributeClassViewModel;
            if (tf2AttributeClassViewModel == null)
                return;

            bool attributeAlreadyExists = Attributes.Any(vm => vm.Class.Name == tf2AttributeClassViewModel.Class.Name);
            if (attributeAlreadyExists)
                return;

            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (_attributeClasses == null)
                return;
            Tf2AttributeClassViewModel tf2AttributeClassViewModel = dropInfo.Data as Tf2AttributeClassViewModel;
            if (tf2AttributeClassViewModel == null)
                return;

            ConfigWeaponAttribute weaponAttribute = tf2AttributeClassViewModel.Class.GetDefaultWeaponAttribute();
            _configWeapon.Attributes.Add(weaponAttribute);
            UpdateConfigAttributes();
        }
    }
}