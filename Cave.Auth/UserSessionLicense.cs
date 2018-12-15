using System;
using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides license to session link
    /// </summary>
    [Table("UserSessionLicenses")]
    public struct UserSessionLicense
    {
        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>The license identifier</summary>
        [Field(Flags = FieldFlags.Index)]
        public long LicenseID;

        /// <summary>The user identifier</summary>
        [Field]
        public long UserID;

        /// <summary>The user session identifier</summary>
        [Field]
        public long UserSessionID;

        /// <summary>The expiration datetime</summary>
        [DateTimeFormat(DateTimeKind.Utc, DateTimeType.Native)]
        [Field]
        public DateTime Expiration;
    }
}
