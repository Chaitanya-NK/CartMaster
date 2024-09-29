using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.Models
{
    public class OTPVerificationRequestModel
    {
        public int UserID { get; set; }
        public string? OTPCode { get; set; }
    }
}
