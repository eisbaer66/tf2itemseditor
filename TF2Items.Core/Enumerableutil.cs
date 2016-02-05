using System;
using System.Collections.Generic;
using System.Linq;

namespace TF2Items.Core
{
    public static class Enumerableutil
    {
        public static bool EquivalentTo<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            var dictionary = new Dictionary<T, int>();

            Action<IEnumerable<T>, int> setCounts = (items, change) =>
            {
                foreach (var item in items)
                {
                    int count;
                    // if not found, count will be the default value of 0
                    dictionary.TryGetValue(item, out count);
                    dictionary[item] = count + change;
                }
            };

            setCounts(first, +1);
            setCounts(second, -1);

            return dictionary.Values.All(value => value == 0);
        }

        public static CompareResult<T> Compare<T, TKey>(this IEnumerable<T> oldItems, IEnumerable<T> newItems, Func<T, TKey> key)
        {
            Dictionary<TKey, T> oldItemsDict = oldItems.ToDictionary(key);
            Dictionary<TKey, T> newItemsDict = newItems.ToDictionary(key);

            CompareResult<T> result = new CompareResult<T>();

            foreach (KeyValuePair<TKey, T> pair in oldItemsDict)
            {
                if (!newItemsDict.ContainsKey(pair.Key))
                {
                    result.RemovedItems.Add(pair.Value);
                    continue;
                }

                result.UpdatedItems.Add(new CompareResult<T>.Update {OldItem = pair.Value, NewItem = newItemsDict[pair.Key]});
            }

            foreach (KeyValuePair<TKey, T> pair in newItemsDict)
            {
                if (oldItemsDict.ContainsKey(pair.Key))
                    continue;
                result.NewItems.Add(pair.Value);
            }

            return result;
        }
    }

    public class CompareResult<T>
    {
        public class Update
        {
            public T OldItem { get; set; }
            public T NewItem { get; set; }
        }

        public IList<T> RemovedItems { get; set; }

        public IList<Update> UpdatedItems { get; set; }

        public IList<T> NewItems { get; set; }

        public CompareResult()
        {
            RemovedItems = new List<T>();
            UpdatedItems = new List<Update>();
            NewItems = new List<T>();
        }
    }
}