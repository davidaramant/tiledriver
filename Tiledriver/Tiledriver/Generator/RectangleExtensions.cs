using System.Drawing;

namespace Tiledriver.Generator
{
    public static class RectangleExtensions
    {
        public static bool PaddedIntersectsWith(this Rectangle source, Rectangle other, int padding)
        {
            var paddedSource = source.CopyWithPadding(padding);
            var paddedOther = other.CopyWithPadding(padding);
            
            return paddedSource.IntersectsWith(paddedOther);
        }

        public static Rectangle CopyWithPadding(this Rectangle rect, int padding)
        {
            return new Rectangle(x: rect.X - padding, y: rect.Y - padding, width: rect.X + padding, height: rect.Y + padding);
        }

        public static int? StraightDistanceFrom(this Rectangle source, Rectangle other)
        {
            // It's above
            if (other.Bottom <= source.Top)
            {
                // It's too far left
                if (other.Right <= source.Left)
                {
                    return null;
                }
                // It's too far right
                if (source.Right <= other.Left)
                {
                    return null;
                }

                return source.Top - other.Bottom;
            }

            // It's below
            if (source.Bottom <= other.Top)
            {
                // It's too far left
                if (other.Right <= source.Left)
                {
                    return null;
                }
                // It's too far right
                if (source.Right <= other.Left)
                {
                    return null;
                }

                return other.Top - source.Bottom;
            }

            // Must match up in the Y-axis, so check the X

            // Is it on the left?
            if (other.Right < source.Left)
            {
                return source.Left - other.Right;
            }
            else
            {
                return other.Left - source.Right;
            }
        }
    }
}
