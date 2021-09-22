// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.LevelGeometry.CaveGeneration
{
    [Flags]
    enum Corners : byte
    {
        BottomLeft = 0b0001,
        BottomRight = 0b0010,
        TopRight = 0b0100,
        TopLeft = 0b1000,

        None = 0,
        All = BottomLeft | BottomRight | TopRight | TopLeft,

        Top = TopLeft | TopRight,
        Bottom = BottomLeft | BottomRight,
        Left = TopLeft | BottomLeft,
        Right = TopRight | BottomRight,

        ExceptBottomLeft = BottomRight | TopRight | TopLeft,
        ExceptBottomRight = BottomLeft | TopRight | TopLeft,
        ExceptTopLeft = BottomLeft | BottomRight | TopRight,
        ExceptTopRight = BottomLeft | BottomRight | TopLeft,

        TopLeftAndBottomRight = TopLeft | BottomRight,
        TopRightAndBottomLeft = TopRight | BottomLeft,
    }
}