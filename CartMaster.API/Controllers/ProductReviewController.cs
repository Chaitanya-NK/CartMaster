using CartMaster.Business.IServices;
using CartMaster.Business.Services;
using CartMaster.Data.Models;
using CartMaster.Static;
using CartMaster.TokenGeneration.TokenInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable 1591

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;
        private readonly IToken _token;
        public ProductReviewController(IProductReviewService productReviewService, IToken token)
        {
            _productReviewService = productReviewService;
            _token = token;
        }

        [HttpPost("HandleProductReviews")]
        public IActionResult HandleProductReviews([FromQuery] string action, [FromBody] ProductReviewModel productReviewModel = null, int reviewId = 0, int productId = 0, int userId = 0)
        {
            try
            {
                switch (action.ToLower())
                {
                    case "getbyid":
                        var productReviews = _productReviewService.GetProductReviews(productId);
                        return Ok(productReviews);

                    case "add":
                        if (productReviewModel == null) return BadRequest("Product Review data is required.");
                        var addReviewResponse = _productReviewService.AddProductReview(productReviewModel);
                        if(addReviewResponse == StaticProduct.ProductNotPurchased)
                        {
                            return BadRequest("You need to purchase the product to leave review");
                        }
                        return Ok(new { success = true, message = StaticProductReview.AddProductReviewSuccess });

                    case "update":
                        if (productReviewModel == null) return BadRequest("Product Review data is required.");
                        var updatedProductReview = _productReviewService.UpdateProductReview(productReviewModel);
                        return Ok(new { success = true, message = StaticProduct.UpdateProductSuccess });

                    case "delete":
                        var deletedProductReview = _productReviewService.DeleteProductReview(reviewId);
                        return Ok(new { success = true, message = StaticProduct.DeleteProductSuccess });

                    case "check":
                        var hasPurchased = _productReviewService.HasUserPurchasedProduct(productId, userId);
                        return Ok(hasPurchased);

                    default:
                        return BadRequest("Invalid action specified.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAverageRatingOfProduct/{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetAverageRatingOfProduct(int productId)
        {
            try
            {
                var averageRating = _productReviewService.GetAverageRatingOfProduct(productId);
                return Ok(averageRating);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetAverageRatingOfAllProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetAverageRatingOfAllProducts()
        {
            try
            {
                var averageRatings = _productReviewService.GetAverageRatingOfAllProducts();
                return Ok(averageRatings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}