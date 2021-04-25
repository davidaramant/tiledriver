// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;

namespace Tiledriver.Core.Extensions.Collections
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this Queue<T> collection, IEnumerable<T> sequence)
        {
            foreach (var item in sequence)
            {
                collection.Enqueue(item);
            }
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> sequence)
        {
            foreach (var item in sequence)
            {
                collection.Add(item);
            }
        }
    }
}