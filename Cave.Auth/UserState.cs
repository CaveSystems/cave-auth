using System;

namespace Cave.Auth
{
    /// <summary>
    /// Provides user account states
    /// </summary>
    [Flags]
    public enum UserState : int
    {
        /// <summary>New user - no rights</summary>
        New = 0,

        /// <summary>Confirmed user - default user rights</summary>
        Confirmed = 1,

        /// <summary>Password reset requested by user - no rights</summary>
        PasswordResetRequested = 2,

        /// <summary>Account disabled - no rights</summary>
        Diabled = 3,

        /// <summary>Account deleted - no longer visible</summary>
        Deleted = 4,
    }
}
