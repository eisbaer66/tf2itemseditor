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
using TF2Items.Core;
using TF2Items.Ui.Dispatch;
using TF2Items.Ui.Services;

namespace TF2Items.Ui.ViewModel
{
    public class Tf2AttributeListViewModel : ViewModelBase, IDragSource
    {

        private readonly ITf2WeaponService _itemsGameService;
        private readonly Func<Tf2AttributeClassViewModel> _getAttributeViewModel;

        private IList<Tf2AttributeClassViewModel> _allAttributes;
        private SmartCollection<Tf2AttributeClassViewModel, string> _attributes;
        private string _filter;

        public AsyncRelayCommand ReadAllAttributesCommand { get; set; }

        public Tf2AttributeListViewModel(ITf2WeaponService itemsGameService, Func<Tf2AttributeClassViewModel> getAttributeViewModel)
        {
            _itemsGameService = itemsGameService;
            _getAttributeViewModel = getAttributeViewModel;

            Attributes = new SmartCollection<Tf2AttributeClassViewModel, string>(vm => vm.Class.Name);
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

        public SmartCollection<Tf2AttributeClassViewModel, string> Attributes
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
            IEnumerable<Tf2AttributeClassViewModel> viewModels = await GetAttributeViewModels();

            Application.Current.Dispatcher.Invoke(() => Attributes.SmartReset(viewModels, (o, n) => o.Class = n.Class));
        }

        private async Task<IEnumerable<Tf2AttributeClassViewModel>> GetAttributeViewModels()
        {
            _allAttributes = (await _itemsGameService.GetAttributeClasses())
                .Select(g =>
                        {
                            Tf2AttributeClassViewModel attributeClassViewModel = _getAttributeViewModel();
                            attributeClassViewModel.Class = g;
                            return attributeClassViewModel;
                        })
                .ToList();
            return _allAttributes;
        }

        private void FilterAttributes()
        {
            Func<Tf2AttributeClassViewModel, bool> matches = w =>
                                                        {
                                                            bool nameContainsPhrase = CultureInfo.InvariantCulture.CompareInfo.IndexOf(w.Class.Name, _filter, CompareOptions.IgnoreCase) >= 0;
                                                            if (nameContainsPhrase)
                                                                return true;

                                                            bool attributeContainsPhrase = w.Class.Attributes.Any(a =>
                                                                                                                  {
                                                                                                                      bool attributeNameContainsPhrase = CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                                                                                                                          a.Name, _filter,
                                                                                                                          CompareOptions.IgnoreCase) >= 0;
                                                                                                                      if (attributeNameContainsPhrase)
                                                                                                                          return true;

                                                                                                                      if (a.Id.ToString() == _filter)
                                                                                                                          return true;

                                                                                                                      return false;
                                                                                                                  });
                                                            if (attributeContainsPhrase)
                                                                return true;

                                                            return false;
                                                        };
            List<Tf2AttributeClassViewModel> weaponsNotMatchingFilter = Attributes.Where(w => !matches(w)).ToList();
            Attributes.Remove(weaponsNotMatchingFilter);

            List<Tf2AttributeClassViewModel> weaponsMatchingFilter = _allAttributes.Where(w => matches(w) && !Attributes.Contains(w)).ToList();
            Attributes.AddRange(weaponsMatchingFilter);
        }

        void IDragSource.StartDrag(IDragInfo dragInfo)
        {
            Tf2AttributeClassViewModel classViewModel = dragInfo.SourceItem as Tf2AttributeClassViewModel;
            if (classViewModel == null)
                return;

            dragInfo.Effects = DragDropEffects.Copy|DragDropEffects.Move;
            dragInfo.Data = classViewModel;
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