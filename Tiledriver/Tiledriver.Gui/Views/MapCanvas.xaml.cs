using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;
using Tiledriver.Gui.ViewModels;

namespace Tiledriver.Gui.Views
{
    public partial class MapCanvas
    {
        private int squareSize = 24;
        private ObservableCollection<MapItemVm> mapItems = new ObservableCollection<MapItemVm>();
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
            mapItems = new ObservableCollection<MapItemVm>();
            selectionMarker = CreateSelectionMarker(squareSize);

            FullArea.Height = map.Height * squareSize;
            FullArea.Width = map.Width * squareSize;
            FullArea.Background = Colors.Black.ToBrush();

            var factory = new MapItemVmFactory(map);

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

        private void AddMapItem(MapItemVm mapItem)
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

            var filteredMapItems = new ObservableCollection<MapItemVm>(mapItems
                .Where(i => i.Coordinates.Equals(coordinate)));

            ShowDetails(filteredMapItems);
        }

        private static Path CreateSelectionMarker(int size)
        {
            return new Path()
            {
                Height = size,
                Width = size,
                Data = Geometry.Parse(MapItemVm.SQUARE),
                Fill = Colors.Transparent.ToBrush(),
                Stroke = Colors.Red.ToBrush(),
                StrokeThickness = 1,
                Stretch = Stretch.Uniform,
                Tag = Guid.NewGuid()
            };
        }

        private void ClearDetailsPane()
        {
            ShowDetails(new ObservableCollection<MapItemVm>());
        }

        private void ShowDetails(ObservableCollection<MapItemVm> mapItems)
        {
            NotifyNewMapItems?.Invoke(this, new MapItemEventArgs(mapItems));
        }
    }

    public class MapItemEventArgs : EventArgs
    {
        public ObservableCollection<MapItemVm> MapItems { get; }

        public MapItemEventArgs(ObservableCollection<MapItemVm> mapItems)
        {
            MapItems = mapItems;
        }
    }
}
