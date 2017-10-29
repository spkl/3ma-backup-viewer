using System;

namespace LateNightStupidities.IIImaBackupReader
{
    /// <summary>
    /// Utility methods used for reading a IIImaBackup.
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Converts a timestamp string from a CSV file to a DateTime.
        /// Input example: 1438371129562.
        /// </summary>
        /// <param name="dateValue">The timestamp string.</param>
        /// <returns>The DateTime in UTC.</returns>
        public static DateTime ReadDateValue(string dateValue)
        {
            dateValue = dateValue.Substring(0, dateValue.Length - 3);
            return DateTimeOffset.FromUnixTimeSeconds(long.Parse(dateValue)).DateTime;
        }

        /// <summary>
        /// Converts an optional timestamp string from a CSV file to a DateTime.
        /// Input example: 1438371129562 or null.
        /// </summary>
        /// <param name="dateValue">The timestamp string.</param>
        /// <returns>The DateTime in UTC or null.</returns>
        public static DateTime? ReadOptionalDateValue(string dateValue)
        {
            if (dateValue == null || dateValue.Length <= 3)
            {
                return null;
            }

            return ReadDateValue(dateValue);
        }
    }
}