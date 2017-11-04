using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content
{
    public abstract class MessageContentViewModel : INotifyPropertyChanged, IDisposable
    {
        public Message Message { get; }

        public MessageViewModel Parent { get; }

        protected MessageContentViewModel(Message message, MessageViewModel parent)
        {
            this.Message = message;
            this.Parent = parent;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
        }
    }
}