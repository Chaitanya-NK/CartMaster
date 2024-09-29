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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IToken _token;
        public CategoryController(ICategoryService categoryService, IToken token)
        {
            _categoryService = categoryService;
            _token = token;
        }

        [HttpPost("HandleCategory")]
        public IActionResult HandleCategory([FromQuery] string action, [FromBody] CategoryModel categoryModel = null, int categoryId = 0)
        {
            try
            {
                switch (action.ToLower())
                {
                    case "getall":
                        var categories = _categoryService.GetAllCategories();
                        return Ok(categories);

                    case "getbyid":
                        var category = _categoryService.GetCategoryById(categoryId);
                        return Ok(category);

                    case "add":
                        if (categoryModel == null) return BadRequest("Category data is required.");
                        var addedCategory = _categoryService.AddCategory(categoryModel);
                        return Ok(new { success = true, message = StaticCategory.AddCategorySuccess });

                    case "update":
                        if (categoryModel == null) return BadRequest("Category data is required.");
                        var updatedCategory = _categoryService.UpdateCategory(categoryModel);
                        return Ok(new { success = true, message = StaticCategory.UpdateCategorySuccess });

                    case "delete":
                        var deletedCategory = _categoryService.DeleteCategory(categoryId);
                        return Ok(new { success = true, message = StaticCategory.DeleteCategorySuccess });

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
