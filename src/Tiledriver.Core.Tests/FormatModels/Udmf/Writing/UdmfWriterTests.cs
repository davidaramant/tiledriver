// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using System.Text;
using Shouldly;
using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.FormatModels.Udmf.Writing;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Udmf.Writing;

public sealed class UdmfWriterTests
{
	[Fact]
	public void ShouldFormatMapNicely()
	{
		var map = new MapData(
			NameSpace: "Doom",
			Vertices: [new Vertex(1, 2)],
			LineDefs: ImmutableArray<LineDef>.Empty,
			SideDefs: [new SideDef(sector: 0, textureMiddle: "texture")],
			Sectors: ImmutableArray<Sector>.Empty,
			Things: ImmutableArray<Thing>.Empty
		);

		using var ms = new MemoryStream();
		map.WriteTo(ms);
		ms.Position = 0;

		using var reader = new StreamReader(ms, Encoding.ASCII);
		var actual = reader.ReadToEnd();

		var lines = new[]
		{
			"namespace = \"Doom\";",
			"vertex // 0",
			"{",
			"x = 1.0;",
			"y = 2.0;",
			"}",
			"",
			"sidedef // 0",
			"{",
			"sector = 0;",
			"textureMiddle = \"texture\";",
			"}",
			"",
		};

		var sb = new StringBuilder();
		foreach (var line in lines)
		{
			sb.Append(line);
			sb.Append("\r\n");
		}

		actual.ShouldBe(sb.ToString());
	}
}
