using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using TF2Items.Core;

namespace TF2Items.Ui
{
    public class SmartCollection<T, TKey> : ObservableCollection<T>
    {
        private readonly Func<T, TKey> _key;

        public SmartCollection(Func<T, TKey> key)
            : base()
        {
            _key = key;
        }

        public SmartCollection(IEnumerable<T> collection, Func<T, TKey> key)
            : base(collection)
        {
            _key = key;
        }

        public SmartCollection(List<T> list, Func<T, TKey> key)
            : base(list)
        {
            _key = key;
        }

        public void Reset(IEnumerable<T> range)
        {
            this.Items.Clear();

            foreach (var item in range)
            {
                Items.Add(item);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, range));
        }

        public void SmartReset(IEnumerable<T> viewModels, Action<T, T> update)
        {
            CompareResult<T> compare = Items.Compare(viewModels, _key);

            Remove(compare.RemovedItems);
            foreach (CompareResult<T>.Update updatedItem in compare.UpdatedItems)
            {
                update(updatedItem.OldItem, updatedItem.NewItem);
            }

            foreach (var item in compare.NewItems)
            {
                Add(item);
            }

        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (T item in range)
            {
                Add(item);
            }
        }

        public void Remove(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                Remove(item);
            }
        }
    }
}