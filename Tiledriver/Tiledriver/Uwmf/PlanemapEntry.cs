using System;
using System.IO;

namespace Tiledriver.Uwmf
{
    public sealed class PlanemapEntry : IUwmfEntry
    {
        public TileId Tile { get; set; }
        public SectorId Sector { get; set; }
        public ZoneId Zone { get; set; }
        public Tag? Tag { get; set; }

        public StreamWriter Write(StreamWriter writer)
        {
            var tagPortion =
                Tag != null ?
                    $",{Tag}" :
                    String.Empty;

            return writer.Line($"{Tile},{Sector},{Zone}{tagPortion}");
        }
    }
}
