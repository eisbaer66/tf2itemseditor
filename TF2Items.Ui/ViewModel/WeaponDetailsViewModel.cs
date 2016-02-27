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
        private Tf2Weapon _model;
        private ConfigWeapon _configWeapon;
        private readonly IWeaponDetailsAttributeViewModelFactory _vmFactory;
        private SmartCollection<WeaponDetailsAttributeViewModel, string> _attributes;
        private IDictionary<string, AttributeClass> _attributeClasses;
        private IDictionary<int, AttributeClass> _attributeClassesByAttributeId;
        private IList<WeaponDetailsAttributeViewModel> _tf2AttributeViewModels;
        private IList<WeaponDetailsAttributeViewModel> _configAttributeViewModels;

        public WeaponDetailsViewModel(IWeaponIconService weaponIconService, ITf2WeaponService attributeService, IWeaponDetailsAttributeViewModelFactory vmFactory)
        {
            _weaponIconService = weaponIconService;
            _attributeService = attributeService;
            _vmFactory = vmFactory;

            Attributes = new SmartCollection<WeaponDetailsAttributeViewModel, string>(vm => vm.Model.Name);
            ReadAttributesCommand = new AsyncRelayCommand(ReadAttributeClasses);
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

        private void EditWeaponAttribute(EditWeaponAttribute msg)
        {
            WeaponDetailsAttributeViewModel vm = Attributes.FirstOrDefault(a => a.Model.Name == msg.Class);
            EditAttribute(vm);
        }

        private void RemoveAttribute(RemoveWeaponAttribute msg)
        {
            WeaponDetailsAttributeViewModel vm = Attributes.FirstOrDefault(a => a.Model.Name == msg.Class);
            if (vm == null)
                return;

            ConfigWeaponAttribute existingAttribute = _configWeapon.Attributes.FirstOrDefault(a => a.Id == vm.WeaponAttribute.Id);
            _configWeapon.Attributes.Remove(existingAttribute);
            _vmFactory.Revoke(Model, vm);
            UpdateConfigAttributes();
        }

        public Tf2Weapon Model
        {
            get { return _model; }
            set
            {
                _model = value;
                Image = new RunNotifyTaskCompletion<string>(GetImage);

                UpdateTf2Attributes();
                RaisePropertyChanged(() => Model);
                RaisePropertyChanged(() => Image);
            }
        }

        public ConfigWeapon ConfigWeapon
        {
            get { return _configWeapon; }
            set
            {
                _configWeapon = value;

                UpdateConfigAttributes();
                RaisePropertyChanged(() => ConfigWeapon);
            }
        }

        private async Task ReadAttributeClasses()
        {
            _attributeClasses = await _attributeService.GetAttributeClassesAsDictionary();
            _attributeClassesByAttributeId = await _attributeService.GetAttributeClassesAsAttributeIdDictionary();
        }

        private void UpdateConfigAttributes()
        {
            if (ConfigWeapon == null)
                return;
            if (_attributeClasses == null)
                return;

            _configAttributeViewModels = ConfigWeapon.Attributes
                                                     .Select(a =>
                                                             {
                                                                 int? attributeId = a.Id;
                                                                 if (!attributeId.HasValue)
                                                                     return null;

                                                                 AttributeClass attributeClass = _attributeClassesByAttributeId[attributeId.Value];

                                                                 WeaponDetailsAttributeViewModel vm = _vmFactory.Get(a, Model, attributeClass);
                                                                 return vm;
                                                             })
                                                     .Where(vm => vm != null)
                                                     .ToList();
#if DEBUG
            if (IsInDesignMode)
            {
                bool lastValue = false;
                foreach (WeaponDetailsAttributeViewModel vm in _configAttributeViewModels)
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
            if (Model == null)
                return;
            if (_attributeClasses == null)
                return;

            _tf2AttributeViewModels = Model.Attributes
                                           .Select(a =>
                                                   {
                                                       AttributeClass attribute = _attributeClasses[a.Class];

                                                       WeaponDetailsAttributeViewModel vm = _vmFactory.Get(a, Model, attribute);
                                                       vm.Predefined = true;
                                                       return vm;
                                                   })
                                           .ToList();
            UpdateAttributeList();
        }

        private void UpdateAttributeList()
        {
            IEnumerable<WeaponDetailsAttributeViewModel> viewModels = _tf2AttributeViewModels ?? new WeaponDetailsAttributeViewModel[0];
            if (_configAttributeViewModels != null)
                viewModels = viewModels.Concat(_configAttributeViewModels);

            Application.Current.Dispatcher.Invoke(() => Attributes.SmartReset(viewModels, (o, n) => { }));
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

        public void EditAttribute(WeaponDetailsAttributeViewModel attribute)
        {
            foreach (WeaponDetailsAttributeViewModel vm in Attributes)
            {
                vm.Editing = vm.Model.Name == attribute.Model.Name;
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

            ConfigWeaponAttribute weaponAttribute = tf2AttributeViewModel.Class.GetDefaultWeaponAttribute();
            _configWeapon.Attributes.Add(weaponAttribute);
            UpdateConfigAttributes();
        }
    }
}