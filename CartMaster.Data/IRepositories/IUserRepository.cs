using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.IRepositories
{
    public interface IUserRepository
    {
        public Task<(int, string)> RegisterUser(UserModel userModel);
        UserWithRole LoginUser(UserDto userDto);
        public List<UserModel> GetAllUsers();
        public UserModel GetUserById(int userId);
        public string UpdateUser(UserModel userModel);
        public string GetToken(string username);
        public string SaveUserAddress(int userId, string address);
        public string GetUserAddressByUserId(int userId);
        int? GetUserIdByEmail(string email);
        void InsertPasswordResetToken(int userId, string token, DateTime expiryDate);
        (int userId, DateTime expiryDate)? ValidatePasswordResetToken(string token);
        void UpdatePassword(int userId, string newPassword);
    }
}
