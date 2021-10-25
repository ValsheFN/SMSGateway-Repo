using FluentEmail.Core;
using FluentEmail.Smtp;
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
        Task<string> SendEmailAsync(string From, string SendTo, string Subject, string Content);
    }

    public class SendMailService : IMailService
    {
        private IConfiguration _configuration;

        public SendMailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> SendEmailAsync(string SendTo, string From ,string Subject, string Content)
        {
            //Grid Mail Service
            /*var apiKey = _configuration["SendGridAPIKey"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@authdemo.com", "JWT Auth Demo");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);*/

            //Gmail Service
            var sender = new SmtpSender(() => new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential(_configuration["ConfirmationEmailAddress"], _configuration["ConfirmationAppKey"]),
                EnableSsl = true,
            });

            Email.DefaultSender = sender;
            var email = Email
                .From(_configuration["ConfirmationEmailAddress"],From)
                .To(SendTo)
                .Subject(Subject)
                .Body(Content);

            var response = await email.SendAsync();
            if (response.Successful)
            {
                return "Success";
            }

            return response.ErrorMessages[0];
        }
    }
}
