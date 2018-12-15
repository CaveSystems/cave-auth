using System;
using Cave.Data;
using Cave.IO;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a group dataset
    /// </summary>
    [Table("Groups")]
    public struct Group : IEquatable<Group>
    {
        /// <summary>Implements the operator ==.</summary>
        /// <param name="group1">The group1.</param>
        /// <param name="group2">The group2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Group group1, Group group2)
        {
            return group1.ID == group2.ID
                && group1.AvatarID == group2.AvatarID
                && group1.Color == group2.Color
                && group1.Name == group2.Name;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="group1">The group1.</param>
        /// <param name="group2">The group2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Group group1, Group group2)
        {
            return group1.ID != group2.ID
                || group1.AvatarID != group2.AvatarID
                || group1.Color != group2.Color
                || group1.Name != group2.Name;
        }

        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.ID | FieldFlags.AutoIncrement)]
        public long ID;

        /// <summary>The group avatar identifier</summary>
        [Field]
        public long AvatarID;

        /// <summary>The group color</summary>
        [Field]
        public uint Color;

        /// <summary>The name of the group</summary>
        [Field(Length = 64)]
        [StringFormat(StringEncoding.UTF8)]
        public string Name;

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Group)
            {
                return base.Equals((Group)obj);
            }

            return false;
        }

        /// <summary>Determines whether the specified <see cref="Group" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="Group" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="Group" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Group other)
        {
            return other.AvatarID == AvatarID
                && other.Color == Color
                && other.Name == Name;
        }
    }
}
