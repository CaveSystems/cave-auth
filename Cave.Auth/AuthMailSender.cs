#region CopyRight 2018
/*
    Copyright (c) 2016-2018 Andreas Rohleder (andreas@rohleder.cc)
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

using System;
using System.Net.Mail;
using Cave.IO;
using Cave.Mail;
using Cave.Text;

namespace Cave.Auth
{
	/// <summary>
	/// Provides a mail sender for the auth system
	/// </summary>
	/// <seealso cref="MailSender" />
	public class AuthMailSender : MailSender
	{
		string Welcome;
		string JoinGroup;

		/// <summary>Initializes a new instance of the <see cref="AuthMailSender"/> class.</summary>
		/// <param name="settings">The settings.</param>
		public AuthMailSender(ISettings settings) : base(settings)
		{
			try
			{
				if (settings == null) throw new ArgumentNullException(nameof(settings));
				//load defaults
				var def = IniReader.Parse("AuthMessages.ini", Properties.Resources.AuthMessages);
				Welcome = def.ReadSection("MSG:Welcome", true).JoinNewLine();
				JoinGroup = def.ReadSection("MSG:JoinGroup", true).JoinNewLine();
				//load from ini
				if (settings.HasSection("MSG:Welcome")) Welcome = settings.ReadSection("MSG:Welcome", true).JoinNewLine();
				if (settings.HasSection("MSG:JoinGroup")) JoinGroup = settings.ReadSection("MSG:JoinGroup", true).JoinNewLine();
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Error while loading configuration from {0}", settings.Name), ex);
			}
		}

		/// <summary>Sends the authentication message.</summary>
		/// <param name="user">The user.</param>
		/// <param name="email">The email.</param>
		/// <returns>Returns true if the email was sent, false otherwise.</returns>
		public bool SendAuthMessage(User user, EmailAddress email)
		{
			MailMessage message = new MailMessage();
			message.To.Add(email.Address);
			message.Subject = "Registration";
			message.IsBodyHtml = true;
			message.Body = Ini.LocalMachineIniFile.ReadSection("MSG::Welcome").JoinNewLine().
				Replace("%NickName%", user.NickName).
				Replace("%EmailAddress%", email.Address).
				Replace("%Code%", email.VerificationCode);
			return Send(message);
		}

		/// <summary>Sends the group invitation message.</summary>
		/// <param name="groupAdmin">The group admin.</param>
		/// <param name="email">The email.</param>
		/// <param name="user">The user.</param>
		/// <param name="group">The group.</param>
		/// <returns>Returns true if the email was sent, false otherwise.</returns>
		public bool SendGroupInvitationMessage(User groupAdmin, MailAddress email, User user, Group group)
		{
			MailMessage message = new MailMessage();
			message.To.Add(email);
			message.Subject = "Group Join";
			message.IsBodyHtml = true;
			message.Body = Ini.LocalMachineIniFile.ReadSection("MSG::JoinGroup").JoinNewLine().
				Replace("%GroupAdmin%", groupAdmin.NickName).
				Replace("%NickName%", user.NickName).
				Replace("%GroupName%", group.Name).
				Replace("%GroupID%", group.ID.ToString());
			return Send(message);
		}
	}
}
