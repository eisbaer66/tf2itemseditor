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
        private Tf2Weapon _model;
        private readonly IWeaponIconService _weaponIconService;
        private readonly ITf2WeaponService _attributeService;
        private readonly IWeaponDetailsAttributeViewModelFactory _vmFactory;
        private SmartCollection<WeaponDetailsAttributeViewModel, string> _attributes;
        private IDictionary<string, AttributeClass> _attributeClasses;

        public WeaponDetailsViewModel(IWeaponIconService weaponIconService, ITf2WeaponService attributeService, IWeaponDetailsAttributeViewModelFactory vmFactory)
        {
            _weaponIconService = weaponIconService;
            _attributeService = attributeService;
            _vmFactory = vmFactory;

            Attributes = new SmartCollection<WeaponDetailsAttributeViewModel, string>(vm => vm.Model.Name);
            ReadAttributesCommand = new AsyncRelayCommand(GetAttributes);
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
            Attributes.Remove(vm);

            _vmFactory.Revoke(Model, vm);
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

                                                                                       WeaponDetailsAttributeViewModel vm = _vmFactory.Get(a, Model, attribute);
                                                                                       vm.Predefined = true;
                                                                                       return vm;
                                                                                   });
#if DEBUG
            if (IsInDesignMode)
            {
                bool lastValue = false;
                foreach (WeaponDetailsAttributeViewModel vm in viewModels)
                {
                    lastValue = !lastValue;
                    vm.Editing = lastValue;
                }
            }
#endif

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

            Tf2WeaponAttribute weaponAttribute = tf2AttributeViewModel.Class.GetDefaultWeaponAttribute();
            WeaponDetailsAttributeViewModel vm = _vmFactory.Get(weaponAttribute, Model, tf2AttributeViewModel.Class);
            Attributes.Add(vm);
        }
    }
}