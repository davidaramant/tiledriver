// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.Utils
{
    public record HslColor(double H, double S, double L)
    {
        public static HslColor FromTuple((double h, double s, double l) hsl) => new(hsl.h, hsl.s, hsl.l);

        public (double h, double s, double l) ToTuple() => (H, S, L);

        public double DistanceTo(HslColor other) =>
            Math.Abs(H - other.H) + Math.Abs(S - other.S) + Math.Abs(L - other.L);
    }
}
