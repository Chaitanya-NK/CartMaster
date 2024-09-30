using CartMaster.Business.IServices;
using CartMaster.Business.Services;
using CartMaster.Data.Models;
using CartMaster.Static;
using CartMaster.TokenGeneration.TokenInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        private readonly IToken _token;
        public CouponController(ICouponService couponService, IToken token)
        {
            _couponService = couponService;
            _token = token;
        }

        [HttpPost("HandleCoupon")]
        public IActionResult HandleCoupon([FromQuery] string action, [FromBody] CouponModel couponModel = null)
        {
            try
            {
                switch (action.ToLower())
                {
                    case "getall":
                        var coupons = _couponService.GetAllCoupons();
                        return Ok(coupons);

                    case "getvalid":
                        var validCoupons = _couponService.GetValidCoupons();
                        return Ok(validCoupons);

                    case "add":
                        if (couponModel == null) return BadRequest("Coupon data is required.");
                        var addedCoupon = _couponService.CreateCoupon(couponModel);
                        return Ok(new { success = true, message = StaticCategory.AddCategorySuccess });

                    case "update":
                        if (couponModel == null) return BadRequest("Coupon data is required.");
                        var updatedCoupon = _couponService.UpdateCoupon(couponModel);
                        return Ok(new { success = true, message = StaticCategory.UpdateCategorySuccess });

                    default:
                        return BadRequest("Invalid action specified.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
