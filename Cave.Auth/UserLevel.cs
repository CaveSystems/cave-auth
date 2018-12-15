using System;
using System.Collections.Generic;
using System.Globalization;
using Cave.Data;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a structure to transmit user levels
    /// </summary>
    [Table("UserLevels")]
    public struct UserLevel
    {
        /// <summary>Creates a list of user level from the specified enum values</summary>
        /// <param name="enumType">Type of the enum.</param>
        /// <returns></returns>
        public static IList<UserLevel> FromEnum(Type enumType)
        {
            List<UserLevel> list = new List<UserLevel>();
            foreach (IConvertible level in Enum.GetValues(enumType))
            {
                list.Add(new UserLevel() { Name = level.ToString(), ID = level.ToInt64(CultureInfo.InvariantCulture) });
            }
            return list;
        }

        /// <summary>The identifier</summary>
        [Field(Flags = FieldFlags.ID)]
        public long ID;

        /// <summary>The name</summary>
        [Field]
        public string Name;
    }
}
