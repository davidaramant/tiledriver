// Copyright (c) 2022, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using FluentAssertions;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class LatticePointTests
{
    [Fact]
    public void ShouldNormalizeBottomMiddlePoint()
    {
        var bottomMiddleAbove = new LatticePoint(new Position(0, 0), SquarePoint.BottomMiddle);
        var topMiddleBelow = new LatticePoint(new Position(0, 1), SquarePoint.TopMiddle);

        bottomMiddleAbove.Should().Be(topMiddleBelow);
    }

    [Fact]
    public void ShouldNormalizeRightMiddlePoint()
    {
        var rightMiddleBefore = new LatticePoint(new Position(0, 0), SquarePoint.RightMiddle);
        var leftMiddleAfter = new LatticePoint(new Position(1, 0), SquarePoint.LeftMiddle);

        rightMiddleBefore.Should().Be(leftMiddleAfter);
    }
}
