// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using FluentAssertions;
using Tiledriver.Core.LevelGeometry.CaveGeneration;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom.SquareModel;

public sealed class CornerTests
{
    [Theory]
    [InlineData(Corners.LowerLeft, SquareSegments.Corner_LowerLeft)]
    [InlineData(Corners.LowerRight, SquareSegments.Corner_LowerRight)]
    [InlineData(Corners.UpperRight, SquareSegments.Corner_UpperRight)]
    [InlineData(Corners.UpperLeft, SquareSegments.Corner_UpperLeft)]
    [InlineData(Corners.None, SquareSegments.None)]
    [InlineData(Corners.All, SquareSegments.All)]
    [InlineData(Corners.Upper, SquareSegments.Corners_Upper)]
    [InlineData(Corners.Lower, SquareSegments.Corners_Lower)]
    [InlineData(Corners.Left, SquareSegments.Corners_Left)]
    [InlineData(Corners.Right, SquareSegments.Corners_Right)]
    [InlineData(Corners.AllButLowerLeft, SquareSegments.Corners_AllButLowerLeft)]
    [InlineData(Corners.AllButLowerRight, SquareSegments.Corners_AllButLowerRight)]
    [InlineData(Corners.AllButUpperLeft, SquareSegments.Corners_AllButUpperLeft)]
    [InlineData(Corners.AllButUpperRight, SquareSegments.Corners_AllButUpperRight)]
    [InlineData(Corners.UpperLeftAndLowerRight, SquareSegments.Corners_UpperLeftAndLowerRight)]
    [InlineData(Corners.UpperRightAndLowerLeft, SquareSegments.Corners_UpperRightAndLowerLeft)]
    public void ShouldConvertCornersToSquareSegments(Corners corners, SquareSegments expectedSegments) =>
        corners.ToSquareSegments().Should().Be(expectedSegments);
}
