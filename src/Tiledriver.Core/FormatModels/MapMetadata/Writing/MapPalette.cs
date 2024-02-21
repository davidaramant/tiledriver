// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE.

using SkiaSharp;

namespace Tiledriver.Core.FormatModels.MapMetadata.Writing
{
    public sealed record MapPalette(
        SKColor Empty,
        SKColor Wall,
        SKColor Door,
        SKColor PushWall,
        SKColor Unreachable,
        SKColor Unknown
    )
    {
        public SKColor PickColor(TileType type) =>
            type switch
            {
                TileType.Empty => Empty,
                TileType.Wall => Wall,
                TileType.Door => Door,
                TileType.PushWall => PushWall,
                TileType.Unreachable => Unreachable,
                _ => Unknown
            };

        public static readonly MapPalette HighlightWalls =
            new(
                Empty: SKColors.Black,
                Wall: SKColors.White,
                Door: SKColors.Red,
                PushWall: SKColors.Red,
                Unreachable: SKColors.Black,
                Unknown: SKColors.Fuchsia
            );

        public static readonly MapPalette CarveOutRooms =
            new(
                Empty: SKColors.White,
                Wall: SKColors.Black,
                Door: SKColors.Red,
                PushWall: SKColors.Red,
                Unreachable: SKColors.Black,
                Unknown: SKColors.Fuchsia
            );

        public static readonly MapPalette Full =
            new(
                Empty: SKColors.DarkCyan,
                Wall: SKColors.Black,
                Door: SKColors.Red,
                PushWall: SKColors.Orange,
                Unreachable: SKColors.Gray,
                Unknown: SKColors.Fuchsia
            );
    }
}
