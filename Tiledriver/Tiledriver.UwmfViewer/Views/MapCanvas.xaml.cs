using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tiledriver.Core.Uwmf;

namespace Tiledriver.UwmfViewer.Views
{
    public partial class MapCanvas
    {
        private readonly int squareSize = 32;
        private Point lastTileCoordinate = new Point(0, 0);
        private List<MapItem> MapItems = new List<MapItem>();

        public MapCanvas()
        {
            InitializeComponent();

            FullArea.MouseMove += (sender, args) =>
            {
                var pos = args.GetPosition(FullArea);
                var currentTileCoordinate = new Point((int)pos.X / squareSize, (int)pos.Y / squareSize);

                if (!currentTileCoordinate.Equals(lastTileCoordinate))
                {
                    lastTileCoordinate = currentTileCoordinate;
                    var details = MapItems.Where(i => i.LayerType == LayerType.Thing)
                        .Where(i => i.Coordinates.Equals(currentTileCoordinate))
                        .ToList();

                    details.ForEach(i => WriteDetails(i.Details));
                }
            };
        }

        public void Update(Map map)
        {
            FullArea.Height = map.Height * squareSize;
            FullArea.Width = map.Width * squareSize;
            MapItems = new List<MapItem>();

            var factory = new MapItemFactory(map);

            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    MapItems.Add(factory.VmForCoordinates(x, y));
                }
            }

            foreach (var thing in map.Things)
            {
                MapItems.Add(factory.VmForThing(thing));
            }
            
            DrawMapItems();
        }

        private void WriteDetails(IEnumerable<DetailProperties> details)
        {
            Console.WriteLine("\n========================================");
            foreach (var fact in details)
            {
                Console.WriteLine($"{fact.Category} :: {fact.Title} :: {fact.Value}");
            }
            Console.WriteLine("========================================\n");

        }

        private void DrawMapItems()
        {
            foreach (var mapItem in MapItems)
            {
                FullArea.Children.Add(mapItem.ToUIElement(squareSize));
            }
        }
    }

    public enum LayerType
    {
        Tile,
        Thing
    }
}
