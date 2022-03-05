// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using FluentAssertions;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom.SquareModel;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class EdgeSegmentTests
{
    [Fact]
    public void ShouldDetermineFrontIfBottomIsOutsideLevel()
    {
        var sd = new SectorDescription(0);
        
        var edge = EdgeSegment.Construct(id:EdgeSegmentId.HorizontalLeft, topOrLeft: sd, bottomOrRight: SectorDescription.OutsideLevel);

        edge.IsFrontTopOrLeft.Should().BeTrue();
        edge.Front.Should().Be(sd);
        edge.Back.Should().Be(SectorDescription.OutsideLevel);
    }

    [Fact]
    public void ShouldDetermineFrontIfTopIsOutsideLevel()
    {
        var sd = new SectorDescription(0);

        var edge = EdgeSegment.Construct(id: EdgeSegmentId.HorizontalLeft, topOrLeft: SectorDescription.OutsideLevel, bottomOrRight: sd );

        edge.IsFrontTopOrLeft.Should().BeFalse();
        edge.Front.Should().Be(sd);
        edge.Back.Should().Be(SectorDescription.OutsideLevel);
    }

    [Fact]
    public void ShouldDetermineFrontIfBottomIsGreater()
    {
        var top = new SectorDescription(0);
        var bottom = new SectorDescription(1);

        var edge = EdgeSegment.Construct(id: EdgeSegmentId.HorizontalLeft, topOrLeft: top, bottomOrRight: bottom);

        edge.IsFrontTopOrLeft.Should().BeTrue();
        edge.Front.Should().Be(top);
        edge.Back.Should().Be(bottom);
    }

    [Fact]
    public void ShouldDetermineFrontIfTopIsGreater()
    {
        var top = new SectorDescription(1);
        var bottom = new SectorDescription(0);

        var edge = EdgeSegment.Construct(id: EdgeSegmentId.HorizontalLeft, topOrLeft: top, bottomOrRight: bottom);

        edge.IsFrontTopOrLeft.Should().BeFalse();
        edge.Front.Should().Be(bottom);
        edge.Back.Should().Be(top);
    }
}
