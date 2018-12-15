using System;
using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a software session dataset
    /// </summary>
    [Table("SoftwareSessions")]
    public struct SoftwareSession
    {
        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.ID)]
        public long ID;

        /// <summary>The session identifier</summary>
        [Field(Flags = FieldFlags.Index)]
        public long SessionID;

        /// <summary>The expiration datetime</summary>
        [DateTimeFormat(DateTimeKind.Utc, DateTimeType.Native)]
        [Field]
        public DateTime Expiration;

        /// <summary>Gets a value indicating whether this instance is expired.</summary>
        /// <value>
        /// <c>true</c> if this instance is expired; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpired => DateTime.UtcNow > Expiration;
    }
}
