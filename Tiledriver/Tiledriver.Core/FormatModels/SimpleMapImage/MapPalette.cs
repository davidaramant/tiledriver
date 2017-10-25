// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Drawing;

namespace Tiledriver.Core.FormatModels.SimpleMapImage
{
    public sealed class MapPalette
    {
        public Color Empty { get; }
        public Color Wall { get; }
        public Color Door { get; }
        public Color Unreachable { get; }
        public Color Unknown { get; }

        public MapPalette(
            Color empty,
            Color wall,
            Color door,
            Color unreachable,
            Color unknown)
        {
            Empty = empty;
            Wall = wall;
            Door = door;
            Unreachable = unreachable;
            Unknown = unknown;
        }

        public static readonly MapPalette HighlightWalls = new MapPalette(
            empty: Color.Black, 
            wall: Color.White, 
            door: Color.Red, 
            unreachable: Color.Black, 
            unknown: Color.Fuchsia);

        public static readonly MapPalette CarveOutRooms = new MapPalette(
            empty: Color.White,
            wall: Color.Black,
            door: Color.Red,
            unreachable: Color.Black,
            unknown: Color.Fuchsia);
    }
}