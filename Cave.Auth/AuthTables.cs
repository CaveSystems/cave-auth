using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cave.Collections.Generic;
using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides user validation tables and functions
    /// </summary>
    public class AuthTables
    {
        //TODO use better RNG implementation
        internal static readonly Random RNG = new();

        /// <summary>Initializes this instance.</summary>
        public AuthTables(IDatabase database, TableFlags flags)
        {
            Users = database.GetTable<long, User>(flags: flags);
            UserConfigurations = database.GetTable<long, UserConfiguration>(flags: flags);
            UserDetails = database.GetTable<long, UserDetail>(flags: flags);
            UserSessions = database.GetTable<long, UserSession>(flags: flags);
            UserSessionData = database.GetTable<long, UserSessionData>(flags: flags);
            UserSessionLicenses = database.GetTable<long, UserSessionLicense>(flags: flags);
            Licenses = database.GetTable<long, License>(flags: flags);
            Groups = database.GetTable<long, Group>(flags: flags);
            GroupMembers = database.GetTable<long, GroupMember>(flags: flags);
            PhoneNumbers = database.GetTable<long, PhoneNumber>(flags: flags);
            EmailAddresses = database.GetTable<long, EmailAddress>(flags: flags);
            Addresses = database.GetTable<long, Address>(flags: flags);
            Countries = database.GetTable<long, Country>(flags: flags);
            Software = database.GetTable<long, Software>(flags: flags);
            SoftwareSessions = database.GetTable<long, SoftwareSession>(flags: flags);
        }

        /// <summary>Accesses the users table.</summary>
        /// <value>The users.</value>
        public ITable<long, User> Users { get; }

        /// <summary>Accesses the users table.</summary>
        /// <value>The users.</value>
        public ITable<long, UserConfiguration> UserConfigurations { get; }

        /// <summary>Accesses the user details table.</summary>
        /// <value>The user details.</value>
        public ITable<long, UserDetail> UserDetails { get; }

        /// <summary>Accesses the user session table.</summary>
        /// <value>The user sessions.</value>
        public ITable<long, UserSession> UserSessions { get; }

        /// <summary>Accesses the user session data table.</summary>
        /// <value>The user session data.</value>
        public ITable<long, UserSessionData> UserSessionData { get; }

        /// <summary>Accesses the user session license table.</summary>
        /// <value>The user session licenses.</value>
        public ITable<long, UserSessionLicense> UserSessionLicenses { get; }

        /// <summary>Accesses the license table.</summary>
        /// <value>The licenses.</value>
        public ITable<long, License> Licenses { get; }

        /// <summary>Accesses the group table.</summary>
        /// <value>The groups.</value>
        public ITable<long, Group> Groups { get; }

        /// <summary>Accesses the group table.</summary>
        /// <value>The groups.</value>
        public ITable<long, GroupMember> GroupMembers { get; }

        /// <summary>Accesses the phone numbers table.</summary>
        /// <value>The phone numbers.</value>
        public ITable<long, PhoneNumber> PhoneNumbers { get; }

        /// <summary>Accesses the email addresses table.</summary>
        /// <value>The email addresses.</value>
        public ITable<long, EmailAddress> EmailAddresses { get; }

        /// <summary>Accesses the addresses table.</summary>
        /// <value>The addresses.</value>
        public ITable<long, Address> Addresses { get; }

        /// <summary>Accesses the countries table.</summary>
        /// <value>The countries.</value>
        public ITable<long, Country> Countries { get; }

        /// <summary>Accesses the countries table.</summary>
        /// <value>The countries.</value>
        public ITable<long, Software> Software { get; }

        /// <summary>Accesses the software session table.</summary>
        /// <value>The software sessions.</value>
        public ITable<long, SoftwareSession> SoftwareSessions { get; }

        /// <summary>
        /// Loads all .net cultures into the countries table
        /// </summary>
        public void LoadCultures()
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            if (Countries.RowCount != cultures.Length)
            {
                Countries.Clear();
                foreach (var culture in cultures)
                {
                    var name = string.IsNullOrEmpty(culture.Name) ? "-" : culture.Name;
                    Countries.Replace(new Country()
                    {
                        ID = (uint)culture.NativeName.GetHashCode(),
                        Code = (uint)culture.LCID,
                        ISO3 = culture.ThreeLetterISOLanguageName,
                        ISO2 = culture.TwoLetterISOLanguageName,
                        Name = name,
                        NativeName = culture.NativeName,
                        EnglishName = culture.EnglishName,
                    });
                }
            }
        }

        /// <summary>Gets all tables of this instance.</summary>
        /// <value>The tables.</value>
        public ITable[] Tables => new ITable[]
        {
            Users, Groups, GroupMembers, PhoneNumbers, EmailAddresses, Addresses, Countries, Software, SoftwareSessions,
            UserSessions, UserSessionData, UserSessionLicenses, Licenses, UserDetails, UserConfigurations,
        };


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
                AvatarID = (uint)((RNG.Next() << 1) ^ RNG.Next()),
                Color = 0xFF000000 | (uint)RNG.Next(0, 0xFFFFFF),
            };
            user.SetRandomSalt();
            user = Users.Insert(user);

            if (string.IsNullOrEmpty(emailAddress))
            {
                email = new EmailAddress();
            }
            else
            {
                var rndBytes = new byte[16];
                RNG.NextBytes(rndBytes);
                email = new EmailAddress() { UserID = user.ID, Address = emailAddress, VerificationCode = Base64.UrlChars.Encode(rndBytes), };
                email = EmailAddresses.Insert(email);
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
                throw new InvalidOperationException(string.Format("Email address {0} not unique, please contact support!", emailAddress));
            }
            if (addresses.Count == 1)
            {
                email = addresses[0];
                if (Users.TryGetStruct(email.UserID, out user))
                {
                    bool allowReset;
                    switch (user.State)
                    {
                        case UserState.Confirmed: allowReset = true; break;
                        case UserState.PasswordResetRequested:
                            allowReset = (DateTime.UtcNow > user.LastUpdate + TimeSpan.FromHours(1));
                            if (!allowReset)
                            {
                                throw new InvalidOperationException("Password reset email was already sent within the last hour.");
                            }
                            break;
                        default: throw new InvalidOperationException("Invalid user state!");
                    }
                    if (allowReset)
                    {
                        user.State = UserState.PasswordResetRequested;
                        user.LastUpdate = DateTime.UtcNow;
                        var rndBytes = new byte[16];
                        RNG.NextBytes(rndBytes);
                        email.VerificationCode = Base64.UrlChars.Encode(rndBytes);
                        Users.Update(user);
                        EmailAddresses.Update(email);
                        return;
                    }
                }
            }
            throw new InvalidOperationException("Invalid Emailaddress!");
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
                throw new InvalidOperationException("UserName or EmailAddress missing!");
            }
            user = new User()
            {
                State = state,
                NickName = userName,
                AuthLevel = level.ToInt32(CultureInfo.InvariantCulture),
                AvatarID = (uint)((RNG.Next() << 1) ^ RNG.Next()),
                Color = 0xFF000000 | (uint)RNG.Next(0, 0xFFFFFF),
            };
            user.SetRandomSalt();
            user.SetPassword(password);
            user = Users.Insert(user);
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
                email = EmailAddresses.Insert(email);
            }
        }

        /// <summary>Gets the user licenses.</summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public License[] GetUserLicenses(long userID) => Licenses.GetStructs(Search.FieldEquals(nameof(License.UserID), userID)).ToArray();

        /// <summary>Gets the group licenses.</summary>
        /// <param name="groupIDs">The group i ds.</param>
        /// <returns></returns>
        public License[] GetGroupLicenses(IEnumerable<long> groupIDs)
        {
            var search = Search.None;
            foreach (var groupID in groupIDs)
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
            if (user.ID <= 0)
            {
                throw new Exception("Invalid User.ID");
            }

            if (email.ID <= 0)
            {
                throw new Exception("Invalid Email.ID");
            }

            user.LastUpdate = DateTime.UtcNow;
            Users.Update(user);
            EmailAddresses.Update(email);
        }

        /// <summary>Tries the add a new email address to a user.</summary>
        /// <param name="email">The email address to add.</param>
        /// <returns></returns>
        public bool TryAddNewEmail(ref EmailAddress email)
        {
            var rows = EmailAddresses.GetStructs("Address", email.Address);
            if (rows.Count > 0) { return false; }
            email = EmailAddresses.Insert(email);
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
                if (!Users.TryGetStruct(emailAddress.UserID, out user) || user.ID == 0)
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
                        EmailAddresses.TryUpdate(email);
                    }
                    user.LastUpdate = DateTime.UtcNow;
                    user.InvalidLogonTries = 0;
                    Users.TryUpdate(user);
                    return true;
                }
                user.InvalidLogonTries++;
                user.LastUpdate = DateTime.UtcNow;
                Users.TryUpdate(user);
            }
            foreach (var userDataset in Users.GetStructs(nameof(User.NickName), login))
            {
                user = userDataset;
                if (user.TestPassword(password))
                {
                    email = EmailAddresses.GetStructs(
                        Search.FieldEquals(nameof(EmailAddress.UserID), user.ID) &
                        Search.FieldEquals(nameof(EmailAddress.Verified), true)).SingleOrDefault();
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
                    Users.TryUpdate(user);
                    return true;
                }
                user.InvalidLogonTries++;
                user.LastUpdate = DateTime.UtcNow;
                Users.TryUpdate(user);
            }
            user = default;
            email = default;
            return false;
        }

        /// <summary>Gets the group membership.</summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="groupID">The group identifier.</param>
        /// <returns></returns>
        public GroupMember GetGroupMembership(long userID, long groupID)
        {
            var members = GroupMembers.GetStructs(
                Search.FieldEquals(nameof(GroupMember.UserID), userID) &
                Search.FieldEquals(nameof(GroupMember.GroupID), groupID))
                .ToArray();

            if (members.Length == 1)
            {
                return members[0];
            }

            return default;
        }

        /// <summary>Finds a valid license.</summary>
        /// <param name="softwareID">The software identifier.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="license">The license.</param>
        /// <returns></returns>
        public bool FindLicense(long softwareID, long userID, out License license)
        {
            var lics = Licenses.GetStructs(nameof(License.SoftwareID), softwareID).ToArray();

            //find the first valid license the user is owner of
            foreach (var lic in lics)
            {
                if (lic.UserID == userID && lic.ValidTill > DateTime.UtcNow)
                {
                    license = lic;
                    return true;
                }
            }

            //check if the user is part of a group license
            var groupIDs = GetUserGroups(userID);
            foreach (var lic in lics)
            {
                if (lic.GroupID <= 0)
                {
                    continue;
                }

                if (groupIDs.Contains(lic.GroupID) && lic.ValidTill > DateTime.UtcNow)
                {
                    license = lic;
                    return true;
                }
            }

            license = default;
            return false;
        }

        /// <summary>Gets the user groups.</summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public Set<long> GetUserGroups(long userID)
        {
            var result = new Set<long>();
            result.IncludeRange(GroupMembers.GetStructs(Search.FieldEquals(nameof(GroupMember.UserID), userID)).Select(gm => gm.GroupID));
            return result;
        }

        /// <summary>Uses a license for the specified session.</summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="session">The user session.</param>
        /// <param name="license">The license.</param>
        /// <param name="activeSessionLicenses">The active session licenses.</param>
        /// <returns>Returns whether a free license could be acquired or false otherwise</returns>
        public bool UseLicense(long userID, UserSession session, License license, out IList<UserSessionLicense> activeSessionLicenses)
        {
            lock (UserSessionLicenses)
            {
                //get active sessions
                activeSessionLicenses = UserSessionLicenses.GetStructs(nameof(UserSessionLicense.LicenseID), license.ID);
                //check if users session has already an active license
                var myLicense = activeSessionLicenses.FirstOrDefault(s => s.UserSessionID == session.ID);
                if (myLicense.ID > 0)
                {
                    license = Licenses[myLicense.LicenseID];
                    myLicense.Expiration = session.Expiration;
                    UserSessionLicenses.Replace(myLicense);
                    return DateTime.UtcNow < license.ValidTill;
                }
                //check if number of sessions is already reached
                if (activeSessionLicenses.Count >= license.MaxSessions)
                {
                    return false;
                }
                //check if number of users is already reached
                if (activeSessionLicenses.Select(a => a.UserID).Distinct().Count() >= license.MaxUsers)
                {
                    return false;
                }
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
