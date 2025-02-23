using Shouldly;
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
			[
				inside, // UpperLeftOuter = 0,
				inside, // UpperLeftInner = 1,
				inside, // UpperRightOuter = 2,
				inside, // UpperRightInner = 3,
				outside, // LowerRightOuter = 4,
				outside, // LowerRightInner = 5,
				outside, // LowerLeftOuter = 6,
				outside // LowerLeftInner = 7,
			]
		);

		var edges = squareSegments.GetInternalEdges().ToDictionary(e => e.Id, e => e);
		edges.Keys.ShouldBe([EdgeSegmentId.HorizontalLeft, EdgeSegmentId.HorizontalRight]);

		var left = edges[EdgeSegmentId.HorizontalLeft];
		left.Left.ShouldBe(SquarePoint.Center);
		left.Right.ShouldBe(SquarePoint.LeftMiddle);

		var right = edges[EdgeSegmentId.HorizontalRight];
		right.Left.ShouldBe(SquarePoint.RightMiddle);
		right.Right.ShouldBe(SquarePoint.Center);
	}

	[Fact]
	public void ShouldDetermineFrontIfTopIsOutsideLevel()
	{
		var inside = new SectorDescription(HeightLevel: 0);
		var outside = SectorDescription.OutsideLevel;

		var squareSegments = new SquareSegmentSectors(
			[
				outside, // UpperLeftOuter = 0,
				outside, // UpperLeftInner = 1,
				outside, // UpperRightOuter = 2,
				outside, // UpperRightInner = 3,
				inside, // LowerRightOuter = 4,
				inside, // LowerRightInner = 5,
				inside, // LowerLeftOuter = 6,
				inside // LowerLeftInner = 7,
			]
		);

		var edges = squareSegments.GetInternalEdges().ToDictionary(e => e.Id, e => e);
		edges.Keys.ShouldBe([EdgeSegmentId.HorizontalLeft, EdgeSegmentId.HorizontalRight]);

		var left = edges[EdgeSegmentId.HorizontalLeft];
		left.Left.ShouldBe(SquarePoint.LeftMiddle);
		left.Right.ShouldBe(SquarePoint.Center);

		var right = edges[EdgeSegmentId.HorizontalRight];
		right.Left.ShouldBe(SquarePoint.Center);
		right.Right.ShouldBe(SquarePoint.RightMiddle);
	}

	[Fact]
	public void ShouldDetermineFrontIfLeftIsOutsideLevel()
	{
		var inside = new SectorDescription(HeightLevel: 0);
		var outside = SectorDescription.OutsideLevel;

		var squareSegments = new SquareSegmentSectors(
			[
				outside, // UpperLeftOuter = 0,
				outside, // UpperLeftInner = 1,
				inside, // UpperRightOuter = 2,
				inside, // UpperRightInner = 3,
				inside, // LowerRightOuter = 4,
				inside, // LowerRightInner = 5,
				outside, // LowerLeftOuter = 6,
				outside // LowerLeftInner = 7,
			]
		);

		var edges = squareSegments.GetInternalEdges().ToDictionary(e => e.Id, e => e);
		edges.Keys.ShouldBe([EdgeSegmentId.VerticalTop, EdgeSegmentId.VerticalBottom]);

		var top = edges[EdgeSegmentId.VerticalTop];
		top.Left.ShouldBe(SquarePoint.Center);
		top.Right.ShouldBe(SquarePoint.TopMiddle);

		var bottom = edges[EdgeSegmentId.VerticalBottom];
		bottom.Left.ShouldBe(SquarePoint.BottomMiddle);
		bottom.Right.ShouldBe(SquarePoint.Center);
	}
}
