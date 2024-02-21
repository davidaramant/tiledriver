// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

namespace Tiledriver.Core.LevelGeometry
{
    public sealed record Rectangle(Position TopLeft, Size Size)
    {
        public Position BottomRight => new(TopLeft.X + Size.Width - 1, TopLeft.Y + Size.Height - 1);
    }
}
