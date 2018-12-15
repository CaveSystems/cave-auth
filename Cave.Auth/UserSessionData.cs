using System;
using Cave.Data;
using Cave.IO;

namespace Cave.Auth
{
    /// <summary>
    /// Provides data storage for <see cref="UserSession"/>
    /// </summary>
    [Table("UserSessionData")]
    public struct UserSessionData
    {
        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.ID)]
        public long ID;

        /// <summary>The user session identifier</summary>
        [Field]
        public long UserSessionID;

        /// <summary>The last changed</summary>
        [Field]
        public DateTime LastChanged;

        /// <summary>The value</summary>
        [Field]
        public Utf8string Value;
    }
}
