using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.TokenGeneration.Models;
using CartMaster.TokenGeneration.TokenInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IToken _token;
        private readonly IEmailService _emailService;
        public UserService(IUserRepository userRepository, IToken token, IEmailService emailService)
        {
            _userRepository = userRepository;
            _token = token;
            _emailService = emailService;
        }
        public List<UserModel> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public string GetUserAddressByUserId(int userId)
        {
            return _userRepository.GetUserAddressByUserId(userId);
        }

        public UserModel GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public string LoginUser(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            var result = _userRepository.LoginUser(userDto);
            if(result != null)
            {
                var tokenModel = new TokenModel
                {
                    UserID = result.UserID.ToString(),
                    UserName = result.Username,
                    Email = result.Email,
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    RoleID = result.RoleID.ToString(),
                    RoleName = result.RoleName,
                    CartID = result.CartID.ToString()
                };
                string token = _token.CreateToken(tokenModel);
                return token;
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid User");
            }
        }

        public async Task<(int, string)> RegisterUser(UserModel userModel)
        {
            return await _userRepository.RegisterUser(userModel);
        }

        public async Task RequestPasswordReset(string email)
        {
            int? userId = _userRepository.GetUserIdByEmail(email);
            if(userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            string token = Guid.NewGuid().ToString();
            DateTime expiryDate = DateTime.Now.AddHours(1);

            InsertPasswordResetToken(userId.Value, token, expiryDate);

            await _emailService.SendPasswordResetEmailAsync(email, token);
        }

        public void InsertPasswordResetToken(int userId, string token, DateTime expiryDate)
        {
            _userRepository.InsertPasswordResetToken(userId, token, expiryDate);
        }

        public async Task<bool> ResetPassword(string token, string newPassword)
        {
            var tokenInfo = _userRepository.ValidatePasswordResetToken(token);

            if(tokenInfo == null || tokenInfo.Value.expiryDate < DateTime.Now)
            {
                return false;
            }

            _userRepository.UpdatePassword(tokenInfo.Value.userId, newPassword);
            return true;
        }

        public string SaveUserAddress(int userId, string address)
        {
            return _userRepository.SaveUserAddress(userId, address);
        }

        public string UpdateUser(UserModel userModel)
        {
            return _userRepository.UpdateUser(userModel);
        }
    }
}
