using System;

namespace Cave.Auth
{
    /// <summary>
    /// User Session Flags
    /// </summary>
	[Flags]
    public enum UserSessionFlags
    {
        /// <summary>
        /// The session is running at localhost
        /// </summary>
		IsLocalhost = 1,
    }
}
