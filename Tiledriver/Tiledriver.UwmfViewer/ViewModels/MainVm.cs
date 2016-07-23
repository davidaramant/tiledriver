using System;
using Tiledriver.Core.Uwmf;

namespace Tiledriver.UwmfViewer.ViewModels
{
    public class MainVm : BaseNotifyingVm
    {
        private Map _map;
        public Map Map
        {
            get { return _map; }
            set { NotifySet(ref _map, value); }
        }
    }
}
