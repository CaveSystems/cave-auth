using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a new password notification
    /// </summary>
    [Table("NewPasswordNotifications")]
    public struct NewPasswordNotification
    {
        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.ID)]
        public long ID;

        /// <summary>The new password</summary>
        [Field]
        public string Password;

        /// <summary>The one time password link</summary>
        [Field]
        public string OTPLink;

        /// <summary>The one time password secret</summary>
        [Field]
        public string OTPSecret;
    }
}
