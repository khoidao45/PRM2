using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EVStation_basedRentalSystem.Services.AuthAPI.Service
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task SendActivationEmail(string toEmail, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(toEmail)) throw new ArgumentException("Recipient email cannot be empty.", nameof(toEmail));

            var emailSettings = _config.GetSection("EmailSettings");
            string smtpHost = emailSettings["SmtpServer"];
            int smtpPort = int.Parse(emailSettings["Port"]);
            string smtpUser = emailSettings["Username"];
            string smtpPass = emailSettings["Password"];
            string fromEmail = emailSettings["SenderEmail"];
            string fromName = emailSettings["SenderName"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
