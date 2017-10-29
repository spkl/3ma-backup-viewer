using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CsvHelper;
using LateNightStupidities.IIImaBackupReader.Messages;

namespace LateNightStupidities.IIImaBackupReader
{
    /// <summary>
    /// A two-person conversation.
    /// </summary>
    [DebuggerDisplay("{" + nameof(ConversationPartner) + "}, {" + nameof(Count) + "} messages")]
    public class Conversation : List<Message>
    {
        /// <summary>
        /// The conversation partner.
        /// </summary>
        public Identity ConversationPartner { get; }

        protected Conversation(IEnumerable<Message> messages, Identity conversationPartner) : base(messages)
        {
            this.ConversationPartner = conversationPartner;
        }

        /// <summary>
        /// Reads a conversation from a message_*.csv file.
        /// </summary>
        /// <param name="path">The csv file path.</param>
        /// <param name="backup">The associated IIImaBackup.</param>
        public static Conversation FromFile(string path, IIImaBackup backup)
        {
            string fileName = Path.GetFileName(path);
            string conversationPartner = fileName.Substring(fileName.IndexOf('_') + 1, fileName.IndexOf('.') - fileName.IndexOf('_') - 1);

            FileInfo file = new FileInfo(path);
            using (TextReader reader = file.OpenText())
            {
                return FromTextReader(reader, backup.Me, new Identity(conversationPartner, backup));
            }
        }

        /// <summary>
        /// Reads a conversation file from a <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">The TextReader.</param>
        /// <param name="me">The ID for which the backup was created.</param>
        /// <param name="conversationPartner">The ID of the conversation partner.</param>
        public static Conversation FromTextReader(TextReader reader, Identity me, Identity conversationPartner)
        {
            List<Message> messages = new List<Message>();
            CsvReader csv = new CsvReader(reader);
            while (csv.Read())
            {
                Message message = Message.FromFields(csv.CurrentRecord);
                message.Creator = message.Outgoing ? me : conversationPartner;
                messages.Add(message);
            }

            return new Conversation(messages, conversationPartner);
        }
    }
}