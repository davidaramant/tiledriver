using Tiledriver.UwmfViewer.ViewModels;

namespace Tiledriver.UwmfViewer.Views
{
    public partial class DetailView
    {
        private DetailViewVm vm;

        public DetailView()
        {
            InitializeComponent();
            DataContext = vm = new DetailViewVm();
        }

        internal void Update(object sender, MapItemEventArgs e)
        {
            vm.Update(e.MapItems);
        }
    }
}
