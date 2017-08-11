// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant and Luke Gilbert
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Tiledriver.Gui.ViewModels
{
    public abstract class MapItemVm
    {
        public Point Coordinates { get; internal set; }
        public LayerType LayerType { get; internal set; }
        public virtual bool ShouldAddToCanvas => true;
        public Path Thumbnail => CreatePath(24);

        public abstract string DetailType { get; }
        public abstract IEnumerable<DetailProperties> GetDetails();
        public Dictionary<string, List<DetailProperties>> GroupedDetails => GetDetails().GroupBy(d => d.Category).ToDictionary(g => g.Key ?? "", g => g.ToList());

        protected Path Element;
        public abstract Path CreatePath(int size);
        public abstract double Left(double size);
        public abstract double Top(double size);
        public abstract double Height(double size);
        public abstract double Width(double size);

        public void UpdatePathSize(double size)
        {
            if (Element != null)
            {
                Element.Height = size;
                Element.Width = size;
                Canvas.SetLeft(Element, Left(size));
                Canvas.SetTop(Element, Top(size));
            }
        }
    }
}
