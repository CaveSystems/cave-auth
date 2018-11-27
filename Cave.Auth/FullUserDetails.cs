﻿#region CopyRight 2018
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

using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides full details for the current user
    /// </summary>
    public class FullUserDetails
    {
        /// <summary>The user</summary>
        public User User;

        /// <summary>The user detail</summary>
        public UserDetail UserDetail;

        /// <summary>The licenses</summary>
        public ITable<License> Licenses;

        /// <summary>The groups</summary>
        public ITable<Group> Groups;

        /// <summary>The group memberships</summary>
        public ITable<GroupMember> GroupMembers;

        /// <summary>The phone numbers</summary>
        public ITable<PhoneNumber> PhoneNumbers;

        /// <summary>The email addresses</summary>
        public ITable<EmailAddress> EmailAddresses;

        /// <summary>The addresses</summary>
        public ITable<Address> Addresses;
    }
}