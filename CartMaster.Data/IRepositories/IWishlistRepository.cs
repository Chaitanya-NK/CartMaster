using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.IRepositories
{
    public interface IWishlistRepository
    {
        public string AddToWishlist(WishlistModel wishlistModel);
        public List<WishlistModel> GetWishlistByUser(int userID);
        public string RemoveFromWishlist(int userID, int productID);
    }
}
