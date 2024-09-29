using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }
        public string AddToWishlist(WishlistModel wishlistModel)
        {
            return _wishlistRepository.AddToWishlist(wishlistModel);
        }

        public List<WishlistModel> GetWishlistByUser(int userID)
        {
            return _wishlistRepository.GetWishlistByUser(userID);
        }

        public string RemoveFromWishlist(int userID, int productID)
        {
            return _wishlistRepository.RemoveFromWishlist(userID, productID);
        }
    }
}
