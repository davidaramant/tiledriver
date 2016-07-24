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
        private int squareSize = 32;
        private Point lastTileCoordinate = new Point(0, 0);
        private List<TileCoordinateDetails> CoordinateDetails = new List<TileCoordinateDetails>();

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
                    var details = CoordinateDetails.Where(i => i.LayerType == LayerType.Thing)
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
            CoordinateDetails = new List<TileCoordinateDetails>();
            var factory = new MapItemFactory(map);

            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    var mapItem = factory.VmForCoordinates(x, y);
                    CoordinateDetails.Add(new TileCoordinateDetails
                    {
                        LayerType = LayerType.Tile,
                        Coordinates = new Point(x, y),
                        Details = mapItem.Details.ToList()
                    });
                    Add(mapItem.ToUIElement(squareSize));
                }
            }
            
            map.Things.ForEach(t =>
            {
                var mapItem = factory.VmForThing(t);
                CoordinateDetails.Add(new TileCoordinateDetails
                {
                    LayerType = LayerType.Thing,
                    Coordinates = new Point(Math.Floor(t.X), Math.Floor(t.Y)),
                    Details = mapItem.Details.ToList()
                });
                Add(mapItem.ToUIElement(squareSize));
            });
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

        private void Add(UIElement element)
        {
            FullArea.Children.Add(element);
        }
    }

    public enum LayerType
    {
        Tile,
        Thing
    }

    public class TileCoordinateDetails
    {
        public Point Coordinates { get; set; }

        public LayerType LayerType { get; set; }

        public IEnumerable<DetailProperties> Details { get; set; }
    }
}
