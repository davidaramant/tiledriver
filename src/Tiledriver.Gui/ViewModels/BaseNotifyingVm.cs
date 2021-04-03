// Copyright (c) 2016, Ryan Clarke and Jason Giles
// Copyright (c) 2017, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tiledriver.Gui.ViewModels
{
    public class BaseNotifyingVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifySet<T>(ref T existing, T value, [CallerMemberName] string propertyName = "")
        {
            if (!existing?.Equals(value) ?? value != null)
            {
                existing = value;
                NotifyPropertyChanged(propertyName);
            }
        }
    }
}
