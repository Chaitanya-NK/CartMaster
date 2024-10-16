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

        public async Task SendPasswordResetEmailAsync(string toEmail, string token)
        {
            string subject = "Password Reset Request";
            string body = $"Please reset your password by clicking the following link: http://localhost:4200/reset-password?token={token}";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(_smtpSettings.From, _smtpSettings.Password),
                    EnableSsl = true,
                    Timeout = 30000
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.From),
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

        public async Task SendEmail(string fromEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
                {
                    Credentials = new NetworkCredential(fromEmail, _smtpSettings.Password),
                    EnableSsl = true,
                    Timeout = 30000
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpSettings.Username),
                    ReplyToList = { new MailAddress(fromEmail) },
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add("chaitanya.nk2002@gmail.com");

                await smtpClient.SendMailAsync(mailMessage);
                Console.WriteLine("Email send successfully");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Faield: ", ex.Message);
                throw;
            }
        }
    }
}
