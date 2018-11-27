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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Cave.Collections.Generic;
using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides user validation tables and functions
    /// </summary>
    public class AuthTables : TableConnector
	{
        //TODO use better RNG implementation
        internal static readonly Random RNG = new Random();

        #region Initialization, private implementation and checks
        private ITable<User> m_Users;
        private ITable<UserDetail> m_UserDetails;
        private ITable<UserSession> m_UserSessions;
		private ITable<UserSessionData> m_UserSessionData;
		private ITable<UserSessionLicense> m_UserSessionLicenses;
        private ITable<License> m_Licenses;
        private ITable<Group> m_Groups;
        private ITable<GroupMember> m_GroupMembers;
        private ITable<PhoneNumber> m_PhoneNumbers;
        private ITable<EmailAddress> m_EmailAddresses;
        private ITable<Address> m_Addresses;
        private ITable<Country> m_Countries;
        private ITable<Software> m_Software;
        private ITable<UserConfiguration> m_UserConfigurations;
        private ITable<SoftwareSession> m_SoftwareSessions;

        /// <summary>Accesses the users table.</summary>
        /// <value>The users.</value>
        public ITable<User> Users
        {
            get
            {
                if (m_Users == null) ConnectTable(ref m_Users);
                return m_Users;
            }
        }

        /// <summary>Accesses the users table.</summary>
        /// <value>The users.</value>
        public ITable<UserConfiguration> UserConfigurations
        {
            get
            {
                if (m_UserConfigurations == null) ConnectTable(ref m_UserConfigurations);
                return m_UserConfigurations;
            }
        }

        /// <summary>Accesses the user details table.</summary>
        /// <value>The user details.</value>
        public ITable<UserDetail> UserDetails
        {
            get
            {
                if (m_UserDetails == null) ConnectTable(ref m_UserDetails);
                return m_UserDetails;
            }
        }

        /// <summary>Accesses the user session table.</summary>
        /// <value>The user sessions.</value>
        public ITable<UserSession> UserSessions
        {
            get
            {
                if (m_UserSessions == null) ConnectTable(ref m_UserSessions);
                return m_UserSessions;
            }
        }

		/// <summary>Accesses the user session data table.</summary>
		/// <value>The user session data.</value>
		public ITable<UserSessionData> UserSessionData
		{
			get
			{
				if (m_UserSessionData == null) ConnectTable(ref m_UserSessionData);
				return m_UserSessionData;
			}
		}

		/// <summary>Accesses the user session license table.</summary>
		/// <value>The user session licenses.</value>
		public ITable<UserSessionLicense> UserSessionLicenses
        {
            get
            {
                if (m_UserSessionLicenses == null) ConnectTable(ref m_UserSessionLicenses);
                return m_UserSessionLicenses;
            }
        }

        /// <summary>Accesses the license table.</summary>
        /// <value>The licenses.</value>
        public ITable<License> Licenses
        {
            get
            {
                if (m_Licenses == null) ConnectTable(ref m_Licenses);
                return m_Licenses;
            }
        }

        /// <summary>Accesses the group table.</summary>
        /// <value>The groups.</value>
        public ITable<Group> Groups
        {
            get
            {
                if (m_Groups == null) ConnectTable(ref m_Groups);
                return m_Groups;
            }
        }

        /// <summary>Accesses the group table.</summary>
        /// <value>The groups.</value>
        public ITable<GroupMember> GroupMembers
        {
            get
            {
                if (m_GroupMembers == null) ConnectTable(ref m_GroupMembers);
                return m_GroupMembers;
            }
        }

        /// <summary>Accesses the phone numbers table.</summary>
        /// <value>The phone numbers.</value>
        public ITable<PhoneNumber> PhoneNumbers
        {
            get
            {
                if (m_PhoneNumbers == null) ConnectTable(ref m_PhoneNumbers);
                return m_PhoneNumbers;
            }
        }

        /// <summary>Accesses the email addresses table.</summary>
        /// <value>The email addresses.</value>
        public ITable<EmailAddress> EmailAddresses
        {
            get
            {
                if (m_EmailAddresses == null) ConnectTable(ref m_EmailAddresses);
                return m_EmailAddresses;
            }
        }

        /// <summary>Accesses the addresses table.</summary>
        /// <value>The addresses.</value>
        public ITable<Address> Addresses
        {
            get
            {
                if (m_Addresses == null) ConnectTable(ref m_Addresses);
                return m_Addresses;
            }
        }

        /// <summary>Accesses the countries table.</summary>
        /// <value>The countries.</value>
        public ITable<Country> Countries
        {
            get
            {
				if (m_Countries == null) ConnectTable(ref m_Countries);
                return m_Countries;
            }
        }

        /// <summary>Accesses the countries table.</summary>
        /// <value>The countries.</value>
        public ITable<Software> Software
        {
            get
            {
                if (m_Software == null) ConnectTable(ref m_Software);
                return m_Software;
            }
        }

        /// <summary>Accesses the software session table.</summary>
        /// <value>The software sessions.</value>
        public ITable<SoftwareSession> SoftwareSessions
        {
            get
            {
                if (m_SoftwareSessions == null) ConnectTable(ref m_SoftwareSessions);
                return m_SoftwareSessions;
            }
        }

        /// <summary>Gets the name of the log source.</summary>
        /// <value>The name of the log source.</value>
        public override string LogSourceName { get { return "AuthTables"; } }

		/// <summary>
		/// Loads all .net cultures into the countries table
		/// </summary>
		public void LoadCultures()
		{
			var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
			if (m_Countries.RowCount != cultures.Length)
			{
				m_Countries.Clear();
				foreach (CultureInfo c in cultures)
				{
					string name = string.IsNullOrEmpty(c.Name) ? "-" : c.Name;
					m_Countries.Replace(new Country()
					{
						ID = CaveSystemData.CalculateID(name),
						Code = (uint)c.LCID,
						ISO3 = c.ThreeLetterISOLanguageName,
						ISO2 = c.TwoLetterISOLanguageName,
						Name = c.Name,
						NativeName = c.NativeName,
						EnglishName = c.EnglishName,
					});
				}
			}
		}

        /// <summary>Gets all tables of this instance.</summary>
        /// <value>The tables.</value>
        public override ITable[] Tables
        {
            get
            {
                return new ITable[]
                {
                    Users, Groups, GroupMembers, PhoneNumbers, EmailAddresses, Addresses, Countries, Software, SoftwareSessions,
                    UserSessions, UserSessionData, UserSessionLicenses, Licenses, UserDetails, UserConfigurations,
                };
            }
        }

        /// <summary>Initializes this instance.</summary>
        public AuthTables()
        {
        }

        #endregion        

		/// <summary>Creates a new user without password (unconfirmed user).</summary>
		/// <param name="userName">(Nick)Name of the user to create</param>
		/// <param name="emailAddress">The email address.</param>
		/// <param name="email">Returns the email dataset.</param>
		/// <param name="user">Returns the user dataset.</param>
		/// <exception cref="ArgumentException">
		/// UserName or EmailAddress missing!
		/// or
		/// Invalid characters at user name!
		/// or
		/// Invalid characters at email address!
		/// or
		/// Username/Email address is already registered!
		/// </exception>
		/// <exception cref="System.ArgumentNullException">UserName</exception>
		public void CreateUser(string userName, string emailAddress, out EmailAddress email, out User user)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(emailAddress))
            {
                throw new ArgumentException("UserName or EmailAddress missing!");
            }
            if (userName.HasInvalidChars(ASCII.Strings.SafeName))
            {
				throw new ArgumentException("Invalid characters at user name!");
            }
            if (emailAddress.HasInvalidChars(ASCII.Strings.SafeName))
            {
				throw new ArgumentException("Invalid characters at email address!");
            }
            if (EmailAddresses.Exist(Search.FieldLike(nameof(EmailAddress.Address), emailAddress)))
            {
				throw new ArgumentException("Username/Email address is already registered!");
            }
			user = new User()
			{
				State = UserState.New,
				NickName = userName,
				AvatarID = (uint)(RNG.Next() << 1 ^ RNG.Next()),
				Color = 0xFF000000 | (uint)RNG.Next(0, 0xFFFFFF),
			};
            user.SetRandomSalt();
            user.ID = Users.Insert(user);
			
            if (string.IsNullOrEmpty(emailAddress))
            {
                email = new EmailAddress();
            }
            else
            {
                byte[] rndBytes = new byte[16];
                RNG.NextBytes(rndBytes);
                email = new EmailAddress() { UserID = user.ID, Address = emailAddress, VerificationCode = Base64.UrlChars.Encode(rndBytes), };
                email.ID = EmailAddresses.Insert(email);
            }
        }

		/// <summary>Resets the password.</summary>
		/// <param name="emailAddress">The email address.</param>
		/// <param name="email">The email.</param>
		/// <param name="user">The user.</param>
		/// <exception cref="ArgumentException">
		/// Email address {0} not unique, please contact support!
		/// or
		/// Password reset email was already sent within the last hour.
		/// or
		/// Invalid Emailaddress!
		/// </exception>
		public void RequestPasswordReset(string emailAddress, out EmailAddress email, out User user)
        {
            var addresses = EmailAddresses.GetStructs(nameof(EmailAddress.Address), emailAddress);
            if (addresses.Count > 1)
            {
				throw new ArgumentException("Email address {0} not unique, please contact support!", emailAddress);
            }
            if (addresses.Count == 1)
            {
                email = addresses[0];
                if (Users.TryGetStruct(email.UserID, out user))
                {
                    bool allowReset = false;
                    switch (user.State)
                    {
                        case UserState.Confirmed: allowReset = true; break;
                        case UserState.PasswordResetRequested:
                            allowReset = (DateTime.UtcNow > user.LastUpdate + TimeSpan.FromHours(1));
                            if (!allowReset)
                            {
								throw new ArgumentException("Password reset email was already sent within the last hour.");
                            }
                            break;
						default: throw new ArgumentNullException("Invalid user state!");
                    }
                    if (allowReset)
                    {
                        user.State = UserState.PasswordResetRequested;
                        user.LastUpdate = DateTime.UtcNow;
                        byte[] rndBytes = new byte[16];
                        RNG.NextBytes(rndBytes);
                        email.VerificationCode = Base64.UrlChars.Encode(rndBytes);
                        Users.Update(user);
                        EmailAddresses.Update(email);
                        return;
                    }
                }
            }
			throw new ArgumentException("Invalid Emailaddress!");
        }

		/// <summary>Creates a new user with the given password.</summary>
		/// <param name="userName">(Nick)Name of the user to create</param>
		/// <param name="emailAddress">The email address.</param>
		/// <param name="password">The password.</param>
		/// <param name="state">State of the user.</param>
		/// <param name="level">The level.</param>
		/// <param name="user">The user.</param>
		/// <param name="email">The email.</param>
		/// <exception cref="ArgumentException">UserName or EmailAddress missing!</exception>
		/// <remarks>Users with state <see cref="UserState.Confirmed" /> automatically gets confirmed.</remarks>
		public void CreateUser(string userName, string emailAddress, string password, UserState state, IConvertible level, out User user, out EmailAddress email)
        {
            if (string.IsNullOrEmpty(userName))
            {
				throw new ArgumentException("UserName or EmailAddress missing!");
            }
			user = new User()
			{
				State = state,
				NickName = userName,
				AuthLevel = level.ToInt32(CultureInfo.InvariantCulture),
                AvatarID = (uint)(RNG.Next() << 1 ^ RNG.Next()),
                Color = 0xFF000000 | (uint)RNG.Next(0, 0xFFFFFF),
            };
            user.SetRandomSalt();
            user.SetPassword(password);
            user.ID = Users.Insert(user);
            if (string.IsNullOrEmpty(emailAddress))
            {
                email = new EmailAddress();
            }
            else
            {
                email = new EmailAddress()
                {
                    UserID = user.ID,
                    Address = emailAddress, 
                    Verified = state == UserState.Confirmed,
                };
                email.ID = EmailAddresses.Insert(email);
            }
        }

        /// <summary>Gets the user licenses.</summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public License[] GetUserLicenses(long userID)
        {
            return Licenses.GetStructs(Search.FieldEquals(nameof(License.UserID), userID)).ToArray();
        }

        /// <summary>Gets the group licenses.</summary>
        /// <param name="groupIDs">The group i ds.</param>
        /// <returns></returns>
        public License[] GetGroupLicenses(IEnumerable<long> groupIDs)
        {
            Search search = Search.None;
            foreach (long groupID in groupIDs)
            {
                search |= Search.FieldEquals(nameof(License.GroupID), groupID);
            }
            return Licenses.GetStructs(search).ToArray();
        }

        /// <summary>Updates the user and email dataset.</summary>
        /// <param name="user">The user dataset.</param>
        /// <param name="email">The email address.</param>
        public void Update(ref User user, ref EmailAddress email)
        {
            if (user.ID <= 0) throw new Exception("Invalid User.ID");
            if (email.ID <= 0) throw new Exception("Invalid Email.ID");
            user.LastUpdate = DateTime.UtcNow;
            Users.Update(user);
            EmailAddresses.Update(email);
        }

        /// <summary>Tries the add a new email address to a user.</summary>
        /// <param name="email">The email address to add.</param>
        /// <returns></returns>
        public bool TryAddNewEmail(ref EmailAddress email)
        {
            long id = EmailAddresses.FindRow("Address", email.Address);
            if (id > 0) { return false; }
            email.ID = EmailAddresses.Insert(email);
            return true;
        }
		
		/// <summary>Performs a login checking the password.</summary>
		/// <param name="login">The login.</param>
		/// <param name="password">The password.</param>
		/// <param name="user">The user.</param>
		/// <param name="email">The email.</param>
		/// <returns></returns>
		public bool Login(string login, string password, out User user, out EmailAddress email)
		{
			var emailAddresses = EmailAddresses.GetStructs(
				Search.FieldEquals(nameof(EmailAddress.Address), login) &
				Search.FieldEquals(nameof(EmailAddress.Verified), true));
			foreach (var emailAddress in emailAddresses)
			{
				user = Users.TryGetStruct(emailAddress.UserID);
				if (user.ID == 0)
				{
					EmailAddresses.Delete(emailAddress.ID);
					continue;
				}
				if (user.TestPassword(password))
				{
					email = emailAddress;
					switch (user.State)
					{
						case UserState.PasswordResetRequested: user.State = UserState.Confirmed; break;
						case UserState.Confirmed: break;
						case UserState.New: throw new ArgumentException("Account needs to be verified prior usage!");
						case UserState.Diabled: throw new ArgumentException("Account is disabled. Please contact support.");
						case UserState.Deleted: throw new ArgumentException("Invalid Account.");
						default: throw new ArgumentException("Unknown user state!");
					}
					if (email.VerificationCode != null)
					{
						email.VerificationCode = null;
						Task.Factory.StartNew((e) => EmailAddresses.TryUpdate((EmailAddress)e), email);
					}
					user.LastUpdate = DateTime.UtcNow;
					user.InvalidLogonTries = 0;
					Task.Factory.StartNew((u) => Users.TryUpdate((User)u), user);
					return true;
				}
				user.InvalidLogonTries++;
				user.LastUpdate = DateTime.UtcNow;
				if (Users is IMemoryTable)
				{
					Users.TryUpdate(user);
				}
				else
				{
					Task.Factory.StartNew((u) => Users.TryUpdate((User)u), user);
				}
			}
			foreach (var userDataset in Users.GetStructs(nameof(User.NickName), login))
			{
				user = userDataset;
				if (user.TestPassword(password))
				{
					email = EmailAddresses.TryGetStruct(
						Search.FieldEquals(nameof(EmailAddress.UserID), user.ID) &
						Search.FieldEquals(nameof(EmailAddress.Verified), true));
					switch (user.State)
					{
						case UserState.PasswordResetRequested: user.State = UserState.Confirmed; break;
						case UserState.Confirmed: break;
						case UserState.New: throw new ArgumentException("Account needs to be verified prior usage!");
						case UserState.Diabled: throw new ArgumentException("Account is disabled. Please contact support.");
						case UserState.Deleted: throw new ArgumentException("Invalid Account.");
						default: throw new ArgumentException("Unknown user state!");
					}
					user.InvalidLogonTries = 0;
					user.LastUpdate = DateTime.UtcNow;
					if (Users is IMemoryTable)
					{
						Users.TryUpdate(user);
					}
					else
					{
						Task.Factory.StartNew((u) => Users.TryUpdate((User)u), user);
					}
					return true;
				}
				user.InvalidLogonTries++;
				user.LastUpdate = DateTime.UtcNow;
				if (Users is IMemoryTable)
				{
					Users.TryUpdate(user);
				}
				else
				{
					Task.Factory.StartNew((u) => Users.TryUpdate((User)u), user);
				}
			}
			user = default(User);
			email = default(EmailAddress);
			return false;
		}

        /// <summary>Gets the group membership.</summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="groupID">The group identifier.</param>
        /// <returns></returns>
        public GroupMember GetGroupMembership(long userID, long groupID)
        {
            GroupMember[] members = GroupMembers.GetStructs(
                Search.FieldEquals(nameof(GroupMember.UserID), userID) &
                Search.FieldEquals(nameof(GroupMember.GroupID), groupID))
                .ToArray();

            if (members.Length == 1) return members[0];
            return default(GroupMember);
        }

        /// <summary>Finds a valid license.</summary>
        /// <param name="softwareID">The software identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="license">The license.</param>
        /// <returns></returns>
        public bool FindLicense(long softwareID, long userID, out License license)
        {
            License[] lics = Licenses.GetStructs(nameof(License.SoftwareID), softwareID).ToArray();

            //find the first valid license the user is owner of
            foreach (License lic in lics)
            {
                if (lic.UserID == userID && lic.ValidTill > DateTime.UtcNow)
                {
                    license = lic;
                    return true;
                }
            }

            //check if the user is part of a group license
            Set<long> groupIDs = GetUserGroups(userID);
            foreach (License lic in lics)
            {
                if (lic.GroupID <= 0) continue;
                if (groupIDs.Contains(lic.GroupID) && lic.ValidTill > DateTime.UtcNow)
                {
                    license = lic;
                    return true;
                }
            }

            license = default(License);
            return false;
        }

        /// <summary>Gets the user groups.</summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public Set<long> GetUserGroups(long userID)
        {
            Set<long> result = new Set<long>();
            result.IncludeRange(GroupMembers.GetStructs(Search.FieldEquals(nameof(GroupMember.UserID), userID)).Select(gm => gm.GroupID));
            return result;
        }

        /// <summary>Uses a license for the specified session.</summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="session">The user session.</param>
        /// <param name="license">The license.</param>
        /// <param name="activeSessionLicenses">The active session licenses.</param>
        /// <returns>Returns whether a free license could be acquired or false otherwise</returns>
        public bool UseLicense(long userID, UserSession session, License license, out List<UserSessionLicense> activeSessionLicenses)
        {
            lock (UserSessionLicenses)
            {
                //get active sessions
                activeSessionLicenses = UserSessionLicenses.GetStructs(nameof(UserSessionLicense.LicenseID), license.ID);
                //check if users session has already an active license
                UserSessionLicense myLicense = activeSessionLicenses.FirstOrDefault(s => s.UserSessionID == session.ID);
                if (myLicense.ID > 0)
                {
                    license = Licenses[myLicense.LicenseID];
                    myLicense.Expiration = session.Expiration;
                    UserSessionLicenses.Replace(myLicense);
                    return DateTime.UtcNow < license.ValidTill;
                }
                //check if number of sessions is already reached
                if (activeSessionLicenses.Count >= license.MaxSessions) return false;
                //check if number of users is already reached
                if (activeSessionLicenses.Select(a => a.UserID).Distinct().Count() >= license.MaxUsers) return false;
                //create license session
                UserSessionLicenses.Insert(new UserSessionLicense()
                {
                    UserID = userID,
                    LicenseID = license.ID,
                    UserSessionID = session.ID,
                    Expiration = session.Expiration,
                });
                return true;
            }
        }
    }
}