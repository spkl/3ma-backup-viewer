using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content
{
    public class FileMessageContentViewModel : MessageContentViewModel<FileMessage>
    {
        public string FileName => this.Message.OriginalFileName;

        public FileMessageContentViewModel(FileMessage message, MessageViewModel parent)
            : base(message, parent)
        {
        }
    }
}