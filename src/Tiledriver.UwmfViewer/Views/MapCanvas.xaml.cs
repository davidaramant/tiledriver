using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;
using Tiledriver.UwmfViewer.ViewModels;

namespace Tiledriver.UwmfViewer.Views
{
    public partial class MapCanvas
    {
        private int squareSize = 48;
        private int viewSquareSize = 24;
        private ObservableCollection<MapItemVm> mapItems = new ObservableCollection<MapItemVm>();
        private Path selectionMarker;
        private Point? selectionCoordinates;
        private WriteableBitmap wb;
        private Map map;

        public event EventHandler<MapItemEventArgs> NotifyNewMapItems;

        public MapCanvas()
        {
            InitializeComponent();

            Canvas.MouseUp += (sender, args) =>
            {
                var pos = args.GetPosition(MapImage);
                selectionCoordinates = new Point((int)pos.X / viewSquareSize, (int)pos.Y / viewSquareSize);
                HandleEventAt(selectionCoordinates.Value);
            };
        }

        public void Resize(int size)
        {
            viewSquareSize = size;

            Canvas.Height = map.Height * size;
            Canvas.Width = map.Width * size;
            MapImage.Height = map.Height * size;
            MapImage.Width = map.Width * size;

            Canvas.Children.Remove(selectionMarker);
            selectionMarker = CreateSelectionMarker(size);
            if (selectionCoordinates.HasValue)
            {
                HandleEventAt(selectionCoordinates.Value);
            }
        }

        public void Update(Map map, int size)
        {
            this.map = map;
            selectionCoordinates = null;
            Resize(size);

            ClearDetailsPane();
            MapImage.Source = null;
            mapItems = new ObservableCollection<MapItemVm>();

            MapImage.StretchDirection = StretchDirection.Both;
            MapImage.Source = null;
            wb = BitmapFactory.New(map.Height * squareSize, map.Width * squareSize);
            MapImage.Source = wb;

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

            using (wb.GetBitmapContext())
            {
                wb.Clear(Colors.Black);

                foreach (var mapItem in mapItems)
                {
                    wb.FillPolygon(mapItem.Points(squareSize), mapItem.Fill);
                }
            }
        }

        private void AddMapItem(MapItemVm mapItem)
        {
            mapItems.Add(mapItem);
        }

        private void HandleEventAt(Point coordinate)
        {
            Canvas.Children.Remove(selectionMarker);
            Canvas.SetLeft(selectionMarker, coordinate.X * viewSquareSize + 32);
            Canvas.SetTop(selectionMarker, coordinate.Y * viewSquareSize + 32);
            Canvas.Children.Add(selectionMarker);

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
                Data = MapItemVm.SQUARE,
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
