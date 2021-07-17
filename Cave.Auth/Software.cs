using System;
using Cave.Collections;
using Cave.Data;
using Cave.IO;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a software dataset
    /// </summary>
    [Table("Software")]
    public struct Software
    {
        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>The (product) name</summary>
        [Field(Flags = FieldFlags.Index, Length = 64)]
        [StringFormat(StringEncoding.ASCII)]
        public string Name;

        /// <summary>The version</summary>
        [Field(Flags = FieldFlags.Index, Length = 32)]
        [StringFormat(StringEncoding.ASCII)]
        public Version Version;

        /// <summary>The license free flag</summary>
        [Field]
        public bool LicenseFree;

        /// <summary>The program identifier</summary>
        [Field(Flags = FieldFlags.Index)]
        public long ProgramID;

        /// <summary>The server ip</summary>
        [Field(Length = 45)]
        [StringFormat(StringEncoding.ASCII)]
        public string ServerIP;

        /// <summary>Hashed Password bytes 512 bits = 64 bytes</summary>
        [Field(Length = 64)]
        public byte[] PasswordBytes;

        /// <summary>Salt used while hashing</summary>
        [Field(Length = 32)]
        public byte[] Salt;

        /// <summary>Sets a new salt (32 bytes)</summary>
        public void SetRandomSalt()
        {
            var rndBytes = new byte[32];
            AuthTables.RNG.NextBytes(rndBytes);
            Salt = rndBytes;
        }

        /// <summary>Replaces the users password without any checks.</summary>
        /// <param name="password">The new password.</param>
        public void SetPassword(string password)
        {
            SetRandomSalt();
            PasswordBytes = GetPasswordBytes(password);
        }

        /// <summary>Generate PasswordBytes</summary>
        /// <param name="password">The plain text password</param>
        /// <returns>byte[] of generated PasswordBytes</returns>
        public byte[] GetPasswordBytes(string password)
        {
            using var derive = new PBKDF2(password, Salt);
            return derive.GetBytes(64);
        }

        /// <summary>Tests the password.</summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool TestPassword(string password)
        {
            var data = GetPasswordBytes(password);
            return DefaultComparer.Equals(data, PasswordBytes);
        }

        /// <summary>Clears the private fields.</summary>
        public void ClearPrivateFields()
        {
            Salt = null;
            PasswordBytes = null;
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => $"[{ID}] {Name} {Version} {Base32.Safe.Encode(ProgramID)}";
    }
}
