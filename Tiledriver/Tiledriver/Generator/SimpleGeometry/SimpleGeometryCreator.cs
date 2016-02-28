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
            var startRoom = new GeometrySlice(CreateStartingRoom());
            var geometry = GeometryStack.Empty.Push(startRoom);

            var directionsToTry = GetRandomDirections().ToArray();
            foreach (var direction in directionsToTry)
            {
                TryAddRoom(startRoom, geometry, direction);
            }

            var results = new AbstractGeometry();
            return results;
        }

        private IEnumerable<Direction> GetRandomDirections()
        {
            Func<bool> include = () => _random.Next(1) == 0;

            if (include())
                yield return Direction.East;
            if (include())
                yield return Direction.South;
            if (include())
                yield return Direction.West;
            if (include())
                yield return Direction.North;
        }

        private Direction GetRandomDirection()
        {
            return (Direction)(_random.Next(4) * 90);
        }

        private void TryAddRoom(GeometrySlice currentSlice, GeometryStack geometry, Direction direction)
        {
            var addHallway = _random.Next(0, 10) > 7;

            switch (direction)
            {
                case Direction.North:
                case Direction.South:
                case Direction.East:
                case Direction.West:
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private Rectangle CreateStartingRoom()
        {
            var roomWidth = _random.Next(minValue: 5, maxValue: 16);
            var roomHeight = _random.Next(minValue: 5, maxValue: 16);

            var left = _random.Next(0, _mapWidth - roomWidth);
            var top = _random.Next(0, _mapHeight - roomHeight);

            return new Rectangle(x: left, y: top, width: roomWidth, height: roomHeight);
        }

        private Rectangle CreateHallway(Point startingPoint, Direction direction)
        {
            int length = _random.Next(3, 7);

            switch (direction)
            {
                case Direction.East:
                    return new Rectangle(x: startingPoint.X, y: startingPoint.Y - 1, width: length, height: 3);
                case Direction.West:
                    return new Rectangle(x: startingPoint.X - length, y: startingPoint.Y - 1, width: length, height: 3);
                case Direction.North:
                    return new Rectangle(x: startingPoint.X - 1, y: startingPoint.Y - length, width: 3, height: length);
                case Direction.South:
                    return new Rectangle(x: startingPoint.X - 1, y: startingPoint.Y, width: 3, height: length);
                default:
                    throw new NotSupportedException();
            }
        }

        private Rectangle CreateRoom(Point startingPoint, Direction direction)
        {
            var roomWidth = _random.Next(minValue: 5, maxValue: 16);
            var roomHeight = _random.Next(minValue: 5, maxValue: 16);

            var room = new Rectangle(x: 0, y: 0, width: roomWidth, height: roomHeight);

            var offset = new Point();

            switch (direction)
            {
                case Direction.East:
                    offset = new Point(x: startingPoint.X, y: startingPoint.Y - roomHeight / 2);
                    break;
                case Direction.West:
                    offset = new Point(x: startingPoint.X - roomWidth, y: startingPoint.Y - roomHeight / 2);
                    break;
                case Direction.North:
                    offset = new Point(x: startingPoint.X - roomWidth / 2, y: startingPoint.Y - roomHeight);
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
