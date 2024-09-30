using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.IRepositories
{
    public interface ICouponRepository
    {
        public List<CouponModel> GetAllCoupons();
        public List<CouponModel> GetValidCoupons();
        public string CreateCoupon(CouponModel couponModel);
        public string UpdateCoupon(CouponModel couponModel);
    }
}
