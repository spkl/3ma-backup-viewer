namespace LateNightStupidities.IIImaBackupReader
{
    /// <summary>
    /// A contact.
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// Gets the ID of the contact.
        /// </summary>
        public Identity Identity { get; internal set; }

        /// <summary>
        /// Gets the verification status.
        /// </summary>
        public string Verification { get; private set; }

        /// <summary>
        /// Gets the first name, if available.
        /// </summary>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the last name, if available.
        /// </summary>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the nick name, if available.
        /// </summary>
        public string NickName { get; private set; }

        /// <summary>
        /// Gets or sets a custom display name.
        /// If this is set to a non-null value, it is used as <see cref="DisplayName"/>,
        /// overriding all other available information.
        /// </summary>
        public string CustomDisplayName { get; set; }

        /// <summary>
        /// Gets the display name of this contact.
        /// If a <see cref="CustomDisplayName"/> is set, it is used.
        /// If not and a <see cref="FirstName"/> is available, a combination of FirstName and LastName ist used.
        /// If not and a <see cref="NickName"/> is available, it is used.
        /// If non of the above are available or set, the <see cref="Identity"/> string is used.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (this.CustomDisplayName != null)
                {
                    return this.CustomDisplayName;
                }

                if (!string.IsNullOrEmpty(this.FirstName))
                {
                    return $"{this.FirstName} {this.LastName}";
                }

                if (!string.IsNullOrEmpty(this.NickName))
                {
                    return this.NickName;
                }

                return this.Identity.ToString();
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return this.DisplayName;
        }

        /// <summary>
        /// Creates a contact from a CSV row.
        /// </summary>
        /// <param name="fields">The fields of the CSV row.</param>
        /// <param name="backup">The associated IIImaBackup.</param>
        public static Contact FromFields(string[] fields, IIImaBackup backup)
        {
            return new Contact
            {
                Identity = new Identity(fields[0], backup),
                Verification = fields[2],
                FirstName = fields[5],
                LastName = fields[6],
                NickName = fields[7]
            };
        }
    }
}