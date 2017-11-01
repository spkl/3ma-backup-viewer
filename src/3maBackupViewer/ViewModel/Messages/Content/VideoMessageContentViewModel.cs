using System.Diagnostics;
using System.Windows.Input;
using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content
{
    public class VideoMessageContentViewModel : MessageContentViewModel<VideoMessage>
    {
        public ICommand OpenVideoCommand { get; }

        public VideoMessageContentViewModel(VideoMessage message, MessageViewModel parent) 
            : base(message, parent)
        {
            this.OpenVideoCommand = new RelayCommand(o => this.OpenVideo());
        }

        private void OpenVideo()
        {
            Process.Start(this.Message.FilePath);
        }
    }
}