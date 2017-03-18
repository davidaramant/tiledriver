// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry
{
    /// <summary>
    /// A logical representation of a map.
    /// </summary>
    public sealed class SparseMap
    {
        public string Name { get; }
        public int Width { get; }
        public int Height { get; }

        private readonly List<IRegion> _regions = new List<IRegion>();

        public SparseMap(int width, int height, string name)
        {
            Width = width;
            Height = height;
            Name = name;
        }

        private IEnumerable<TileSpace> GetEntries()
        {
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    yield return GetEntryForPosition(row, col);
                }
            }
        }

        private TileSpace GetEntryForPosition(int row, int col)
        {
            // HACK: Sectors are always hardcoded to 0.
            // TODO: Get rid of that.  Based on the TileTheme I guess?
            var fallbackEntry = new TileSpace(tile: -1, sector: 0, zone: -1);

            // TODO: Is this way of generating a zone number sustainable?
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
                        return new TileSpace(tile: -1, sector: 0, zone: indexedRegion.Item1);

                    case MapTileType.Textured:
                        return new TileSpace(
                            tile: tile.Theme.Id,
                            sector: 0,
                            zone: indexedRegion.Item1,
                            tag: tile.Tag ?? 0);

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

        public MapData Compile()
        {
            // TODO: Move sector definition into TileTheme
            return new MapData
            (
                nameSpace: "Wolf3D",
                tileSize: 64,
                name: Name,
                width: Width,
                height: Height,
                tiles: TileTheme.GetAll().OrderBy(t => t.Id).Select(t => t.Definition),
                sectors: new[]
                {
                    new Sector
                    {
                        TextureCeiling = "#383838",
                        TextureFloor = "#707070",
                    }
                },
                zones: new[]
                {
                    new Zone(),
                },
                planes: new[] { new Plane { Depth = 64 } },
                planeMaps: new[] { new PlaneMap(GetEntries()) },
                things: _regions.SelectMany(r => r.GetThings()),
                triggers: _regions.SelectMany(r => r.GetTriggers())
            );
        }
    }
}