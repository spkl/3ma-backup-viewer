using System;
using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages
{
    public abstract class MessageViewModel : MessageListViewModel
    {
        private Message Message { get; }

        public string Sender => this.Message.Creator.DisplayName;

        public bool Outgoing => this.Message.Outgoing;

        public DateTime Timestamp => this.Message.CreatedAt.ToLocalTime();

        public string Time => this.Message.CreatedAt.ToLocalTime().ToShortTimeString();

        protected MessageViewModel(Message message)
        {
            this.Message = message;
        }
    }
}