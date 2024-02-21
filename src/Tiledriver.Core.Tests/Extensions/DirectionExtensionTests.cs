// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Tiledriver.Core.Extensions.Directions;
using Tiledriver.Core.LevelGeometry;
using Xunit;

namespace Tiledriver.Core.Tests.Extensions;

public sealed class DirectionExtensionTests
{
	[Theory]
	[InlineData(0, 0, 3, 3, 2)] // Top Left
	[InlineData(1, 0, 3, 3, 3)] // Top Middle
	[InlineData(2, 0, 3, 3, 2)] // Top Right
	[InlineData(0, 1, 3, 3, 3)] // Middle Left
	[InlineData(1, 1, 3, 3, 4)] // Middle Middle
	[InlineData(2, 1, 3, 3, 3)] // Middle Right
	[InlineData(0, 2, 3, 3, 2)] // Bottom Left
	[InlineData(1, 2, 3, 3, 3)] // Bottom Middle
	[InlineData(2, 2, 3, 3, 2)] // Bottom Right
	public void ShouldReturnValidAdjacentPoints(int x, int y, int width, int height, int expectedAdjacent)
	{
		var location = new Position(x, y);
		var bounds = new Size(width, height);

		location
			.GetAdjacentPoints(bounds, clockWise: false, start: Direction.East)
			.Should()
			.HaveCount(expectedAdjacent);
	}

	[Fact]
	public void ShouldReturnPointsInDefaultOrder()
	{
		VerifyDirections(
			clockWise: false,
			start: Direction.East,
			expectedDirections: new[] { Direction.East, Direction.North, Direction.West, Direction.South, }
		);
	}

	[Fact]
	public void ShouldReturnPointsInReversedOrder()
	{
		VerifyDirections(
			clockWise: true,
			start: Direction.West,
			expectedDirections: new[] { Direction.West, Direction.North, Direction.East, Direction.South, }
		);
	}

	[Fact]
	public void ShouldGetDirectionsInCounterClockwiseOrder()
	{
		var actual = DirectionExtensions.GetDirections(start: Direction.North, clockWise: false).ToArray();
		var expected = new[] { Direction.North, Direction.West, Direction.South, Direction.East, };

		actual.Should().BeEquivalentTo(expected);
	}

	[Fact]
	public void ShouldGetDirectionsInClockwiseOrder()
	{
		var actual = DirectionExtensions.GetDirections(start: Direction.North, clockWise: true).ToArray();
		var expected = new[] { Direction.North, Direction.East, Direction.South, Direction.West, };

		actual.Should().BeEquivalentTo(expected);
	}

	private static void VerifyDirections(bool clockWise, Direction start, IEnumerable<Direction> expectedDirections)
	{
		var directions = new Position(1, 1)
			.GetAdjacentPoints(new Size(3, 3), clockWise: clockWise, start: start)
			.Select(tuple => tuple.direction)
			.ToArray();

		directions.Should().BeEquivalentTo(expectedDirections);
	}
}
