// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.FormatModels.Uwmf;
using Tiledriver.Gui.ViewModels;

namespace Tiledriver.Gui.Views
{
    public partial class MapCanvas
    {
        private int _squareSize = 24;
        private ObservableCollection<MapItemVm> _mapItems = new ObservableCollection<MapItemVm>();
        private Path _selectionMarker;

        public event EventHandler<MapItemEventArgs> NotifyNewMapItems;

        public MapCanvas()
        {
            InitializeComponent();

            FullArea.MouseUp += (sender, args) =>
            {
                var pos = args.GetPosition(FullArea);
                HandleEventAt(new Point((int)pos.X / _squareSize, (int)pos.Y / _squareSize));
            };
        }

        public void Update(MapData mapData, int size)
        {
            _squareSize = size;

            ClearDetailsPane();
            FullArea.Children.Clear();
            _mapItems = new ObservableCollection<MapItemVm>();
            _selectionMarker = CreateSelectionMarker(_squareSize);

            FullArea.Height = mapData.Height * _squareSize;
            FullArea.Width = mapData.Width * _squareSize;
            FullArea.Background = Colors.Black.ToBrush();

            var factory = new MapItemVmFactory(mapData);

            // Squares
            for (var x = 0; x < mapData.Width; x++)
            {
                for (var y = 0; y < mapData.Height; y++)
                {
                    AddMapItem(factory.BuildSquare(x, y));
                }
            }

            // Things
            foreach (var thing in mapData.Things)
            {
                AddMapItem(factory.BuildThing(thing));
            }

            // Triggers
            foreach (var trigger in mapData.Triggers)
            {
                AddMapItem(factory.BuildTrigger(trigger));
            }
        }

        private void AddMapItem(MapItemVm mapItem)
        {
            _mapItems.Add(mapItem);
            if (mapItem.ShouldAddToCanvas)
            {
                FullArea.Children.Add(mapItem.CreatePath(_squareSize));
            }
        }

        private void HandleEventAt(Point coordinate)
        {
            FullArea.Children.Remove(_selectionMarker);
            Canvas.SetLeft(_selectionMarker, coordinate.X * _squareSize);
            Canvas.SetTop(_selectionMarker, coordinate.Y * _squareSize);
            FullArea.Children.Add(_selectionMarker);

            var filteredMapItems = new ObservableCollection<MapItemVm>(_mapItems
                .Where(i => i.Coordinates.Equals(coordinate)));

            ShowDetails(filteredMapItems);
        }

        private static Path CreateSelectionMarker(int size)
        {
            return new Path()
            {
                Height = size,
                Width = size,
                Data = GeometryCache.Square,
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
