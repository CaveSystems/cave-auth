using Cave.Data;
using Cave.IO;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a country dataset
    /// </summary>
    [Table("Countries")]
    public struct Country
    {
        /// <summary>Implements the operator ==.</summary>
        /// <param name="country1">The country1.</param>
        /// <param name="country2">The country2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Country country1, Country country2) => country1.ID == country2.ID;

        /// <summary>Implements the operator !=.</summary>
        /// <param name="country1">The country1.</param>
        /// <param name="country2">The country2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Country country1, Country country2) => country1.ID != country2.ID;

        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>The international country code</summary>
        [Field]
        public uint Code;

        /// <summary>Country name in the format languagecode2-country/regioncode2.</summary>
        [StringFormat(StringEncoding.UTF8)]
        [Field(Length = 64)]
        public string Name;

        /// <summary>Country name, consisting of the language, the country/region, and the optional script, that the culture is set to display.</summary>
        [StringFormat(StringEncoding.UTF8)]
        [Field(Length = 64)]
        public string NativeName;

        /// <summary>The english name of the country</summary>
        [StringFormat(StringEncoding.UTF8)]
        [Field(Length = 64)]
        public string EnglishName;

        /// <summary>ISO 639-1 two-letter code for the language.</summary>
        [Field(Length = 2)]
        public string ISO2;

        /// <summary>ISO 639-2 three-letter code for the language</summary>
        [Field(Length = 3)]
        public string ISO3;

        /// <summary>
        /// Obtains a string describing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Country {Name}";

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Country country)
            {
                return base.Equals(country);
            }

            return false;
        }

        /// <summary>Determines whether the specified <see cref="Country" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="Country" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="Country" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Country other) => other.ID == ID;
    }
}
