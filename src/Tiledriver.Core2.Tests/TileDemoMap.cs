// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.Tests
{
    public static class TileDemoMap
    {
        private const int Columns = 10;
        const int SpaceBetween = 2;

        public static MapData Create()
        {
            var originalTiles = DefaultTile.Lookup.Values.ToList();

            var rows = (int)Math.Ceiling((double)originalTiles.Count / Columns);

            var mapSize = new MapSize(
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
                    new Sector
                    (
                        TextureCeiling: "#C0C0C0",
                        TextureFloor: "#A0A0A0"
                    )),
                Zones: ImmutableList.Create<Zone>().Add(new Zone()),
                Planes: ImmutableList.Create<Plane>().Add(new Plane(Depth: 64)),
                PlaneMaps: ImmutableList.Create<ImmutableArray<TileSpace>>()
                    .Add(CreateGeometry(originalTiles, mapSize, rows)),
                Things: ImmutableList.Create<Thing>().Add(new Thing
                (
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

        private static ImmutableArray<TileSpace> CreateGeometry(List<Tile> tiles, MapSize mapSize, int rows)
        {
            var boundaryTileIndex = tiles.IndexOf(DefaultTile.GrayStone1);

            var board =
                new MutableMapBoard(mapSize)
                    .Fill(new MapArea(new MapPosition(0, 0), mapSize), tile: boundaryTileIndex)
                    .Fill(new MapArea(new MapPosition(1, 1), new MapSize(mapSize.Width - 2, mapSize.Height - 2)), tile: -1);


            foreach (var row in Enumerable.Range(0, rows))
            {
                int y = ((SpaceBetween + 1) * row) + (SpaceBetween + 1);
                foreach (var col in Enumerable.Range(0, Columns))
                {
                    int x = ((SpaceBetween + 1) * col) + (SpaceBetween + 1);

                    var tile = row * Columns + col;

                    board.Set(new MapPosition(x, y), tile: tile);
                }
            }

            return board.ToPlaneMap();
        }
    }
}