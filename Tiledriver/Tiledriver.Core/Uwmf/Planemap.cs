// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tiledriver.Core.Uwmf
{
    public partial class PlaneMap
    {
        public PlaneMap(IEnumerable<TileSpace> spaces)
        {
            TileSpaces.AddRange(spaces);
        }

        private static void WriteBlocks(Stream stream, IEnumerable<TileSpace> tileSpaces)
        {
            WriteLine(stream, String.Join(",\n", tileSpaces.Select(_ => _.AsString())));
        }
    }
}