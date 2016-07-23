using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;
using Tiledriver.UwmfViewer.Utilities;

namespace Tiledriver.UwmfViewer.Views
{
    public class SquareFactory
    {
        private int size = 32;
        private Map map;

        public SquareFactory(Map map, int size)
        {
            this.map = map;
            this.size = size;
        }

        public Square ForCoordinates(int x, int y)
        {
            var tileSpace = map.TileSpaceAt(x, y);

            var square = new Square(x, y, size)
            {
                Tile = map.TileAt(tileSpace.Tile),
                Sector = map.SectorAt(tileSpace.Sector),
                Zone = tileSpace.Zone
            };

            return square;
        }
    }

    public class Square
    {
        private int canvasX;
        private int canvasY;
        private int size;

        public Tile Tile { get; set; }
        public Sector Sector { get; set; }
        public int Zone { get; set; }

        public Square(int x, int y, int size)
        {
            canvasX = x * size;
            canvasY = y * size;
            this.size = size;
        }

        public List<UIElement> ToUIElements()
        {
            return new List<UIElement>{
                Wall()
            };
        }

        public UIElement Wall()
        {
            var element = new Rectangle()
                {
                    Height = size,
                    Width = size,
                    Fill = FillColor()
                };

            Canvas.SetLeft(element, canvasX);
            Canvas.SetTop(element, canvasY);

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
