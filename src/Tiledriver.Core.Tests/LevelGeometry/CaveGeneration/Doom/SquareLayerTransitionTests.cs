// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using FluentAssertions;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class SquareLayerTransitionTests
{
	[Fact]
	public void ShouldReturnCorrectHeightsForOutsideOfMap() =>
		TestHeightLookup(
			new Dictionary<Position, int> { },
			new Dictionary<SquareSegment, int>()
			{
				{ SquareSegment.UpperLeftOuter, -1 },
				{ SquareSegment.UpperLeftInner, -1 },
				{ SquareSegment.UpperRightOuter, -1 },
				{ SquareSegment.UpperRightInner, -1 },
				{ SquareSegment.LowerRightOuter, -1 },
				{ SquareSegment.LowerRightInner, -1 },
				{ SquareSegment.LowerLeftOuter, -1 },
				{ SquareSegment.LowerLeftInner, -1 },
			}
		);

	[Fact]
	public void ShouldReturnCorrectHeightsForUniformHeights() =>
		TestHeightLookup(
			new Dictionary<Position, int>
			{
				{ new Position(0, 0), 0 },
				{ new Position(1, 0), 0 },
				{ new Position(0, 1), 0 },
				{ new Position(1, 1), 0 },
			},
			new Dictionary<SquareSegment, int>()
			{
				{ SquareSegment.UpperLeftOuter, 0 },
				{ SquareSegment.UpperLeftInner, 0 },
				{ SquareSegment.UpperRightOuter, 0 },
				{ SquareSegment.UpperRightInner, 0 },
				{ SquareSegment.LowerRightOuter, 0 },
				{ SquareSegment.LowerRightInner, 0 },
				{ SquareSegment.LowerLeftOuter, 0 },
				{ SquareSegment.LowerLeftInner, 0 },
			}
		);

	[Fact]
	public void ShouldReturnCorrectHeightsForLeftUpperCorner() =>
		TestHeightLookup(
			new Dictionary<Position, int> { { new Position(0, 0), 0 }, },
			new Dictionary<SquareSegment, int>()
			{
				{ SquareSegment.UpperLeftOuter, -1 },
				{ SquareSegment.UpperLeftInner, -1 },
				{ SquareSegment.UpperRightOuter, -1 },
				{ SquareSegment.UpperRightInner, -1 },
				{ SquareSegment.LowerRightOuter, -1 },
				{ SquareSegment.LowerRightInner, -1 },
				{ SquareSegment.LowerLeftOuter, 0 },
				{ SquareSegment.LowerLeftInner, -1 },
			}
		);

	[Fact]
	public void ShouldReturnCorrectHeightsForUpperHalf() =>
		TestHeightLookup(
			new Dictionary<Position, int> { { new Position(0, 0), 0 }, { new Position(1, 0), 0 }, },
			new Dictionary<SquareSegment, int>()
			{
				{ SquareSegment.UpperLeftOuter, -1 },
				{ SquareSegment.UpperLeftInner, -1 },
				{ SquareSegment.UpperRightOuter, -1 },
				{ SquareSegment.UpperRightInner, -1 },
				{ SquareSegment.LowerRightOuter, 0 },
				{ SquareSegment.LowerRightInner, 0 },
				{ SquareSegment.LowerLeftOuter, 0 },
				{ SquareSegment.LowerLeftInner, 0 },
			}
		);

	private static void TestHeightLookup(
		Dictionary<Position, int> interiorDistances,
		Dictionary<SquareSegment, int> expectedHeight
	)
	{
		var lookup = SquareLayerTransition.GetHeightLookup(interiorDistances, new Position(0, 0));

		foreach (var segmentAndHeight in expectedHeight)
		{
			lookup(segmentAndHeight.Key).Should().Be(segmentAndHeight.Value);
		}
	}
}
