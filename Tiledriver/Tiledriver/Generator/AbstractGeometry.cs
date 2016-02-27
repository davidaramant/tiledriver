using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiledriver.Generator
{
    /// <remarks>
    /// These should be rendered as regions in order since the edges will overlap:
    /// - Hallways
    /// - Rooms
    /// - Doors need to be added to the first room it belongs to (it shouldn't matter which one)
    /// </remarks>
    public sealed class AbstractGeometry
    {
        public readonly List<Rectangle> Hallways = new List<Rectangle>();
        public readonly List<Rectangle> Rooms = new List<Rectangle>();
        public readonly List<Point> Doors = new List<Point>();
    }
}
