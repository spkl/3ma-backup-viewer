using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content
{
    public class TextMessageContentViewModel : MessageContentViewModel<TextMessage>
    {
        public string Text => this.Message.Text;

        public TextMessageContentViewModel(TextMessage message, MessageViewModel parent)
            : base(message, parent)
        {
        }
    }
}