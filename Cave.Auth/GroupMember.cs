using System;
using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Group member link dataset. This links a user dataset to a group dataset allowing each user to join multiple different groups.
    /// </summary>
    [Table("GroupMembers")]
    public struct GroupMember : IEquatable<GroupMember>
    {
        /// <summary>Implements the operator ==.</summary>
        /// <param name="groupMember1">The group member1.</param>
        /// <param name="groupMember2">The group member2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(GroupMember groupMember1, GroupMember groupMember2)
        {
            return groupMember1.ID == groupMember2.ID
                && groupMember1.GroupID == groupMember2.GroupID
                && groupMember1.UserID == groupMember2.UserID;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="groupMember1">The group member1.</param>
        /// <param name="groupMember2">The group member2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(GroupMember groupMember1, GroupMember groupMember2)
        {
            return groupMember1.ID != groupMember2.ID
                || groupMember1.GroupID != groupMember2.GroupID
                || groupMember1.UserID != groupMember2.UserID;
        }

        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.ID | FieldFlags.AutoIncrement)]
        public long ID;

        /// <summary>The group identifier</summary>
        [Field(Flags = FieldFlags.Index)]
        public long GroupID;

        /// <summary>The user identifier</summary>
        [Field(Flags = FieldFlags.Index)]
        public long UserID;

        /// <summary>The datetime of the invite</summary>
        [Field]
        [DateTimeFormat(DateTimeKind.Utc, DateTimeType.Native)]
        public DateTime InviteDateTime;

        /// <summary>The flags</summary>
        [Field]
        public GroupMemberFlags Flags;

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is GroupMember member)
            {
                return base.Equals(member);
            }

            return false;
        }

        /// <summary>Determines whether the specified <see cref="GroupMember" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="GroupMember" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="GroupMember" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(GroupMember other) => other.GroupID == GroupID && other.UserID == UserID;
    }
}
