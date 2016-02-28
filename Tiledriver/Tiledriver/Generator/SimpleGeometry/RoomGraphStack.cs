using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Tiledriver.Generator.SimpleGeometry
{
    public sealed class RoomGraphStack
    {
        private readonly RoomNode _room;
        private readonly RoomConnection _connection;
        private readonly RoomGraphStack _tail;

        public RoomGraphStack(RoomNode room)
        {
            _room = room;
        }

        private RoomGraphStack(
            RoomNode node,
            RoomGraphStack tail,
            RoomConnection connection)
        {
            _room = node;
            _tail = tail;
            _connection = connection;
        }

        public RoomGraphStack Pop()
        {
            if (_tail == null)
            {
                throw new InvalidOperationException("Already at the starting point of the stack.");
            }
            return _tail;
        }

        public RoomGraphStack AddRoom(RoomNode newRoom, RoomConnection newConnection)
        {
            if (!IsNewConnectionValid(newRoom, newConnection))
            {
                throw new InvalidOperationException("This connection already exists.");
            }

            return new RoomGraphStack(
                node: newRoom,
                tail: this,
                connection: newConnection);
        }

        public bool IsNewConnectionValid(RoomNode newRoom, RoomConnection newConnection)
        {
            // Find out what this connection means.  It must be from an existing existingRoom in a given direction.
            RoomNode existingRoom = GetExistingRoomFromConnection(newRoom, newConnection);
            Direction direction = 0;

            switch (newConnection.Axis)
            {
                case ConnectionAxis.EastWest:
                    direction = newConnection.Room1 == newRoom ? Direction.West : Direction.East;
                    break;
                case ConnectionAxis.NorthSouth:
                    direction = newConnection.Room1 == newRoom ? Direction.North : Direction.South;
                    break;
                default:
                    throw new InvalidOperationException("This axis is unknown.");
            }

            // See if this connection already exists
            return !DoesConnectionExistForExistingRoom(existingRoom, direction);
        }

        public bool DoesConnectionExistForExistingRoom(RoomNode existingRoom, Direction direction)
        {
            return GetAllConnections().Any(connection => connection.IsConnectionFor(existingRoom, direction));
        }

        public bool CanThisConnectedRoomBeAdded(RoomNode newRoom, RoomConnection newConnection)
        {
            if (!IsNewConnectionValid(newRoom, newConnection))
            {
                return false;
            }

            // The new existingRoom WILL intersect with the existingRoom it's being connected too, but it should intersect with any others
            return
                GetAllRegions().
                Except(new[] { GetExistingRoomFromConnection(newRoom, newConnection).Bounds }).
                All(room => !room.IntersectsWith(newRoom.Bounds));
        }

        private static RoomNode GetExistingRoomFromConnection(RoomNode newRoom, RoomConnection newConnection)
        {
            return newConnection.Room1 == newRoom ? newConnection.Room2 : newConnection.Room1;
        }

        public IEnumerable<Direction> GetOpenConnections(RoomNode existingRoom)
        {
            return
                new[] { Direction.East, Direction.North, Direction.West, Direction.South }.
                Where(direction => !DoesConnectionExistForExistingRoom(existingRoom, direction));
        }

        #region Aggregation methods

        public IEnumerable<Rectangle> GetAllRoomBounds()
        {
            var stackCell = this;
            while (stackCell != null)
            {
                if (stackCell._room.Type == RoomType.Room)
                {
                    yield return stackCell._room.Bounds;
                }
                stackCell = stackCell._tail;
            }
        }

        public IEnumerable<Rectangle> GetAllHallwayBounds()
        {
            var stackCell = this;
            while (stackCell != null)
            {
                if (stackCell._room.Type == RoomType.Hallway)
                {
                    yield return stackCell._room.Bounds;
                }
                stackCell = stackCell._tail;
            }
        }

        public IEnumerable<Point> GetAllDoorLocations()
        {
            var stackCell = this;
            while (stackCell._connection != null)
            {
                yield return stackCell._connection.Location;
                stackCell = stackCell._tail;
            }
        }

        private IEnumerable<Rectangle> GetAllRegions()
        {
            var stackCell = this;
            while (stackCell != null)
            {
                yield return stackCell._room.Bounds;
                stackCell = stackCell._tail;
            }
        }

        private IEnumerable<RoomConnection> GetAllConnections()
        {
            var stackCell = this;
            while (stackCell._connection != null) // The start existingRoom does not have a connection at that layer
            {
                yield return stackCell._connection;
                stackCell = stackCell._tail;
            }
        }

        #endregion Aggregation methods
    }
}
