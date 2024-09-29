using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.IServices
{
    public interface ICartService
    {
        public string CreateCart(int userId);
        public string AddProductToCart(CartItemModel cartItemModel);
        public string RemoveProductFromCart(int cartId, int productId);
        public string UpdateCartItemQuantity(int cartId, int productId, int quantity);
        public List<CartItemModel> GetCartByUserId(int userId);
        public int GetCartItemCountByCartId(int cartId);
    }
}
