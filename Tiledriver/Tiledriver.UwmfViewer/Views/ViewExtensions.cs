using System.Windows.Media;

namespace Tiledriver.UwmfViewer.Views
{
    static class ViewExtensions
    {
        public static SolidColorBrush ToBrush(this Color color) => new SolidColorBrush(color);
    }
}
