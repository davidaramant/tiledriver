using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;
using Tiledriver.UwmfViewer.Views;

namespace Tiledriver.UwmfViewer.ViewModels
{
    public class SquareVm : MapItemVm
    {
        private readonly int x;
        private readonly int y;
        private readonly Tile tile;
        private readonly Sector sector;
        private readonly int zone;

        public SquareVm(int x, int y, Tile tile, Sector sector, int zone)
        {
            this.x = x;
            this.y = y;
            this.tile = tile;
            this.sector = sector;
            this.zone = zone;
            Coordinates = new Point(x, y);
            LayerType = LayerType.Tile;
        }

        public override Path CreatePath(int size)
        {
            element = new Path()
            {
                Width = Width(size),
                Height = Width(size),
                Stretch = Stretch.Uniform
            };
            SetProperties(element);

            Canvas.SetLeft(element, Left(size));
            Canvas.SetTop(element, Top(size));

            return element;
        }

        public override double Left(double size) => x * size;
        public override double Top(double size) => y * size;
        public override double Height(double size) => size;
        public override double Width(double size) => size;

        public override bool ShouldAddToCanvas => tile != null;

        public override string DetailType => tile == null ? "Space" : (tile.TextureNorth.StartsWith("DOOR") || tile.TextureNorth.StartsWith("SLOT")) ? "Door" : "Wall";

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
                path = MapItemVm.SQUARE;
            }
            else if (tile.TextureNorth.StartsWith("DOOR"))
            {
                color = Colors.Gray.ToBrush();
                path = MapItemVm.NSDOOR;
            }
            else if (tile.TextureNorth.StartsWith("SLOT"))
            {
                color = Colors.Gray.ToBrush();
                path = MapItemVm.EWDOOR;
            }
            else
            {
                color = Colors.DarkGray.ToBrush();
                path = MapItemVm.SQUARE;
            }

            element.Fill = color;
            element.Stroke = color;
            element.Data = Geometry.Parse(path);
        }
    }
}
