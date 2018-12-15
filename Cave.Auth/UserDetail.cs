using System;
using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides user details
    /// </summary>
    [Table("UserDetails")]
    public struct UserDetail
    {
        /// <summary>The identifier, this matches <see cref="User.ID"/></summary>
        [Field(Flags = FieldFlags.ID)]
        public long UserID;

        /// <summary>The first name</summary>
        [Field(Length = 32)]
        public string FirstName;

        /// <summary>The last name</summary>
        [Field(Length = 32)]
        public string LastName;

        /// <summary>The gender</summary>
        [Field]
        public Gender Gender;

        /// <summary>The birthday</summary>
        [Field]
        public DateTime Birthday;

        /// <summary>The phone password</summary>
        [Field]
        public int PhonePIN;
    }
}
