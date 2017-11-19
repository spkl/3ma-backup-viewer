using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        /// <summary>
        /// Gets the file path of the conversation file.
        /// </summary>
        public string FilePath { get; private set; }

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
                Conversation conversation = FromTextReader(reader, backup.Me, new Identity(conversationPartner, backup));
                conversation.FilePath = path;
                return conversation;
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
            Conversation conversation = new Conversation(Enumerable.Empty<Message>(), conversationPartner);
            while (csv.Read())
            {
                Message message = Message.FromFields(csv.CurrentRecord);
                if (message == null)
                {
                    continue;
                }

                message.Creator = message.Outgoing ? me : conversationPartner;
                message.Conversation = conversation;
                messages.Add(message);
            }

            conversation.AddRange(messages);
            return conversation;
        }
    }
}