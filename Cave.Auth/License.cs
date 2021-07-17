using System;
using Cave.Data;
using Cave.IO;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a software license
    /// </summary>
    [Table("Licenses")]
    public struct License
    {
        /// <summary>
        /// ID of the email
        /// </summary>
        [Field(Flags = FieldFlags.ID | FieldFlags.AutoIncrement)]
        public long ID;

        /// <summary>
        /// ID of the <see cref="Software"/> the license is for. 
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long SoftwareID;

        /// <summary>
        /// ID of the <see cref="User"/> owning the license. 
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long UserID;

        /// <summary>
        /// ID of the <see cref="Group"/> allowed to use the license.
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long GroupID;

        /// <summary>The description</summary>
        [Field(Length = 32)]
        [StringFormat(StringEncoding.ASCII)]
        public string Description;

        /// <summary>The allowed simultaneous sessions (-1 : unlimited, 0 : none, &gt;0 : number of sessions)</summary>
        [Field]
        public short MaxSessions;

        /// <summary>The allowed simultaneous users (-1 : unlimited, 0 : none, &gt;0 : number of users)</summary>
        [Field]
        public short MaxUsers;

        /// <summary>The certificate (for offline software)</summary>
        [Field]
        public byte[] Certificate;

        /// <summary>The valid till datetime</summary>
        [Field]
        [DateTimeFormat(DateTimeKind.Utc, DateTimeType.Native)]
        public DateTime ValidTill;

        /// <summary>The components</summary>
        [Field]
        [StringFormat(StringEncoding.ASCII)]
        public string Components;

        /// <summary>
        /// Obtains a string describing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "License" + ((ID > 0) ? "[" + ID + "] " : " ");
    }
}
