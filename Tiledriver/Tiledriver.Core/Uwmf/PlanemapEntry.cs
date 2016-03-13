﻿// Copyright (c) 2016 David Aramant
// Distributed under the GNU GPL v2. For full terms see the file LICENSE.

using System;

namespace Tiledriver.Core.Uwmf
{
    public sealed class PlanemapEntry
    {
        public TileId Tile { get; set; }
        public SectorId Sector { get; set; }
        public ZoneId Zone { get; set; }
        public Tag Tag { get; set; }

        public PlanemapEntry(){}

        public PlanemapEntry(TileId tile, SectorId sector, ZoneId zone, Tag tag = Tag.Default)
        {
            Tile = tile;
            Sector = sector;
            Zone = zone;
            Tag = tag;
        }

        public override string ToString()
        {
            var tagPortion =
                Tag != Tag.Default ?
                    $",{(int)Tag}" :
                    String.Empty;

            return $"\t{{{(int)Tile},{(int)Sector},{(int)Zone}{tagPortion}}}";
        }
    }
}