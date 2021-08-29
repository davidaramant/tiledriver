// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;
using System.Collections.Generic;

namespace Tiledriver.Core.LevelGeometry.Lighting
{
    public sealed class LightMap
    {
        private readonly int[] _lightLevels;
        public Size Size { get; }
        public LightRange Range { get; }

        public int this[int col, int row] => _lightLevels[row * Size.Width + col];
        public int this[Position p] => this[p.X, p.Y];

        public int GetSafeLight(Position p)
        {
            if (p.X < 0 || p.X >= Size.Width ||
                p.Y < 0 || p.Y >= Size.Height)
                return 0;

            return this[p];
        }

        public LightMap(LightRange range, Size size)
        {
            Range = range;
            Size = size;
            _lightLevels = new int[size.Width * size.Height];
        }

        public LightMap Blackout()
        {
            Array.Fill(_lightLevels, -Range.DarkLevels);
            return this;
        }

        public void Lighten(Position point, int amount)
        {
            ref int current = ref _lightLevels[point.Y * Size.Width + point.X];
            current = Math.Min(current + amount, Range.LightLevels);
        }

        public IEnumerable<int> GetLightLevels() => _lightLevels;
    }
}