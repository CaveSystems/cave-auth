using System;
using Cave.Data;

namespace Cave.Auth
{
    /// <summary>Provides a phone adataset</summary>
    /// <seealso cref="System.IEquatable{Phone}" />
    [Table("PhoneNumbers")]
    public struct PhoneNumber : IEquatable<PhoneNumber>
    {
        /// <summary>Implements the operator ==.</summary>
        /// <param name="phoneNumber1">The phone number1.</param>
        /// <param name="phoneNumber2">The phone number2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(PhoneNumber phoneNumber1, PhoneNumber phoneNumber2)
        {
            return phoneNumber1.ID == phoneNumber2.ID
                && phoneNumber1.Number == phoneNumber2.Number
                && phoneNumber1.UserID == phoneNumber2.UserID
                && phoneNumber1.IsVerified == phoneNumber2.IsVerified;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="phoneNumber1">The phone number1.</param>
        /// <param name="phoneNumber2">The phone number2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(PhoneNumber phoneNumber1, PhoneNumber phoneNumber2)
        {
            return phoneNumber1.ID != phoneNumber2.ID
                || phoneNumber1.Number != phoneNumber2.Number
                || phoneNumber1.UserID != phoneNumber2.UserID
                || phoneNumber1.IsVerified != phoneNumber2.IsVerified;
        }

        /// <summary>
        /// ID of the phone
        /// </summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>
        /// ID of the <see cref="User"/>
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long UserID;

        /// <summary>The country identifier</summary>
        [Field]
        public long CountryID;

        /// <summary>The prefix</summary>
        [Field]
        public int Prefix;

        /// <summary>
        /// The main phone number part
        /// </summary>
        [Field]
        public long Number;

        /// <summary>Is this the primary number of the user</summary>
        [Field]
        public bool IsPrimary;

        /// <summary>
        /// Is a phone number verified!?
        /// </summary>
        [Field]
        public bool IsVerified;

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode() => CountryID.GetHashCode() ^ Prefix.GetHashCode() ^ Number.GetHashCode();

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is PhoneNumber number)
            {
                return base.Equals(number);
            }

            return false;
        }

        /// <summary>Determines whether the specified <see cref="PhoneNumber" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="PhoneNumber" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="PhoneNumber" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(PhoneNumber other) => other.Number == Number
                && other.UserID == UserID
                && other.IsVerified == IsVerified;

        /// <summary>
        /// Obtains a string describing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"PhoneNumber {CountryID} {Prefix} {Number}";
    }
}
