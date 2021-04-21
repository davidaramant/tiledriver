// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using Tiledriver.Core.LevelGeometry;

namespace Tiledriver.Core.FormatModels.Uwmf
{
    public sealed partial record MapData
    {
        public Size Dimensions => new(Width, Height);

        private sealed class ImmutableBoard : IBoard
        {
            private readonly ImmutableArray<TileSpace> _planeMap;

            public Size Dimensions { get; }
            public TileSpace this[Position pos] => _planeMap[pos.Y * Dimensions.Width + pos.X];

            public ImmutableBoard(Size dimensions, ImmutableArray<TileSpace> planeMap)
            {
                Dimensions = dimensions;
                _planeMap = planeMap;
            }

            public ImmutableArray<TileSpace> ToPlaneMap() => _planeMap;        
            public Canvas ToCanvas() => new Canvas(Dimensions).Fill(_planeMap);
        }

        public IBoard GetBoard() => new ImmutableBoard(Dimensions, PlaneMaps[0]);
    }
}