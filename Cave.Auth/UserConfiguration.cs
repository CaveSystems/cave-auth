using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides user configuration settings
    /// </summary>
    [Table("UserConfigurations")]
    public struct UserConfiguration
    {
        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>The user identifier</summary>
        [Field(Flags = FieldFlags.Index)]
        public long UserID;

        /// <summary>The program identifier</summary>
        [Field(Flags = FieldFlags.Index)]
        public long ProgramID;

        /// <summary>The data</summary>
        [Field]
        public byte[] Data;
    }
}
