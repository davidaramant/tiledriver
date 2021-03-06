﻿// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.FormatModels.MapMetadata.Extensions
{
    public static class ExtensionMethods
    {
        public static Position GetPosition(this Thing thing) => new Position((int) thing.X, (int) thing.Y);

        public static void AddAllSurrounding(this Queue<Position> positions, Position p)
        {
            positions.Enqueue(p.Right());
            positions.Enqueue(p.Left());
            positions.Enqueue(p.Above());
            positions.Enqueue(p.Below());
        }

        public static bool Contains(this Size bounds, Position position)
        {
            return
                position.X >= 0 &&
                position.X < bounds.Width &&
                position.Y >= 0 &&
                position.Y < bounds.Height;
        }

        public static Position Right(this Position p) => new(p.X + 1, p.Y);
        public static Position Left(this Position p) => new(p.X - 1, p.Y);
        public static Position Below(this Position p) => new(p.X, p.Y + 1);
        public static Position Above(this Position p) => new(p.X, p.Y - 1);
    }
}