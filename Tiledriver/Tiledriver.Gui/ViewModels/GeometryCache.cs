// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Windows.Media;

namespace Tiledriver.Gui.ViewModels
{
    public static class GeometryCache
    {
        public static Geometry NoPath => Geometry.Parse("");
        public static Geometry TrianglePath => Geometry.Parse("M 16 0 L 28 32 L 4 32 Z");
        public static Geometry SquarePath => Geometry.Parse("M 0 0 L 1 0 L 1 1 L 0 1 Z");
        public static Geometry EastWestDoorPath => Geometry.Parse("M 0 0 L 1 0 L 1 2 L 0 2 Z");
        public static Geometry NorthSouthDoorPath => Geometry.Parse("M 0 0 L 2 0 L 2 1 L 0 1 Z");
        public static Geometry DiamondPath => Geometry.Parse("M 1 0 L 2 2 L 1 4 L 0 2 Z");
        public static Geometry CirclePath => Geometry.Parse("M0,0 A5,5 0 0 0 0,10 A5,5 0 0 0 0,0");
        public static Geometry CrossPath => Geometry.Parse("M 0 1 L 1 1 L 1 0 L 2 0 L 2 1 L 3 1 L 3 2 L 2 2 L 2 3 L 1 3 L 1 2 L 0 2 Z");
        public static Geometry GunPath => Geometry.Parse("M 0 0 L 12 0 L 10 2 L 12 9 L 7 9 L 6 3 L 0 3 Z");
        public static Geometry DogPath => Geometry.Parse("M 8 0 L 15 7 L 11 8 L 16 16 L 0 16 L 5 8 L 1 7 Z");
        public static Geometry CrownPath => Geometry.Parse("M 1 9 L 0 0 L 4 4 L 6 0 L 8 4 L 12 0 L 11 9 Z");
        public static Geometry ManPath => Geometry.Parse("M 8 0 L 12 4 L 12 9 L 16 16 L 0 16 L 4 9 L 4 4 Z");
        public static Geometry BossPath => Geometry.Parse("M 8 0 L 16 8 L 16 16 L 12 16 L 8 12 L 4 16 L 0 16 L 0 8 Z");
        public static Geometry ArrowPath => Geometry.Parse("M 8 0 L 16 8 L 11 8 L 11 16 L 5 16 L 5 8 L 0 8 Z");
        public static Geometry PacmanGhostPath => Geometry.Parse("M 8 0 L 12 2 L 14 6 L 16 16 L 13 16 L 11 12 L 9 16 L 7 16 L 5 12 L 3 16 L 0 16 L 2 6 L 4 2 Z");
        public static Geometry AmmoPath => Geometry.Parse("M 8 0 L 11 2 L 13 6 L 13 16 L 3 16 L 3 6 L 5 2 Z");
        public static Geometry KeyPath => Geometry.Parse("M 0 12 L 0 7 L 9 7 L 12 4 L 16 8 L 12 12 L 9 9 L 2 9 L 2 12 Z");
        public static Geometry FourArrowPath => Geometry.Parse("M0,0 A5,5 0 0 0 0,10 A5,5 0 0 0 0,0");
    }
}