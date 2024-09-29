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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public string AddProductToCart(CartItemModel cartItemModel)
        {
            return _cartRepository.AddProductToCart(cartItemModel);
        }

        public string CreateCart(int userId)
        {
            return _cartRepository.CreateCart(userId);
        }

        public List<CartItemModel> GetCartByUserId(int userId)
        {
            return _cartRepository.GetCartByUserId(userId);
        }

        public int GetCartItemCountByCartId(int cartId)
        {
            return _cartRepository.GetCartItemCountByCartId(cartId);
        }

        public string RemoveProductFromCart(int cartId, int productId)
        {
            return _cartRepository.RemoveProductFromCart(cartId, productId);
        }

        public string UpdateCartItemQuantity(int cartId, int productId, int quantity)
        {
            return _cartRepository.UpdateCartItemQuantity(cartId, productId, quantity);
        }
    }
}
