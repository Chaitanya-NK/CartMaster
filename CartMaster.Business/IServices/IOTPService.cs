using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.IServices
{
    public interface IOTPService
    {
        Task SendOTP(int userId, string email);
        bool VerifyOTP(int userId, string otpCode);
    }
}
