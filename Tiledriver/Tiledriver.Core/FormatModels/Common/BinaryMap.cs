// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;

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

        public ImmutableArray<ushort> GeometryPlane { get; }
        public ImmutableArray<ushort> ThingPlane { get; }
        public ImmutableArray<ushort> SectorPlane { get; }

        public BinaryMap(
            string name,
            ushort width,
            ushort height,
            ushort[] plane0,
            ushort[] plane1,
            ushort[] plane2)
        {
            Name = name;
            Size = new Size(width, height);
            GeometryPlane = plane0.ToImmutableArray();
            ThingPlane = plane1.ToImmutableArray();
            SectorPlane = plane2.ToImmutableArray();
        }

        public IEnumerable<OldMapSpot> GetAllSpots(BinaryMapPlaneId planeId)
        {
            ImmutableArray<ushort> PickPlane(BinaryMapPlaneId id)
            {
                switch (id)
                {
                    case BinaryMapPlaneId.Geometry:
                        return GeometryPlane;
                    case BinaryMapPlaneId.Thing:
                        return ThingPlane;
                    case BinaryMapPlaneId.Sector:
                        return SectorPlane;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(planeId), $"Unknown plane ID: {id}");
                }
            }

            var plane = PickPlane(planeId);

            for (int index = 0; index < plane.Length; index++)
            {
                var x = index % Size.Width;
                var y = index / Size.Height;
                yield return new OldMapSpot(plane[index], index, x, y);
            }
        }
    }
}
