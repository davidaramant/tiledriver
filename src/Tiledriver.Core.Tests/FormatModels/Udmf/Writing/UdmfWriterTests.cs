// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Immutable;
using System.IO;
using System.Text;
using FluentAssertions;
using Tiledriver.Core.FormatModels.Udmf;
using Tiledriver.Core.FormatModels.Udmf.Writing;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Udmf.Writing
{
	public sealed class UdmfWriterTests
	{
		[Fact]
		public void ShouldFormatMapNicely()
		{
			var map = new MapData(
				NameSpace: "Doom",
				Vertices: ImmutableArray.Create(new Vertex(1, 2)),
				LineDefs: ImmutableArray<LineDef>.Empty,
				SideDefs: ImmutableArray.Create(new SideDef(sector: 0, textureMiddle: "texture")),
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
				"sidedef",
				"{",
				"\ttextureMiddle = \"texture\";",
				"\tsector = 0;",
				"}",
				"vertex",
				"{",
				"\tx = 1;",
				"\ty = 2;",
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
}
