using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content
{
    public class ImageMessageContentViewModel : MessageContentViewModel<ImageMessage>
    {
        public ImageSource Source => this.lazySource.Value;

        private readonly Lazy<ImageSource> lazySource;

        public ImageMessageContentViewModel(ImageMessage message, MessageViewModel parent) 
            : base(message, parent)
        {
            this.lazySource = new Lazy<ImageSource>(() =>
            {
                try
                {
                    return new BitmapImage(new Uri(this.Message.FilePath));
                }
                catch
                {
                    // TODO put in placeholder image
                    return null;
                }
            });
        }
    }
}