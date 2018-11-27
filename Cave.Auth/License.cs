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
    /// Provides a software license
    /// </summary>
    [Table("Licenses")]
    public struct License 
    {
        /// <summary>
        /// ID of the email
        /// </summary>
        [Field(Flags = FieldFlags.ID | FieldFlags.AutoIncrement)]
        public long ID;

        /// <summary>
        /// ID of the <see cref="Software"/> the license is for. 
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long SoftwareID;

        /// <summary>
        /// ID of the <see cref="User"/> owning the license. 
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long UserID;

        /// <summary>
        /// ID of the <see cref="Group"/> allowed to use the license.
        /// </summary>
        [Field(Flags = FieldFlags.Index)]
        public long GroupID;

        /// <summary>The description</summary>
        [Field(Length = 32)]
        [StringFormat(StringEncoding.ASCII)]
        public string Description;

        /// <summary>The allowed simultaneous sessions (-1 : unlimited, 0 : none, &gt;0 : number of sessions)</summary>
        [Field]
        public short MaxSessions;

        /// <summary>The allowed simultaneous users (-1 : unlimited, 0 : none, &gt;0 : number of users)</summary>
        [Field]
        public short MaxUsers;

        /// <summary>The certificate (for offline software)</summary>
        [Field]
        public byte[] Certificate;

        /// <summary>The valid till datetime</summary>
        [Field]
        [DateTimeFormat(DateTimeKind.Utc, DateTimeType.Native)]
        public DateTime ValidTill;

        /// <summary>The components</summary>
        [Field]
        [StringFormat(StringEncoding.ASCII)]
        public string Components;

        /// <summary>
        /// Obtains a string describing this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "License" + ((ID > 0) ? "[" + ID + "] " : " ");
        }
    }
}
