using System.Collections.Immutable;
using Shouldly;
using Tiledriver.FormatModels.Udmf;

namespace Tiledriver.Tests.FormatModels.Udmf.Reading;

public static class UdmfComparison
{
	public static void AssertEqual(MapData actual, MapData expected)
	{
		actual.NameSpace.ShouldBe(expected.NameSpace);

		CompareCollections(actual: actual.Vertices, expected: expected.Vertices);
		CompareCollections(actual: actual.LineDefs, expected: expected.LineDefs);
		CompareCollections(actual: actual.SideDefs, expected: expected.SideDefs);
		CompareCollections(actual: actual.Sectors, expected: expected.Sectors);
		CompareCollections(actual: actual.Things, expected: expected.Things);
	}

	private static void CompareCollections<T>(ImmutableArray<T> actual, ImmutableArray<T> expected)
	{
		actual.ShouldBeEquivalentTo(expected);
	}
}
