// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Reading
{
    public static class UwmfComparison
    {
        public static void AssertEqual(MapData actual, MapData expected)
        {
            actual.NameSpace.Should().Be(expected.NameSpace);
            actual.TileSize.Should().Be(expected.TileSize);
            actual.Name.Should().Be(expected.Name);
            actual.Width.Should().Be(expected.Width);
            actual.Height.Should().Be(expected.Height);
            actual.TileSize.Should().Be(expected.TileSize);
            actual.Width.Should().Be(expected.Width);

            CompareCollections(actual:actual.Planes, expected:expected.Planes);
            CompareCollections(actual:actual.Sectors, expected:expected.Sectors);
            CompareCollections(actual:actual.Things, expected:expected.Things);
            CompareCollections(actual:actual.Tiles, expected:expected.Tiles);
            CompareCollections(actual:actual.Triggers, expected:expected.Triggers);
            CompareCollections(actual:actual.Zones, expected:expected.Zones);

            actual.PlaneMaps.Should().HaveCount(expected.PlaneMaps.Count);

            for (int index = 0; index < actual.PlaneMaps.Count; index++)
            {
                actual.PlaneMaps[index].TileSpaces.Should().BeEquivalentTo(
                    expected.PlaneMaps[index].TileSpaces,options => options.ComparingByMembers<TileSpace>() );
            }
        }

        private static void CompareCollections<T>(ImmutableList<T> actual, ImmutableList<T> expected)
        {
            actual.Should().BeEquivalentTo(expected, options => options.ComparingByMembers<T>());
        }
    }
}