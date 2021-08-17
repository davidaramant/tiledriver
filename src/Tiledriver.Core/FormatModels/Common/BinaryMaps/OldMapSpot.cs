// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.FormatModels.Common.BinaryMaps
{
    public sealed record OldMapSpot(ushort OldNum, int Index, int X, int Y)
    {
        public Position Location => new(X, Y);
    }
}
