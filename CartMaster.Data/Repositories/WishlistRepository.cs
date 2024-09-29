using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public WishlistRepository(IConfiguration configuration)
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
            return StaticWishlist.OperationSuccess;
        }

        public string AddToWishlist(WishlistModel wishlistModel)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@UserId", wishlistModel.UserID },
                { "@ProductId", wishlistModel.ProductID },
                { "@ProductName", wishlistModel.ProductName },
                { "@ProductDescription", wishlistModel.ProductDescription },
                { "@Price", wishlistModel.Price },
                { "@ImageURL", wishlistModel.ImageURL },
            };

            return ExecuteNonQuery("AddToWishlist", parameters);
        }

        public string RemoveFromWishlist(int userID, int productID)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@UserId", userID },
                { "@ProductId", productID }
            };

            return ExecuteNonQuery("RemoveFromWishlist", parameters);
        }

        // reusable method to execute non-query commands ---- this can be used for get.
        private List<WishlistModel> GetWishlists(string storedProcedure, Dictionary<string, object> parameters = null)
        {
            var wishlists = new List<WishlistModel>();

            using (var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedure, connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            sqlCommand.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                    }

                    connection.Open();

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            var wishlist = new WishlistModel
                            {
                                WishlistID = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("WishlistID")),
                                UserID = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("UserID")),
                                ProductID = sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("ProductID")),
                                ProductName = sqlDataReader.GetString(sqlDataReader.GetOrdinal("ProductName")),
                                ProductDescription = sqlDataReader.GetString(sqlDataReader.GetOrdinal("ProductDescription")),
                                Price = sqlDataReader.GetDecimal(sqlDataReader.GetOrdinal("Price")),
                                ImageURL = sqlDataReader.GetString(sqlDataReader.GetOrdinal("ImageURL"))
                            };
                            wishlists.Add(wishlist);
                        }
                    }

                    connection.Close();
                }
            }
            return wishlists;
        }

        public List<WishlistModel> GetWishlistByUser(int userID)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@UserId", userID }
            };
            return GetWishlists("GetWishlistByUser", parameters);
        }
    }
}
