using System.Windows.Media;

namespace Tiledriver.Gui.Views
{
    static class ViewExtensions
    {
        public static SolidColorBrush ToBrush(this Color color) => new SolidColorBrush(color);
    }
}
