using System;
using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides an address dataset
    /// </summary>
    [Table("Addresses")]
    public struct Address : IEquatable<Address>
    {
        /// <summary>Implements the operator ==.</summary>
        /// <param name="address1">The address1.</param>
        /// <param name="address2">The address2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Address address1, Address address2)
        {
            return address1.CountryID == address2.CountryID
                && address1.ID == address2.ID
                && address1.Text == address2.Text
                && address1.UserID == address2.UserID;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="address1">The address1.</param>
        /// <param name="address2">The address2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Address address1, Address address2)
        {
            return address1.CountryID != address2.CountryID
                || address1.ID != address2.ID
                || address1.Text != address2.Text
                || address1.UserID != address2.UserID;
        }

        /// <summary>
        /// ID of the address
        /// </summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>
        /// ID of the User <see cref="User"/>
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long UserID;

        /// <summary>
        /// ID of the Country <see cref="Country"/>
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long CountryID;

        /// <summary>
        /// The first address
        /// </summary>
        [Field(Length = 255)]
        public string Text;

        /// <summary>Is this the primary address of the user</summary>
        [Field]
        public bool IsPrimary;

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Address address)
            {
                return base.Equals(address);
            }

            return false;
        }

        /// <summary>Determines whether the specified <see cref="Address" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="Address" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="Address" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Address other)
        {
            return other.CountryID == CountryID
                && other.Text == Text
                && other.UserID == UserID;
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Obtains a string describing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                "Address" + ((ID > 0) ? "[" + ID + "]" : "") +
                " User: " + UserID +
                " Country: " + CountryID +
                " Text: " + Text.ReplaceNewLine(", ");
        }
    }
}
