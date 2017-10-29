using System.Text.RegularExpressions;

namespace LateNightStupidities.IIImaBackupReader.Messages
{
    /// <summary>
    /// A message with an attached file.
    /// </summary>
    public class FileMessage : MediaMessage
    {
        /// <summary>
        /// Gets the mime type of the attached file.
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// Gets the original file name of the attached file.
        /// This is the name the file had when it was sent.
        /// </summary>
        public string OriginalFileName { get; private set; }

        internal FileMessage()
        {
        }

        /// <inheritdoc />
        protected override void ReadPropertiesFromBody()
        {
            Regex rx = new Regex(@"^\[("".*""|null),("".*""|null),("".*""|null),\d+,("".*""|null),.*\]$");
            Match match = rx.Match(this.Body);
            GroupCollection groups = match.Groups;

            string mimeTypeString = groups[3].Value;
            this.MimeType = mimeTypeString == "null" ? null : mimeTypeString.Trim('"');

            string originalFileNameString = groups[4].Value;
            this.OriginalFileName = originalFileNameString == "null" ? null : originalFileNameString.Trim('"');
        }
    }
}
