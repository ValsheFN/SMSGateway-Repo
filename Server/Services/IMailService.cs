using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SMSGateway.Server.Services
{
    public interface IMailService
    {
        String SendEmailAsync(string From, string SendTo, string Subject, string Content);
    }

    public class SendMailService : IMailService
    {
        private IConfiguration _configuration;

        public SendMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public String SendEmailAsync(string SendTo, string From ,string Subject, string Content)
        {
            //Gmail Service
            /*var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential(_configuration["ConfirmationEmailAddress"], _configuration["ConfirmationAppKey"]),
                EnableSsl = true
            });*/

            //SMTP Server
            var sender = new SmtpSender(() => new SmtpClient("mail.teralinktec.com")
            {
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential(_configuration["ConfirmationEmailAddress"], _configuration["ConfirmationAppKey"]),
                EnableSsl = true
            });

            Email.DefaultSender = sender;
            var email = Email
                .From(_configuration["ConfirmationEmailAddress"], From)
                .To(SendTo)
                .Subject(Subject)
                .Body(Content, true);

            email.SendAsync().ConfigureAwait(false);
            return "Success";

            /*MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            message.From = new MailAddress(_configuration["ConfirmationEmailAddress"]);
            message.To.Add(new MailAddress(SendTo));
            message.Subject = Subject;
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = Content;
            smtp.Port = 587;
            smtp.Host = "mail.teralinktec.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_configuration["ConfirmationEmailAddress"], _configuration["ConfirmationAppKey"]);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(message).ConfigureAwait(false);

            return "Success";*/
        }
    }
}
