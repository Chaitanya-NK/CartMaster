using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;

namespace CartMaster.Business.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public string CreateOrder(int userId, decimal totalAmount, int? couponId = null)
        {
            return _orderRepository.CreateOrder(userId, totalAmount, couponId);
        }

        public OrderModel GetOrderDetailsByOrderId(int orderId)
        {
            return _orderRepository.GetOrderDetailsByOrderId(orderId);
        }

        public List<OrderModel> ViewUserOrders(int userId)
        {
            return _orderRepository.ViewUserOrders(userId);
        }

        public string UpdateOrderStatus(int orderId, string status)
        {
            return _orderRepository.UpdateOrderStatus(orderId, status);
        }

        public string CancelOrder(int orderId)
        {
            return _orderRepository.CancelOrder(orderId);
        }

        public string RequestReturnByOrderItemId(int orderItemId)
        {
            return _orderRepository.RequestReturnByOrderItemId(orderItemId);
        }

        public string ProcessReturnByOrderItemId(int orderItemId, string retrunStatus)
        {
            return _orderRepository.ProcessReturnByOrderItemId(orderItemId, retrunStatus);
        }
    }
}
