// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tiledriver.Core.FormatModels.Text.Uwmf
{
    public partial class PlaneMap
    {
        private static void WriteBlocks(Stream stream, IEnumerable<TileSpace> tileSpaces)
        {
            WriteLine(stream, String.Join(",\n", tileSpaces.Select(_ => _.AsString())));
        }
    }
}