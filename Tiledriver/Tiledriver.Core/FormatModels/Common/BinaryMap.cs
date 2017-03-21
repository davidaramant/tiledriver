// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;

namespace Tiledriver.Core.FormatModels.Common
{
    public sealed class BinaryMap
    {
        public string Name { get; }
        public Size Size { get; }

        public ImmutableArray<ushort> GeometryPlane { get; }
        public ImmutableArray<ushort> ThingPlane { get; }
        public ImmutableArray<ushort> FloorCeilingPlane { get; }

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
            FloorCeilingPlane = plane2.ToImmutableArray();
        }

        // TODO: Make an enum for the plane index
        public IEnumerable<OldMapSpot> GetAllSpots(int planeIndex)
        {
            ImmutableArray<ushort> PickPlane(int index)
            {
                switch (index)
                {
                    case 0:
                        return GeometryPlane;
                    case 1:
                        return ThingPlane;
                    case 2:
                        return FloorCeilingPlane;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(planeIndex));
                }
            }

            var plane = PickPlane(planeIndex);

            for (int index = 0; index < plane.Length; index++)
            {
                var x = index % Size.Width;
                var y = index / Size.Height;
                yield return new OldMapSpot(plane[index], index, x, y);
            }
        }
    }
}
