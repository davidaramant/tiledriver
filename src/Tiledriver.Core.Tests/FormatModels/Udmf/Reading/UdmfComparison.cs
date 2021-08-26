// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.
using System.Collections.Immutable;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Udmf;

namespace Tiledriver.Core.Tests.FormatModels.Udmf.Reading
{
    public static class UdmfComparison
    {
        public static void AssertEqual(MapData actual, MapData expected)
        {
            actual.NameSpace.Should().Be(expected.NameSpace);

            CompareCollections(actual:actual.Vertices, expected:expected.Vertices);
            CompareCollections(actual:actual.LineDefs, expected:expected.LineDefs);
            CompareCollections(actual:actual.SideDefs, expected:expected.SideDefs);
            CompareCollections(actual:actual.Sectors, expected:expected.Sectors);
            CompareCollections(actual:actual.Things, expected:expected.Things);
        }

        private static void CompareCollections<T>(ImmutableArray<T> actual, ImmutableArray<T> expected)
        {
            actual.Should().BeEquivalentTo(expected, options => options.ComparingByMembers<T>());
        }
    }
}