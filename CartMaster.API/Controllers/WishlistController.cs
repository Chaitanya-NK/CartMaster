using CartMaster.Business.IServices;
using CartMaster.Business.Services;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("HandleWishlist")]
        public IActionResult HandleWishlist([FromQuery] string action, [FromBody] WishlistModel wishlistModel = null, int productId = 0, int userId = 0)
        {
            try
            {
                switch (action.ToLower())
                {
                    case "getbyid":
                        var wishlist = _wishlistService.GetWishlistByUser(userId);
                        return Ok(wishlist);

                    case "add":
                        if (wishlistModel == null) return BadRequest("Wishlist data is required.");
                        var addedWishlist = _wishlistService.AddToWishlist(wishlistModel);
                        return Ok(new { success = true, message = StaticProduct.AddProductSuccess });

                    case "delete":
                        var deletedWishlist = _wishlistService.RemoveFromWishlist(userId, productId);
                        return Ok(new { success = true, message = StaticProduct.DeleteProductSuccess });

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
