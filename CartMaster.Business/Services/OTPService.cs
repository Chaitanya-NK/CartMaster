using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.Services
{
    public class OTPService : IOTPService
    {
        private readonly IOTPRepository _otpRepository;
        private readonly IEmailService _emailService;
        public OTPService(IOTPRepository otpRepository, IEmailService emailService)
        {
            _otpRepository = otpRepository;
            _emailService = emailService;
        }
        public async Task SendOTP(int userId, string email)
        {
            var otpCode = new Random().Next(100000, 999999).ToString();

            await _otpRepository.SaveOTPAsync(userId, otpCode);

            var subject = "OTP For Cart Master Website Registration";
            var body = $"Your OTP code is {otpCode}. It is valid upto 5 minutes.";
            try
            {
                await _emailService.SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending OTP to email: {email}. Exception: {ex.Message}");
            }
        }

        public bool VerifyOTP(int userId, string otpCode)
        {
            return _otpRepository.VerifyOTP(userId, otpCode);
        }
    }
}
