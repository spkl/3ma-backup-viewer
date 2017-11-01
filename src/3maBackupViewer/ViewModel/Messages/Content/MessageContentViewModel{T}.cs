using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content
{
    public class MessageContentViewModel<T> : MessageContentViewModel where T : Message
    {
        protected T Message { get; }

        protected MessageContentViewModel(T message, MessageViewModel parent) : base(message, parent)
        {
            this.Message = message;
        }
    }
}