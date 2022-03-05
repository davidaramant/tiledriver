// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Xunit;
using FluentAssertions;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class EdgeSegmentIdExtensionsTests
{
    [Theory]
    [InlineData(EdgeSegmentId.DiagTopLeft, SquarePoint.TopMiddle)]
    [InlineData(EdgeSegmentId.DiagTopRight, SquarePoint.RightMiddle)]
    [InlineData(EdgeSegmentId.DiagBottomRight, SquarePoint.RightMiddle)]
    [InlineData(EdgeSegmentId.DiagBottomLeft, SquarePoint.BottomMiddle)]
    [InlineData(EdgeSegmentId.HorizontalLeft, SquarePoint.Center)]
    [InlineData(EdgeSegmentId.HorizontalRight, SquarePoint.RightMiddle)]
    [InlineData(EdgeSegmentId.VerticalTop, SquarePoint.TopMiddle)]
    [InlineData(EdgeSegmentId.VerticalBottom, SquarePoint.Center)]
    public void ShouldFindLeftSideOfEdge(EdgeSegmentId id, SquarePoint expectedPoint)
    {
        id.GetPoint(topOrLeftIsFront: true, leftSide: true).Should().Be(expectedPoint);
        id.GetPoint(topOrLeftIsFront: false, leftSide: false).Should().Be(expectedPoint);
    }

    [Theory]
    [InlineData(EdgeSegmentId.DiagTopLeft, SquarePoint.LeftMiddle)]
    [InlineData(EdgeSegmentId.DiagTopRight, SquarePoint.TopMiddle)]
    [InlineData(EdgeSegmentId.DiagBottomRight, SquarePoint.BottomMiddle)]
    [InlineData(EdgeSegmentId.DiagBottomLeft, SquarePoint.LeftMiddle)]
    [InlineData(EdgeSegmentId.HorizontalLeft, SquarePoint.LeftMiddle)]
    [InlineData(EdgeSegmentId.HorizontalRight, SquarePoint.Center)]
    [InlineData(EdgeSegmentId.VerticalTop, SquarePoint.Center)]
    [InlineData(EdgeSegmentId.VerticalBottom, SquarePoint.BottomMiddle)]
    public void ShouldFindRightSideOfEdge(EdgeSegmentId id, SquarePoint expectedPoint)
    {
        id.GetPoint(topOrLeftIsFront: true, leftSide: false).Should().Be(expectedPoint);
        id.GetPoint(topOrLeftIsFront: false, leftSide: true).Should().Be(expectedPoint);
    }

}
