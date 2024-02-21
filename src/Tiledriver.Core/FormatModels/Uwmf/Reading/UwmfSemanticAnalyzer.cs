// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Common;
using Tiledriver.Core.FormatModels.Uwmf.Reading.AbstractSyntaxTree;

namespace Tiledriver.Core.FormatModels.Uwmf.Reading;

public static partial class UwmfSemanticAnalyzer
{
	private static MapSquare ReadMapSquare(IntTuple tuple) =>
		tuple.Values.Length switch
		{
			3 => new MapSquare(Tile: tuple.Values[0].Value, Sector: tuple.Values[1].Value, Zone: tuple.Values[2].Value),
			4
				=> new MapSquare(
					Tile: tuple.Values[0].Value,
					Sector: tuple.Values[1].Value,
					Zone: tuple.Values[2].Value,
					Tag: tuple.Values[3].Value
				),
			_
				=> throw new ParsingException(
					$"Unexpected number of integers in MapSquare at {tuple.StartLocation} - expected 3 or 4."
				)
		};

	private static ImmutableArray<MapSquare> ReadPlaneMap(IntTupleBlock block)
	{
		// Since these things are immutable, there's no problem reusing references.
		var cache = new Dictionary<MapSquare, MapSquare>();
		var tileSpaces = new List<MapSquare>(block.Tuples.Length);

		foreach (var tuple in block.Tuples)
		{
			var ts = ReadMapSquare(tuple);

			if (!cache.ContainsKey(ts))
			{
				cache.Add(ts, ts);
			}

			tileSpaces.Add(cache[ts]);
		}

		return tileSpaces.ToImmutableArray();
	}
}
