using System;
using Cave.Collections;
using Cave.Data;
using Cave.IO;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a user dataset
    /// </summary>
    [Table("Users")]
    public struct User : IEquatable<User>
    {
        /// <summary>Implements the operator ==.</summary>
        /// <param name="user1">The user1.</param>
        /// <param name="user2">The user2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(User user1, User user2)
        {
            return user1.ID == user2.ID
                && user1.AvatarID == user2.AvatarID
                && user1.Color == user2.Color
                && user1.InvalidLogonTries == user2.InvalidLogonTries
                && user1.LastUpdate == user2.LastUpdate
                && user1.NickName == user2.NickName
                && user1.PasswordBytes == user2.PasswordBytes
                && user1.Salt == user2.Salt
                && user1.State == user2.State;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="user1">The user1.</param>
        /// <param name="user2">The user2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(User user1, User user2)
        {
            return user1.ID != user2.ID
                || user1.AvatarID != user2.AvatarID
                || user1.Color != user2.Color
                || user1.InvalidLogonTries != user2.InvalidLogonTries
                || user1.LastUpdate != user2.LastUpdate
                || user1.NickName != user2.NickName
                || user1.PasswordBytes != user2.PasswordBytes
                || user1.Salt != user2.Salt
                || user1.State != user2.State;
        }

        /// <summary>
        /// ID of the user
        /// </summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>The nick name or display name of the user</summary>
        [Field(Flags = FieldFlags.Index, Length = 42)]
        [StringFormat(StringEncoding.UTF8)]
        public string NickName;

        /// <summary>The users avatar identifier</summary>
        [Field]
        public long AvatarID;

        /// <summary>The users color</summary>
        [Field]
        public uint Color;

        /// <summary>The authentication level in the auth server system</summary>
        [Field]
        public int AuthLevel;

        /// <summary>Hashed Password bytes 512 bits = 64 bytes</summary>
        [Field(Length = 64)]
        public byte[] PasswordBytes;

        /// <summary>Salt used while hashing</summary>
        [Field(Length = 32)]
        public byte[] Salt;

        /// <summary>Obtains the <see cref=" UserState" /></summary>
        [Field]
        public UserState State;

        /// <summary>DateTime of the last update of the dataset</summary>
        [Field]
        [DateTimeFormat(DateTimeKind.Utc, DateTimeType.BigIntHumanReadable)]
        public DateTime LastUpdate;

        /// <summary>Number of logon tries with incorrect credentials</summary>
        [Field]
        public int InvalidLogonTries;

        /// <summary>Sets a new salt (32 bytes)</summary>
        public void SetRandomSalt()
        {
            var rndBytes = new byte[32];
            AuthTables.RNG.NextBytes(rndBytes);
            Salt = rndBytes;
            LastUpdate = DateTime.UtcNow;
        }

        /// <summary>Generate PasswordBytes</summary>
        /// <param name="password">The plain text password</param>
        /// <returns>byte[] of generated PasswordBytes</returns>
        public byte[] GetPasswordBytes(string password)
        {
            using var derive = new PBKDF2(password, Salt);
            return derive.GetBytes(64);
        }

        /// <summary>Change password after checking the old one</summary>
        /// <param name="oldPass">the old plain password</param>
        /// <param name="newPass">the new plain password</param>
        /// <exception cref="Exception">Old Password does not match!</exception>
        public void ChangePassword(string oldPass, string newPass)
        {
            if (!TestPassword(oldPass))
            {
                throw new ArgumentException("Old Password does not match!");
            }
            SetPassword(newPass);
        }

        /// <summary>Tests the password.</summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool TestPassword(string password)
        {
            var data = GetPasswordBytes(password);
            return DefaultComparer.Equals(data, PasswordBytes);
        }

        /// <summary>Replaces the users password without any checks.</summary>
        /// <param name="password">The new password.</param>
        public void SetPassword(string password)
        {
            LastUpdate = DateTime.UtcNow;
            if (Salt == null)
            {
                SetRandomSalt();
            }

            PasswordBytes = GetPasswordBytes(password);
        }

        /// <summary>Clears the personal fields.</summary>
        public User ClearPrivateFields()
        {
            PasswordBytes = null;
            Salt = null;
            return this;
        }

        /// <summary>Obtains a string describing this instance</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString() => ((ID > 0) ? "[" + ID + "] " : "") + State + " " + NickName;

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is User user)
            {
                return base.Equals(user);
            }

            return false;
        }

        /// <summary>Determines whether the specified <see cref="User" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="User" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="User" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(User other) => other.AvatarID == AvatarID
                && other.Color == Color
                && other.InvalidLogonTries == InvalidLogonTries
                && other.LastUpdate == LastUpdate
                && other.NickName == NickName
                && other.PasswordBytes == PasswordBytes
                && other.Salt == Salt
                && other.State == State;
    }
}
