namespace CartMaster.Data.Models
{
    public class OrderItemModel
    {
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string? ProductName {  get; set; }
        public string? ProductDescription { get; set; }
        public string? ImageURL { get; set; }
        public string? ReturnStatus { get; set; }
        public bool ReturnRequested { get; set; }
    }
}
