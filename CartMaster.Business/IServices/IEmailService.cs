using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail,  string subject, string body);
        Task SendPasswordResetEmailAsync(string toEmail, string token);
        Task SendEmail(string fromEmail, string subject, string body);
    }
}
