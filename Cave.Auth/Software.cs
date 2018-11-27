#region CopyRight 2018
/*
    Copyright (c) 2010-2018 Andreas Rohleder (andreas@rohleder.cc)
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
#endregion License
#region Authors & Contributors
/*
   Author:
     Andreas Rohleder <andreas@rohleder.cc>

   Contributors:
 */
#endregion Authors & Contributors

using Cave.Collections;
using Cave.Data;
using Cave.IO;
using Cave.Text;
using System;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a software dataset
    /// </summary>
    [Table("Software")]
    public struct Software
    {
        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

        /// <summary>The (product) name</summary>
        [Field(Flags = FieldFlags.Index, Length = 64)]
        [StringFormat(StringEncoding.ASCII)]
        public string Name;

        /// <summary>The version</summary>
        [Field(Flags = FieldFlags.Index, Length = 32)]
        [StringFormat(StringEncoding.ASCII)]
        public Version Version;

        /// <summary>The license free flag</summary>
        [Field]
        public bool LicenseFree;

        /// <summary>The program identifier</summary>
        [Field(Flags = FieldFlags.Index)]
        public long ProgramID;

        /// <summary>The server ip</summary>
        [Field(Length = 45)]
        [StringFormat(StringEncoding.ASCII)]
        public string ServerIP;

        /// <summary>Hashed Password bytes 512 bits = 64 bytes</summary>
        [Field(Length = 64)]
        public byte[] PasswordBytes;

        /// <summary>Salt used while hashing</summary>
        [Field(Length = 32)]
        public byte[] Salt;

        /// <summary>Sets a new salt (32 bytes)</summary>
        public void SetRandomSalt()
        {
            byte[] rndBytes = new byte[32];
            AuthTables.RNG.NextBytes(rndBytes);
            Salt = rndBytes;
        }

        /// <summary>Replaces the users password without any checks.</summary>
        /// <param name="password">The new password.</param>
        public void SetPassword(string password)
        {
            SetRandomSalt();
            PasswordBytes = GetPasswordBytes(password);
        }

        /// <summary>Generate PasswordBytes</summary>
        /// <param name="password">The plain text password</param>
        /// <returns>byte[] of generated PasswordBytes</returns>
        public byte[] GetPasswordBytes(string password)
        {
            using (PBKDF2 derive = new PBKDF2(password, Salt))
            {
                return derive.GetBytes(64);
            }
        }

        /// <summary>Tests the password.</summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool TestPassword(string password)
        {
            byte[] data = GetPasswordBytes(password);
            return DefaultComparer.Equals(data, PasswordBytes);
        }

        /// <summary>Clears the private fields.</summary>
        public void ClearPrivateFields()
        {
            Salt = null;
            PasswordBytes = null;
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"[{ID}] {Name} {Version} {Base32.Safe.Encode(ProgramID)}";
        }
    }
}
