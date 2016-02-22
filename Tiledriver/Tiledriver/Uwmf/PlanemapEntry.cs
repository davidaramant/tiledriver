using System;

namespace Tiledriver.Uwmf
{
    public sealed class PlanemapEntry
    {
        public TileId Tile { get; set; }
        public SectorId Sector { get; set; }
        public ZoneId Zone { get; set; }
        public Tag Tag { get; set; }

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
