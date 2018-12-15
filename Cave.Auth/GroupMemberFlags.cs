using System;

namespace Cave.Auth
{
    /// <summary>
    /// Provides group member flags
    /// </summary>
    [Flags]
    public enum GroupMemberFlags
    {
        /// <summary>Member has joined the group</summary>
        HasJoined = 1,

        /// <summary>The user is admin of this group (all flags active)</summary>
        IsAdmin = 0xFFFF,
    }
}
