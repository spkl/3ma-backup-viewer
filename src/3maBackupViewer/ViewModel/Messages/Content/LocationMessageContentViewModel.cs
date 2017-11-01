using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content
{
    public class LocationMessageContentViewModel : MessageContentViewModel<LocationMessage>
    {
        public string LatLong => $"{this.Message.Location.Latitude}, {this.Message.Location.Longitude}";

        public string Address => this.Message.Location.Address;

        public string Name => this.Message.Location.Name;

        public LocationMessageContentViewModel(LocationMessage message, MessageViewModel parent) 
            : base(message, parent)
        {
        }
    }
}