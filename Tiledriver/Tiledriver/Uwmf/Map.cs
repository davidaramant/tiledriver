using System.Collections.Generic;
using System.IO;

namespace Tiledriver.Uwmf
{
    /// <summary>
    /// A UWMF map.
    /// </summary>
    public sealed class Map : IUwmfEntry
    {
        public const string Namespace = "Wolf3D";
        public int TileSize { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public readonly List<Tile> Tiles = new List<Tile>();
        public readonly List<Sector> Sectors = new List<Sector>();
        public readonly List<Zone> Zones = new List<Zone>();
        public readonly List<Plane> Planes = new List<Plane>();
        public readonly List<Planemap> Planemaps = new List<Planemap>();
        public readonly List<Thing> Things = new List<Thing>();
        public readonly List<Trigger> Triggers = new List<Trigger>();

        public StreamWriter Write(StreamWriter writer)
        {
            return writer.
                Attribute("namespace", Namespace).
                Attribute("tilesize", TileSize).
                Attribute("name", Name).
                Attribute("width", Width).
                Attribute("height", Height).
                Blocks(Tiles).
                Blocks(Sectors).
                Blocks(Zones).
                Blocks(Planes).
                Blocks(Planemaps).
                Blocks(Things).
                Blocks(Triggers);
        }
    }
}
