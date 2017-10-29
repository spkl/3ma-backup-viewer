using System;
using System.Globalization;
using System.Text.RegularExpressions;
using LateNightStupidities.IIImaBackupReader.Ballots;

namespace LateNightStupidities.IIImaBackupReader.Messages
{
    /// <summary>
    /// A ballot message.
    /// </summary>
    public class BallotMessage : Message
    {
        /// <summary>
        /// Gets the ID of the ballot.
        /// </summary>
        public ulong BallotId { get; private set; }

        /// <summary>
        /// Gets the action that this messages does regarding the ballot.
        /// </summary>
        public BallotAction Action { get; private set; }

        /// <summary>
        /// Gets the associated ballot.
        /// </summary>
        public Ballot Ballot { get; internal set; }

        internal BallotMessage()
        {
        }

        /// <inheritdoc />
        protected override void ReadPropertiesFromBody()
        {
            Regex rx = new Regex(@"^\[(\d+),(\d+)\]$");
            Match match = rx.Match(this.Body);
            GroupCollection groups = match.Groups;

            BallotAction action;
            switch (groups[1].Value)
            {
                case "1":
                    action = BallotAction.Create;
                    break;
                case "3":
                    action = BallotAction.Close;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.Action = action;
            this.BallotId = ulong.Parse(groups[2].Value, NumberFormatInfo.InvariantInfo);
        }
    }
}
