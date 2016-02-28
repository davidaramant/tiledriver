using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiledriver.Generator.SimpleGeometry
{
    public class SimpleGeometryCreator
    {
        private readonly int _mapWidth;
        private readonly int _mapHeight;
        private readonly Random _random;

        public SimpleGeometryCreator(int mapWidth, int mapHeight, Random random)
        {
            _mapWidth = mapWidth;
            _mapHeight = mapHeight;
            _random = random;
        }

        public AbstractGeometry Create()
        {
            var startRoom = new RoomNode(CreateStartingRoomBounds(), RoomType.Room);
            var roomStack = new RoomGraphStack(startRoom);

            foreach (var direction in roomStack.GetOpenConnections(startRoom))
            {
                var result = TryAddRoom(startRoom, roomStack, direction);
                if (result.Item1)
                {
                    roomStack = result.Item2;
                }
            }

            var results = new AbstractGeometry();
            results.Rooms.AddRange(roomStack.GetAllRoomBounds());
            results.Hallways.AddRange(roomStack.GetAllHallwayBounds());
            results.Doors.AddRange(roomStack.GetAllDoorLocations());
            return results;
        }

        private Tuple<bool, RoomGraphStack> TryAddRoom(
            RoomNode currentRoom,
            RoomGraphStack roomStack,
            Direction directionFromCurrentRoom)
        {
            var roomType = (_random.Next(0, 10) <= 7) ? RoomType.Room : RoomType.Hallway;

            var connectionLocation = currentRoom.GetStartingPointFacing(directionFromCurrentRoom, _random);

            var newRoom = CreateBoundsForTypeOfRoom(roomType, connectionLocation, directionFromCurrentRoom);

            var newConnection = MakeNewConnection(
                oldRoom: currentRoom,
                newRoom: newRoom,
                directionFromOldRoom: directionFromCurrentRoom,
                location: connectionLocation);

            if (roomStack.CanThisConnectedRoomBeAdded(newRoom, newConnection))
            {
                return Tuple.Create(true, roomStack.AddRoom(newRoom, newConnection));
            }

            return Tuple.Create(false, (RoomGraphStack)null);
        }

        private static RoomConnection MakeNewConnection(
            RoomNode oldRoom,
            RoomNode newRoom,
            Direction directionFromOldRoom,
            Point location)
        {
            switch (directionFromOldRoom)
            {
                case Direction.East:
                    return RoomConnection.CreateEastWest(eastRoom: newRoom, westRoom: oldRoom, location: location);
                case Direction.West:
                    return RoomConnection.CreateEastWest(eastRoom: oldRoom, westRoom: newRoom, location: location);
                case Direction.North:
                    return RoomConnection.CreateNorthSouth(northRoom: newRoom, southRoom: oldRoom, location: location);
                case Direction.South:
                    return RoomConnection.CreateNorthSouth(northRoom: oldRoom, southRoom: newRoom, location: location);
                default:
                    throw new NotSupportedException();
            }
        }

        private RoomNode CreateBoundsForTypeOfRoom(RoomType roomType, Point connectionLocation, Direction direction)
        {
            if (roomType == RoomType.Room)
            {
                return new RoomNode(CreateRoomBounds(connectionLocation, direction), roomType);
            }
            else
            {
                return new RoomNode(CreateHallwayBounds(connectionLocation, direction), roomType);
            }
        }

        private Rectangle CreateStartingRoomBounds()
        {
            var roomWidth = _random.Next(minValue: 5, maxValue: 16);
            var roomHeight = _random.Next(minValue: 5, maxValue: 16);

            var left = _random.Next(0, _mapWidth - roomWidth);
            var top = _random.Next(0, _mapHeight - roomHeight);

            return new Rectangle(x: left, y: top, width: roomWidth, height: roomHeight);
        }

        private Rectangle CreateHallwayBounds(Point startingPoint, Direction startingPointDirection)
        {
            var width = 5;
            int length = _random.Next(3, 10);

            switch (startingPointDirection)
            {
                case Direction.East:
                    return new Rectangle(x: startingPoint.X, y: startingPoint.Y - 2, width: length, height: width);
                case Direction.West:
                    return new Rectangle(x: startingPoint.X - length + 1, y: startingPoint.Y - 2, width: length, height: width);
                case Direction.North:
                    return new Rectangle(x: startingPoint.X - 2, y: startingPoint.Y - length + 1, width: width, height: length);
                case Direction.South:
                    return new Rectangle(x: startingPoint.X - 2, y: startingPoint.Y, width: width, height: length);
                default:
                    throw new NotSupportedException();
            }
        }

        private Rectangle CreateRoomBounds(Point startingPoint, Direction startingPointDirection)
        {
            var roomWidth = _random.Next(minValue: 5, maxValue: 16);
            var roomHeight = _random.Next(minValue: 5, maxValue: 16);

            var room = new Rectangle(x: 0, y: 0, width: roomWidth, height: roomHeight);

            var offset = new Point();

            switch (startingPointDirection)
            {
                case Direction.East:
                    offset = new Point(x: startingPoint.X, y: startingPoint.Y - roomHeight / 2);
                    break;
                case Direction.West:
                    offset = new Point(x: startingPoint.X - roomWidth + 1, y: startingPoint.Y - roomHeight / 2);
                    break;
                case Direction.North:
                    offset = new Point(x: startingPoint.X - roomWidth / 2, y: startingPoint.Y - roomHeight + 1);
                    break;
                case Direction.South:
                    offset = new Point(x: startingPoint.X - roomWidth / 2, y: startingPoint.Y);
                    break;
                default:
                    throw new NotSupportedException();
            }

            room.Offset(offset);

            return room;
        }
    }
}
