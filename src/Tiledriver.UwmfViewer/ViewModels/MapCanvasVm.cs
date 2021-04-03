using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiledriver.UwmfViewer.ViewModels
{
    public class MapCanvasVm : BaseNotifyingVm
    {
        private ObservableCollection<MapItemVm> mapItems;
        public ObservableCollection<MapItemVm> MapItems { get; set; }
    }
}
