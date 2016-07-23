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
        public MapCanvas()
        {
            InitializeComponent();
        }

        public void Update(Map map)
        {
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Add(new Square(map, x, y).ToUIElement());
                }
            }
        }

        public void Add(UIElement element)
        {
            FullArea.Children.Add(element);
        }
    }
}
