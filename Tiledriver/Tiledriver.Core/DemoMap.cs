// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;
using Tiledriver.Core.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core
{
    public static class DemoMap
    {
        public static Map Create()
        {
            var map = new Map
            {
                Name = "Test Output",
                Width = 64,
                Height = 64,
                TileSize = 64,
                Tiles =
                {
                    new Tile
                    {
                        TextureNorth = "GSTONEA1",
                        TextureSouth = "GSTONEA1",
                        TextureEast = "GSTONEA2",
                        TextureWest = "GSTONEA2",
                    },
                },
                Sectors =
                {
                    new Sector
                    {
                        TextureCeiling = "#C0C0C0",
                        TextureFloor = "#A0A0A0",
                    }
                },
                Zones =
                {
                    new Zone { },
                },
                Planes = { new Plane { Depth = 64 } },
                Planemaps = { new Planemap(CreateGeometry(width: 64, height: 64)) },
                Things =
                {
                    new Thing
                    {
                        Type = WolfActor.Player1Start.Id,
                        X = 1.5,
                        Y = 1.5,
                        Z = 0,
                        Angle = 0,
                        Skill1 = true,
                        Skill2 = true,
                        Skill3 = true,
                        Skill4 = true,
                    },
                }
            };

            return map;
        }

        private static IEnumerable<PlanemapEntry> CreateGeometry(int width, int height)
        {
            var entries = new PlanemapEntry[height, width];

            var solidTile = new PlanemapEntry(0, 0, ZoneId.NotSpecified);
            var emptyTile = new PlanemapEntry(TileId.NotSpecified, 0, 0);

            // ### Build a big empty square

            // Top wall
            for (var col = 0; col < width; col++)
            {
                entries[0, col] = solidTile;
            }

            for (var row = 1; row < height - 1; row++)
            {
                entries[row, 0] = solidTile;
                for (var col = 1; col < width - 1; col++)
                {
                    entries[row, col] = emptyTile;
                }
                entries[row, width - 1] = solidTile;
            }

            // bottom wall
            for (var col = 0; col < width; col++)
            {
                entries[height - 1, col] = solidTile;
            }


            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    yield return entries[row, col];
                }
            }
        }
    }
}
