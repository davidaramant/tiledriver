// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using JetBrains.Annotations;

namespace Tiledriver.Core.FormatModels.Common
{
    public sealed class BinaryMap
    {
        public ImmutableArray<ushort> Plane1 { get; }
        public ImmutableArray<ushort> Plane2 { get; }
        public ImmutableArray<ushort> Plane3 { get; }

        public BinaryMap(
            int dimension, 
            [NotNull] IEnumerable<ushort> plane1,
            [NotNull] IEnumerable<ushort> plane2,
            [NotNull] IEnumerable<ushort> plane3)
        {
            Plane1 = plane1.ToImmutableArray();
            Plane2 = plane2.ToImmutableArray();
            Plane3 = plane3.ToImmutableArray();

            if (Plane1.Length != dimension)
            {
                throw new ArgumentOutOfRangeException(nameof(plane1),"Wrong length");
            }
            if (Plane2.Length != dimension)
            {
                throw new ArgumentOutOfRangeException(nameof(plane2), "Wrong length");
            }
            if (Plane3.Length != dimension)
            {
                throw new ArgumentOutOfRangeException(nameof(plane3), "Wrong length");
            }
        }
    }
}
