using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages
{
    public class TextMessageViewModel : MessageViewModel<TextMessage>
    {
        public string Text => this.Message.Text;

        public TextMessageViewModel(TextMessage message) : base(message)
        {
        }
    }
}