#region CopyRight 2018
/*
    Copyright (c) 2003-2018 Andreas Rohleder (andreas@rohleder.cc)
    All rights reserved
*/
#endregion
#region License LGPL-3
/*
    This program/library/sourcecode is free software; you can redistribute it
    and/or modify it under the terms of the GNU Lesser General Public License
    version 3 as published by the Free Software Foundation subsequent called
    the License.

    You may not use this program/library/sourcecode except in compliance
    with the License. The License is included in the LICENSE file
    found at the installation directory or the distribution package.

    Permission is hereby granted, free of charge, to any person obtaining
    a copy of this software and associated documentation files (the
    "Software"), to deal in the Software without restriction, including
    without limitation the rights to use, copy, modify, merge, publish,
    distribute, sublicense, and/or sell copies of the Software, and to
    permit persons to whom the Software is furnished to do so, subject to
    the following conditions:

    The above copyright notice and this permission notice shall be included
    in all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
    EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
    MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
    NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
    LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
    OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
    WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion
#region Authors & Contributors
/*
   Author:
     Andreas Rohleder <andreas@rohleder.cc>

   Contributors:
 */
#endregion

using Cave.Data;
using Cave.Text;
using System;

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
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is GroupMember) return base.Equals((GroupMember)obj);
            return false;
        }

        /// <summary>Determines whether the specified <see cref="GroupMember" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="GroupMember" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="GroupMember" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(GroupMember other)
        {
            return other.GroupID == GroupID && other.UserID == UserID;
        }
    }
}
