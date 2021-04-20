﻿// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;

namespace Tiledriver.Core.Extensions.Collections
{
    public static class CollectionExtensions
    {
        public static Dictionary<T1, T2> CondenseToDictionary<T, T1, T2>(
            this IEnumerable<T> sequence,
            Func<T, T1> keySelector,
            Func<T, T2> valueSelector)
        {
            var d = new Dictionary<T1, T2>();

            foreach (var e in sequence)
            {
                d[keySelector(e)] = valueSelector(e);
            }

            return d;
        }

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