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
using System.Text;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a country dataset
    /// </summary>
    [Table("UserSessions")]
    public struct UserSession 
    {
        /// <summary>
        /// ID of the auth
        /// </summary>
        [Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

		/// <summary>Gets the string identifier.</summary>
		/// <value>The string identifier.</value>
		public string StringID { get { return Base64.Default.Encode(ID); } }

		/// <summary>The user identifier</summary>
		[Field(Flags = FieldFlags.Index)]
        public long UserID;

		/// <summary>The user agent</summary>
		[StringFormat(StringEncoding.ASCII)]
        [Field(Length = 255)]
        public string UserAgent;

        /// <summary>The source</summary>
        [StringFormat(StringEncoding.ASCII)]
        [Field(Length = 45)]
        public string Source;

        /// <summary>The expiration datetime</summary>
        [DateTimeFormat(DateTimeKind.Utc, DateTimeType.Native)]
        [Field]
        public DateTime Expiration;

		/// <summary>
		/// Provides additional flags for the session
		/// </summary>
		[Field]
		public UserSessionFlags Flags;

		/// <summary>Gets a value indicating whether this instance is expired.</summary>
		/// <value>
		/// <c>true</c> if this instance is expired; otherwise, <c>false</c>.
		/// </value>
		public bool IsExpired()
		{
			return DateTime.UtcNow > Expiration;
		}

		/// <summary>Returns true if this instance is valid.</summary>
		/// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
		public bool IsValid()
		{
			return Source != null & UserAgent != null && !IsExpired();
		}

		/// <summary>Gets a value indicating whether this instance is authenticated.</summary>
		/// <value>
		/// <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
		/// </value>
		public bool IsAuthenticated()
		{
			return UserID > 0 && IsValid();
		}

		/// <summary>
		/// Obtains a string describing this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("UserSession");
            builder.AppendFormat(" {0}", StringID);
			if (IsAuthenticated())
			{
				builder.Append(" authenticated");
			}
			else
			{
				builder.Append(" not authenticated");
			}
			return builder.ToString();
        }
    }
}
