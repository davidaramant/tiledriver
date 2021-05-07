// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

namespace Tiledriver.Core.FormatModels.MapInfo
{
    public sealed record OffsetGameBorder(
        int Offset,
        string TopLeft,
        string Top,
        string TopRight,
        string Left,
        string Right,
        string BottomLeft,
        string Bottom,
        string BottomRight) : IGameBorder;
}