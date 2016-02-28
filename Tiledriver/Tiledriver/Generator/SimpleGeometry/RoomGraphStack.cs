﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Tiledriver.Generator.SimpleGeometry
{
    public sealed class RoomGraphStack
    {
        private readonly RoomNode Room;
        private readonly RoomConnection _connection;
        private readonly RoomGraphStack _tail;

        public RoomGraphStack(RoomNode room)
        {
            Room = room;
        }

        private RoomGraphStack(
            RoomNode node,
            RoomGraphStack tail,
            RoomConnection connection)
        {
            Room = node;
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
            // Find out what this connection means.  It must be from an existing room in a given direction.
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

            // The new room WILL intersect with the room it's being connected too, but it should intersect with any others
            return
                GetAllRegions().
                Except(new[] { GetExistingRoomFromConnection(newRoom, newConnection).Bounds }).
                Any(room => !room.IntersectsWith(newRoom.Bounds));
        }

        private static RoomNode GetExistingRoomFromConnection(RoomNode newRoom, RoomConnection newConnection)
        {
            return newConnection.Room1 == newRoom ? newConnection.Room2 : newConnection.Room1;
        }

        public IEnumerable<Direction> GetOpenConnections(RoomNode room)
        {
            return
                new[] { Direction.East, Direction.North, Direction.West, Direction.South }.
                Where(direction => !DoesConnectionExistForExistingRoom(room, direction));
        }

        #region Aggregation methods

        public IEnumerable<Rectangle> GetAllRooms()
        {
            var stackCell = this;
            while (stackCell != null)
            {
                if (stackCell.Room.Type == RoomType.Room)
                {
                    yield return stackCell.Room.Bounds;
                }
                stackCell = stackCell._tail;
            }
        }

        public IEnumerable<Rectangle> GetAllHallways()
        {
            var stackCell = this;
            while (stackCell != null)
            {
                if (stackCell.Room.Type == RoomType.Hallway)
                {
                    yield return stackCell.Room.Bounds;
                }
                stackCell = stackCell._tail;
            }
        }

        public IEnumerable<Point> GetAllDoors()
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
                yield return stackCell.Room.Bounds;
                stackCell = stackCell._tail;
            }
        }

        private IEnumerable<RoomConnection> GetAllConnections()
        {
            var stackCell = this;
            while (stackCell._connection != null) // The start room does not have a connection at that layer
            {
                yield return stackCell._connection;
                stackCell = stackCell._tail;
            }
        }

        #endregion Aggregation methods
    }
}
