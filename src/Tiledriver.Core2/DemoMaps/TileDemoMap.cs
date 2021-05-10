// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.DemoMaps
{
    public static class TileDemoMap
    {
        private const int Columns = 10;
        const int SpaceBetween = 2;

        public static MapData Create()
        {
            var originalTiles = DefaultTile.Lookup.Values.ToList();

            var rows = (int)Math.Ceiling((double)originalTiles.Count / Columns);

            var mapSize = new Size(
                2 + (SpaceBetween + 1) * Columns + SpaceBetween,
                2 + (SpaceBetween + 1) * rows + SpaceBetween);

            return new MapData
            (
                NameSpace: "Wolf3D",
                TileSize: 64,
                Name: "Tile Demo",
                Width: mapSize.Width,
                Height: mapSize.Height,
                Tiles: originalTiles.ToImmutableList(),
                Sectors: ImmutableList.Create<Sector>().Add(
                    new Sector(
                        TextureCeiling: "#C0C0C0",
                        TextureFloor: "#A0A0A0"
                    )),
                Zones: ImmutableList.Create<Zone>().Add(new Zone()),
                Planes: ImmutableList.Create<Plane>().Add(new Plane(Depth: 64)),
                PlaneMaps: ImmutableList.Create<ImmutableArray<MapSquare>>()
                    .Add(CreateGeometry(originalTiles, mapSize, rows)),
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

        private static ImmutableArray<MapSquare> CreateGeometry(List<Tile> tiles, Size size, int rows)
        {
            var boundaryTileIndex = tiles.IndexOf(DefaultTile.GrayStone1);

            var board =
                new Canvas(size)
                    .Fill(new Rectangle(new Position(0, 0), size), tile: boundaryTileIndex)
                    .Fill(new Rectangle(new Position(1, 1), new Size(size.Width - 2, size.Height - 2)), tile: -1);

            foreach (var row in Enumerable.Range(0, rows))
            {
                int y = ((SpaceBetween + 1) * row) + (SpaceBetween + 1);
                foreach (var col in Enumerable.Range(0, Columns))
                {
                    int x = ((SpaceBetween + 1) * col) + (SpaceBetween + 1);

                    var tile = row * Columns + col;

                    board.Set(new Position(x, y), tile: tile);
                }
            }

            return board.ToPlaneMap();
        }
    }
}