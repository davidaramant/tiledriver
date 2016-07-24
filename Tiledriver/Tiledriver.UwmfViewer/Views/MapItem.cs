﻿using System.Windows;

namespace Tiledriver.UwmfViewer.Views
{
    public abstract class MapItem
    {
        public const string TRIANGLE = "M 16 0 L 28 32 L 4 32 Z";
        public const string SQUARE = "M 0 0 L 1 0 L 1 1 L 0 1 Z";
        public const string EWDOOR = "M 0 0 L 1 0 L 1 2 L 0 2 Z";
        public const string NSDOOR = "M 0 0 L 2 0 L 2 1 L 0 1 Z";
        public const string DIAMOND = "M 1 0 L 2 2 L 1 4 L 0 2 Z";
        public const string CIRCLE = "M0,0 A50,50 0 0 0 0,100 A50,50 0 0 0 0,0";
        public const string CROSS = "M 0 1 L 1 1 L 1 0 L 2 0 L 2 1 L 3 1 L 3 2 L 2 2 L 2 3 L 1 3 L 1 2 L 0 2 Z";
        public const string GUN = "M 0 0 L 12 0 L 10 2 L 12 9 L 7 9 L 6 3 L 0 3 Z";
        public const string DOG = "M 8 0 L 15 7 L 11 8 L 16 16 L 0 16 L 5 8 L 1 7 Z";
        public const string CROWN = "M 1 9 L 0 0 L 4 4 L 6 0 L 8 4 L 12 0 L 11 9 Z";
        public const string MAN = "M 8 0 L 11 2 L 11 8 L 16 16 L 0 16 L 5 8 L 5 2 Z";
        public const string ARROW = "M 8 0 L 16 8 L 11 8 L 11 16 L 5 16 L 5 8 L 0 8 Z";
        public const string PACMAN_GHOST = "M 8 0 L 12 2 L 14 6 L 16 16 L 13 16 L 11 12 L 9 16 L 7 16 L 5 12 L 3 16 L 0 16 L 2 6 L 4 2 Z";
        public const string KEY = "M 0 12 L 0 7 L 9 7 L 12 4 L 16 8 L 12 12 L 9 9 L 2 9 L 2 12 Z";

        public abstract UIElement ToUIElement(int size);
    }
}
