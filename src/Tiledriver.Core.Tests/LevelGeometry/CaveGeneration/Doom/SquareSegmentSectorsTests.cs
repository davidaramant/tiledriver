using System.Linq;
using FluentAssertions;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class SquareSegmentSectorsTests
{
	[Fact]
	public void ShouldDetermineFrontIfBottomIsOutsideLevel()
	{
		var inside = new SectorDescription(HeightLevel: 0);
		var outside = SectorDescription.OutsideLevel;

		var squareSegments = new SquareSegmentSectors(
			new[]
			{
				inside, // UpperLeftOuter = 0,
				inside, // UpperLeftInner = 1,
				inside, // UpperRightOuter = 2,
				inside, // UpperRightInner = 3,
				outside, // LowerRightOuter = 4,
				outside, // LowerRightInner = 5,
				outside, // LowerLeftOuter = 6,
				outside, // LowerLeftInner = 7,
			}
		);

		var edges = squareSegments.GetInternalEdges().ToDictionary(e => e.Id, e => e);
		edges.Keys.Should().BeEquivalentTo(new[] { EdgeSegmentId.HorizontalLeft, EdgeSegmentId.HorizontalRight });

		var left = edges[EdgeSegmentId.HorizontalLeft];
		left.Left.Should().Be(SquarePoint.Center);
		left.Right.Should().Be(SquarePoint.LeftMiddle);

		var right = edges[EdgeSegmentId.HorizontalRight];
		right.Left.Should().Be(SquarePoint.RightMiddle);
		right.Right.Should().Be(SquarePoint.Center);
	}

	[Fact]
	public void ShouldDetermineFrontIfTopIsOutsideLevel()
	{
		var inside = new SectorDescription(HeightLevel: 0);
		var outside = SectorDescription.OutsideLevel;

		var squareSegments = new SquareSegmentSectors(
			new[]
			{
				outside, // UpperLeftOuter = 0,
				outside, // UpperLeftInner = 1,
				outside, // UpperRightOuter = 2,
				outside, // UpperRightInner = 3,
				inside, // LowerRightOuter = 4,
				inside, // LowerRightInner = 5,
				inside, // LowerLeftOuter = 6,
				inside, // LowerLeftInner = 7,
			}
		);

		var edges = squareSegments.GetInternalEdges().ToDictionary(e => e.Id, e => e);
		edges.Keys.Should().BeEquivalentTo(new[] { EdgeSegmentId.HorizontalLeft, EdgeSegmentId.HorizontalRight });

		var left = edges[EdgeSegmentId.HorizontalLeft];
		left.Left.Should().Be(SquarePoint.LeftMiddle);
		left.Right.Should().Be(SquarePoint.Center);

		var right = edges[EdgeSegmentId.HorizontalRight];
		right.Left.Should().Be(SquarePoint.Center);
		right.Right.Should().Be(SquarePoint.RightMiddle);
	}

	[Fact]
	public void ShouldDetermineFrontIfLeftIsOutsideLevel()
	{
		var inside = new SectorDescription(HeightLevel: 0);
		var outside = SectorDescription.OutsideLevel;

		var squareSegments = new SquareSegmentSectors(
			new[]
			{
				outside, // UpperLeftOuter = 0,
				outside, // UpperLeftInner = 1,
				inside, // UpperRightOuter = 2,
				inside, // UpperRightInner = 3,
				inside, // LowerRightOuter = 4,
				inside, // LowerRightInner = 5,
				outside, // LowerLeftOuter = 6,
				outside, // LowerLeftInner = 7,
			}
		);

		var edges = squareSegments.GetInternalEdges().ToDictionary(e => e.Id, e => e);
		edges.Keys.Should().BeEquivalentTo(new[] { EdgeSegmentId.VerticalBottom, EdgeSegmentId.VerticalTop });

		var top = edges[EdgeSegmentId.VerticalTop];
		top.Left.Should().Be(SquarePoint.Center);
		top.Right.Should().Be(SquarePoint.TopMiddle);

		var bottom = edges[EdgeSegmentId.VerticalBottom];
		bottom.Left.Should().Be(SquarePoint.BottomMiddle);
		bottom.Right.Should().Be(SquarePoint.Center);
	}
}
