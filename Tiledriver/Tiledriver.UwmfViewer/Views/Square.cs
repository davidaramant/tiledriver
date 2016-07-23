using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;
using Tiledriver.UwmfViewer.Utilities;

namespace Tiledriver.UwmfViewer.Views
{
    public class Square
    {
        private const int dimension = 8;

        private int canvasX;
        private int canvasY;

        public Tile Tile { get; set; }

        public Square(Map map, int x, int y)
        {
            canvasX = x * dimension;
            canvasY = y * dimension;

            var tileSpace = map.TileSpaceAt(x, y);

            Tile = map.TileAt(tileSpace.Tile);
        }

        public UIElement ToUIElement()
        {
            var rect = new Rectangle()
            {
                Height = dimension,
                Width = dimension,
                Fill = FillColor(),
                Stroke = FillColor(),
                StrokeThickness = 0,
                Margin = new Thickness(0)
            };

            Canvas.SetLeft(rect, canvasX);
            Canvas.SetTop(rect, canvasY);

            return rect;
        }

        private SolidColorBrush FillColor()
        {
            if (Tile == null)
            {
                return Colors.Black.ToBrush();
            }

            if (Tile.TextureNorth.StartsWith("DOOR"))
            {
                return Colors.DarkBlue.ToBrush();
            }
            if (Tile.TextureNorth.StartsWith("SLOT"))
            {
                return Colors.DarkBlue.ToBrush();
            }
            return Colors.LightGray.ToBrush();
        }
    }
}
