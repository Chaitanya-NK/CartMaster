using CartMaster.Business.IServices;
using CartMaster.Data.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
#pragma warning disable 8604

namespace CartMaster.Business.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                    EnableSsl = true,
                    Timeout = 30000
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(address: _smtpSettings.From),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);
                Console.WriteLine($"Sending mail to {toEmail} from {_smtpSettings.From}");

                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email send successfully");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw;
            }
        }
    }
}
