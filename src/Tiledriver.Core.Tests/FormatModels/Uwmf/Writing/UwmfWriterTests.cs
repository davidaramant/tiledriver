// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using System.Text;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Uwmf.Writing;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Writing;

public sealed class UwmfWriterTests
{
	[Fact]
	public void ShouldFormatMapNicely()
	{
		var map = new MapData(
			NameSpace: "Wolf 3D",
			TileSize: 64,
			Name: "Some Name",
			Width: 2,
			Height: 1,
			Tiles: ImmutableArray.Create(
				new Tile(TextureEast: "east", TextureNorth: "north", TextureWest: "west", TextureSouth: "south")
			),
			Sectors: ImmutableArray<Sector>.Empty,
			Zones: ImmutableArray<Zone>.Empty,
			Planes: ImmutableArray<Plane>.Empty,
			PlaneMaps: ImmutableArray.Create(
				new[] { new MapSquare(1, 2, 3), new MapSquare(4, 5, 6, 7) }.ToImmutableArray()
			),
			Things: ImmutableArray<Thing>.Empty,
			Triggers: ImmutableArray<Trigger>.Empty
		);

		using var ms = new MemoryStream();
		map.WriteTo(ms);
		ms.Position = 0;

		using var reader = new StreamReader(ms, Encoding.ASCII);
		var actual = reader.ReadToEnd();

		var lines = new[]
		{
			"namespace = \"Wolf 3D\";",
			"tileSize = 64;",
			"name = \"Some Name\";",
			"width = 2;",
			"height = 1;",
			"tile",
			"{",
			"\ttextureEast = \"east\";",
			"\ttextureNorth = \"north\";",
			"\ttextureWest = \"west\";",
			"\ttextureSouth = \"south\";",
			"}",
			"planeMap",
			"{",
			"\t{1,2,3},",
			"\t{4,5,6,7}",
			"}",
		};

		var sb = new StringBuilder();
		foreach (var line in lines)
		{
			sb.AppendLine(line);
		}

		actual.Should().Be(sb.ToString());
	}
}
