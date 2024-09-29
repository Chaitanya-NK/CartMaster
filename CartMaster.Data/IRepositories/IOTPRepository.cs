using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.IRepositories
{
    public interface IOTPRepository
    {
        Task SaveOTPAsync(int userId, string otpCode);
        bool VerifyOTP(int userId, string otpCode);
    }
}
