using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using LateNightStupidities.IIImaBackupReader;
using LateNightStupidities.IIImaBackupReader.Messages;
using LateNightStupidities.IIImaBackupViewer.ViewModel.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel
{
    public class ConversationViewModel : INotifyPropertyChanged
    {
        private string filterText;
        private bool filterActive;
        private DateTime? filterFromDate;
        private DateTime? filterToDate;
        private TypeFilter filterType;

        private Conversation Conversation { get; }

        public string DisplayName => this.Conversation.ConversationPartner.DisplayName;

        public string MessageCount => this.Conversation.Count.ToString();

        public string ToolTip =>
            $"ID: {this.Conversation.ConversationPartner}\r\n" +
            $"Conversation started: {this.Conversation.FirstOrDefault()?.CreatedAt.ToLocalTime().ToString() ?? "unknown"}";

        private List<MessageListViewModel> MessageViewModels { get; }

        private Dictionary<DateTime, List<MessageViewModel>> MessageViewModelsByDate { get; }

        public ICollectionView Messages { get; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public TypeFilter[] AvailableFilterTypes { get; } =
        {
            new TypeFilter(null, "All"),
            new TypeFilter("TEXT", "Text"),
            new TypeFilter("IMAGE", "Image"),
            new TypeFilter("AUDIO", "Audio"),
            new TypeFilter("VIDEO", "Video"),
            new TypeFilter("LOCATION", "Location"),
            new TypeFilter("BALLOT", "Ballot"),
        };

        public string FilterText
        {
            get => this.filterText;
            set
            {
                this.filterText = value;
                this.OnPropertyChanged();
                this.Messages.Refresh();
            }
        }

        public bool FilterActive
        {
            get => this.filterActive;
            set
            {
                this.filterActive = value;
                this.OnPropertyChanged();
                this.Messages.Refresh();

                if (!value)
                {
                    // Hack: Keep selected item in view when the filter is deactivated.
                    object currentItem = this.Messages.CurrentItem;
                    this.Messages.MoveCurrentToFirst();
                    this.Messages.MoveCurrentTo(currentItem);
                }
            }
        }

        public DateTime? FilterFromDate
        {
            get => this.filterFromDate;
            set
            {
                this.filterFromDate = value;
                this.OnPropertyChanged();
                this.Messages.Refresh();
            }
        }

        public DateTime? FilterToDate
        {
            get => this.filterToDate;
            set
            {
                this.filterToDate = value;
                this.OnPropertyChanged();
                this.Messages.Refresh();
            }
        }

        public TypeFilter FilterType
        {
            get => this.filterType;
            set
            {
                this.filterType = value;
                this.OnPropertyChanged();
                this.Messages.Refresh();
            }
        }

        public ICommand FindNextCommand { get; }

        public ICommand FindPreviousCommand { get; }

        public ConversationViewModel(Conversation conversation)
        {
            DateTime lastDate = DateTime.MinValue.Date;
            this.Conversation = conversation;

            this.MessageViewModels = new List<MessageListViewModel>();
            this.MessageViewModelsByDate = new Dictionary<DateTime, List<MessageViewModel>>();
            foreach (Message message in conversation)
            {
                DateTime messageDate = message.CreatedAt.ToLocalTime().Date;
                if (messageDate != lastDate)
                {
                    this.MessageViewModels.Add(new DateMarkerViewModel(messageDate));
                    lastDate = messageDate;
                }

                MessageViewModel mvm = new MessageViewModel(message);
                this.MessageViewModels.Add(mvm);

                if (!this.MessageViewModelsByDate.TryGetValue(messageDate, out List<MessageViewModel> messagesOfThisDate))
                {
                    messagesOfThisDate = new List<MessageViewModel>();
                    this.MessageViewModelsByDate.Add(messageDate, messagesOfThisDate);
                }
                messagesOfThisDate.Add(mvm);
            }

            this.Messages = CollectionViewSource.GetDefaultView(this.MessageViewModels);
            this.Messages.Filter = this.MatchesFilter;

            this.StartDate = this.MessageViewModels.OfType<MessageViewModel>().FirstOrDefault()?.Timestamp;
            this.EndDate = this.MessageViewModels.OfType<MessageViewModel>().LastOrDefault()?.Timestamp;
            this.FilterFromDate = this.StartDate;
            this.FilterToDate = this.EndDate;
            this.FilterType = this.AvailableFilterTypes[0];

            this.FindNextCommand = new RelayCommand(this.FindNext, o => !this.FilterActive);
            this.FindPreviousCommand = new RelayCommand(this.FindPrevious, o => !this.FilterActive);
        }

        private void FindNext(object obj)
        {
            int index = this.GetSelectedIndex();
            MessageViewModel nextMatch = this.MessageViewModels
                .Skip(index + 1)
                .OfType<MessageViewModel>()
                .FirstOrDefault(this.MatchesFilterTyped);
            if (nextMatch != null)
            {
                this.Messages.MoveCurrentTo(nextMatch);
            }
        }

        private void FindPrevious(object obj)
        {
            int index = this.GetSelectedIndex();
            MessageViewModel previousMatch = ((IEnumerable<MessageListViewModel>) this.MessageViewModels)
                .Reverse()
                .Skip(this.MessageViewModels.Count - index)
                .OfType<MessageViewModel>()
                .FirstOrDefault(this.MatchesFilterTyped);
            if (previousMatch != null)
            {
                this.Messages.MoveCurrentTo(previousMatch);
            }
        }

        private int GetSelectedIndex()
        {
            int index;
            if (this.Messages.CurrentItem is MessageViewModel currentItem)
            {
                index = this.MessageViewModels.IndexOf(currentItem);
            }
            else
            {
                index = 0;
            }
            return index;
        }

        private bool MatchesFilter(object o)
        {
            if (!this.FilterActive)
            {
                return true;
            }

            if (o is DateMarkerViewModel dmvm)
            {
                return this.DateMarkerMatchesFilter(dmvm);
            }

            if (o is MessageViewModel mvm)
            {
                return this.MatchesFilterTyped(mvm);
            }

            return false;
        }

        private bool DateMarkerMatchesFilter(DateMarkerViewModel dmvm)
        {
            return this.MessageViewModelsByDate[dmvm.Timestamp].Any(this.MatchesFilterTyped);
        }

        private bool MatchesFilterTyped(MessageViewModel mvm)
        {
            Message message = mvm.Content.Message;

            if (!string.IsNullOrEmpty(this.FilterText))
            {
                CultureInfo culture = CultureInfo.CurrentCulture;
                CompareInfo compareInfo = culture.CompareInfo;
                if (compareInfo.IndexOf(message.Body, this.FilterText, CompareOptions.IgnoreCase) < 0 &&
                    compareInfo.IndexOf(message.Caption, this.FilterText, CompareOptions.IgnoreCase) < 0)
                {
                    return false;
                }
            }

            if (this.FilterType.Type != null)
            {
                if (message.Type != this.FilterType.Type)
                {
                    return false;
                }
            }

            if (this.FilterFromDate != null)
            {
                if (mvm.Timestamp < this.FilterFromDate.Value)
                {
                    return false;
                }
            }

            if (this.FilterToDate != null)
            {
                if (mvm.Timestamp > this.FilterToDate.Value.AddDays(1))
                {
                    return false;
                }
            }

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}