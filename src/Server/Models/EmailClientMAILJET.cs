using System;
using System.Collections.Generic;
using System.Linq;
using Zalandu.Server.Models.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;
using System.IO;

namespace Zalandu.Server.Models
{
    public class EmailClientMAILJET : IEmailClient
    {
        private readonly IOptions<EmailServerMAILJET> __configServerMAILJET;

        public EmailClientMAILJET(IOptions<EmailServerMAILJET> objConfigServerMAILJET)
        {
            this.__configServerMAILJET = objConfigServerMAILJET;
        }

#nullable enable
        public void SendEmail(string toEmailClient, string subject, string body, string? attachedName)
        {
            String _email           = "myemail@mydomain.com"

            SmtpClient _clientSMTP  = new SmtpClient();
            _clientSMTP.Host        = this.__configServerMAILJET.Value.ServerName;
            _clientSMTP.Credentials = new NetworkCredential(this.__configServerMAILJET.Value.APIKey, this.__configServerMAILJET.Value.SecretKey);

            MailMessage _messageToSend = new MailMessage(_email, toEmailClient);
            _messageToSend.Subject     = subject;
            _messageToSend.IsBodyHtml  = true;
            _messageToSend.Body        = body;

            if (!String.IsNullOrEmpty(attachedName))
            {
                FileStream _fileContent = new FileStream(attachedName, FileMode.Open, FileAccess.Read);

                _messageToSend.Attachments.Add(new Attachment(_fileContent, attachedName, "application/pdf"));
            }
            _clientSMTP.SendAsync(_messageToSend, null);
        }
        #nullable disable
    }
}
