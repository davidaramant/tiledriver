using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Tiledriver.Core.Uwmf;

namespace Tiledriver.UwmfViewer.Views
{
    /// <summary>
    /// Interaction logic for MapCanvas.xaml
    /// </summary>
    public partial class MapCanvas : UserControl
    {
        private int squareSize = 32;

        public MapCanvas()
        {
            InitializeComponent();
        }

        public void Update(Map map)
        {
            FullArea.Height = map.Height * squareSize;
            FullArea.Width = map.Width * squareSize;
            var factory = new MapItemFactory(map);

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Add(factory.VmForCoordinates(x, y).ToUIElement(squareSize));
                }
            }
            
            map.Things.ForEach(t =>
            {
                Add(factory.VmForThing(t).ToUIElement(squareSize));
            });
        }

        public void Add(UIElement element)
        {
            FullArea.Children.Add(element);
        }
    }
}
