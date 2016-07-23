using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Tiledriver.UwmfViewer.Views
{
    static class ViewExtensions
    {
        public static SolidColorBrush ToBrush(this Color color) => new SolidColorBrush(color);

        public static PointCollection ToPointsCollection(this string points) =>
            new PointCollection(points
                .Split(' ')
                .Select(pairString => pairString.Split(','))
                .Select(pairArray => new Point(double.Parse(pairArray[0]), double.Parse(pairArray[1]))));
    }
}
