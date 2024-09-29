namespace CartMaster.Data.Models
{
    public class CartItemModel
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string? ProductName { get; set; }
        public decimal Price { get; set; }
        public string? ImageURL { get; set; }
    }
}
