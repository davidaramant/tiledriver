using System;
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

        public override string DetailType => Tile != null ? "Wall" : "Space";

        public override IEnumerable<DetailProperties> Details
        {
            get
            {
                yield return new DetailProperties("Position", "X", x.ToString());
                yield return new DetailProperties("Position", "Y", y.ToString());

                if (Sector != null)
                {
                    yield return new DetailProperties("Sector", "CeilingTexture", Sector.TextureCeiling);
                    yield return new DetailProperties("Sector", "FloorTexture", Sector.TextureFloor);
                }

                if (Zone >= 0)
                {
                    yield return new DetailProperties("Miscellaneous", "Zone", Zone.ToString());
                }
                
                if (Tile != null)
                {
                    yield return new DetailProperties("Texture", "Texture North", Tile.TextureNorth);
                    yield return new DetailProperties("Texture", "Texture East", Tile.TextureEast);
                    yield return new DetailProperties("Texture", "Texture South", Tile.TextureSouth);
                    yield return new DetailProperties("Texture", "Texture West", Tile.TextureWest);

                    yield return new DetailProperties("Texture Offset", "Offset Vertical", Tile.OffsetVertical ? "Yes" : "No");
                    yield return new DetailProperties("Texture Offset", "Offset Horizontal", Tile.OffsetHorizontal ? "Yes" : "No");

                    yield return new DetailProperties("Texture Blocking", "Blocking North", Tile.BlockingNorth ? "Yes" : "No");
                    yield return new DetailProperties("Texture Blocking", "Blocking East", Tile.BlockingEast ? "Yes" : "No");
                    yield return new DetailProperties("Texture Blocking", "Blocking South", Tile.BlockingSouth ? "Yes" : "No");
                    yield return new DetailProperties("Texture Blocking", "Blocking West", Tile.BlockingWest ? "Yes" : "No");
                }
            }
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
