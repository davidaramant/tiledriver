// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;

namespace Tiledriver.Core.FormatModels.Uwmf
{
    public partial class TileSpace
    {
        public string AsString()
        {
            CheckSemanticValidity();

            var tagPortion =
                Tag != 0 ?
                    $",{(int)Tag}" :
                    String.Empty;

            return $"\t{{{(int)Tile},{(int)Sector},{(int)Zone}{tagPortion}}}";
        }
    }
}
