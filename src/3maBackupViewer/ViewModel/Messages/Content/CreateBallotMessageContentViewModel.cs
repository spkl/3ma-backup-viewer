using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content
{
    public class CreateBallotMessageContentViewModel : MessageContentViewModel<BallotMessage>
    {
        public string Name => this.Message.Ballot.Name;

        public CreateBallotMessageContentViewModel(BallotMessage message, MessageViewModel parent)
            : base(message, parent)
        {
        }
    }
}