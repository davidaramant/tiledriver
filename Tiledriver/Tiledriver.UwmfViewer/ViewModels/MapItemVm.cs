using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tiledriver.UwmfViewer.ViewModels
{
    public abstract class MapItemVm
    {
        public static Geometry TRIANGLE = Geometry.Parse("M 16 0 L 28 32 L 4 32 Z");
        public static Geometry SQUARE = Geometry.Parse("M 0 0 L 1 0 L 1 1 L 0 1 Z");
        public static Geometry EWDOOR = Geometry.Parse("M 0 0 L 1 0 L 1 2 L 0 2 Z");
        public static Geometry NSDOOR = Geometry.Parse("M 0 0 L 2 0 L 2 1 L 0 1 Z");
        public static Geometry DIAMOND = Geometry.Parse("M 1 0 L 2 2 L 1 4 L 0 2 Z");
        public static Geometry CIRCLE = Geometry.Parse("M0,0 A5,5 0 0 0 0,10 A5,5 0 0 0 0,0");
        public static Geometry CROSS = Geometry.Parse("M 0 1 L 1 1 L 1 0 L 2 0 L 2 1 L 3 1 L 3 2 L 2 2 L 2 3 L 1 3 L 1 2 L 0 2 Z");
        public static Geometry GUN = Geometry.Parse("M 0 0 L 12 0 L 10 2 L 12 9 L 7 9 L 6 3 L 0 3 Z");
        public static Geometry DOG = Geometry.Parse("M 8 0 L 15 7 L 11 8 L 16 16 L 0 16 L 5 8 L 1 7 Z");
        public static Geometry CROWN = Geometry.Parse("M 1 9 L 0 0 L 4 4 L 6 0 L 8 4 L 12 0 L 11 9 Z");
        public static Geometry MAN = Geometry.Parse("M 8 0 L 12 4 L 12 9 L 16 16 L 0 16 L 4 9 L 4 4 Z");
        public static Geometry BOSS = Geometry.Parse("M 8 0 L 16 8 L 16 16 L 12 16 L 8 12 L 4 16 L 0 16 L 0 8 Z");
        public static Geometry ARROW = Geometry.Parse("M 8 0 L 16 8 L 11 8 L 11 16 L 5 16 L 5 8 L 0 8 Z");
        public static Geometry PACMAN_GHOST = Geometry.Parse("M 8 0 L 12 2 L 14 6 L 16 16 L 13 16 L 11 12 L 9 16 L 7 16 L 5 12 L 3 16 L 0 16 L 2 6 L 4 2 Z");
        public static Geometry AMMO = Geometry.Parse("M 8 0 L 11 2 L 13 6 L 13 16 L 3 16 L 3 6 L 5 2 Z");
        public static Geometry KEY = Geometry.Parse("M 0 12 L 0 7 L 9 7 L 12 4 L 16 8 L 12 12 L 9 9 L 2 9 L 2 12 Z");

        public Point Coordinates { get; internal set; }
        public LayerType LayerType { get; internal set; }
        public virtual bool ShouldAddToCanvas => true;
        public Path Thumbnail => CreatePath(24);

        public abstract string DetailType { get; }
        public abstract IEnumerable<DetailProperties> Details { get; }
        public Dictionary<string, List<DetailProperties>> GroupedDetails => Details.GroupBy(d => d.Category).ToDictionary(g => g.Key ?? "", g => g.ToList());

        protected Path element;
        public abstract Path CreatePath(int size);
        public abstract double Left(double size);
        public abstract double Top(double size);
        public abstract double Height(double size);
        public abstract double Width(double size);
        public virtual void Rotate(double size) {}
        public virtual void UnRotate(double size) { }

        public void UpdatePathSize(double size)
        {
            if (element != null)
            {
                UnRotate(element.ActualHeight);
                element.Height = size;
                element.Width = size;
                Rotate(size);
                Canvas.SetLeft(element, Left(size));
                Canvas.SetTop(element, Top(size));
            }
        }
    }
}
