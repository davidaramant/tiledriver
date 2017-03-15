using System.Collections.ObjectModel;

namespace Tiledriver.Gui.ViewModels
{
    public class DetailViewVm : BaseNotifyingVm
    {
        private ObservableCollection<MapItemVm> mapItems;
        public ObservableCollection<MapItemVm> MapItems
        {
            get { return mapItems; }
            set { NotifySet(ref mapItems, value); }
        }

        public void Update(ObservableCollection<MapItemVm> mapItems)
        {
            MapItems = mapItems;
        }
    }
}
