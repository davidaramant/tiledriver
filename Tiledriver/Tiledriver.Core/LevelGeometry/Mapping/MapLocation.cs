// Copyright (c) 2017, Leon Organ
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    class MapLocation
    {
        private readonly MapData _data;

        public MapLocation(MapData data, int x, int y)
        {
            _data = data;
            X = x;
            Y = y;
        }

        public int Y { get; }

        public int X { get; }

        public Tile Tile => _data.Tiles[TilesSpace.Tile];

        public TileSpace TilesSpace => _data.PlaneMaps[0].TileSpaces[X * _data.Width + Y];

        public IEnumerable<Thing> Things => _data.Things.Where(t => t.X - X < 1 && t.Y - Y < 1);

        public IEnumerable<Trigger> Actions => _data.Triggers.Where(t => t.X == X && t.Y == Y);

        public MapLocation North()
        {
            if (Y < 1)
                return null;

            return new MapLocation(_data, X, Y-1);
        }

        public bool CanMoveUp()
        {

            if (Tile.BlockingNorth)
                return false;

            var targetArea = North();

            if (targetArea == null)
                return false;

            if (targetArea.Things.Any(t => Actor.GetAll().Single(a => a.ClassName == t.Type).Blocks))
                return false;

            return true;
        }
    }
}
