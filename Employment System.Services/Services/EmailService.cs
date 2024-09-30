using Employment_System.Domain.IServices;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employment_System.Services.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Mohamed Esmail", "m7med.esmail22@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email)); // Optionally provide a name
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();

            // Connect to the SMTP server
            await smtp.ConnectAsync("smtp.gmail.com", 587, false);

            // Authenticate using your email credentials
            await smtp.AuthenticateAsync("m7med.esmail22@gmail.com", "inva pohs yrnb kuvt");

            // Send the email asynchronously
            await smtp.SendAsync(emailMessage);

            // Disconnect from the SMTP server
            await smtp.DisconnectAsync(true);
        }

    }
}
