using System;
using System.Net.Mail;
using Cave.IO;
using Cave.Mail;

namespace Cave.Auth
{
    /// <summary>
    /// Provides a mail sender for the auth system
    /// </summary>
    /// <seealso cref="MailSender" />
    public class AuthMailSender : MailSender
    {
        /// <summary>
        /// Gets or sets the welcome message sent to users after registration.
        /// </summary>
        public string WelcomeMessage { get; set; }

        /// <summary>
        /// Gets or sets the message sent to users after joining a group.
        /// </summary>
        public string JoinGroupMessage { get; set; }

        /// <inheritdoc/>
        public override void LoadConfig(IniReader settings)
        {
            base.LoadConfig(settings);
            try
            {
                if (settings == null)
                {
                    throw new ArgumentNullException(nameof(settings));
                }
                //load defaults
                var def = IniReader.Parse("AuthMessages.ini", Properties.Resources.AuthMessages);
                WelcomeMessage = def.ReadSection("MSG:Welcome", true).JoinNewLine();
                JoinGroupMessage = def.ReadSection("MSG:JoinGroup", true).JoinNewLine();
                //load from ini
                if (settings.HasSection("MSG:Welcome"))
                {
                    WelcomeMessage = settings.ReadSection("MSG:Welcome", true).JoinNewLine();
                }

                if (settings.HasSection("MSG:JoinGroup"))
                {
                    JoinGroupMessage = settings.ReadSection("MSG:JoinGroup", true).JoinNewLine();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error while loading configuration from {0}", settings), ex);
            }
        }

        /// <summary>Sends the authentication message.</summary>
        /// <param name="user">The user.</param>
        /// <param name="email">The email.</param>
        /// <returns>Returns true if the email was sent, false otherwise.</returns>
        public bool SendAuthMessage(User user, EmailAddress email)
        {
            var message = new MailMessage();
            message.To.Add(email.Address);
            message.Subject = "Registration";
            message.IsBodyHtml = true;
            message.Body = WelcomeMessage.
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
            var message = new MailMessage();
            message.To.Add(email);
            message.Subject = "Group Join";
            message.IsBodyHtml = true;
            message.Body = JoinGroupMessage.
                Replace("%GroupAdmin%", groupAdmin.NickName).
                Replace("%NickName%", user.NickName).
                Replace("%GroupName%", group.Name).
                Replace("%GroupID%", group.ID.ToString());
            return Send(message);
        }
    }
}
