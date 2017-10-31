using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages
{
    public abstract class MessageListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}