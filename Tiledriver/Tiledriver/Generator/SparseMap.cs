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

        private readonly Queue<IRegion> _regions = new Queue<IRegion>();

        public SparseMap(int width, int height)
        {
            Width = width;
            Height = height;
        }

        private IEnumerable<PlanemapEntry> GetEntries()
        {
            // HACK: Sectors are always hardcoded to 0.
            var undefinedEntry = new PlanemapEntry(TileId.NotSpecified, (SectorId) 0, ZoneId.NotSpecified);

            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    // See if this falls inside of a region
                    var region = _regions.SingleOrDefault(r => r.BoundingBox.Contains(col, row));
                    if (region != null)
                    {
                        yield return region.GetForPosition(row, col);
                    }
                    else
                    {
                        yield return undefinedEntry;
                    }
                }
            }
        }

        public void AddRegion(IRegion region)
        {
            // TODO: Do we want to sanity check this?
            _regions.Enqueue(region);
        }

        public void RemoveLastRegion()
        {
            _regions.Dequeue();
        }

        public Map Compile()
        {
            var map = new Map
            {
                Name = "Test Ouput",
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
            map.Tiles.AddRange(PrefabTile.GetAll().OrderBy(t => t.Id).Select(t => t.Definition));
            map.Zones.AddRange(Enumerable.Repeat(new Zone(), _regions.Count));
            map.Planemaps.Add(new Planemap(GetEntries()));
            map.Things.AddRange( _regions.SelectMany(r=>r.GetThings()));
            map.Triggers.AddRange(_regions.SelectMany(r => r.GetTriggers()));

            return map;
        }
    }
}
