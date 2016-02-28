using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiledriver.Generator.SimpleGeometry
{
    public static class RectangleExtensions
    {
        public static int RightEdge(this Rectangle rectangle)
        {
            return rectangle.Right - 1;
        }

        public static int BottomEdge(this Rectangle rectangle)
        {
            return rectangle.Bottom - 1;
        }

        public static int LeftEdge(this Rectangle rectangle)
        {
            return rectangle.Left;
        }

        public static int TopEdge(this Rectangle rectangle)
        {
            return rectangle.Top;
        }
    }
}
