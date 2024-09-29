using System;
namespace CartMaster.Data.Models
{
    public class CartModel
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<CartItemModel> CartItems { get; set; }
    }
}
