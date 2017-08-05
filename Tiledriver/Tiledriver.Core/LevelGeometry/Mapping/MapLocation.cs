﻿// Copyright (c) 2017, Leon Organ and Aaron Alexander
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Core.Wolf3D;

namespace Tiledriver.Core.LevelGeometry.Mapping
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MapLocation : IEquatable<MapLocation>
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

        public Tile Tile
        {
            get
            {
                if (TilesSpace.Tile == -1)
                    return null;

                return _data.Tiles[TilesSpace.Tile];
            }
            set
            {
                if (null == value)
                {
                    TilesSpace.Tile = -1;
                }
                else
                {
                    var index = _data.Tiles.IndexOf(value);
                    TilesSpace.Tile = index;
                }
            }
        }

        public TileSpace TilesSpace => _data.PlaneMaps[0].TileSpaces[Y * _data.Width + X];

        public IEnumerable<Thing> Things => _data.Things.Where(t => (int)Math.Floor(t.X) == X && (int)Math.Floor(t.Y) == Y);

        public void AddThing(string className)
        {
            var thing = new Thing(className, X + 0.5, Y + 0.5, 0, 0);
            _data.Things.Add(thing);
        }

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
            if (Y > _data.Height-1)
                return null;

            return new MapLocation(_data, X, Y + 1);
        }

        public MapLocation East()
        {
            if (X > _data.Width - 1)
                return null;

            return new MapLocation(_data, X+1, Y);
        }

        public bool CanMoveNorth()
        {
            return CanMove(t => t.BlockingNorth, t => t.BlockingSouth, North);
        }

        public bool CanMoveWest()
        {
            return CanMove(t => t.BlockingWest, t => t.BlockingEast, West);
        }

        public bool CanMoveSouth()
        {
            return CanMove(t => t.BlockingSouth, t => t.BlockingNorth, South);
        }

        public bool CanMoveEast()
        {
            return CanMove(t => t.BlockingEast, t => t.BlockingWest, East);
        }

        private bool CanMove(Func<Tile, bool> blockInTargetDirection, Func<Tile, bool> blockingInInverseDirection, Func<MapLocation> targetSelector)
        {
            if (null != Tile)
            {
                if (blockInTargetDirection(Tile))
                    return false;
            }

            var targetArea = targetSelector();

            if (targetArea == null)
                return false;

            if (null == targetArea.Tile)
                return true;

            if (blockingInInverseDirection(targetArea.Tile))
                return false;

            if (targetArea.HasBlocker())
                return false;

            return true;
        }
		
		public bool HasBlocker()
        {
            return Things.Any(t => Actor.GetAll().Single(a => a.ClassName == t.Type).Blocks);
        }

        public override bool Equals(object obj)
        {
            var castedOther = obj as MapLocation;
            return Equals(castedOther);
        }

        public bool Equals(MapLocation other)
        {
            if (null == other)
                return false;

            return X == other.X && Y == other.Y;
        }

        private string DebuggerDisplay => $"MapLocation: ({X}, {Y})";
    }
}