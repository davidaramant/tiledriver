// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using FluentAssertions;
using Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;
using Xunit;

namespace Tiledriver.Core.Tests.LevelGeometry.CaveGeneration.Doom;

public sealed class SectorDescriptionTests
{
    [Fact]
    public void ShouldIdentifyOutsideOfLevel()
    {
        SectorDescription.OutsideLevel.IsOutsideLevel.Should().BeTrue();
        new SectorDescription(HeightLevel: 2).IsOutsideLevel.Should().BeFalse();
    }

    [Fact]
    public void ShouldConsiderTheOutSideToBeLarger()
    {
        var d = new SectorDescription(HeightLevel: 2);
        var outside = SectorDescription.OutsideLevel;

        d.Should().BeLessThan(outside);
        outside.Should().BeGreaterThan(d);
    }

    [Fact]
    public void ShouldSortCorrectly()
    {
        var d1 = new SectorDescription(HeightLevel: 2);
        var d2 = new SectorDescription(HeightLevel: 3);

        d1.Should().BeLessThan(d2);
        d2.Should().BeGreaterThan(d1);
    }
}
