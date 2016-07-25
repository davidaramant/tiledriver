﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Tiledriver.Core.Uwmf;
using static System.Diagnostics.Debug;

namespace Tiledriver.UwmfViewer.Views
{
    public partial class MapCanvas
    {
        private int squareSize = 24;
        private List<MapItem> MapItems = new List<MapItem>();

        public event EventHandler<MapItemEventArgs> NotifyNewMapItems;

        public MapCanvas()
        {
            InitializeComponent();

            FullArea.MouseUp += (sender, args) =>
            {
                var pos = args.GetPosition(FullArea);
                var currentTileCoordinate = new Point((int)pos.X / squareSize, (int)pos.Y / squareSize);
                HandleEventAt(currentTileCoordinate);
            };
        }

        public void Update(Map map, int size)
        {
            squareSize = size;

            ClearDetailsPane();
            FullArea.Children.Clear();

            FullArea.Height = map.Height * squareSize;
            FullArea.Width = map.Width * squareSize;
            MapItems = new List<MapItem>();

            var factory = new MapItemFactory(map);

            // Squares
            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    AddMapItem(factory.VmForCoordinates(x, y));
                }
            }

            // Things
            foreach (var thing in map.Things)
            {
                AddMapItem(factory.VmForThing(thing));
            }
        }

        private void AddMapItem(MapItem mapItem)
        {
            MapItems.Add(mapItem);
            if (mapItem.ShouldAddToCanvas)
            {
                FullArea.Children.Add(mapItem.ToUIElement(squareSize));
            }
        }

        private void ClearDetailsPane()
        {
            NotifyNewMapItems?.Invoke(this, new MapItemEventArgs(new List<MapItem>()));
        }

        private void HandleEventAt(Point coordinate)
        {
            var filteredMapItems = MapItems
                .Where(i => i.Coordinates.Equals(coordinate))
                .ToList();

            filteredMapItems.ForEach(i => DebugDetails(i.Details));
            ShowDetails(filteredMapItems);
        }

        private void DebugDetails(IEnumerable<DetailProperties> mapItems)
        {
            WriteLine("\n========================================");
            foreach (var item in mapItems)
            {
                WriteLine($"{item.Category} :: {item.Title} :: {item.Value}");
            }
            WriteLine("========================================\n");
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
