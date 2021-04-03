// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;
using Tiledriver.Core.FormatModels.MapMetadata;

namespace Tiledriver.Core.FormatModels.SimpleMapImage
{
    public sealed class MapPalette
    {
        public Color Empty { get; }
        public Color Wall { get; }
        public Color Door { get; }
        public Color PushWall { get; }
        public Color Unreachable { get; }
        public Color Unknown { get; }

        public MapPalette(
            Color empty,
            Color wall,
            Color door,
            Color pushWall,
            Color unreachable,
            Color unknown)
        {
            Empty = empty;
            Wall = wall;
            Door = door;
            PushWall = pushWall;
            Unreachable = unreachable;
            Unknown = unknown;
        }

        public Color PickColor(TileType type)
        {
            switch (type)
            {
                case TileType.Empty: return Empty;
                case TileType.Wall: return Wall;
                case TileType.Door: return Door;
                case TileType.PushWall: return PushWall;
                case TileType.Unreachable: return Unreachable;
                default: return Unknown;
            }
        }

        public static readonly MapPalette HighlightWalls = new MapPalette(
            empty: Color.Black, 
            wall: Color.White, 
            door: Color.Red, 
            pushWall: Color.Red,
            unreachable: Color.Black, 
            unknown: Color.Fuchsia);

        public static readonly MapPalette CarveOutRooms = new MapPalette(
            empty: Color.White,
            wall: Color.Black,
            door: Color.Red,
            pushWall: Color.Red,
            unreachable: Color.Black,
            unknown: Color.Fuchsia);

        public static readonly MapPalette Full = new MapPalette(
            empty: Color.DarkCyan,
            wall: Color.Black,
            door: Color.Red,
            pushWall: Color.Orange,
            unreachable: Color.Gray,
            unknown: Color.Fuchsia);
    }
}