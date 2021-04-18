// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;

namespace Tiledriver.Core.FormatModels.Common
{
    public sealed record OldMapSpot(ushort OldNum, int Index, int X, int Y)
    {
        public Point Location => new(X, Y);
    }
}
