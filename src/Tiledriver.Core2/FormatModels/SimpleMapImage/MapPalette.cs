// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;
using Tiledriver.Core.FormatModels.MapMetadata;

namespace Tiledriver.Core.FormatModels.SimpleMapImage
{
    public sealed record MapPalette(
            Color Empty,
            Color Wall,
            Color Door,
            Color PushWall,
            Color Unreachable,
            Color Unknown)
        {

        public Color PickColor(TileType type) =>
            type switch
            {
                TileType.Empty => Empty,
                TileType.Wall => Wall,
                TileType.Door => Door,
                TileType.PushWall => PushWall,
                TileType.Unreachable => Unreachable,
                _ => Unknown
            };

        public static readonly MapPalette HighlightWalls = new MapPalette(
            Empty: Color.Black, 
            Wall: Color.White, 
            Door: Color.Red, 
            PushWall: Color.Red,
            Unreachable: Color.Black, 
            Unknown: Color.Fuchsia);

        public static readonly MapPalette CarveOutRooms = new MapPalette(
            Empty: Color.White,
            Wall: Color.Black,
            Door: Color.Red,
            PushWall: Color.Red,
            Unreachable: Color.Black,
            Unknown: Color.Fuchsia);

        public static readonly MapPalette Full = new MapPalette(
            Empty: Color.DarkCyan,
            Wall: Color.Black,
            Door: Color.Red,
            PushWall: Color.Orange,
            Unreachable: Color.Gray,
            Unknown: Color.Fuchsia);
    }
}