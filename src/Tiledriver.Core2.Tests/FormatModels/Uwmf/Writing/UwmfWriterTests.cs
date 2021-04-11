// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 


using System.Collections.Immutable;
using System.IO;
using System.Text;
using FluentAssertions;
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
                        .Add(new TileSpace(1,2,3))
                        .Add(new TileSpace(4,5,6,7)))),
                Things: ImmutableList<Thing>.Empty,
                Triggers: ImmutableList<Trigger>.Empty);

            using var ms = new MemoryStream();
            map.WriteTo(ms);
            ms.Position = 0;

            using var reader = new StreamReader(ms, Encoding.ASCII);
            var actual = reader.ReadToEnd();

            string expected = 
                "namespace = \"Wolf 3D\";\r\n" +
                "tileSize = 64;\r\n" +
                "name = \"Some Name\";\r\n" +
                "width = 2;\r\n" +
                "height = 1;\r\n" +
                "tile\r\n" +
                "{\r\n" +
                "\ttextureEast = \"east\";\r\n" +
                "\ttextureNorth = \"north\";\r\n" +
                "\ttextureWest = \"west\";\r\n" +
                "\ttextureSouth = \"south\";\r\n" +
                "}\r\n" +
                "planeMap\r\n" +
                "{\r\n" +
                "\t{1,2,3},\r\n" +
                "\t{4,5,6,7}\r\n" +
                "}\r\n";

            actual.Should().Be(expected);
        }
    }
}