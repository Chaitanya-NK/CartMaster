using CartMaster.Business.IServices;
using CartMaster.Data.Models;
using CartMaster.Static;
using CartMaster.TokenGeneration.TokenInterface;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable 1591

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IToken _token;
        public CartController(ICartService cartService, IToken token)
        {
            _cartService = cartService;
            _token = token;
        }

        [HttpPost("HandleCart")]
        public IActionResult HandleCart([FromQuery] string action, [FromBody] CartItemModel cartItemModel = null, int cartId = 0, int productId = 0, int quantity = 0, int userId = 0)
        {
            try
            {
                switch (action.ToLower())
                {
                    case "getbyid":
                        var cartItems = _cartService.GetCartByUserId(userId);
                        return Ok(cartItems);

                    case "create":
                        var cart = _cartService.CreateCart(userId);
                        return Ok(cart);

                    case "add":
                        if (cartItemModel == null) return BadRequest("Cart Item data is required.");
                        var addedCartItem = _cartService.AddProductToCart(cartItemModel);
                        return Ok(new { success = true, message = StaticProduct.AddProductSuccess });

                    case "update":
                        if (cartItemModel == null) return BadRequest("Cart Item data is required.");
                        var updatedCartItem = _cartService.UpdateCartItemQuantity(cartId, productId, quantity);
                        return Ok(new { success = true, message = StaticProduct.UpdateProductSuccess });

                    case "delete":
                        var deletedCartItem = _cartService.RemoveProductFromCart(cartId, productId);
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

        [HttpGet("GetCartItemCountByCartId/{cartId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetCartItemCountByCartId(int cartId)
        {
            try
            {
                int count = _cartService.GetCartItemCountByCartId(cartId);
                return Ok(count);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}