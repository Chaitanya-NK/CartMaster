using CartMaster.Business.IServices;
using CartMaster.Data.Models;
using CartMaster.TokenGeneration.TokenInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IToken _token;
        public MenuItemController(IMenuItemService menuItemService, IToken token)
        {
            _menuItemService = menuItemService;
            _token = token;
        }

        [HttpGet("GetMenuItemsByRoleId")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetMenuItemsByRoleId(int roleID)
        {
            try
            {
                /*var getToken = Request.Headers["Authorization"];
                var token = _token.ReadToken(getToken);
                if (token == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
                else
                {
                    var menuItem = _menuItemService.GetMenuItemsByRoleId(roleId);
                    return Ok(menuItem);
                }*/
                var menuItem = _menuItemService.GetMenuItemsByRoleId(roleID);
                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
