using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tiledriver.Generator;
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
                        Arg0 = 1, // Tag
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

        public static Map CreateWithSparseMap()
        {
            Random random = new Random();
            var tagSequence = new TagSequence();
            var sparseMap = new SparseMap(64, 64);

            var bigRoom = new Room(
                new Rectangle(x: 0, y: 0, width: 64, height: 64),
                GetBox(width: 64, height: 64, theme: TileTheme.GrayStone1),
                tagSequence);

            sparseMap.AddRegion(bigRoom);

            var littleRoom = new Room(
                new Rectangle(x: 0, y: 0, width: 4, height: 4),
                GetBox(4, 4, TileTheme.GrayStone1),
                tagSequence);


            littleRoom.AddThing(new RegionThing(
                locationOffset: new Point(1, 1),
                actor: WolfActor.Guard,
                facing: Direction.NorthWest));

            littleRoom.AddDoor(roomRow: 3, roomCol: 1, facingNorthSouth: true);
            littleRoom.AddDoor(roomRow: 1, roomCol: 3, facingNorthSouth: false);

            sparseMap.AddRegion(littleRoom);

            //var purpleRoom = new Room(new Rectangle(20, 20, 6, 16), GetBox(6, 16, TileTheme.Purple), tagSequence);
            //purpleRoom.AddDoor(roomRow: 0, roomCol: 2, facingNorthSouth: true);

            //purpleRoom.AddThing(new RegionThing(
            //    locationOffset: new Point(2, 4),
            //    actor: WolfActor.SSGuard,
            //    facing: Direction.North));

            //sparseMap.AddRegion(purpleRoom);

            AbstractGeometry geometry = new AbstractGeometry();
            geometry.Rooms.Add(new Rectangle(6, 19, 6, 10));
            geometry.Hallways.Add(new Rectangle(11, 21, 10, 5));
            geometry.Doors.Add(new Point(6, 23));
            geometry.Doors.Add(new Point(11, 23));
            geometry.Doors.Add(new Point(20, 22));

            geometry.Rooms.Add(new Rectangle(20, 17, 8, 15));
            geometry.Doors.Add(new Point(24, 17));
            geometry.Doors.Add(new Point(24, 31));

            geometry.Hallways.Add(new Rectangle(22, 31, 5, 10));
            geometry.Rooms.Add(new Rectangle(20, 40, 6, 16));
            geometry.Doors.Add(new Point(23, 40));
            geometry.Doors.Add(new Point(23, 55));
            geometry.Doors.Add(new Point(25, 49));
            List<Room> rooms = RandomGenerator.BuildRoomsFromAbstractGeometry(geometry, random, tagSequence);
            List<Room> hallways = RandomGenerator.BuildHallwaysFromAbstractGeometry(geometry, random, tagSequence);
            foreach (Room room in hallways)
            {
                sparseMap.AddRegion(room);
            }
            foreach (Room room in rooms)
            {
                sparseMap.AddRegion(room);
            }

            return sparseMap.Compile();
        }

        private static MapTile[,] GetBox(int width, int height, TileTheme theme)
        {
            var entries = new MapTile[height, width];

            var wallTile = MapTile.Textured(theme);

            // Top wall
            for (var col = 0; col < width; col++)
            {
                entries[0, col] = wallTile;
            }

            for (var row = 1; row < height - 1; row++)
            {
                entries[row, 0] = wallTile;
                for (var col = 1; col < width - 1; col++)
                {
                    entries[row, col] = MapTile.EmptyTile;
                }
                entries[row, width - 1] = wallTile;
            }

            // bottom wall
            for (var col = 0; col < width; col++)
            {
                entries[height - 1, col] = wallTile;
            }

            return entries;
        }


        public static Map CreateWithRandomRegions()
        {
            var tagSequence = new TagSequence();
            var sparseMap = new SparseMap(64, 64);

            var randomlyThemedRoom = new Room(
                boundingBox: new Rectangle(0, 0, 16, 16),
                tiles: CreateBoxOfMapTiles(16, 16, new[] { TileTheme.BlueBrickWall, TileTheme.BlueBrickSkull, TileTheme.BlueBrickSwastika }, new Random()),
                tagSequence: tagSequence);
            randomlyThemedRoom.AddThing(
                new RegionThing(
                    locationOffset: new Point(4, 4),
                    actor: WolfActor.Player1Start,
                    facing: Direction.NorthWest));

            sparseMap.AddRegion(randomlyThemedRoom);

            return sparseMap.Compile();
        }

        private static MapTile[,] CreateBoxOfMapTiles(int width, int height, IEnumerable<TileTheme> wallChoices, Random random)
        {
            // TODO: Probability distributions for wall themes

            var entries = new MapTile[height, width];

            // Top wall
            for (var col = 0; col < width; col++)
            {
                entries[0, col] = MapTile.Textured(GetRandomTheme(wallChoices, random));
            }

            for (var row = 1; row < height - 1; row++)
            {
                entries[row, 0] = MapTile.Textured(GetRandomTheme(wallChoices, random));
                for (var col = 1; col < width - 1; col++)
                {
                    entries[row, col] = MapTile.EmptyTile;
                }
                entries[row, width - 1] = MapTile.Textured(GetRandomTheme(wallChoices, random));
            }

            // bottom wall
            for (var col = 0; col < width; col++)
            {
                entries[height - 1, col] = MapTile.Textured(GetRandomTheme(wallChoices, random));
            }

            return entries;
        }

        private static TileTheme GetRandomTheme(IEnumerable<TileTheme> options, Random random)
        {
            var choiceIndex = random.Next(options.Count());
            return options.ElementAt(choiceIndex);
        }
    }
}