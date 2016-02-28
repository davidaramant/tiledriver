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
        private static readonly int OrderedDecorationProbability = 66;
        private static readonly int RandomDecorationProbability = 100;

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
            IEnumerable<RegionTheme> availableThemes = RegionTheme.GetAvailableThemes();


            return availableThemes.ElementAt(random.Next(availableThemes.Count()));
        }

        private static int GetNewProbability(Random random)
        {
            return random.Next(100);
        }

        // TODO Make this private
        public static List<Room> BuildRoomsFromAbstractGeometry(AbstractGeometry geometry, Random random, TagSequence tagSequence)
        {
            List<Room> rooms = new List<Room>();
            int roomCount = geometry.Rooms.Count;
            for (int roomIndex = 0; roomIndex < roomCount; roomIndex++)
            {
                Rectangle roomRectangle = geometry.Rooms[roomIndex];
                bool finalRoom = false;//roomIndex == roomCount - 1;
                RegionTheme regionTheme;
                if (finalRoom)
                {
                    regionTheme = RegionTheme.EndRoom;
                }
                else
                {
                    regionTheme = GetRegionThemeForRoom(random);
                }

                MapTile[,] tiles = new MapTile[roomRectangle.Height, roomRectangle.Width];

                BuildRoomWallsAndFloor(roomRectangle, tiles, regionTheme, random);

                Room room = new Room(roomRectangle, tiles, tagSequence);

                AddDoorsToRoom(geometry, roomRectangle, room);

                AddLightsToRoom(roomRectangle, room);

                AddEnemiesAndDecorationsToRoom(random, roomRectangle, regionTheme, room, 0 == roomIndex);

                if (finalRoom)
                {
                    room.AddEndgameTrigger();
                }

                rooms.Add(room);
            }

            return rooms;
        }

        public static List<Room> BuildHallwaysFromAbstractGeometry(AbstractGeometry geometry, Random random, TagSequence tagSequence)
        {
            List<Room> rooms = new List<Room>();
            foreach (Rectangle roomRectangle in geometry.Hallways)
            {
                RegionTheme regionTheme = GetRegionThemeForRoom(random);
                MapTile[,] tiles = new MapTile[roomRectangle.Height, roomRectangle.Width];

                BuildRoomWallsAndFloor(roomRectangle, tiles, regionTheme, random);

                Room room = new Room(roomRectangle, tiles, tagSequence);
                AddDoorsToRoom(geometry, roomRectangle, room);
                AddLightsToRoom(roomRectangle, room);

                rooms.Add(room);
            }

            return rooms;
        }


        private static void AddEnemiesAndDecorationsToRoom(Random random, Rectangle roomRectangle, RegionTheme regionTheme, Room room, bool firstRoom)
        {
            HashSet<Point> usedPositions = new HashSet<Point>();
            HashSet<int> decorationIndices;
            WolfActor decoration;

            // TODO: make sure decorations do not block doors? Simply add space beside door to list of "used spaces"?
            List<Point> doorPositions = room.GetThings().Select(door => new Point((int)door.X - roomRectangle.Left, (int)door.Y - roomRectangle.Top)).ToList();
            foreach(Point doorPosition in doorPositions)
            {
                usedPositions.Add(new Point(doorPosition.X+1, doorPosition.Y));
                usedPositions.Add(new Point(doorPosition.X-1, doorPosition.Y));
                usedPositions.Add(new Point(doorPosition.X, doorPosition.Y+1));
                usedPositions.Add(new Point(doorPosition.X, doorPosition.Y-1));
            }

            // Place ordered decorations against walls
            if (regionTheme.OrderedDecorations.Count > 0 )
            {
                // against top wall
                if (GetNewProbability(random) <= OrderedDecorationProbability)
                {
                    decoration = GetRandomOrderedDecoration(regionTheme, random);
                    decorationIndices = Determine2DObjectSymmetry(roomRectangle.Width, random);
                    foreach (int index in decorationIndices )
                    {
                        Point position = new Point(index, 1);
                        PlaceDecorationAtPositionIfUnused(random, room, usedPositions, decoration, position);
                    }
                }
                // against bottom wall
                if (GetNewProbability(random) <= OrderedDecorationProbability)
                {
                    decoration = GetRandomOrderedDecoration(regionTheme, random);
                    decorationIndices = Determine2DObjectSymmetry(roomRectangle.Width, random);
                    foreach (int index in decorationIndices)
                    {
                        Point position = new Point(index, roomRectangle.Height-2);
                        PlaceDecorationAtPositionIfUnused(random, room, usedPositions, decoration, position);
                    }
                }

                // against left wall
                if (GetNewProbability(random) <= OrderedDecorationProbability)
                {
                    decoration = GetRandomOrderedDecoration(regionTheme, random);
                    decorationIndices = Determine2DObjectSymmetry(roomRectangle.Height, random);
                    foreach (int index in decorationIndices)
                    {
                        Point position = new Point(1, index);
                        PlaceDecorationAtPositionIfUnused(random, room, usedPositions, decoration, position);
                    }
                }
                // against right wall
                if (GetNewProbability(random) <= OrderedDecorationProbability)
                {
                    decoration = GetRandomOrderedDecoration(regionTheme, random);
                    decorationIndices = Determine2DObjectSymmetry(roomRectangle.Height, random);
                    foreach (int index in decorationIndices)
                    {
                        Point position = new Point(roomRectangle.Width - 2, index);
                        PlaceDecorationAtPositionIfUnused(random, room, usedPositions, decoration, position);
                    }
                }
            }

            if (firstRoom)
            {
                Point offset = getUnusedRoomPosition(roomRectangle, room, usedPositions, random);
                usedPositions.Add(offset);
                room.AddThing(new RegionThing(
                    locationOffset: offset,
                    actor: WolfActor.Player1Start,
                    facing: GetRandomDirection(random)));
            }

            // Place enemies (skip if first room)
            if (!firstRoom && (regionTheme.EnemyTypes.Count > 0))
            {
                int numEnemies = (roomRectangle.Width - 2) * (roomRectangle.Height - 2) / 16;

                for (int index = 0; index < numEnemies; index++)
                {
                    Point position = getUnusedRoomPosition(roomRectangle, room, usedPositions, random);
                    usedPositions.Add(position);
                    room.AddThing(new RegionThing(
                        locationOffset: position,
                        actor: GetRandomEnemy(regionTheme, random),
                        facing: GetRandomDirection(random)));
                }
            }

            // Place random decorations
            if (regionTheme.RandomDecorations.Count > 0)
            {
                int numDecorations = (roomRectangle.Width - 2) * (roomRectangle.Height - 2) / 16;
                for (int index = 0; index < numDecorations; index++)
                {
                    Point position = getUnusedRoomPosition(roomRectangle, room, usedPositions, random);
                    usedPositions.Add(position);
                    room.AddThing(new RegionThing(
                        locationOffset: position,
                        actor: GetRandomRandomDecoration(regionTheme, random),
                        facing: GetRandomDirection(random)));
                }
            }
        }

        private static void PlaceDecorationAtPositionIfUnused(Random random, Room room, HashSet<Point> usedPositions, WolfActor decoration, Point position)
        {
            if (!usedPositions.Contains(position))
            {
                usedPositions.Add(position);
                room.AddThing(new RegionThing(
                    locationOffset: position,
                    actor: decoration,
                    facing: GetRandomDirection(random)));
            }
        }

        private static Point getUnusedRoomPosition(Rectangle roomRectangle, Room room, HashSet<Point> usedPositions, Random random )
        {
            // TODO: have this look at things in the room, instead of relying on a passed-in list
            Point position;
            int failureCount = -1;
            do
            {
                failureCount++;
                position = new Point(random.Next(1, roomRectangle.Width - 1), random.Next(1, roomRectangle.Height - 1));
            } while (usedPositions.Contains(position) && failureCount < 100);

            return position;
        }

        private static void AddDoorsToRoom(AbstractGeometry geometry, Rectangle roomRectangle, Room room)
        {
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

        private static WolfActor GetRandomActorFromList(List<WolfActor> themeList, Random random)
        {
            return themeList[random.Next(themeList.Count)];
        }

        private static WolfActor GetRandomEnemy(RegionTheme regionTheme, Random random)
        {
            return GetRandomActorFromList(regionTheme.EnemyTypes, random);
        }

        private static WolfActor GetRandomOrderedDecoration(RegionTheme regionTheme, Random random)
        {
            return GetRandomActorFromList(regionTheme.OrderedDecorations, random);
        }

        private static WolfActor GetRandomRandomDecoration(RegionTheme regionTheme, Random random)
        {
            return GetRandomActorFromList(regionTheme.RandomDecorations, random);
        }

        private static void AddLightsToRoom(Rectangle roomRectangle, Room room)
        {
            int interiorWidth = roomRectangle.Width - 2;
            int interiorHeight = roomRectangle.Height - 2;

            bool horizontalIsOdd = interiorWidth % 2 != 0;
            bool verticalIsOdd = interiorHeight % 2 != 0;

            if( horizontalIsOdd && verticalIsOdd )
            {
                // rooms that are odd in both dimensions get lights evenly spaced
                for (int row = 2; row < roomRectangle.Height - 1; row += 2)
                {
                    for (int col = 2; col < roomRectangle.Width - 1; col += 2)
                    {
                        room.AddThing(new RegionThing(
                            locationOffset: new Point(col, row),
                            actor: WolfActor.CeilingLight,
                            facing: Direction.North));
                    }
                }
            }
            else if( !horizontalIsOdd && !verticalIsOdd && interiorWidth > 3 && interiorHeight > 3)
            {
                // rooms that are even in both dimensions get lights in the corners
                room.AddThing(new RegionThing(
                    locationOffset: new Point(1, 1),
                    actor: WolfActor.CeilingLight,
                    facing: Direction.North));
                room.AddThing(new RegionThing(
                    locationOffset: new Point(1, interiorHeight),
                    actor: WolfActor.CeilingLight,
                    facing: Direction.North));
                room.AddThing(new RegionThing(
                    locationOffset: new Point(interiorWidth, 1),
                    actor: WolfActor.CeilingLight,
                    facing: Direction.North));
                room.AddThing(new RegionThing(
                    locationOffset: new Point(interiorWidth, interiorHeight),
                    actor: WolfActor.CeilingLight,
                    facing: Direction.North));
            }

        }

        private static void BuildRoomWallsAndFloor(Rectangle roomRectangle, MapTile[,] tiles, RegionTheme theme, Random random)
        {
            // top wall
            TileTheme decoration = theme.DecorationWalls.Count > 0 ? theme.DecorationWalls[random.Next(theme.DecorationWalls.Count)] : null;
            HashSet<int> decorationIndices = theme.DecorationWalls.Count > 0 ? Determine2DObjectSymmetry(roomRectangle.Width, random) : null;
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
            decorationIndices = theme.DecorationWalls.Count > 0 ? Determine2DObjectSymmetry(roomRectangle.Width, random):null;
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
            decorationIndices = theme.DecorationWalls.Count > 0 ? Determine2DObjectSymmetry(roomRectangle.Height, random) : null;
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
            decorationIndices = theme.DecorationWalls.Count > 0 ? Determine2DObjectSymmetry(roomRectangle.Height, random):null;
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

            // Floor
            for (int row = 1; row < roomRectangle.Height - 1; row++)
            {
                for (int col = 1; col < roomRectangle.Width - 1; col++)
                {
                    tiles[row, col] = MapTile.EmptyTile;
                }
            }

        }

        private static HashSet<int> Determine2DObjectSymmetry(int length, Random random)
        {
            HashSet<int> indices = new HashSet<int>();
            if ( GetNewProbability(random) < WallDecorationProbability)
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
