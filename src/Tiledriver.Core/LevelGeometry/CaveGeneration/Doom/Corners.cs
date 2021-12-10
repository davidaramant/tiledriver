// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration.Doom;

[Flags]
public enum Corners : byte
{
    LowerLeft = 0b0001,
    LowerRight = 0b0010,
    UpperRight = 0b0100,
    UpperLeft = 0b1000,

    None = 0,
    All = LowerLeft | LowerRight | UpperRight | UpperLeft,

    Upper = UpperLeft | UpperRight,
    Lower = LowerLeft | LowerRight,
    Left = UpperLeft | LowerLeft,
    Right = UpperRight | LowerRight,

    AllButLowerLeft = LowerRight | UpperRight | UpperLeft,
    AllButLowerRight = LowerLeft | UpperRight | UpperLeft,
    AllButUpperLeft = LowerLeft | LowerRight | UpperRight,
    AllButUpperRight = LowerLeft | LowerRight | UpperLeft,

    UpperLeftAndLowerRight = UpperLeft | LowerRight,
    UpperRightAndLowerLeft = UpperRight | LowerLeft,
}
