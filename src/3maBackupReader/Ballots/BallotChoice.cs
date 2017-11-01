using System;
using System.Collections.Generic;

namespace LateNightStupidities.IIImaBackupReader.Ballots
{
    /// <summary>
    /// An available choice in a ballot.
    /// </summary>
    public class BallotChoice
    {
        /// <summary>
        /// Gets the ID of the choice.
        /// </summary>
        public ulong Id { get; private set; }

        /// <summary>
        /// Gets the ballot in which this choice could be made.
        /// </summary>
        public Ballot Ballot { get; internal set; }

        /// <summary>
        /// Gets the AID of the choice.
        /// </summary>
        public long Aid { get; private set; }

        /// <summary>
        /// Gets the AID of the choice, as referenced in a vote.
        /// </summary>
        public long ChoiceId => this.Aid;

        /// <summary>
        /// Gets the data type of the choice, e. g. Text.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// Gets the display name of the choice.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the order number of this choice in its ballot.
        /// </summary>
        public long Order { get; private set; }

        /// <summary>
        /// Gets the "created at" timestamp.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Gets the optional "modified at" timestamp.
        /// </summary>
        public DateTime? ModifiedAt { get; private set; }

        /// <summary>
        /// Gets all the votes cast for this choice.
        /// </summary>
        public List<BallotVote> Votes { get; } = new List<BallotVote>();

        /// <summary>
        /// Creates a choice from a CSV row.
        /// </summary>
        /// <param name="fields">The fields of the CSV row.</param>
        public static BallotChoice FromFields(string[] fields)
        {
            return new BallotChoice
            {
                Id = ulong.Parse(fields[0]),
                Aid = long.Parse(fields[2]),
                Type = fields[3],
                Name = fields[4],
                Order = long.Parse(fields[6]),
                CreatedAt = Util.ReadDateValue(fields[7]),
                ModifiedAt = Util.ReadOptionalDateValue(fields[8])
            };
        }
    }
}