using System;
using System.Collections.ObjectModel;
using System.Linq;
using LateNightStupidities.IIImaBackupReader;
using LateNightStupidities.IIImaBackupReader.Messages;
using LateNightStupidities.IIImaBackupViewer.ViewModel.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel
{
    public class ConversationViewModel
    {
        private Conversation Conversation { get; }

        public string DisplayName => this.Conversation.ConversationPartner.DisplayName;

        public string MessageCount => this.Conversation.Count.ToString();

        public string ToolTip =>
            $"ID: {this.Conversation.ConversationPartner}\r\nConversation started: {this.Conversation.FirstOrDefault()?.CreatedAt.ToLocalTime().ToString() ?? "unknown"}";

        public ObservableCollection<MessageListViewModel> Messages { get; } = new ObservableCollection<MessageListViewModel>();

        public ConversationViewModel(Conversation conversation)
        {
            DateTime lastDate = DateTime.MinValue.Date;
            this.Conversation = conversation;
            foreach (Message message in conversation)
            {
                DateTime messageDate = message.CreatedAt.ToLocalTime().Date;
                if (messageDate != lastDate)
                {
                    this.Messages.Add(new DateMarkerViewModel(messageDate));
                    lastDate = messageDate;
                }

                if (message is TextMessage textMessage)
                {
                    this.Messages.Add(new TextMessageViewModel(textMessage));
                }

                if (message is ImageMessage imageMessage)
                {
                    this.Messages.Add(new ImageMessageViewModel(imageMessage));
                }
            }
        }
    }
}