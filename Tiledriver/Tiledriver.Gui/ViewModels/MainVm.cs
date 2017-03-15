using Tiledriver.Core.Uwmf;

namespace Tiledriver.Gui.ViewModels
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
