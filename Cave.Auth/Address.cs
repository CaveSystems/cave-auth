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
    /// Provides an address dataset
    /// </summary>
    [Table("Addresses")]
    public struct Address : IEquatable<Address>
    {
        /// <summary>Implements the operator ==.</summary>
        /// <param name="address1">The address1.</param>
        /// <param name="address2">The address2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Address address1, Address address2)
        {
            return address1.CountryID == address2.CountryID
                && address1.ID == address2.ID
                && address1.Text == address2.Text
                && address1.UserID == address2.UserID;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="address1">The address1.</param>
        /// <param name="address2">The address2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Address address1, Address address2)
        {
            return address1.CountryID != address2.CountryID
                || address1.ID != address2.ID
                || address1.Text != address2.Text
                || address1.UserID != address2.UserID;
        }

        /// <summary>
        /// ID of the address
        /// </summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>
        /// ID of the User <see cref="User"/>
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long UserID;

        /// <summary>
        /// ID of the Country <see cref="Country"/>
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long CountryID;
        
        /// <summary>
        /// The first address
        /// </summary>
        [Field(Length = 255)]
        public string Text;

        /// <summary>Is this the primary address of the user</summary>
        [Field]
        public bool IsPrimary;

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Address) return base.Equals((Address)obj);
            return false;
        }

        /// <summary>Determines whether the specified <see cref="Address" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="Address" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="Address" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Address other)
        {
            return other.CountryID == CountryID
                && other.Text == Text
                && other.UserID == UserID;
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Obtains a string describing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                "Address" + ((ID > 0) ? "[" + ID + "]" : "") +
                " User: " + UserID +
                " Country: " + CountryID +
                " Text: " + Text.ReplaceNewLine(", ");
        }
    }
}
