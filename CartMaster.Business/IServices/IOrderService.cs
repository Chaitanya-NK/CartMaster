using CartMaster.Data.Models;

namespace CartMaster.Business.IServices
{
    public interface IOrderService
    {
        public string CreateOrder(int userId, decimal totalAmount, int? couponId = null);
        public string UpdateOrderStatus(int orderId, string status);
        public List<OrderModel> ViewUserOrders(int userId);
        public OrderModel GetOrderDetailsByOrderId(int orderId);
        public string CancelOrder(int orderId);
        public string RequestReturnByOrderItemId(int orderItemId);
        public string ProcessReturnByOrderItemId(int orderItemId, string returnStatus);
    }
}
