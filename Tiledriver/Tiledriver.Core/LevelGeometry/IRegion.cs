// Copyright (c) 2016, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Drawing;
using Tiledriver.Core.FormatModels.Text.Uwmf;

namespace Tiledriver.Core.LevelGeometry
{
    public interface IRegion
    {
        Rectangle BoundingBox { get; }

        IEnumerable<Thing> GetThings();

        IEnumerable<Trigger> GetTriggers();

        MapTile GetTileAtPosition(int mapRow, int mapCol);
    }
}