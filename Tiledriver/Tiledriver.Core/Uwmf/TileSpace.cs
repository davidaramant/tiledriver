﻿// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.Uwmf
{
    public sealed class TileSpace
    {
        public int Tile { get; set; }
        public int Sector { get; set; }
        public int Zone { get; set; }
        public int Tag { get; set; }

        public TileSpace(){}

        public TileSpace(int tile, int sector, int zone, int tag = 0)
        {
            Tile = tile;
            Sector = sector;
            Zone = zone;
            Tag = tag;
        }

        public override string ToString()
        {
            var tagPortion =
                Tag != 0 ?
                    $",{(int)Tag}" :
                    String.Empty;

            return $"\t{{{(int)Tile},{(int)Sector},{(int)Zone}{tagPortion}}}";
        }
    }
}