// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Collections.ObjectModel;

namespace Tiledriver.Gui.ViewModels
{
    public class MapCanvasVm : BaseNotifyingVm
    {
        public ObservableCollection<MapItemVm> MapItems { get; set; }
    }
}
