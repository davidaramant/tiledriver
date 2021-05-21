// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.FormatModels.Common
{
    public enum BinaryMapPlaneId
    {
        Geometry,
        Thing,
        Sector,
    }

    public sealed class BinaryMap
    {
        public string Name { get; }
        public Size Size { get; }

        private readonly ushort[][] _planes;

        public BinaryMap(
            string name,
            ushort width,
            ushort height,
            IEnumerable<ushort[]> planes)
        {
            Name = name;
            Size = new Size(width, height);
            _planes = planes.ToArray();
        }

        public IEnumerable<ushort> GetRawPlaneData(BinaryMapPlaneId planeId) => _planes[(int)planeId];

        public IEnumerable<OldMapSpot> GetAllSpots(BinaryMapPlaneId planeId)
        {
            var plane = _planes[(int)planeId];

            for (int index = 0; index < plane.Length; index++)
            {
                var x = index % Size.Width;
                var y = index / Size.Height;
                yield return new OldMapSpot(plane[index], index, x, y);
            }
        }
    }
}
