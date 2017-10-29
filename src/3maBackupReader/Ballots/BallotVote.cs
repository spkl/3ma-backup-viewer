using System;

namespace LateNightStupidities.IIImaBackupReader.Ballots
{
    /// <summary>
    /// A vote cast in a ballot.
    /// </summary>
    public class BallotVote
    {
        /// <summary>
        /// Gets the ID of the vote.
        /// </summary>
        public ulong Id { get; private set; }

        /// <summary>
        /// Gets the associated ballot.
        /// </summary>
        public Ballot Ballot { get; internal set; }

        /// <summary>
        /// Gets the choice for which this vote was cast.
        /// </summary>
        public BallotChoice Choice { get; internal set; }

        /// <summary>
        /// Gets the ID that cast this vote.
        /// </summary>
        public Identity Identity { get; private set; }

        /// <summary>
        /// Gets the "created at" timestamp.
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Gets the "modified at" timestamp.
        /// </summary>
        public DateTime ModifiedAt { get; private set; }

        /// <summary>
        /// Creates a vote from a CSV row.
        /// </summary>
        /// <param name="fields">The fields of the CSV row.</param>
        /// <param name="backup">The associated IIImaBackup.</param>
        public static BallotVote FromFields(string[] fields, IIImaBackup backup)
        {
            if (fields[4] == "0")
            {
                throw new InvalidOperationException();
            }

            return new BallotVote
            {
                Id = ulong.Parse(fields[0]),
                Identity = new Identity(fields[3], backup),
                CreatedAt = Util.ReadDateValue(fields[5]),
                ModifiedAt = Util.ReadDateValue(fields[6])
            };
        }
    }
}