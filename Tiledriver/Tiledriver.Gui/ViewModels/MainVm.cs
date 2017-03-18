// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Core.FormatModels.Uwmf;

namespace Tiledriver.Gui.ViewModels
{
    public class MainVm : BaseNotifyingVm
    {
        private MapData _mapData;
        public MapData MapData
        {
            get { return _mapData; }
            set { NotifySet(ref _mapData, value); }
        }
    }
}
