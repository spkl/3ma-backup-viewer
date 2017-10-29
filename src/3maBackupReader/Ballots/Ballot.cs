using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LateNightStupidities.IIImaBackupReader.Ballots
{
    /// <summary>
    /// A ballot.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Creator) + "}: {" + nameof(Name) + "}")]
    public class Ballot
    {
        /// <summary>
        /// Gets the associated IIImaBackup.
        /// </summary>
        public IIImaBackup Backup { get; private set; }

        /// <summary>
        /// Gets the ID of this ballot.
        /// </summary>
        public ulong Id { get; private set; }

        /// <summary>
        /// Gets the ID of the ballot, as referenced in a message.
        /// </summary>
        public ulong BallotId => this.Id;

        /// <summary>
        /// Gets the AID of the ballot, as referenced in the choices or votes.
        /// </summary>
        public string Aid { get; private set; }

        /// <summary>
        /// Gets the creator of the ballot.
        /// </summary>
        public Identity Creator { get; private set; }

        /// <summary>
        /// Gets the name of the ballot.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the state of the ballot, e. g. CLOSED.
        /// </summary>
        public string State { get; private set; }

        /// <summary>
        ///  Gets the assessment type of the ballot, e. g. SINGLE_CHOICE.
        /// </summary>
        public string Assessment { get; private set; }

        /// <summary>
        /// Gets the result type of the ballot, e. g. RESULT_ON_CLOSE.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Gets the type of choice of the ballot, e. g. TEXT.
        /// </summary>
        public string ChoiceType { get; private set; }

        /// <summary>
        /// Gets the "last viewed at" timestamp.
        /// </summary>
        public DateTime LastViewedAt { get; private set; }

        /// <summary>
        /// Gets the "created at" timestamp.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Gets the "modified at" timestamp.
        /// </summary>
        public DateTime ModifiedAt { get; private set; }

        /// <summary>
        /// Gets the available choices of this ballot.
        /// </summary>
        public List<BallotChoice> Choices = new List<BallotChoice>();

        /// <summary>
        /// Gets all cast votes of this ballot.
        /// </summary>
        public List<BallotVote> Votes = new List<BallotVote>();

        /// <summary>
        /// Adds a choice to the ballot from a CSV row.
        /// </summary>
        /// <param name="fields">The fields of the CSV row.</param>
        public void AddChoice(string[] fields)
        {
            BallotChoice choice = BallotChoice.FromFields(fields);
            this.Choices.Add(choice);
            choice.Ballot = this;
        }

        /// <summary>
        /// Adds a vote to the ballot from a CSV row.
        /// </summary>
        /// <param name="choice">The choice that was chosen in the vote.</param>
        /// <param name="fields">The fields of the CSV row.</param>
        public void AddVote(BallotChoice choice, string[] fields)
        {
            BallotVote vote = BallotVote.FromFields(fields, this.Backup);
            this.Votes.Add(vote);
            choice.Votes.Add(vote);
            vote.Choice = choice;
            vote.Ballot = this;
        }

        /// <summary>
        /// Creates a ballot from a CSV row.
        /// </summary>
        /// <param name="fields">The fields of the CSV row.</param>
        /// <param name="backup">The associated IIImaBackup.</param>
        public static Ballot FromFields(string[] fields, IIImaBackup backup)
        {
            return new Ballot
            {
                Backup = backup,
                Id = ulong.Parse(fields[0]),
                Aid = fields[1],
                Creator = new Identity(fields[2], backup),
                Name = fields[5],
                State = fields[6],
                Assessment = fields[7],
                Type = fields[8],
                ChoiceType = fields[9],
                LastViewedAt = Util.ReadDateValue(fields[10]),
                CreatedAt = Util.ReadDateValue(fields[11]),
                ModifiedAt = Util.ReadDateValue(fields[12])
            };
        }
    }
}