// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using Tiledriver.Gui.ViewModels;

namespace Tiledriver.Gui.Views
{
    public partial class DetailView
    {
        private DetailViewVm _vm;

        public DetailView()
        {
            InitializeComponent();
            DataContext = _vm = new DetailViewVm();
        }

        internal void Update(object sender, MapItemEventArgs e)
        {
            _vm.Update(e.MapItems);
        }
    }
}
