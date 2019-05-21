using System;
using System.Text;
using Cave.Data;
using Cave.IO;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a country dataset
    /// </summary>
    [Table("UserSessions")]
    public struct UserSession
    {
        /// <summary>
        /// ID of the auth
        /// </summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>Gets the string identifier.</summary>
        /// <value>The string identifier.</value>
        public string StringID => Base64.Default.Encode(ID);

        /// <summary>The user identifier</summary>
        [Field(Flags = FieldFlags.Index)]
        public long UserID;

        /// <summary>The user agent</summary>
        [StringFormat(StringEncoding.ASCII)]
        [Field(Length = 255)]
        public string UserAgent;

        /// <summary>The source</summary>
        [StringFormat(StringEncoding.ASCII)]
        [Field(Length = 45)]
        public string Source;

        /// <summary>The expiration datetime</summary>
        [DateTimeFormat(DateTimeKind.Utc, DateTimeType.Native)]
        [Field]
        public DateTime Expiration;

        /// <summary>
        /// Provides additional flags for the session
        /// </summary>
        /// <remarks>
        /// This flag shall not be saved to a database!
        /// </remarks>
        public UserSessionFlags Flags;

        /// <summary>Gets a value indicating whether this instance is expired.</summary>
        /// <value>
        /// <c>true</c> if this instance is expired; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpired()
        {
            return DateTime.UtcNow > Expiration;
        }

        /// <summary>Returns true if this instance is valid.</summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        public bool IsValid()
        {
            return Source != null & UserAgent != null && !IsExpired();
        }

        /// <summary>Gets a value indicating whether this instance is authenticated.</summary>
        /// <value>
        /// <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated()
        {
            return UserID > 0 && IsValid();
        }

        /// <summary>
        /// Obtains a string describing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UserSession");
            builder.AppendFormat(" {0}", StringID);
            if (IsAuthenticated())
            {
                builder.Append(" authenticated");
            }
            else
            {
                builder.Append(" not authenticated");
            }
            return builder.ToString();
        }
    }
}
