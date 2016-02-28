using System.Collections.Generic;
using System.Drawing;

namespace Tiledriver.Generator.SimpleGeometry
{
    public enum RoomType
    {
        Room,
        Hallway,
    }

    public sealed class RoomNode
    {
        public Rectangle Bounds { get; }
        public RoomType Type { get; }

        public RoomNode(Rectangle bounds, RoomType type)
        {
            Bounds = bounds;
            Type = type;
        }
    }
}
