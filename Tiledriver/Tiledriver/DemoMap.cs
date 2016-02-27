using System.Collections.Generic;
using Tiledriver.Uwmf;
using Tiledriver.Wolf3D;

namespace Tiledriver
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
                    new Tile
                    {
                        TextureNorth = "DOOR1_1",
                        TextureSouth = "DOOR1_1",
                        TextureEast = "SLOT1_1",
                        TextureWest = "SLOT1_1",
                        OffsetHorizontal = true,
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
                        Y = 4.5,
                        Angle = 90,
                        Skill1 = true,
                        Skill2 = true,
                        Skill3 = true,
                        Skill4 = true,
                    },
                    new Thing
                    {
                        Type = WolfActor.Guard.Id,
                        X = 1.5,
                        Y = 1.5,
                        Angle = 270,
                        Skill1 = true,
                        Skill2 = true,
                        Skill3 = true,
                        Skill4 = true,
                    }
                },
                Triggers =
                {
                    new Trigger
                    {
                        X = 1,
                        Y = 3,
                        Z = 0,
                        Action = 1,
                        Arg0 = 1,
                        Arg1 = 16, // Speed
                        Arg2 = 300, // Delay
                        Arg3 = 0, // Lock
                        Arg4 = 1, // 1 for doors facing North/South, 0 for doors facing East/West
                        PlayerUse = true,
                        Repeatable = true,
                        MonsterUse = true,
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

            // Make a room
            entries[1, 3] = solidTile;
            entries[2, 3] = solidTile;
            entries[3, 2] = solidTile;
            entries[3, 3] = solidTile;

            // Make the inside a different sound zone
            entries[1, 1] = new PlanemapEntry(TileId.NotSpecified, 0, (ZoneId)1);
            entries[1, 2] = new PlanemapEntry(TileId.NotSpecified, 0, (ZoneId)1);
            entries[2, 1] = new PlanemapEntry(TileId.NotSpecified, 0, (ZoneId)1);
            entries[2, 2] = new PlanemapEntry(TileId.NotSpecified, 0, (ZoneId)1);

            //door
            entries[3, 1] = new PlanemapEntry((TileId)1, 0, ZoneId.NotSpecified, (Tag)1);


            // Return all the tiles in the correct order
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
