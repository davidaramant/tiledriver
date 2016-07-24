using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;

namespace Tiledriver.UwmfViewer.Views
{
    public class Square : MapItem
    {
        private int x;
        private int y;

        public Tile Tile { get; set; }
        public Sector Sector { get; set; }
        public int Zone { get; set; }

        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override UIElement ToUIElement(int size)
        {
            var element = new Path()
            {
                Height = size,
                Width = size,
                Fill = FillColor(),
                Data = Geometry.Parse(MapItem.SQUARE),
                Stroke = FillColor(),
                Stretch = Stretch.Uniform
            };

            Canvas.SetLeft(element, x * size);
            Canvas.SetTop(element, y * size);

            return element;
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
