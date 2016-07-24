using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Tiledriver.UwmfViewer.ViewModels
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
