using CartMaster.Business.IServices;
using CartMaster.Business.Services;
using CartMaster.Data.Models;
using CartMaster.Static;
using DocumentFormat.OpenXml.Spreadsheet;
using Irony.Parsing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSessionController : ControllerBase
    {
        private readonly IUserSessionService _userSessionService;
        public UserSessionController(IUserSessionService userSessionService)
        {
            _userSessionService = userSessionService;
        }

        [HttpPost("HandleUserSession")]
        public IActionResult HandleUsersession([FromQuery] string action, [FromBody] UserSessionTracking userSessionTracking, Guid sessionID, int userID)
        {
            try
            {
                switch (action.ToLower())
                {
                    case "add":
                        var sessionId = _userSessionService.StartSession(userID);
                        return Ok(new { success = true, sessionID = sessionId });

                    case "update":
                        var updatedProduct = _userSessionService.EndSession(sessionID);
                        return Ok(new { success = true, message = "successfully logged out." });

                    case "logsessionchange":
                        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
                        _userSessionService.LogUserSessionChange(userSessionTracking.UserID, userSessionTracking.SessionID, ipAddress);
                        return Ok();

                    case "getsessiontracking":
                        var sessionTracking = _userSessionService.GetUserSessionTracking(userID);
                        return Ok(sessionTracking);

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
