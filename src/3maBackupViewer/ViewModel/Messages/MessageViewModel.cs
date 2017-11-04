using System;
using LateNightStupidities.IIImaBackupReader.Messages;
using LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages
{
    public class MessageViewModel : MessageListViewModel
    {
        private Message Message { get; }

        public MessageContentViewModel Content { get; protected set; }

        public string Sender => this.Message.Creator.DisplayName;

        public bool Outgoing => this.Message.Outgoing;

        public DateTime Timestamp => this.Message.CreatedAt.ToLocalTime();

        public string Time => this.Timestamp.ToShortTimeString();

        public string LongTime => $"{this.Timestamp.ToLongDateString()}, {this.Timestamp.ToLongTimeString()}";

        public MessageViewModel(Message message)
        {
            this.Message = message;
            this.CreateContent();
        }

        private void CreateContent()
        {
            switch (this.Message)
            {
                case TextMessage textMessage:
                    this.Content = new TextMessageContentViewModel(textMessage, this);
                    break;
                case ImageMessage imageMessage:
                    this.Content = new ImageMessageContentViewModel(imageMessage, this);
                    break;
                case LocationMessage locationMessage:
                    this.Content = new LocationMessageContentViewModel(locationMessage, this);
                    break;
                case AudioMessage audioMessage:
                    this.Content = new AudioMessageContentViewModel(audioMessage, this);
                    break;
                case VideoMessage videoMessage:
                    this.Content = new VideoMessageContentViewModel(videoMessage, this);
                    break;
                case BallotMessage ballotMessage:
                    switch (ballotMessage.Action)
                    {
                        case BallotAction.Create:
                            this.Content = new CreateBallotMessageContentViewModel(ballotMessage, this);
                            break;
                        case BallotAction.Close:
                            this.Content = new CloseBallotMessageContentViewModel(ballotMessage, this);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                case FileMessage fileMessage:
                    this.Content = new FileMessageContentViewModel(fileMessage, this);
                    break;
            }
        }
    }
}