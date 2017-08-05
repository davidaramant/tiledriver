// Copyright (c) 2017, Leon Organ
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
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

        public IEnumerable<Thing> Things => _data.Things.Where(t => (int)Math.Floor(t.X) == X && (int)Math.Floor(t.Y) == Y);

        public IEnumerable<Trigger> Actions => _data.Triggers.Where(t => t.X == X && t.Y == Y);

        public MapLocation North()
        {
            if (Y < 1)
                return null;

            return new MapLocation(_data, X, Y-1);
        }

        public MapLocation West()
        {
            if (X < 1)
                return null;

            return new MapLocation(_data, X-1, Y);
        }

        public MapLocation South()
        {
            if (Y < _data.Height-1)
                return null;

            return new MapLocation(_data, X, Y + 1);
        }

        public MapLocation East()
        {
            if (X < _data.Width - 1)
                return null;

            return new MapLocation(_data, X+1, Y);
        }

        public bool CanMoveNorth()
        {
            if (Tile.BlockingNorth)
                return false;

            var targetArea = North();

            if (targetArea == null)
                return false;

            if (targetArea.Tile.BlockingSouth)
                return false;

            if (targetArea.Things.Any(t => Actor.GetAll().Single(a => a.ClassName == t.Type).Blocks))
                return false;

            return true;
        }

        public bool CanMoveWest()
        {
            if (Tile.BlockingWest)
                return false;

            var targetArea = West();

            if (targetArea == null)
                return false;

            if (targetArea.Tile.BlockingEast)
                return false;

            if (targetArea.Things.Any(t => Actor.GetAll().Single(a => a.ClassName == t.Type).Blocks))
                return false;

            return true;
        }

        public bool CanMoveSouth()
        {
            if (Tile.BlockingSouth)
                return false;

            var targetArea = South();

            if (targetArea == null)
                return false;

            if (targetArea.Tile.BlockingNorth)
                return false;

            if (targetArea.Things.Any(t => Actor.GetAll().Single(a => a.ClassName == t.Type).Blocks))
                return false;

            return true;
        }

        public bool CanMoveEast()
        {
            if (Tile.BlockingEast)
                return false;

            var targetArea = East();

            if (targetArea == null)
                return false;

            if (targetArea.Tile.BlockingWest)
                return false;

            if (targetArea.Things.Any(t => Actor.GetAll().Single(a => a.ClassName == t.Type).Blocks))
                return false;

            return true;
        }
    }
}
