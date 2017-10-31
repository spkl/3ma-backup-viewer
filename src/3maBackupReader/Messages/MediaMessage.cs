using System.IO;

namespace LateNightStupidities.IIImaBackupReader.Messages
{
    /// <summary>
    /// A message with attached media.
    /// This can be a file, an image, a video or audio.
    /// </summary>
    public abstract class MediaMessage : Message
    {
        /// <summary>
        /// Gets the file name of the attached media in the backup folder.
        /// </summary>
        public string FileName => $"message_media_{this.Uid}";

        /// <summary>
        /// Gets the file path of the attached media.
        /// </summary>
        public string FilePath => Path.Combine(Path.GetDirectoryName(this.Conversation.FilePath), this.FileName);
    }
}