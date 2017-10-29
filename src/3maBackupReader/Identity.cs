namespace LateNightStupidities.IIImaBackupReader
{
    /// <summary>
    /// A 3ma ID.
    /// </summary>
    public struct Identity
    {
        private readonly string id;

        private readonly IIImaBackup backup;

        /// <summary>
        /// The display name for this ID.
        /// This information is retrieved from the associated <see cref="IIImaBackup"/>.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (this.backup.Contacts.TryGetValue(this, out Contact contact))
                {
                    return contact.DisplayName;
                }

                return this.id;
            }
        }

        /// <summary>
        /// Creates a new 3ma ID.
        /// </summary>
        /// <param name="id">The ID string.</param>
        /// <param name="backup">The associated IIImaBackup.</param>
        public Identity(string id, IIImaBackup backup)
        {
            this.id = id;
            this.backup = backup;
        }

        /// <summary>
        /// Creates a new 3ma ID without an associated IIImaBackup.
        /// The <see cref="DisplayName"/> cannot be used when using this constructor.
        /// </summary>
        /// <param name="id">The ID string.</param>
        public Identity(string id) : this(id, null)
        {
        }

        public bool Equals(Identity other)
        {
            return string.Equals(this.id, other.id);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Identity identity && this.Equals(identity);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public static bool operator ==(Identity left, Identity right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Identity left, Identity right)
        {
            return !left.Equals(right);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.id;
        }
    }
}