using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a transaction key
    /// </summary>
    [Table]
    public struct TransactionKey
    {
        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.ID)]
        public long SessionID;

        /// <summary>Code</summary>
        [Field]
        public string Key;
    }
}
