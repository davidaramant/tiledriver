// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Immutable;
using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry
{
    public interface IMapBoard
    {
        MapSize Dimensions { get; }
        TileSpace this[MapPosition pos] { get; }
        ImmutableArray<TileSpace> ToPlaneMap();
        MutableMapBoard ToMutable();
    }
}