using System.Globalization;
using System.Text.RegularExpressions;

namespace LateNightStupidities.IIImaBackupReader.Messages
{
    /// <summary>
    /// A location.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets the latitude value of the location.
        /// </summary>
        public double Latitude { get; private set; }

        /// <summary>
        /// Gets the longitude value of the location.
        /// </summary>
        public double Longitude { get; private set; }

        /// <summary>
        /// Gets the address of the location. May be null.
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// Gets the display name of the location. May be null.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Creates a location from the message body meta data.
        /// </summary>
        /// <param name="body">The message body meta data.</param>
        public static Location FromBody(string body)
        {
            Regex rx = new Regex(@"^\[(-?\d+\.\d+),(-?\d+\.\d+),\d+\.\d+,("".*""|null),("".*""|null)\]$");
            Match match = rx.Match(body);
            GroupCollection groups = match.Groups;

            double lat = double.Parse(groups[1].Value, NumberFormatInfo.InvariantInfo);
            double lon = double.Parse(groups[2].Value, NumberFormatInfo.InvariantInfo);
            string address = groups[3].Value;
            address = address == "null" ? null : address.Trim('"');
            string name = groups[4].Value;
            name = name == "null" ? null : name.Trim('"');

            return new Location()
            {
                Latitude = lat,
                Longitude = lon,
                Address = address,
                Name = name
            };
        }
    }
}
