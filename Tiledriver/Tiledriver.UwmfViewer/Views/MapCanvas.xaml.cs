using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;
using Tiledriver.UwmfViewer.Utilities;

namespace Tiledriver.UwmfViewer.Views
{
    /// <summary>
    /// Interaction logic for MapCanvas.xaml
    /// </summary>
    public partial class MapCanvas : UserControl
    {
        private int squareSize = 32;
        private SquareFactory squareFactory;

        public MapCanvas()
        {
            InitializeComponent();
        }

        public void Update(Map map)
        {
            FullArea.Height = map.Height * squareSize;
            FullArea.Width = map.Width * squareSize;
            squareFactory = new SquareFactory(map, squareSize);

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Add(squareFactory.ForCoordinates(x, y).ToUIElements());
                }
            }

            map.Things.ForEach(t =>
            {
                Polygon element = Thing("8,0 16,16 0,16", Colors.Yellow);
                Canvas.SetLeft(element, t.X * squareSize - element.Width / 2);
                Canvas.SetTop(element, t.Y * squareSize - element.Height / 2);
                Add(element);
            });
        }

        private Polygon Thing(string shape, Color color)
        {
            return new Polygon()
            {
                Height = squareSize / 2,
                Width = squareSize / 2,
                Points = shape.ToPointsCollection(),
                Fill = color.ToBrush()
            };
        }

        public void Add(UIElement element)
        {
            FullArea.Children.Add(element);
        }

        public void Add(List<UIElement> elements)
        {
            elements.ForEach(Add);
        }
    }
}
