using CartMaster.Business.IServices;
using CartMaster.Data.Models;
using CartMaster.Static;
using CartMaster.TokenGeneration.TokenInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
#pragma warning disable 1591
#pragma warning disable 1591

namespace CartMaster.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IToken _token;
        private readonly IOTPService _otpService;
        public UserController(IUserService userService, IToken token, IOTPService otpService)
        {
            _userService = userService;
            _token = token;
            _otpService = otpService;
        }

        [HttpGet("GetAllUsers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetAllUsers()
        {
            try
            {
                var getToken = Request.Headers["Authorization"].ToString();
                var token = _token.ReadToken(getToken);
                /*if (token.RoleName == "ADMIN")
                {
                    var tokenn = Request.Headers["Authorization"];
                    var tokenModel = _token.ReadToken(tokenn);
                    if (tokenModel == null)
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized);
                    }
                    else
                    {*/
                        var users = _userService.GetAllUsers();
                        if (users == null)
                        {
                            return NotFound("Users Not Found");
                        }
                        return Ok(users);
                /*    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }*/
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserById/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetUserById(int userId)
        {
            try
            {
                var getToken = Request.Headers["Authorization"];
                var token = _token.ReadToken(getToken);
                if (token == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
                else
                {
                    var users = _userService.GetUserById(userId);
                    if (users == null)
                    {
                        return NotFound("Users Not Found");
                    }
                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("UpdateUser")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult UpdateUser(UserModel userModel)
        {
            try
            {
                var getToken = Request.Headers["Authorization"];
                var token = _token.ReadToken(getToken);
                if (token == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
                else
                {
                    var result = _userService.UpdateUser(userModel);
                    return Ok(result);
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(UserModel), 201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register(UserModel userModel)
        {
            try
            {
                var (userId, email) = await _userService.RegisterUser(userModel);
                if (userId == 0)
                {
                    return BadRequest(userId);
                }
                if(userId > 0)
                {
                    /*CookieOptions options = new CookieOptions
                    {
                        Expires = DateTime.Now.AddMinutes(30),
                        HttpOnly = true,
                        Secure = false
                    };
                    Response.Cookies.Append("UserID", userId.ToString(), options);*/

                    await _otpService.SendOTP(userId, email);
                    return Ok(new { userId = userId, email = email, message = "Registration Successful" });
                }
                return BadRequest("User Registration Failed");
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("VerifyOTP")]
        public IActionResult VerifyOTP([FromBody] OTPVerificationRequestModel otpVerificationModel)
        {
            /*if(Request.Cookies.TryGetValue("UserID", out string userIdValue))
            {
                int userId = int.Parse(userIdValue);*/
                var isValid = _otpService.VerifyOTP(otpVerificationModel.UserID, otpVerificationModel.OTPCode);

                if (!isValid)
                {
                    return BadRequest("Invalid or expired OTP");
                }
                return Ok(new { success = true, message = "OTP Verification Successful" });
            /*}*/
            /*return BadRequest("Session expired or user id not found");*/
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserModel), 201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult Login(UserDto userDto)
        {
            try
            {
                string data = _userService.LoginUser(userDto);
                if(data == StaticLogin.InvalidUser)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized);
                }
                else
                {
                    return Ok(new { Token = data });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("SaveUserAddress")]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult SaveUserAddress(int userId, string address)
        {
            try
            {
                var date = _userService.SaveUserAddress(userId, address);
                return Ok(new { success = true, message = StaticUser.SaveUserAddressSuccess });
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetUserAddressByUserId/{userId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public IActionResult GetUserAddressByUserId(int userId)
        {
            try
            {
                var address = _userService.GetUserAddressByUserId(userId);
                return Ok(address);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
