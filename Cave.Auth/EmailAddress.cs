using System;
using Cave.Data;
using Cave.IO;

namespace Cave.Auth
{
    /// <summary>
    /// Provides an user's email address
    /// </summary>
    [Table("EmailAddresses")]
    public struct EmailAddress : IEquatable<EmailAddress>
    {
        /// <summary>Implements the operator ==.</summary>
        /// <param name="email1">The email1.</param>
        /// <param name="email2">The email2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(EmailAddress email1, EmailAddress email2)
        {
            return email1.ID == email2.ID
                && email1.Address == email2.Address
                && email1.UserID == email2.UserID
                && email1.VerificationCode == email2.VerificationCode
                && email1.Verified == email2.Verified;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="email1">The email1.</param>
        /// <param name="email2">The email2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(EmailAddress email1, EmailAddress email2)
        {
            return email1.ID != email2.ID
                || email1.Address != email2.Address
                || email1.UserID != email2.UserID
                || email1.VerificationCode != email2.VerificationCode
                || email1.Verified != email2.Verified;
        }

        /// <summary>
        /// ID of the email
        /// </summary>
        [Field(Flags = FieldFlags.ID | FieldFlags.AutoIncrement)]
        public long ID;

        /// <summary>
        /// ID of the <see cref="User"/>
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long UserID;

        /// <summary>
        /// Email Address
        /// </summary>
        [Field(Length = 64)]
        [StringFormat(StringEncoding.ASCII)]
        public string Address;

        /// <summary>
        /// Is a email verified?
        /// </summary>
        [Field]
        public bool Verified;

        /// <summary>
        /// Verification Code
        /// </summary>
        [Field]
        [StringFormat(StringEncoding.ASCII)]
        public string VerificationCode;

        /// <summary>
        /// Obtains a string describing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                "Email" +
                ((ID > 0) ? "[" + ID + "] " : " ") +
                ((!string.IsNullOrEmpty(Address)) ? Address : "<no address>");
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is EmailAddress)
            {
                return base.Equals((EmailAddress)obj);
            }

            return false;
        }

        /// <summary>Determines whether the specified <see cref="EmailAddress" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="EmailAddress" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="EmailAddress" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(EmailAddress other)
        {
            return other.Address == Address
                && other.UserID == UserID
                && other.VerificationCode == VerificationCode
                && other.Verified == Verified;
        }
    }
}
