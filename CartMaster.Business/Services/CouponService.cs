using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }
        public string CreateCoupon(CouponModel couponModel)
        {
            return _couponRepository.CreateCoupon(couponModel);
        }

        public List<CouponModel> GetAllCoupons()
        {
            return _couponRepository.GetAllCoupons();
        }

        public List<CouponModel> GetValidCoupons()
        {
            return _couponRepository.GetValidCoupons();
        }

        public string UpdateCoupon(CouponModel couponModel)
        {
            return _couponRepository.UpdateCoupon(couponModel);
        }
    }
}
