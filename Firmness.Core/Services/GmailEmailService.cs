using Microsoft.Extensions.Configuration;
using System; // Added for InvalidOperationException
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Firmness.Core.Services
{
    public class GmailEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public GmailEmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            // Retrieve settings, ensuring they are not null
            string? smtpServer = emailSettings["SmtpServer"];
            string? smtpPortString = emailSettings["SmtpPort"];
            string? senderEmail = emailSettings["SenderEmail"];
            string? senderPassword = emailSettings["SenderPassword"];
            string? senderName = emailSettings["SenderName"];

            // Basic validation and null checks
            if (string.IsNullOrEmpty(smtpServer) ||
                string.IsNullOrEmpty(smtpPortString) ||
                string.IsNullOrEmpty(senderEmail) ||
                string.IsNullOrEmpty(senderPassword) ||
                string.IsNullOrEmpty(senderName))
            {
                throw new InvalidOperationException("One or more email settings are missing or empty in appsettings.json. Please check SmtpServer, SmtpPort, SenderEmail, SenderPassword, and SenderName.");
            }

            int smtpPort = int.Parse(smtpPortString); // Now safe to parse

            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName), // Now safe to use
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
