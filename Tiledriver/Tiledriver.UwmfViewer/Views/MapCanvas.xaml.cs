using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;

namespace Tiledriver.UwmfViewer.Views
{
    public partial class MapCanvas
    {
        private int squareSize = 24;
        private List<MapItem> mapItems = new List<MapItem>();
        private Path selectionMarker;

        public event EventHandler<MapItemEventArgs> NotifyNewMapItems;

        public MapCanvas()
        {
            InitializeComponent();

            FullArea.MouseUp += (sender, args) =>
            {
                var pos = args.GetPosition(FullArea);
                HandleEventAt(new Point((int)pos.X / squareSize, (int)pos.Y / squareSize));
            };
        }

        public void Update(Map map, int size)
        {
            squareSize = size;

            ClearDetailsPane();
            FullArea.Children.Clear();
            mapItems = new List<MapItem>();
            selectionMarker = CreateSelectionMarker(squareSize);

            FullArea.Height = map.Height * squareSize;
            FullArea.Width = map.Width * squareSize;
            FullArea.Background = Colors.Black.ToBrush();

            var factory = new MapItemFactory(map);

            // Squares
            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    AddMapItem(factory.BuildSquare(x, y));
                }
            }

            // Things
            foreach (var thing in map.Things)
            {
                AddMapItem(factory.BuildThing(thing));
            }
        }

        private void AddMapItem(MapItem mapItem)
        {
            mapItems.Add(mapItem);
            if (mapItem.ShouldAddToCanvas)
            {
                FullArea.Children.Add(mapItem.CreatePath(squareSize));
            }
        }

        private void HandleEventAt(Point coordinate)
        {
            FullArea.Children.Remove(selectionMarker);
            Canvas.SetLeft(selectionMarker, coordinate.X * squareSize);
            Canvas.SetTop(selectionMarker, coordinate.Y * squareSize);
            FullArea.Children.Add(selectionMarker);

            var filteredMapItems = mapItems
                .Where(i => i.Coordinates.Equals(coordinate))
                .ToList();

            ShowDetails(filteredMapItems);
        }

        private static Path CreateSelectionMarker(int size)
        {
            return new Path()
            {
                Height = size,
                Width = size,
                Data = Geometry.Parse(MapItem.SQUARE),
                Fill = Colors.Transparent.ToBrush(),
                Stroke = Colors.Red.ToBrush(),
                StrokeThickness = 1,
                Stretch = Stretch.Uniform,
                Tag = Guid.NewGuid()
            };
        }

        private void ClearDetailsPane()
        {
            ShowDetails(new List<MapItem>());
        }

        private void ShowDetails(List<MapItem> mapItems)
        {
            NotifyNewMapItems?.Invoke(this, new MapItemEventArgs(mapItems));
        }
    }

    public enum LayerType
    {
        Tile,
        Thing
    }

    public class MapItemEventArgs : EventArgs
    {
        public IEnumerable<MapItem> MapItems { get; }

        public MapItemEventArgs(IEnumerable<MapItem> mapItems)
        {
            MapItems = mapItems;
        }
    }
}
