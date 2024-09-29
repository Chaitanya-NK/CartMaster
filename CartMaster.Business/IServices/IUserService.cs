﻿using CartMaster.Data.Models;

namespace CartMaster.Business.IServices
{
    public interface IUserService
    {
        public Task<(int, string)> RegisterUser(UserModel userModel);
        string LoginUser(UserDto userDto);
        public List<UserModel> GetAllUsers();
        public UserModel GetUserById(int userId);
        public string UpdateUser(UserModel userModel);
        public string SaveUserAddress(int userId, string address);
        public string GetUserAddressByUserId(int userId);
    }
}