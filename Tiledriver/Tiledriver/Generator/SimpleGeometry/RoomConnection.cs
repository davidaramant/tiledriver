using System;
using System.Drawing;

namespace Tiledriver.Generator.SimpleGeometry
{
    public enum ConnectionAxis
    {
        NorthSouth,
        EastWest,
    }

    /// <remarks>
    /// The meaning of Room1/Room2 depend on their order and the type of connection.
    /// </remarks>
    public sealed class RoomConnection
    {
        public RoomNode Room1 { get; }
        public RoomNode Room2 { get; }
        public Point Location { get; }
        public ConnectionAxis Axis { get; }

        private RoomConnection( 
                RoomNode room1, 
                RoomNode room2, 
                Point location, 
                ConnectionAxis axis )
        {
            Room1 = room1;
            Room2 = room2;
            Location = location;
            Axis = axis;
        }

        public static RoomConnection CreateNorthSouth( RoomNode northRoom, RoomNode southRoom, Point location)
        {
            return new RoomConnection(
                room1: northRoom,
                room2: southRoom,
                location: location,
                axis: ConnectionAxis.NorthSouth);
        }

        public static RoomConnection CreateEastWest(RoomNode eastRoom, RoomNode westRoom, Point location)
        {
            return new RoomConnection(
                room1: eastRoom,
                room2: westRoom,
                location: location,
                axis: ConnectionAxis.EastWest);
        }

        public bool IsConnectionFor( RoomNode room, Direction direction )
        {
            switch (direction)
            {
                case Direction.East:
                    return Axis == ConnectionAxis.EastWest && Room1 == room;

                case Direction.North:
                    return Axis == ConnectionAxis.NorthSouth && Room2 == room;

                case Direction.West:
                    return Axis == ConnectionAxis.EastWest && Room2 == room;

                case Direction.South:
                    return Axis == ConnectionAxis.NorthSouth && Room1 == room;

                default:
                    throw new InvalidOperationException("Can't handle that direction.");
            }
        }
    }
}
