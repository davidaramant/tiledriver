using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tiledriver.UwmfViewer.ViewModels
{
    public class BaseNotifyingVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifySet<T>(ref T existing, T value, [CallerMemberName] string propertyName = "")
        {
            if (existing?.Equals(value) ?? value != null)
            {
                existing = value;
                NotifyPropertyChanged(propertyName);
            }
        }
    }
}
