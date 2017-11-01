using System.IO;

namespace LateNightStupidities.IIImaBackupReader.Messages
{
    /// <summary>
    /// A message with an attached video.
    /// </summary>
    public class VideoMessage : MediaMessage
    {
        /// <summary>
        /// Gets the file name of the thumbnail in the backup folder.
        /// </summary>
        public string ThumbnailFileName => $"message_media_{this.Uid}";

        /// <summary>
        /// Gets the file path of the thumbnail.
        /// </summary>
        public string ThumbnailFilePath => Path.Combine(Path.GetDirectoryName(this.Conversation.FilePath), this.ThumbnailFileName);

        internal VideoMessage()
        {
        }
    }
}
