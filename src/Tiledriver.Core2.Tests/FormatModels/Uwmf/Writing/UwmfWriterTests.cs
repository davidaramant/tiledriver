// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using FluentAssertions;
using System.Collections.Immutable;
using System.IO;
using System.Text;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.FormatModels.Uwmf.Writing;
using Xunit;

namespace Tiledriver.Core.Tests.FormatModels.Uwmf.Writing
{
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
                Tiles: ImmutableList.Create<Tile>().Add(new Tile(
                    TextureEast: "east",
                    TextureNorth: "north",
                    TextureWest: "west",
                    TextureSouth: "south")),
                Sectors: ImmutableList<Sector>.Empty,
                Zones: ImmutableList<Zone>.Empty,
                Planes: ImmutableList<Plane>.Empty,
                PlaneMaps: ImmutableList.Create<PlaneMap>().Add(new PlaneMap(
                    ImmutableList.Create<TileSpace>()
                        .Add(new TileSpace(1, 2, 3))
                        .Add(new TileSpace(4, 5, 6, 7)))),
                Things: ImmutableList<Thing>.Empty,
                Triggers: ImmutableList<Trigger>.Empty);

            using var ms = new MemoryStream();
            map.WriteTo(ms);
            ms.Position = 0;

            using var reader = new StreamReader(ms, Encoding.ASCII);
            var actual = reader.ReadToEnd();

            var lines =
                new[]{
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
                    "}"
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