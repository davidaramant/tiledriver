// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Windows.Media;

namespace Tiledriver.Gui.Views
{
    static class ViewExtensions
    {
        public static SolidColorBrush ToBrush(this Color color) => new SolidColorBrush(color);
    }
}
