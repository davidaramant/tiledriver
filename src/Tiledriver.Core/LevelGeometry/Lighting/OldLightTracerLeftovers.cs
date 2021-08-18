// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.


using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Utils;
using Tiledriver.Core.Utils.ConnectedComponentLabeling;

namespace Tiledriver.Core.LevelGeometry.Lighting
{
    // Some of this stuff gets moved before/after the actual light tracing process
    public sealed class OldLightTracerLeftovers
    {


        struct NeighborLevels
        {
            public readonly int North;
            public readonly int East;
            public readonly int South;
            public readonly int West;

            public NeighborLevels(int north, int east, int south, int west)
            {
                North = north;
                East = east;
                South = south;
                West = west;
            }
        }

        sealed class TileSequence
        {
            private readonly Dictionary<NeighborLevels, int> _levelComboToIndex = new Dictionary<NeighborLevels, int>();

            public int GetTileIndex(NeighborLevels neighbors)
            {
                if (_levelComboToIndex.TryGetValue(neighbors, out int index))
                {
                    return index;
                }

                var nextIndex = _levelComboToIndex.Count;
                _levelComboToIndex.Add(neighbors, nextIndex);
                return nextIndex;
            }

            public IEnumerable<Tile> GetTileDefinitions() =>
                _levelComboToIndex.OrderBy(pair => pair.Value).Select(pair =>
                    new Tile(
                        TextureNorth: $"bwa{pair.Key.North}",
                        TextureSouth: $"bwa{pair.Key.South}",
                        TextureEast: $"bwb{pair.Key.East}",
                        TextureWest: $"bwb{pair.Key.West}"));
        }

    }
}