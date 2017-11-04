using System;

namespace LateNightStupidities.IIImaBackupReader.Messages
{
    /// <summary>
    /// A message.
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// Gets the conversation this message belongs to.
        /// </summary>
        public Conversation Conversation { get; internal set; }

        /// <summary>
        /// Gets the API ID.
        /// </summary>
        public string ApiId { get; private set; }

        /// <summary>
        /// Gets the UID of the message.
        /// </summary>
        public string Uid { get; private set; }

        /// <summary>
        /// Gets whether this message was sent from the identity 
        /// for which the backup was created (<see cref="IIImaBackup.Me"/>).
        /// </summary>
        public bool Outgoing { get; private set; }

        /// <summary>
        /// Gets the "posted at" timestamp. (Local creation date?)
        /// </summary>
        public DateTime PostedAt { get; private set; }

        /// <summary>
        /// Gets the "created at" timestamp. (Server creation date?)
        /// </summary>
        public DateTime CreatedAt { get; private set; }

        /// <summary>
        /// Gets the optional "modified at" date. (Read date?)
        /// </summary>
        public DateTime? ModifiedAt { get; private set; }

        /// <summary>
        /// Gets the data type of the message, e. g. TEXT or IMAGE.
        /// </summary>
        public string Type { get; private set; }

        private string body;

        /// <summary>
        /// Gets the body of the message.
        /// This contains the text or more meta data.
        /// </summary>
        public string Body
        {
            get => this.body;
            set
            {
                this.body = value;
                this.ReadPropertiesFromBody();
            }
        }

        /// <summary>
        /// Gets the caption of the message.
        /// </summary>
        public string Caption { get; private set; }

        /// <summary>
        /// Gets the ID that sent this message.
        /// </summary>
        public Identity Creator { get; internal set; }

        /// <summary>
        /// Override this method to read meta data from the body field.
        /// </summary>
        protected virtual void ReadPropertiesFromBody()
        {
        }

        /// <summary>
        /// Creates a message from a CSV row.
        /// </summary>
        /// <param name="fields">The fields of the CSV row.</param>
        public static Message FromFields(string[] fields)
        {
            string apiId = fields[0];
            string uid = fields[1];
            bool outgoing = fields[2] == "1";
            DateTime postedAt = Util.ReadDateValue(fields[6]);
            DateTime createdAt = Util.ReadDateValue(fields[7]);
            DateTime? modifiedAt = Util.ReadOptionalDateValue(fields[8]);
            string type = fields[9];
            string body = fields[10];
            string caption = fields[11];

            Message msg;
            switch(type)
            {
                case "TEXT":
                    msg = new TextMessage();
                    break;
                case "LOCATION":
                    msg = new LocationMessage();
                    break;
                case "IMAGE":
                    msg = new ImageMessage();
                    break;
                case "AUDIO":
                    msg = new AudioMessage();
                    break;
                case "VIDEO":
                    msg = new VideoMessage();
                    break;
                case "BALLOT":
                    msg = new BallotMessage();
                    break;
                case "FILE":
                    msg = new FileMessage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            msg.ApiId = apiId;
            msg.Uid = uid;
            msg.Outgoing = outgoing;
            msg.PostedAt = postedAt;
            msg.CreatedAt = createdAt;
            msg.ModifiedAt = modifiedAt;
            msg.Body = body;
            msg.Caption = caption;
            msg.Type = type;

            return msg;
        }
    }
}
