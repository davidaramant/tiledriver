using System.Windows.Media;
using Tiledriver.UwmfViewer.Utilities;

namespace Tiledriver.UwmfViewer.Views
{
    public static class ViewExtensions
    {
        public static SolidColorBrush ToBrush(this Color color) =>
            new SolidColorBrush(color).Tee(x => x.Freeze());

        public static SolidColorBrush ToUnfrozenBrush(this Color color) =>
            new SolidColorBrush(color);
    }
}
