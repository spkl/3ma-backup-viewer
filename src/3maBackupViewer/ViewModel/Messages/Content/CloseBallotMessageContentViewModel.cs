using System.Collections.Generic;
using LateNightStupidities.IIImaBackupReader.Ballots;
using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupViewer.ViewModel.Messages.Content
{
    public class CloseBallotMessageContentViewModel : MessageContentViewModel<BallotMessage>
    {
        public string Name => this.Message.Ballot.Name;

        public string[] Results
        {
            get
            {
                List<string> lines = new List<string>();
                foreach (BallotChoice ballotChoice in this.Message.Ballot.Choices)
                {
                    int votes = ballotChoice.Votes.Count;
                    lines.Add($"{ballotChoice.Name}: {votes} vote{(votes == 1 ? "" : "s")}");

                    foreach (BallotVote vote in ballotChoice.Votes)
                    {
                        lines.Add($"    {vote.Identity.DisplayName}");
                    }
                }

                return lines.ToArray();
            }
        }

        public CloseBallotMessageContentViewModel(BallotMessage message, MessageViewModel parent) 
            : base(message, parent)
        {
        }
    }
}