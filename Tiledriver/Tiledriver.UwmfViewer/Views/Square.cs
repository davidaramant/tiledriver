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
                Stretch = Stretch.Uniform
            };
            SetProperties(element);

            Canvas.SetLeft(element, x * size);
            Canvas.SetTop(element, y * size);

            return element;
        }

        private void SetProperties(Path element)
        {
            SolidColorBrush color;
            string path;
            if (Tile == null)
            {
                color = Colors.Black.ToBrush();
                path = MapItem.SQUARE;
            }
            else if (Tile.TextureNorth.StartsWith("DOOR"))
            {
                color = Colors.Gray.ToBrush();
                path = MapItem.NSDOOR;
            }
            else if (Tile.TextureNorth.StartsWith("SLOT"))
            {
                color = Colors.Gray.ToBrush();
                path = MapItem.EWDOOR;
            }
            else
            {
                color = Colors.LightGray.ToBrush();
                path = MapItem.SQUARE;
            }

            element.Fill = color;
            element.Stroke = color;
            element.Data = Geometry.Parse(path);
        }
    }
}
