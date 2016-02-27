using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public Stream WriteTo(Stream stream)
        {
            CheckSemanticValidity();

            return stream.
                Attribute("namespace", Namespace, indent: false).
                Attribute("tilesize", TileSize, indent: false).
                Attribute("name", Name, indent: false).
                Attribute("width", Width, indent: false).
                Attribute("height", Height, indent: false).
                Blocks(Tiles).
                Blocks(Sectors).
                Blocks(Zones).
                Blocks(Planes).
                Blocks(Planemaps).
                Blocks(Things).
                Blocks(Triggers);
        }

        public void CheckSemanticValidity()
        {
            // TODO: Expand to check all planemaps
            var expectedEntryCount = Width * Height;
            var actualEntryCount = Planemaps.First().Entries.Count;

            if (actualEntryCount != expectedEntryCount)
            {
                throw new MapConstructionException($"Invalid number of planemap entries. Expected {expectedEntryCount} but got {actualEntryCount}.");
            }

            CheckCollection(Planemaps.First().Entries, entry => (int)entry.Tile, Tiles.Count, "Tiles");
            CheckCollection(Planemaps.First().Entries, entry => (int)entry.Sector, Sectors.Count, "Sectors");
            CheckCollection(Planemaps.First().Entries, entry => (int)entry.Zone, Zones.Count, "Zones");

            if (Things.Any(t => t.X < 0 || t.X >= Width || t.Y < 0 || t.Y >= Height))
            {
                throw new MapConstructionException("A Thing was placed outside the map bounds.");
            }

            // TODO: Check that none of the things are inside a wall
        }

        private static void CheckCollection(
                    IEnumerable<PlanemapEntry> entries,
                    Func<PlanemapEntry, int> idGrabber,
                    int definedCount,
                    string name)
        {
            if (entries.Select(idGrabber).Any(id => id >= definedCount))
            {
                throw new MapConstructionException($"Invalid ids found for {name}.");
            }
        }
    }
}
