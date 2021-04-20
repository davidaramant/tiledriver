// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.FormatModels.Uwmf
{
    public sealed partial record MapData
    {
        private sealed class ImmutableMapBoard : IMapBoard
        {
            private readonly ImmutableArray<TileSpace> _planeMap;

            public Size Dimensions { get; }
            public TileSpace this[MapPosition pos] => _planeMap[pos.Y * Dimensions.Width + pos.X];

            public ImmutableMapBoard(Size dimensions, ImmutableArray<TileSpace> planeMap)
            {
                Dimensions = dimensions;
                _planeMap = planeMap;
            }
        }

        public IMapBoard GetMapBoard() => new ImmutableMapBoard(new Size(Width, Height), PlaneMaps[0]);
    }
}