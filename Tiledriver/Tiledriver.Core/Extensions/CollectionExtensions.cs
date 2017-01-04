// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                set.Add(item);
            }
        }

        public static void AddRange<T1,T2>(this IDictionary<T1,T2> dictionary, IEnumerable<KeyValuePair<T1,T2>> items)
        {
            foreach (var item in items)
            {
                dictionary.Add(item);
            }
        }

        public static void Merge<T1, T2>(this Dictionary<T1, T2> dictionary, Dictionary<T1, T2> otherDictionary)
        {
            foreach (var kvp in otherDictionary)
            {
                dictionary[kvp.Key] = kvp.Value;
            }
        }
    }
}