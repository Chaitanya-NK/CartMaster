using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.Models
{
    public class CouponModel
    {
        public int CouponID { get; set; }
        public string? CouponName { get; set; }
        public string? CouponDescription { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsValid { get; set; }
        public int DiscountPercentage { get; set; }
    }

    public class ApplyCouponModel
    {
        public int OrderID { get; set; }
        public string? CouponName { get; set; }
        public int UserID { get; set; }
    }
}
