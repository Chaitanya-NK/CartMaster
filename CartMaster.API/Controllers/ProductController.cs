using CartMaster.Business.IServices;
using CartMaster.Data.Models;
using CartMaster.Static;
using CartMaster.TokenGeneration.TokenInterface;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable 1591
#pragma warning disable 8625

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IToken _token;
        public ProductController(IProductService productService, IToken token)
        {
            _productService = productService;
            _token = token;
        }

        [HttpPost("HandleProduct")]
        public IActionResult HandleProduct([FromQuery] string action, [FromBody] ProductModel productModel = null, int productId = 0, int categoryId = 0)
        {
            try
            {
                switch (action.ToLower())
                {
                    case "getall":
                        var products = _productService.GetAllProducts();
                        return Ok(products);

                    case "getbyid":
                        var product = _productService.GetProductById(productId);
                        return Ok(product);

                    case "getbycategoryid":
                        var productsByCategory = _productService.GetProductsByCategoryId(categoryId);
                        return Ok(productsByCategory);

                    case "add":
                        if (productModel == null) return BadRequest("Product data is required.");
                        var addedProduct = _productService.AddProduct(productModel);
                        return Ok(new { success = true, message = StaticProduct.AddProductSuccess });

                    case "update":
                        if (productModel == null) return BadRequest("Product data is required.");
                        var updatedProduct = _productService.UpdateProduct(productModel);
                        return Ok(new { success = true, message = StaticProduct.UpdateProductSuccess });

                    case "delete":
                        var deletedProduct = _productService.DeleteProduct(productId);
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