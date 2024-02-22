// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;

namespace Tiledriver.Core.Extensions.Enumerable;

public static class EnumerableExtensions
{
	public static int MinIndex<T>(this IEnumerable<T> sequence, Func<T, double> selector)
	{
		int minIndex = 0;
		double min = double.MaxValue;

		int index = 0;
		foreach (var value in sequence)
		{
			var current = selector(value);

			if (current < min)
			{
				minIndex = index;
				min = current;
			}

			index++;
		}

		return minIndex;
	}
}
