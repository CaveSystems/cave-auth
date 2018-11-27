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

namespace Cave.Auth
{
    /// <summary>
    /// Provides a country dataset
    /// </summary>
    [Table("Countries")]
    public struct Country
    {
        /// <summary>Implements the operator ==.</summary>
        /// <param name="country1">The country1.</param>
        /// <param name="country2">The country2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Country country1, Country country2)
        {
			return country1.ID == country2.ID;
        }

        /// <summary>Implements the operator !=.</summary>
        /// <param name="country1">The country1.</param>
        /// <param name="country2">The country2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Country country1, Country country2)
        {
			return country1.ID != country2.ID;
        }

		/// <summary>The identifier</summary>
		[Field(Flags = FieldFlags.AutoIncrement | FieldFlags.ID)]
        public long ID;

		/// <summary>The international country code</summary>
		[Field]
		public uint Code;

		/// <summary>Country name in the format languagecode2-country/regioncode2.</summary>
		[StringFormat(StringEncoding.UTF8)]
		[Field(Length = 64)]
		public string Name;

		/// <summary>Country name, consisting of the language, the country/region, and the optional script, that the culture is set to display.</summary>
		[StringFormat(StringEncoding.UTF8)]
        [Field(Length = 64)]
        public string NativeName;

		/// <summary>The english name of the country</summary>
		[StringFormat(StringEncoding.UTF8)]
		[Field(Length = 64)]
		public string EnglishName;

		/// <summary>ISO 639-1 two-letter code for the language.</summary>
		[Field(Length = 2)]
        public string ISO2;

		/// <summary>ISO 639-2 three-letter code for the language</summary>
		[Field(Length = 3)]
        public string ISO3;

		/// <summary>
		/// Obtains a string describing this instance
		/// </summary>
		/// <returns></returns>
		public override string ToString()
        {
			return $"Country {Name}";
		}

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
            if (obj is Country) return base.Equals((Country)obj);
            return false;
        }

        /// <summary>Determines whether the specified <see cref="Country" />, is equal to this instance.</summary>
        /// <param name="other">The <see cref="Country" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="Country" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Country other)
        {
            return other.ID == ID;
        }
    }
}
