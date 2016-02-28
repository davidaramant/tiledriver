using System.Drawing;

namespace Tiledriver.Generator.SimpleGeometry
{
    public sealed class GeometrySlice
    {
        public Rectangle Room { get; }
        public Rectangle? Hallway { get; }

        public GeometrySlice(Rectangle room, Rectangle? hallway = null)
        {
            Room = room;
            Hallway = hallway;
        }
    }
}
