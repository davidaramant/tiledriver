// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;

namespace Tiledriver.Core.FormatModels.Common
{
    public struct OldMapSpot
    {
        public readonly ushort OldNum;
        public readonly int Index;
        public readonly Point Location;

        public OldMapSpot(ushort oldNum, int index, int x, int y)
        {
            OldNum = oldNum;
            Index = index;
            Location = new Point(x,y);
        }
    }
}
