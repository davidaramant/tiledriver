// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

		public static IList<T> AddAndContinue<T>(this IList<T> list, params T[] items)
		{
			list.AddRange(items);
			return list;
		}

		public static IList<T> AddRangeAndContinue<T>(this IList<T> list, IEnumerable<T> items)
		{
			list.AddRange(items);
			return list;
		}

		// Cheat because it's absurd that interface doesn't have this
		public static int FindIndex<T>(this IReadOnlyList<T> list, Predicate<T> predicate) =>
			((List<T>)list).FindIndex(predicate);

		public static void Shuffle<T>(this IList<T> list, Random? random = null)
		{
			var rng = random ?? new Random();

			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				(list[k], list[n]) = (list[n], list[k]);
			}
		}

		public static TValue GetValueOr<TKey, TValue>(
			this IReadOnlyDictionary<TKey, TValue> dict,
			TKey key,
			TValue defaultValue
		)
			where TValue : struct => dict.TryGetValue(key, out var value) ? value : defaultValue;
	}
}
