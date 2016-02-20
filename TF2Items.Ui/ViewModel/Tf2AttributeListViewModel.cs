using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GongSolutions.Wpf.DragDrop;
using TF2Items.Ui.Dispatch;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public class Tf2AttributeListViewModel : ViewModelBase, IDragSource
    {

        private readonly ITf2WeaponService _itemsGameService;
        private readonly Func<Tf2AttributeViewModel> _getAttributeViewModel;

        private IList<Tf2AttributeViewModel> _allAttributes;
        private SmartCollection<Tf2AttributeViewModel, int?> _attributes;
        private string _filter;

        public AsyncRelayCommand ReadAllAttributesCommand { get; set; }

        public Tf2AttributeListViewModel(ITf2WeaponService itemsGameService, Func<Tf2AttributeViewModel> getAttributeViewModel)
        {
            _itemsGameService = itemsGameService;
            _getAttributeViewModel = getAttributeViewModel;

            Attributes = new SmartCollection<Tf2AttributeViewModel, int?>(vm => vm.Model.Id);
            ReadAllAttributesCommand = new AsyncRelayCommand(GetAttributes);
#if DEBUG
            if (IsInDesignMode)
            {
                Task.Run(() =>
                         {
                             ReadAllAttributesCommand.Execute(null);
                         }).Wait();
            }
#endif
        }

        public SmartCollection<Tf2AttributeViewModel, int?> Attributes
        {
            get { return _attributes; }
            set
            {
                _attributes = value;
                RaisePropertyChanged(() => Attributes);
            }
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                FilterAttributes();
                RaisePropertyChanged(() => Filter);
            }
        }

        private async Task GetAttributes()
        {
            IEnumerable<Tf2AttributeViewModel> viewModels = await GetAttributeViewModels();

            Application.Current.Dispatcher.Invoke(() => Attributes.SmartReset(viewModels, (o, n) => o.Model = n.Model));
        }

        private async Task<IEnumerable<Tf2AttributeViewModel>> GetAttributeViewModels()
        {
            _allAttributes = (await _itemsGameService.GetAttributeClasses())
                .Select(a =>
                        {
                            Tf2AttributeViewModel attributeViewModel = _getAttributeViewModel();
                            attributeViewModel.Class = a;
                            return attributeViewModel;
                        })
                .ToList();
            return _allAttributes;
        }

        private void FilterAttributes()
        {
            Func<Tf2AttributeViewModel, bool> matches = w =>
                                                        {
                                                            bool nameContainsPhrase = CultureInfo.InvariantCulture.CompareInfo.IndexOf(w.Model.Name, _filter, CompareOptions.IgnoreCase) >= 0;
                                                            if (nameContainsPhrase)
                                                                return true;

                                                            bool classContainsPhrase = CultureInfo.InvariantCulture.CompareInfo.IndexOf(w.Model.Class, _filter, CompareOptions.IgnoreCase) >= 0;
                                                            if (classContainsPhrase)
                                                                return true;

                                                            return false;
                                                        };
            List<Tf2AttributeViewModel> weaponsNotMatchingFilter = Attributes.Where(w => !matches(w)).ToList();
            Attributes.Remove(weaponsNotMatchingFilter);

            List<Tf2AttributeViewModel> weaponsMatchingFilter = _allAttributes.Where(w => matches(w) && !Attributes.Contains(w)).ToList();
            Attributes.AddRange(weaponsMatchingFilter);
        }

        void IDragSource.StartDrag(IDragInfo dragInfo)
        {
            Tf2AttributeViewModel viewModel = dragInfo.SourceItem as Tf2AttributeViewModel;
            if (viewModel == null)
                return;

            dragInfo.Effects = DragDropEffects.Copy|DragDropEffects.Move;
            dragInfo.Data = viewModel;
        }

        bool IDragSource.CanStartDrag(IDragInfo dragInfo)
        {
            return dragInfo.SourceItem != null;
        }

        void IDragSource.Dropped(IDropInfo dropInfo)
        {
        }

        void IDragSource.DragCancelled()
        {
        }

        bool IDragSource.TryCatchOccurredException(Exception exception)
        {
            Debug.WriteLine(exception);
            return true;
        }
    }
}