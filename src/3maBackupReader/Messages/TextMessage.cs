namespace LateNightStupidities.IIImaBackupReader.Messages
{
    /// <summary>
    /// A text message.
    /// </summary>
    public class TextMessage : Message
    {
        /// <summary>
        /// The text of the message.
        /// </summary>
        public string Text => this.Body;

        internal TextMessage()
        {
        }
    }
}
