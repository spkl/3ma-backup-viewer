using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using LateNightStupidities.IIImaBackupReader.Ballots;
using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupReader
{
    /// <summary>
    /// A full 3ma backup set.
    /// </summary>
    public class IIImaBackup
    {
        /// <summary>
        /// Gets the directory where the backup files are located.
        /// </summary>
        public string BackupDirectory { get; }

        /// <summary>
        /// The ID for which this backup was created.
        /// </summary>
        public Identity Me { get; }

        /// <summary>
        /// The conversations contained in the backup.
        /// These are two-person conversations, not group conversations.
        /// </summary>
        public Conversation[] Conversations { get; private set; }

        /// <summary>
        /// The ballots contained in the backup.
        /// Ballots can also be accessed by <see cref="BallotMessage.Ballot"/>.
        /// </summary>
        public Ballot[] Ballots { get; private set; }

        /// <summary>
        /// The contacts contained in the backup.
        /// </summary>
        public Dictionary<Identity, Contact> Contacts { get; private set; }

        /// <summary>
        /// Creates a new IIImaBackup instance.
        /// </summary>
        /// <param name="directory">The directory where the backup files are located.</param>
        /// <param name="meIdentity">The ID for which this backup was created.</param>
        public IIImaBackup(string directory, string meIdentity)
        {
            this.BackupDirectory = Path.GetFullPath(directory);
            this.Me = new Identity(meIdentity, this);
        }

        /// <summary>
        /// Reads all available backup data.
        /// </summary>
        public void Read()
        {
            string[] conversationFiles = Directory.GetFiles(this.BackupDirectory, "message_*.csv");
            this.ReadConversations(conversationFiles);

            string ballotFile = Path.Combine(this.BackupDirectory, "ballot.csv");
            this.Ballots = this.ReadBallots(ballotFile);

            string ballotChoiceFile = Path.Combine(this.BackupDirectory, "ballot_choice.csv");
            this.ReadBallotChoices(ballotChoiceFile);

            string ballotVoteFile = Path.Combine(this.BackupDirectory, "ballot_vote.csv");
            this.ReadBallotVotes(ballotVoteFile);

            this.ConnectBallotsToMessages();

            string contactsFile = Path.Combine(this.BackupDirectory, "contacts.csv");
            this.ReadContacts(contactsFile);
        }

        /// <summary>
        /// Reads all message_*.csv files.
        /// </summary>
        private void ReadConversations(string[] conversationFiles)
        {
            this.Conversations = conversationFiles.Select(f => Conversation.FromFile(f, this)).ToArray();
        }

        /// <summary>
        /// Read ballot.csv.
        /// </summary>
        private Ballot[] ReadBallots(string ballotFile)
        {
            if (!File.Exists(ballotFile))
            {
                return new Ballot[0];
            }

            List<Ballot> ballots = new List<Ballot>();
            FileInfo file = new FileInfo(ballotFile);
            using (TextReader reader = file.OpenText())
            {
                CsvReader csv = new CsvReader(reader);
                while (csv.Read())
                {
                    ballots.Add(Ballot.FromFields(csv.CurrentRecord, this));
                }
            }

            return ballots.ToArray();
        }

        /// <summary>
        /// Read ballot_choice.csv.
        /// </summary>
        private void ReadBallotChoices(string ballotChoiceFile)
        {
            if (!File.Exists(ballotChoiceFile))
            {
                return;
            }

            FileInfo file = new FileInfo(ballotChoiceFile);
            using (TextReader reader = file.OpenText())
            {
                CsvReader csv = new CsvReader(reader);
                while (csv.Read())
                {
                    string ballotKey = csv.CurrentRecord[1];
                    ballotKey = ballotKey.Split('-')[0];
                    Ballot ballot = this.Ballots.First(b => b.Aid == ballotKey);
                    ballot.AddChoice(csv.CurrentRecord);
                }
            }
        }

        /// <summary>
        /// Read ballot_vote.csv.
        /// </summary>
        private void ReadBallotVotes(string ballotVoteFile)
        {
            if (!File.Exists(ballotVoteFile))
            {
                return;
            }

            FileInfo file = new FileInfo(ballotVoteFile);
            using (TextReader reader = file.OpenText())
            {
                CsvReader csv = new CsvReader(reader);
                while (csv.Read())
                {
                    if (csv.CurrentRecord[4] == "0")
                    {
                        // "choice" column is 0, so this choice was not made.
                        continue;
                    }

                    string ballotKey = csv.CurrentRecord[1];
                    ballotKey = ballotKey.Split('-')[0];
                    Ballot ballot = this.Ballots.First(b => b.Aid == ballotKey);
                    BallotChoice choice = ballot.Choices.First(c => c.ChoiceId == long.Parse(csv.CurrentRecord[2]));
                    ballot.AddVote(choice, csv.CurrentRecord);
                }
            }
        }

        /// <summary>
        /// Connects the found ballots to the messages where they are referenced.
        /// </summary>
        private void ConnectBallotsToMessages()
        {
            var ballotDictionary = this.Ballots.ToDictionary(b => b.BallotId);
            foreach (BallotMessage ballotMessage in this.Conversations.SelectMany(c => c).OfType<BallotMessage>())
            {
                ballotMessage.Ballot = ballotDictionary[ballotMessage.BallotId];
            }
        }

        /// <summary>
        /// Reads contacts.csv.
        /// </summary>
        private void ReadContacts(string contactsFile)
        {
            List<Contact> contacts = new List<Contact>();
            FileInfo file = new FileInfo(contactsFile);
            using (TextReader reader = file.OpenText())
            {
                CsvReader csv = new CsvReader(reader);
                while (csv.Read())
                {
                    contacts.Add(Contact.FromFields(csv.CurrentRecord, this));
                }
            }

            this.Contacts = contacts.ToDictionary(c => c.Identity);
            this.Contacts.Add(this.Me, new Contact() {CustomDisplayName = "Me", Identity = this.Me});
        }
    }
}