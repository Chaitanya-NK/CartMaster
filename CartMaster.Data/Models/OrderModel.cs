﻿namespace CartMaster.Data.Models
{
    public class OrderModel
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemModel> OrderItems { get; set; }
    }
}
