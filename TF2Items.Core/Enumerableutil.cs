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
         
    }
}