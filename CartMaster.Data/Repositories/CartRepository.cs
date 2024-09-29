using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CartMaster.Data.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public CartRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString(StaticStrings.DBString);
            _connection = new SqlConnection(connectionString);
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


        public string AddProductToCart(CartItemModel cartItemModel)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CartID", cartItemModel.CartID },
                { "ProductID", cartItemModel.ProductID },
                { "Quantity", cartItemModel.Quantity },
                { "Price", cartItemModel.Price },
                { "@ProductName", cartItemModel.ProductName },
                { "@ImageURL", cartItemModel.ImageURL }
            };

            return ExecuteNonQuery("AddProductToCart", parameters);
        }

        public string CreateCart(int userId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "UserID", userId }
            };

            return ExecuteNonQuery("CreateCart", parameters);
        }

        public string UpdateCartItemQuantity(int cartId, int productId, int quantity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CartID", cartId },
                { "ProductID", productId },
                { "Quantity", quantity }
            };

            return ExecuteNonQuery("UpdateCartItemQuantity", parameters);
        }

        public string RemoveProductFromCart(int cartId, int productId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "CartID", cartId },
                { "ProductID", productId }
            };

            return ExecuteNonQuery("RemoveProductFromCart", parameters);
        }

        // reusable method to execute non-query commands ---- this can be used for get.
        private List<CartItemModel> GetCart(string storedProcedure, Dictionary<string, object> parameters)
        {
            var cartItems = new List<CartItemModel>();

            using (var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if(parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }

                    connection.Open();

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while(sqlDataReader.Read())
                        {
                            var cartItem = new CartItemModel
                            {
                                CartItemID = (int)sqlDataReader["CartItemID"],
                                CartID = (int)sqlDataReader["CartID"],
                                ProductID = (int)sqlDataReader["ProductID"],
                                Quantity = (int)sqlDataReader["Quantity"],
                                ProductName = sqlDataReader["ProductName"].ToString(),
                                Price = (decimal)sqlDataReader["Price"],
                                ImageURL = sqlDataReader["ImageURL"].ToString()
                            };
                            cartItems.Add(cartItem);
                        }
                    }

                    connection.Close();
                }
            }
            return cartItems;
        }

        public List<CartItemModel> GetCartByUserId(int userId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "UserID", userId }
            };

            return GetCart("GetCartByUserId", parameters);
        }

        public int GetCartItemCountByCartId(int cartId)
        {
            using(var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetCartItemCountByCartId", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@CartID", cartId);

                    connection.Open();
                    var count = (int)sqlCommand.ExecuteScalar();
                    connection.Close();
                    return count;
                }
            }
        }
    }
}