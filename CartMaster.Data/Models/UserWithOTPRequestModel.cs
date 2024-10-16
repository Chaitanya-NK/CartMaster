using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.Models
{
    public class UserWithOTPRequestModel
    {
        public UserModel? UserModel {  get; set; }
        public OTPVerificationRequestModel? OTPModel { get; set; }
    }
}
