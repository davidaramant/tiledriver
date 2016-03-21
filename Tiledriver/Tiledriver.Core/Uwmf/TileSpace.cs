// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.Uwmf
{
    public partial class TileSpace
    {
        public TileSpace(int tile, int sector, int zone, int tag = 0)
        {
            Tile = tile;
            Sector = sector;
            Zone = zone;
            Tag = tag;
        }

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
