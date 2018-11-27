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
    /// <summary>Provides a phone adataset</summary>
    /// <seealso cref="System.IEquatable{Phone}" />
    [Table("PhoneNumbers")]
    public struct PhoneNumber : IEquatable<PhoneNumber>
    {
        /// <summary>Implements the operator ==.</summary>
        /// <param name="phoneNumber1">The phone number1.</param>
        /// <param name="phoneNumber2">The phone number2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(PhoneNumber phoneNumber1, PhoneNumber phoneNumber2)
        {
            return phoneNumber1.ID == phoneNumber2.ID
                && phoneNumber1.Number == phoneNumber2.Number
                && phoneNumber1.UserID == phoneNumber2.UserID
                && phoneNumber1.IsVerified == phoneNumber2.IsVerified;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="phoneNumber1">The phone number1.</param>
        /// <param name="phoneNumber2">The phone number2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(PhoneNumber phoneNumber1, PhoneNumber phoneNumber2)
        {
            return phoneNumber1.ID != phoneNumber2.ID
                || phoneNumber1.Number != phoneNumber2.Number
                || phoneNumber1.UserID != phoneNumber2.UserID
                || phoneNumber1.IsVerified != phoneNumber2.IsVerified;
        }

        /// <summary>
        /// ID of the phone
        /// </summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>
        /// ID of the <see cref="User"/>
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long UserID;

        /// <summary>The country identifier</summary>
        [Field]
        public long CountryID;

        /// <summary>The prefix</summary>
        [Field]
        public int Prefix;

        /// <summary>
        /// The main phone number part
        /// </summary>
        [Field]
        public long Number;

        /// <summary>Is this the primary number of the user</summary>
        [Field]
        public bool IsPrimary;

        /// <summary>
        /// Is a phone number verified!?
        /// </summary>
        [Field]
        public bool IsVerified;

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. </returns>
        public override int GetHashCode()
        {
            return CountryID.GetHashCode() ^ Prefix.GetHashCode() ^ Number.GetHashCode();
        }

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is PhoneNumber) return base.Equals((PhoneNumber)obj);
            return false;
        }

        /// <summary>Determines whether the specified <see cref="PhoneNumber" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="PhoneNumber" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="PhoneNumber" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(PhoneNumber other)
        {
            return other.Number == Number
                && other.UserID == UserID
                && other.IsVerified == IsVerified;
        }

        /// <summary>
        /// Obtains a string describing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"PhoneNumber {CountryID} {Prefix} {Number}";
        }
    }
}
