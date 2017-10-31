using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages
{
    public class ImageMessageViewModel : MessageViewModel<ImageMessage>
    {
        public ImageSource Source => new BitmapImage(new Uri(this.Message.FilePath));

        public ImageMessageViewModel(ImageMessage message) : base(message)
        {
        }
    }
}