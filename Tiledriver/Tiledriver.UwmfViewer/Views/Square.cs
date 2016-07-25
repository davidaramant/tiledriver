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
        private readonly int x;
        private readonly int y;
        private readonly Tile tile;
        private readonly Sector sector;
        private readonly int zone;

        public Square(int x, int y, Tile tile, Sector sector, int zone)
        {
            this.x = x;
            this.y = y;
            this.tile = tile;
            this.sector = sector;
            this.zone = zone;
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

        public override string DetailType => tile != null ? "Wall" : "Space";

        public override IEnumerable<DetailProperties> Details
        {
            get
            {
                yield return new DetailProperties("Position", "X", x.ToString());
                yield return new DetailProperties("Position", "Y", y.ToString());

                if (sector != null)
                {
                    yield return new DetailProperties("Sector", "CeilingTexture", sector.TextureCeiling);
                    yield return new DetailProperties("Sector", "FloorTexture", sector.TextureFloor);
                }

                if (zone >= 0)
                {
                    yield return new DetailProperties("Miscellaneous", "Zone", zone.ToString());
                }
                
                if (tile != null)
                {
                    yield return new DetailProperties("Texture", "Texture North", tile.TextureNorth);
                    yield return new DetailProperties("Texture", "Texture East", tile.TextureEast);
                    yield return new DetailProperties("Texture", "Texture South", tile.TextureSouth);
                    yield return new DetailProperties("Texture", "Texture West", tile.TextureWest);

                    yield return new DetailProperties("Texture Offset", "Offset Vertical", tile.OffsetVertical ? "Yes" : "No");
                    yield return new DetailProperties("Texture Offset", "Offset Horizontal", tile.OffsetHorizontal ? "Yes" : "No");

                    yield return new DetailProperties("Texture Blocking", "Blocking North", tile.BlockingNorth ? "Yes" : "No");
                    yield return new DetailProperties("Texture Blocking", "Blocking East", tile.BlockingEast ? "Yes" : "No");
                    yield return new DetailProperties("Texture Blocking", "Blocking South", tile.BlockingSouth ? "Yes" : "No");
                    yield return new DetailProperties("Texture Blocking", "Blocking West", tile.BlockingWest ? "Yes" : "No");
                }
            }
        }

        private void SetProperties(Path element)
        {
            SolidColorBrush color;
            string path;
            if (tile == null)
            {
                color = Colors.Black.ToBrush();
                path = MapItem.SQUARE;
            }
            else if (tile.TextureNorth.StartsWith("DOOR"))
            {
                color = Colors.Gray.ToBrush();
                path = MapItem.NSDOOR;
            }
            else if (tile.TextureNorth.StartsWith("SLOT"))
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
