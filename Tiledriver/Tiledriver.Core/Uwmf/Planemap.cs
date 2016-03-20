// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System.Collections.Generic;

namespace Tiledriver.Core.Uwmf
{
    public partial class PlaneMap
    {
        public PlaneMap(IEnumerable<TileSpace> spaces)
        {
            TileSpaces.AddRange(spaces);
        }
    }
}