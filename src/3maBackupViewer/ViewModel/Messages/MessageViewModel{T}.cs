using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages
{
    public abstract class MessageViewModel<T> : MessageViewModel where T : Message
    {
        protected T Message { get; }

        protected MessageViewModel(T message) : base(message)
        {
            this.Message = message;
        }
    }
}