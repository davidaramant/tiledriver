using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tiledriver.Uwmf;
using Tiledriver.Wolf3D;

namespace Tiledriver.Generator
{
    /// <summary>
    /// A logical representation of a map.
    /// </summary>
    public sealed class SparseMap
    {
        public int Width { get; }
        public int Height { get; }

        private readonly List<IRegion> _regions = new List<IRegion>();

        public SparseMap(int width, int height)
        {
            Width = width;
            Height = height;
        }

        private IEnumerable<PlanemapEntry> GetEntries()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    yield return GetEntryForPosition(row, col);
                }
            }
        }

        private PlanemapEntry GetEntryForPosition(int row, int col)
        {
            // HACK: Sectors are always hardcoded to 0.
            var fallbackEntry = new PlanemapEntry(TileId.NotSpecified, (SectorId)0, ZoneId.NotSpecified);

            var indexedRegionsForPosition = FindIndexedRegionsForPoint(row, col);

            // Find the last region that has something for this position.
            foreach (var indexedRegion in indexedRegionsForPosition.Reverse())
            {
                var tile = indexedRegion.Item2.GetTileAtPosition(row, col);

                switch (tile.Type)
                {
                    case MapTileType.Null:
                        // Do nothing
                        break;

                    case MapTileType.EmptySpace:
                        return new PlanemapEntry(TileId.NotSpecified, (SectorId)0, (ZoneId)indexedRegion.Item1);

                    case MapTileType.Textured:
                        return new PlanemapEntry(
                            (TileId) tile.Theme.Id, 
                            (SectorId) 0, 
                            (ZoneId) indexedRegion.Item1,
                            tag: tile.Tag.HasValue ? (Tag)tile.Tag : Tag.Default);

                    default:
                        throw new Exception("Unknown map tile type.");
                }

            }

            return fallbackEntry;
        }

        private IEnumerable<Tuple<int, IRegion>> FindIndexedRegionsForPoint(int row, int col)
        {
            return
                _regions.Select((region, index) => Tuple.Create(index, region))
                    .Where(ir => ir.Item2.BoundingBox.Contains(x: col, y: row));
        }

        public void AddRegion(IRegion region)
        {
            // TODO: Check that the region is inside of the map area
            _regions.Add(region);
        }

        public void RemoveLastRegion()
        {
            _regions.RemoveAt(_regions.Count - 1);
        }

        public Map Compile()
        {
            var map = new Map
            {
                Name = "Let's Kill Hitler",
                Width = Width,
                Height = Height,
                TileSize = 64,
                Sectors =
                {
                    new Sector
                    {
                        TextureCeiling = "#383838",
                        TextureFloor = "#707070",
                    }
                },
                Planes = { new Plane { Depth = 64 } },
            };
            map.Tiles.AddRange(TileTheme.GetAll().OrderBy(t => t.Id).Select(t => t.Definition));
            map.Zones.AddRange(Enumerable.Repeat(new Zone(), _regions.Count));
            map.Planemaps.Add(new Planemap(GetEntries()));
            map.Things.AddRange(_regions.SelectMany(r => r.GetThings()));
            map.Triggers.AddRange(_regions.SelectMany(r => r.GetTriggers()));

            return map;
        }
    }
}
