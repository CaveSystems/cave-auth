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
