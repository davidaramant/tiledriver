// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Textures;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.LevelGeometry.CanvasDrawingExtensions;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.DemoMaps
{
    public static class TexturesDemoMap
    {
        private const int NumGradients = 5;
        private const int NumLevels = NumGradients + 1 + NumGradients;
        private const int HorizontalBuffer = 3;
        private const int Height = 9;

        public static MapData Create()
        {
            var originalTiles = DefaultTile.Lookup.Values.ToList();

            var mapSize = new Size(
                HorizontalBuffer + NumLevels + HorizontalBuffer,
                Height);

            var (planeMap, tiles, textures) = CreateGeometry(mapSize);

            return new MapData
            (
                NameSpace: "Wolf3D",
                TileSize: 64,
                Name: "Textures Demo",
                Width: mapSize.Width,
                Height: mapSize.Height,
                Tiles: tiles,
                Sectors: ImmutableList.Create<Sector>().Add(
                    new Sector(
                        TextureCeiling: "#C0C0C0",
                        TextureFloor: "#A0A0A0"
                    )),
                Zones: ImmutableList.Create<Zone>().Add(new Zone()),
                Planes: ImmutableList.Create<Plane>().Add(new Plane(Depth: 64)),
                PlaneMaps: ImmutableList.Create<ImmutableArray<MapSquare>>()
                    .Add(planeMap),
                Things: ImmutableList.Create<Thing>().Add(new Thing(
                    Type: Actor.Player1Start.ClassName,
                    X: 1.5,
                    Y: 1.5,
                    Z: 0,
                    Angle: 0,
                    Skill1: true,
                    Skill2: true,
                    Skill3: true,
                    Skill4: true
                )),
                Triggers: ImmutableList<Trigger>.Empty
            );
        }

        private static (ImmutableArray<MapSquare>, ImmutableList<Tile>, IReadOnlyList<Texture>) CreateGeometry(Size size)
        {
            var tiles = new List<Tile> { DefaultTile.GrayStone1 };
            var textures = new List<Texture>();

            var board =
                new Canvas(size)
                    .FillRectangle(size.ToRectangle(), tile: -1)
                    .OutlineRectangle(size.ToRectangle(), tile: 0);

            var middleY = Height / 2;

            board.Set(HorizontalBuffer + NumGradients, middleY, tile: 0);


            return (board.ToPlaneMap(), tiles.ToImmutableList(),textures);
        }
    }
}