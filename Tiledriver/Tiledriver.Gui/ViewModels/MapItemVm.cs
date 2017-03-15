// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Tiledriver.Gui.ViewModels
{
    public abstract class MapItemVm
    {
        public const string TrianglePath = "M 16 0 L 28 32 L 4 32 Z";
        public const string SquarePath = "M 0 0 L 1 0 L 1 1 L 0 1 Z";
        public const string EastWestDoorPath = "M 0 0 L 1 0 L 1 2 L 0 2 Z";
        public const string NorthSouthDoorPath = "M 0 0 L 2 0 L 2 1 L 0 1 Z";
        public const string DiamondPath = "M 1 0 L 2 2 L 1 4 L 0 2 Z";
        public const string CirclePath = "M0,0 A5,5 0 0 0 0,10 A5,5 0 0 0 0,0";
        public const string CrossPath = "M 0 1 L 1 1 L 1 0 L 2 0 L 2 1 L 3 1 L 3 2 L 2 2 L 2 3 L 1 3 L 1 2 L 0 2 Z";
        public const string GunPath = "M 0 0 L 12 0 L 10 2 L 12 9 L 7 9 L 6 3 L 0 3 Z";
        public const string DogPath = "M 8 0 L 15 7 L 11 8 L 16 16 L 0 16 L 5 8 L 1 7 Z";
        public const string CrownPath = "M 1 9 L 0 0 L 4 4 L 6 0 L 8 4 L 12 0 L 11 9 Z";
        public const string ManPath = "M 8 0 L 12 4 L 12 9 L 16 16 L 0 16 L 4 9 L 4 4 Z";
        public const string BossPath = "M 8 0 L 16 8 L 16 16 L 12 16 L 8 12 L 4 16 L 0 16 L 0 8 Z";
        public const string ArrowPath = "M 8 0 L 16 8 L 11 8 L 11 16 L 5 16 L 5 8 L 0 8 Z";
        public const string PacmanGhostPath = "M 8 0 L 12 2 L 14 6 L 16 16 L 13 16 L 11 12 L 9 16 L 7 16 L 5 12 L 3 16 L 0 16 L 2 6 L 4 2 Z";
        public const string AmmoPath = "M 8 0 L 11 2 L 13 6 L 13 16 L 3 16 L 3 6 L 5 2 Z";
        public const string KeyPath = "M 0 12 L 0 7 L 9 7 L 12 4 L 16 8 L 12 12 L 9 9 L 2 9 L 2 12 Z";

        public Point Coordinates { get; internal set; }
        public LayerType LayerType { get; internal set; }
        public virtual bool ShouldAddToCanvas => true;
        public Path Thumbnail => CreatePath(24);

        public abstract string DetailType { get; }
        public abstract IEnumerable<DetailProperties> Details { get; }
        public Dictionary<string, List<DetailProperties>> GroupedDetails => Details.GroupBy(d => d.Category).ToDictionary(g => g.Key ?? "", g => g.ToList());

        protected Path Element;
        public abstract Path CreatePath(int size);
        public abstract double Left(double size);
        public abstract double Top(double size);
        public abstract double Height(double size);
        public abstract double Width(double size);
        public virtual void Rotate(double size) {}
        public virtual void UnRotate(double size) { }

        public void UpdatePathSize(double size)
        {
            if (Element != null)
            {
                UnRotate(Element.ActualHeight);
                Element.Height = size;
                Element.Width = size;
                Rotate(size);
                Canvas.SetLeft(Element, Left(size));
                Canvas.SetTop(Element, Top(size));
            }
        }
    }
}
