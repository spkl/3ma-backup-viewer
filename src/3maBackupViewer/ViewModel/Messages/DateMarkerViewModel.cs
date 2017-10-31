using System;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages
{
    public class DateMarkerViewModel : MessageListViewModel
    {
        private DateTime date;

        public string Date => this.date.ToLongDateString();

        public DateMarkerViewModel(DateTime date)
        {
            this.date = date;
        }
    }
}