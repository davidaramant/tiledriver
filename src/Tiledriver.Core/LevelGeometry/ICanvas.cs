﻿// Copyright (c) 2021, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Core.LevelGeometry
{
    public interface ICanvas : IBoard
    {
        new MapSquare this[Position pos] { get; set; }
        new MapSquare this[int x, int y] { get; set; }

        ICanvas Set(Position pos, int? tile = null, int? sector = null, int? zone = null, int? tag = null);
        ICanvas Set(int x, int y, int? tile = null, int? sector = null, int? zone = null, int? tag = null);
    }
}
