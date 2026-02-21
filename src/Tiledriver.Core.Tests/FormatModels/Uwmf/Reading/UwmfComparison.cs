using System.Collections.Immutable;
using Shouldly;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Reading;

public static class UwmfComparison
{
	public static void AssertEqual(MapData actual, MapData expected)
	{
		actual.NameSpace.ShouldBe(expected.NameSpace);
		actual.TileSize.ShouldBe(expected.TileSize);
		actual.Name.ShouldBe(expected.Name);
		actual.Width.ShouldBe(expected.Width);
		actual.Height.ShouldBe(expected.Height);
		actual.TileSize.ShouldBe(expected.TileSize);
		actual.Width.ShouldBe(expected.Width);

		CompareCollections(actual: actual.Planes, expected: expected.Planes);
		CompareCollections(actual: actual.Sectors, expected: expected.Sectors);
		CompareCollections(actual: actual.Things, expected: expected.Things);
		CompareCollections(actual: actual.Tiles, expected: expected.Tiles);
		CompareCollections(actual: actual.Triggers, expected: expected.Triggers);
		CompareCollections(actual: actual.Zones, expected: expected.Zones);

		actual.PlaneMaps.Length.ShouldBe(expected.PlaneMaps.Length);

		for (int index = 0; index < actual.PlaneMaps.Length; index++)
		{
			actual.PlaneMaps[index].ShouldBeEquivalentTo(expected.PlaneMaps[index]);
		}
	}

	private static void CompareCollections<T>(ImmutableArray<T> actual, ImmutableArray<T> expected)
	{
		actual.ShouldBeEquivalentTo(expected);
	}
}
