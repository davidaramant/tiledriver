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
            current = Math.Min(current + amount, LightTracer.LightLevels);
        }

        public IEnumerable<int> GetLightLevels() => _lightLevels;
    }
}