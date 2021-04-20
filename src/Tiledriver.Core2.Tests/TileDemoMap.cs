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

        public static MapData Create()
        {
            var originalTiles = DefaultTile.Lookup.Values.ToList();

            var rows = (int) Math.Ceiling((double) originalTiles.Count / Columns);

            const int spaceBetween = 2;

            var mapSize = new MapSize(
                2 + (spaceBetween + 1) * Columns + spaceBetween,
                2 + (spaceBetween + 1) * rows + spaceBetween);

            var boundaryTileIndex = originalTiles.IndexOf(DefaultTile.GrayStone1);

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
            throw new NotImplementedException();
        }
    }
}