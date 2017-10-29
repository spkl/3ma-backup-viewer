namespace LateNightStupidities.IIImaBackupReader.Messages
{
    /// <summary>
    /// A message with an attach location marker.
    /// </summary>
    public class LocationMessage : Message
    {
        /// <summary>
        /// The attached location.
        /// </summary>
        public Location Location { get; private set; }

        internal LocationMessage()
        {
        }

        /// <inheritdoc />
        protected override void ReadPropertiesFromBody()
        {
            this.Location = Location.FromBody(this.Body);
        }
    }
}
