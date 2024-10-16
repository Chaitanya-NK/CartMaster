using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CartMaster.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString(StaticStrings.DBString);
            _connection = new SqlConnection(connectionString);
            _httpContextAccessor = httpContextAccessor;
        }

        // reusable method to execute non-query commands ---- this can be used for add, update, and delete.
        private string ExecuteNonQuery(string storedProcedure, Dictionary<string, object> parameters)
        {
            using (var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    foreach (var parameter in parameters)
                    {
                        sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return StaticProduct.OperationSuccess;
        }

        public string CreateOrder(int userId, decimal totalAmount, int? couponId = null)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userId },
                { "@TotalAmount", totalAmount }
            };

            if (couponId.HasValue)
            {
                parameters.Add("@CouponID", couponId.Value);
            }

            return ExecuteNonQuery("CreateOrder", parameters);
        }

        public string UpdateOrderStatus(int orderId, string status)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderId },
                { "@Status", status }
            };

            return ExecuteNonQuery("UpdateOrderStatus", parameters);
        }

        public string CancelOrder(int orderId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderId }
            };

            return ExecuteNonQuery("CancelOrderByOrderId", parameters);
        }

        public string RequestReturnByOrderItemId(int orderItemId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OrderItemID", orderItemId }
            };

            return ExecuteNonQuery("RequestReturnByOrderItemId", parameters);
        }

        public string ProcessReturnByOrderItemId(int orderItemId, string returnStatus)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OrderItemID", orderItemId },
                { "@ReturnStatus", returnStatus }
            };

            return ExecuteNonQuery("ProcessReturnByOrderItemId", parameters);
        }

        public OrderModel GetOrderDetailsByOrderId(int orderId)
        {
            OrderModel orderModel = null;

            using(var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetOrderDetailsByOrderId", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@OrderID", orderId);

                    connection.Open();
                    using(SqlDataReader sqlDataReader  = sqlCommand.ExecuteReader())
                    {
                        if(sqlDataReader.Read())
                        {
                            orderModel = new OrderModel
                            {
                                OrderID = (int)sqlDataReader["OrderID"],
                                OrderDate = (DateTime)sqlDataReader["OrderDate"],
                                Status = sqlDataReader["Status"].ToString(),
                                TotalAmount = (decimal)sqlDataReader["TotalAmount"],
                                DiscountAmount = sqlDataReader["DiscountAmount"] != DBNull.Value ? (decimal)sqlDataReader["DiscountAmount"] : default,
                                FinalAmount = sqlDataReader["FinalAmount"] != DBNull.Value ? (decimal)sqlDataReader["FinalAmount"] : default,
                                CouponID = sqlDataReader["CouponID"] != DBNull.Value ? (int)sqlDataReader["CouponID"] : default,
                                OrderItems = new List<OrderItemModel>()
                            };
                        }

                        if (sqlDataReader.NextResult())
                        {
                            while(sqlDataReader.Read())
                            {
                                orderModel.OrderItems.Add(new OrderItemModel
                                {
                                    ProductID = (int)sqlDataReader["ProductID"],
                                    Quantity = (int)sqlDataReader["Quantity"],
                                    Price = (decimal)sqlDataReader["Price"],
                                    ProductName = sqlDataReader["ProductName"].ToString(),
                                    ProductDescription = sqlDataReader["ProductDescription"].ToString(),
                                    ImageURL = $"https://{_httpContextAccessor.HttpContext.Request.Host}/images/ProductImages/{sqlDataReader["ImageURL"].ToString()}",
                                    ReturnStatus = sqlDataReader["ReturnStatus"] != DBNull.Value ? sqlDataReader["ReturnStatus"].ToString() : default,
                                    ReturnRequested = sqlDataReader["ReturnRequested"] != DBNull.Value ? (bool)sqlDataReader["ReturnRequested"] : default
                                });
                            }
                        }
                    }
                    connection.Close();
                }
            }
            return orderModel;
        }
        public List<OrderModel> ViewUserOrders(int userId)
        {
            var orders = new List<OrderModel>();
            var ordersDict = new Dictionary<int, OrderModel>();

            using (var connection = _connection)
            {
                using (var sqlCommand = new SqlCommand("ViewUserOrders", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();

                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            var orderId = sqlDataReader["OrderID"] != DBNull.Value ? (int)sqlDataReader["OrderID"] : default;

                            if (!ordersDict.TryGetValue(orderId, out var order))
                            {
                                order = new OrderModel
                                {
                                    OrderID = orderId,
                                    UserID = userId,
                                    OrderDate = sqlDataReader["OrderDate"] != DBNull.Value ? (DateTime)sqlDataReader["OrderDate"] : default,
                                    Status = sqlDataReader["Status"] != DBNull.Value ? sqlDataReader["Status"].ToString() : string.Empty,
                                    TotalAmount = sqlDataReader["TotalAmount"] != DBNull.Value ? (decimal)sqlDataReader["TotalAmount"] : default,
                                    DiscountAmount = sqlDataReader["DiscountAmount"] != DBNull.Value ? (decimal)sqlDataReader["DiscountAmount"] : default,
                                    FinalAmount = sqlDataReader["FinalAmount"] != DBNull.Value ? (decimal)sqlDataReader["FinalAmount"] : default,
                                    CouponID = sqlDataReader["CouponID"] != DBNull.Value ? (int)sqlDataReader["CouponID"] : default,
                                    OrderItems = new List<OrderItemModel>()
                                };
                                ordersDict.Add(orderId, order);
                            }

                            // Check if the columns exist before accessing
                            var orderItemId = sqlDataReader["OrderItemID"] != DBNull.Value ? (int)sqlDataReader["OrderItemID"] : default;
                            var productId = sqlDataReader["ProductID"] != DBNull.Value ? (int)sqlDataReader["ProductID"] : default;
                            var quantity = sqlDataReader["Quantity"] != DBNull.Value ? (int)sqlDataReader["Quantity"] : default;
                            var price = sqlDataReader["Price"] != DBNull.Value ? (decimal)sqlDataReader["Price"] : default;
                            var productName = sqlDataReader["ProductName"] != DBNull.Value ? sqlDataReader["ProductName"].ToString() : default;
                            var productDescription = sqlDataReader["ProductDescription"] != DBNull.Value ? sqlDataReader["ProductDescription"].ToString() : default;
                            var imageURL = $"https://{_httpContextAccessor.HttpContext.Request.Host}/images/ProductImages/{sqlDataReader["ImageURL"].ToString()}";
                            var returnStatus = sqlDataReader["ReturnStatus"] != DBNull.Value ? sqlDataReader["ReturnStatus"].ToString() : default;
                            var returnRequested = sqlDataReader["ReturnRequested"] != DBNull.Value ? (bool)sqlDataReader["ReturnRequested"] : default;

                            order.OrderItems.Add(new OrderItemModel
                            {
                                OrderItemID = orderItemId,
                                ProductID = productId,
                                Quantity = quantity,
                                Price = price,
                                OrderID = orderId,
                                ProductName = productName,
                                ProductDescription = productDescription,
                                ImageURL = imageURL,
                                ReturnStatus = returnStatus,
                                ReturnRequested = returnRequested
                            });
                        }
                    }
                    connection.Close();
                }
            }

            orders.AddRange(ordersDict.Values);
            return orders;
        }
    }
}
