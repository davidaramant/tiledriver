using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Uwmf;
using Tiledriver.Wolf3D;

namespace Tiledriver.Generator
{
    public static class RandomGenerator
    {
        private static readonly int WallDecorationProbability = 100;

        public static Map Create(int seed)
        {
            var random = new Random(seed);

            var tagSequence = new TagSequence();
            var sparseMap = new SparseMap(64, 64);

            var regionsToGenerate = 5;

            for (int i = 0; i < regionsToGenerate; i++)
            {
                var regionShape = PickRegionShape(random);
                switch (regionShape)
                {
                    case RegionShape.RectangularRoom:
                        sparseMap.AddRegion(BuildRectangularRoom(random, tagSequence));
                        break;
                }
            }

            return sparseMap.Compile();
        }

        private static RegionShape PickRegionShape(Random random)
        {
            var probability = random.Next(100);

            return RegionShape.RectangularRoom;
            //if (probability < 80)
            //    return RegionShape.RectangularRoom;
            //else if (probability < 90)
            //    return RegionShape.HorizontalHallway;
            //else
            //    return RegionShape.VerticalHallway;
        }

        private static IRegion BuildRectangularRoom(Random random, TagSequence tagSequence)
        {
            var regionTheme = GetRegionThemeForRoom(random);

            // Inclusive lower bound, exclusive upper bound
            var width = random.Next(5, 16);
            var height = random.Next(5, 16);

            // TODO: Add in decoration walls
            // TODO: Location
            return new Room(
                boundingBox: new Rectangle(x: 0, y: 0, width: width, height: height),
                tiles: CreateBoxOfMapTiles(width, height, regionTheme.NormalWalls, random),
                tagSequence: tagSequence);
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

        private static RegionTheme GetRegionThemeForRoom(Random random)
        {
            var probability = random.Next(100);

            if (probability < 50)
                return RegionTheme.Dungeon;
            else
                return RegionTheme.OfficerLounge;
        }

        // TODO Make this private
        public static List<Room> BuildRoomsFromAbstractGeometry(AbstractGeometry geometry, Random random, TagSequence tagSequence)
        {
            List<Room> rooms = new List<Room>();
            foreach (Rectangle roomRectangle in geometry.Rooms)
            {
                RegionTheme regionTheme = GetRegionThemeForRoom(random);
                MapTile[,] tiles = new MapTile[roomRectangle.Height, roomRectangle.Width];

                BuildRoomWalls(roomRectangle, tiles, regionTheme, random);

                for(int row = 1; row < roomRectangle.Height - 1; row++)
                {
                    for(int col = 1; col < roomRectangle.Width - 1; col++)
                    {
                        tiles[row, col] = MapTile.EmptyTile;
                    }
                }

                Room room = new Room(roomRectangle, tiles, tagSequence);
                // Place doors
                foreach (Point door in geometry.Doors)
                {
                    if ((door.X == roomRectangle.Left) || (door.X == roomRectangle.Right - 1)
                        && (door.Y >= roomRectangle.Top) && (door.Y < roomRectangle.Bottom))
                    {
                        room.AddDoor(door.Y - roomRectangle.Top, door.X - roomRectangle.Left, false);
                    }
                    else if ((door.Y == roomRectangle.Top) || (door.Y == roomRectangle.Bottom - 1)
                        && (door.X >= roomRectangle.Left) && (door.X < roomRectangle.Right))
                    {
                        room.AddDoor(door.Y - roomRectangle.Top, door.X - roomRectangle.Left, true);
                    }
                }

                // Place enemies
                if (regionTheme.EnemyTypes.Count > 0)
                {
                    int numEnemies = (roomRectangle.Width - 2) * (roomRectangle.Height - 2) / 16;
                    HashSet<Point> enemyPositions = new HashSet<Point>();
                    for (int index = 0; index < numEnemies; index++)
                    {
                        Point position;
                        do
                        {
                            position = new Point(random.Next(1, roomRectangle.Width - 1), random.Next(1, roomRectangle.Height - 1));
                        } while (enemyPositions.Contains(position));
                        enemyPositions.Add(position);
                        room.AddThing(new RegionThing(
                            locationOffset: position,
                            actor: GetRandomEnemy(regionTheme, random),
                            facing: GetRandomDirection(random)));
                    }
                }

                rooms.Add(room);
            }

            return rooms;
        }

        private static Direction GetRandomDirection(Random random)
        {
            switch(random.Next(8))
            {
                case 0:
                    return Direction.East;
                case 1:
                    return Direction.NorthEast;
                case 2:
                    return Direction.North;
                case 3:
                    return Direction.NorthWest;
                case 4:
                    return Direction.West;
                case 5:
                    return Direction.SouthWest;
                case 6:
                    return Direction.South;
                default:
                    break;
            }
            return Direction.SouthEast;
        }

        private static WolfActor GetRandomEnemy(RegionTheme regionTheme, Random random)
        {
            return regionTheme.EnemyTypes[random.Next(regionTheme.EnemyTypes.Count)];
        }

        public static List<Room> BuildHallwaysFromAbstractGeometry(AbstractGeometry geometry, Random random, TagSequence tagSequence)
        {
            List<Room> rooms = new List<Room>();
            foreach (Rectangle roomRectangle in geometry.Hallways)
            {
                RegionTheme regionTheme = GetRegionThemeForRoom(random);
                MapTile[,] tiles = new MapTile[roomRectangle.Height, roomRectangle.Width];

                BuildRoomWalls(roomRectangle, tiles, regionTheme, random);

                for (int row = 1; row < roomRectangle.Height - 1; row++)
                {
                    for (int col = 1; col < roomRectangle.Width - 1; col++)
                    {
                        tiles[row, col] = MapTile.EmptyTile;
                    }
                }

                Room room = new Room(roomRectangle, tiles, tagSequence);

                // Add Health or ammo
                if (random.Next(2) > 0)
                {
                    WolfActor ammoType = WolfActor.Clip;
                    int probability = random.Next(100);
                    if (probability < 1)
                    {
                        ammoType = WolfActor.GatlingGun;
                    }
                    else if (probability < 10)
                    {
                        ammoType = WolfActor.MachineGun;
                    }
                    room.AddThing(new RegionThing(
                        locationOffset: new Point(room.BoundingBox.Width / 2, room.BoundingBox.Height / 2),
                        actor: ammoType,
                        facing: Direction.North));
                }
                else
                {
                    WolfActor healthType = WolfActor.DogFood;
                    int probability = random.Next(100);
                    if (probability < 1)
                    {
                        healthType = WolfActor.ExtraLife;
                    }
                    else if (probability < 10)
                    {
                        healthType = WolfActor.FirstAidKit;
                    }
                    else if (probability < 30)
                    {
                        healthType = WolfActor.Food;
                    }

                    room.AddThing(new RegionThing(
                        locationOffset: new Point(room.BoundingBox.Width / 2, room.BoundingBox.Height / 2),
                        actor: healthType,
                        facing: Direction.North));
                }

                rooms.Add(room);
            }

            return rooms;
        }

        private static void BuildRoomWalls(Rectangle roomRectangle, MapTile[,] tiles, RegionTheme theme, Random random)
        {
            // top wall
            TileTheme decoration = theme.DecorationWalls.Count > 0 ? theme.DecorationWalls[random.Next(theme.DecorationWalls.Count)] : null;
            HashSet<int> decorationIndices = Determine2DObjectSymmetry(roomRectangle.Width, theme.DecorationWalls, random);
            for (int index = 0; index < roomRectangle.Width; index++)
            {
                if (decorationIndices.Contains(index))
                {
                    tiles[0, index] = MapTile.Textured(decoration);
                }
                else
                {
                    tiles[0, index] = MapTile.Textured(theme.NormalWalls[random.Next(theme.NormalWalls.Count)]);
                }
            }

            // bottom wall
            decoration = theme.DecorationWalls.Count > 0 ? theme.DecorationWalls[random.Next(theme.DecorationWalls.Count)] : null;
            decorationIndices = Determine2DObjectSymmetry(roomRectangle.Width, theme.DecorationWalls, random);
            for (int index = 0; index < roomRectangle.Width; index++)
            {
                if (decorationIndices.Contains(index))
                {
                    tiles[roomRectangle.Height - 1, index] = MapTile.Textured(decoration);
                }
                else
                {
                    tiles[roomRectangle.Height - 1, index] = MapTile.Textured(theme.NormalWalls[random.Next(theme.NormalWalls.Count)]);
                }
            }

            // left wall
            decoration = theme.DecorationWalls.Count > 0 ? theme.DecorationWalls[random.Next(theme.DecorationWalls.Count)] : null;
            decorationIndices = Determine2DObjectSymmetry(roomRectangle.Height, theme.DecorationWalls, random);
            for (int index = 1; index < roomRectangle.Height - 1; index++)
            {
                if (decorationIndices.Contains(index))
                {
                    tiles[index, 0] = MapTile.Textured(decoration);
                }
                else
                {
                    tiles[index, 0] = MapTile.Textured(theme.NormalWalls[random.Next(theme.NormalWalls.Count)]);
                }
            }

            // right wall
            decoration = theme.DecorationWalls.Count > 0 ? theme.DecorationWalls[random.Next(theme.DecorationWalls.Count)] : null;
            decorationIndices = Determine2DObjectSymmetry(roomRectangle.Height, theme.DecorationWalls, random);
            for (int index = 1; index < roomRectangle.Height - 1; index++)
            {
                if (decorationIndices.Contains(index))
                {
                    tiles[index, roomRectangle.Width - 1] = MapTile.Textured(decoration);
                }
                else
                {
                    tiles[index, roomRectangle.Width - 1] = MapTile.Textured(theme.NormalWalls[random.Next(theme.NormalWalls.Count)]);
                }
            }
        }

        private static HashSet<int> Determine2DObjectSymmetry(int length, List<TileTheme> decorations, Random random)
        {
            HashSet<int> indices = new HashSet<int>();
            if ((decorations.Count > 0) && (random.Next(100) < WallDecorationProbability))
            {
                int lengthWithoutCorners = length - 2;
                if (lengthWithoutCorners >= 5)
                {
                    indices.Add(3);
                    indices.Add(lengthWithoutCorners - 2);
                }
            }

            return indices;
        }
    }
}
