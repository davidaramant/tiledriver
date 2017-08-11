// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Gui.Views;

namespace Tiledriver.Gui.ViewModels
{
    public sealed class SquareVm : MapItemVm
    {
        private readonly int _x;
        private readonly int _y;
        private readonly Tile _tile;
        private readonly Sector _sector;
        private readonly int _zone;

        public SquareVm(int x, int y, Tile tile, Sector sector, int zone)
        {
            _x = x;
            _y = y;
            _tile = tile;
            _sector = sector;
            _zone = zone;
            Coordinates = new Point(x, y);
            LayerType = LayerType.Tile;
        }

        public override Path CreatePath(int size)
        {
            Element = new Path()
            {
                Width = Width(size),
                Height = Width(size),
                Stretch = Stretch.Uniform
            };
            SetProperties(Element);

            Canvas.SetLeft(Element, Left(size));
            Canvas.SetTop(Element, Top(size));

            return Element;
        }

        public override double Left(double size) => _x * size;
        public override double Top(double size) => _y * size;
        public override double Height(double size) => size;
        public override double Width(double size) => size;

        public override bool ShouldAddToCanvas => _tile != null;

        public override string DetailType => _tile == null ? "Space" : (_tile.TextureNorth.StartsWith("DOOR") || _tile.TextureNorth.StartsWith("SLOT")) ? "Door" : "Wall";

        public override IEnumerable<DetailProperties> Details
        {
            get
            {
                yield return new DetailProperties("Position", "X", _x.ToString());
                yield return new DetailProperties("Position", "Y", _y.ToString());

                if (_sector != null)
                {
                    yield return new DetailProperties("Sector", "CeilingTexture", _sector.TextureCeiling);
                    yield return new DetailProperties("Sector", "FloorTexture", _sector.TextureFloor);
                }

                if (_zone >= 0)
                {
                    yield return new DetailProperties("Miscellaneous", "Zone", _zone.ToString());
                }
                
                if (_tile != null)
                {
                    yield return new DetailProperties("Texture", "Texture North", _tile.TextureNorth);
                    yield return new DetailProperties("Texture", "Texture East", _tile.TextureEast);
                    yield return new DetailProperties("Texture", "Texture South", _tile.TextureSouth);
                    yield return new DetailProperties("Texture", "Texture West", _tile.TextureWest);

                    yield return new DetailProperties("Texture Offset", "Offset Vertical", _tile.OffsetVertical ? "Yes" : "No");
                    yield return new DetailProperties("Texture Offset", "Offset Horizontal", _tile.OffsetHorizontal ? "Yes" : "No");

                    yield return new DetailProperties("Texture Blocking", "Blocking North", _tile.BlockingNorth ? "Yes" : "No");
                    yield return new DetailProperties("Texture Blocking", "Blocking East", _tile.BlockingEast ? "Yes" : "No");
                    yield return new DetailProperties("Texture Blocking", "Blocking South", _tile.BlockingSouth ? "Yes" : "No");
                    yield return new DetailProperties("Texture Blocking", "Blocking West", _tile.BlockingWest ? "Yes" : "No");
                }
            }
        }

        private void SetProperties(Path element)
        {
            SolidColorBrush color;
            Geometry path;
            if (_tile == null)
            {
                color = Colors.Black.ToBrush();
                path = GeometryCache.SquarePath;
            }
            else if (_tile.TextureNorth.StartsWith("DOOR"))
            {
                color = Colors.Gray.ToBrush();
                path = GeometryCache.NorthSouthDoorPath;
            }
            else if (_tile.TextureNorth.StartsWith("SLOT"))
            {
                color = Colors.Gray.ToBrush();
                path = GeometryCache.EastWestDoorPath;
            }
            else
            {
                color = Colors.DarkGray.ToBrush();
                path = GeometryCache.SquarePath;
            }

            element.Fill = color;
            element.Stroke = color;
            element.Data =path;
        }
    }
}
