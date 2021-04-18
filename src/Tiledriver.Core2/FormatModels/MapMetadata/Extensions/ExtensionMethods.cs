// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Drawing;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.FormatModels.MapMetadata.Extensions
{
    public static class ExtensionMethods
    {
        public static Point GetPosition(this Thing thing) => new Point((int)thing.X, (int)thing.Y);

        public static void AddAllSurrounding(this Queue<Point> points, Point p)
        {
            points.Enqueue(p.Right());
            points.Enqueue(p.Left());
            points.Enqueue(p.Above());
            points.Enqueue(p.Below());
        }

        public static bool Contains(this Size bounds, Point point)
        {
            return
                point.X >= 0 &&
                point.X < bounds.Width &&
                point.Y >= 0 &&
                point.Y < bounds.Height;
        }

        public static Point Right(this Point p) => new Point(p.X + 1, p.Y);
        public static Point Left(this Point p) => new Point(p.X - 1, p.Y);
        public static Point Below(this Point p) => new Point(p.X, p.Y + 1);
        public static Point Above(this Point p) => new Point(p.X, p.Y - 1);
    }
}