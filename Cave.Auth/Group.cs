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
using Cave.IO;
using Cave.Text;
using System;

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
                && group1.AvatarID== group2.AvatarID
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
            if (obj is Group) return base.Equals((Group)obj);
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
