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
        public UserService(IUserRepository userRepository, IToken token)
        {
            _userRepository = userRepository;
            _token = token;
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
