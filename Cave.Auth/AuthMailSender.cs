using System;
using System.Net.Mail;
using Cave.Mail;

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
                if (settings == null)
                {
                    throw new ArgumentNullException(nameof(settings));
                }
                //load defaults
                IniReader def = IniReader.Parse("AuthMessages.ini", Properties.Resources.AuthMessages);
                Welcome = def.ReadSection("MSG:Welcome", true).JoinNewLine();
                JoinGroup = def.ReadSection("MSG:JoinGroup", true).JoinNewLine();
                //load from ini
                if (settings.HasSection("MSG:Welcome"))
                {
                    Welcome = settings.ReadSection("MSG:Welcome", true).JoinNewLine();
                }

                if (settings.HasSection("MSG:JoinGroup"))
                {
                    JoinGroup = settings.ReadSection("MSG:JoinGroup", true).JoinNewLine();
                }
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
