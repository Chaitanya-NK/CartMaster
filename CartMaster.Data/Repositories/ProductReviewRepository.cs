using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.Repositories
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public ProductReviewRepository(IConfiguration configuration)
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

        public string AddProductReview(ProductReviewModel productReviewModel)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ProductID", productReviewModel.ProductID },
                { "@UserID", productReviewModel.UserID },
                { "@Rating", productReviewModel.Rating },
                { "@Comment", productReviewModel.Comment },
                { "@Username", productReviewModel.Username }
            };

            return ExecuteNonQuery("AddProductReview", parameters);
        }

        public string UpdateProductReview(ProductReviewModel productReviewModel)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ReviewID", productReviewModel.ReviewID },
                { "@ProductID", productReviewModel.ProductID },
                { "@UserID", productReviewModel.UserID },
                { "@Rating", productReviewModel.Rating },
                { "@Comment", productReviewModel.Comment }
            };

            return ExecuteNonQuery("UpdateProductReview", parameters);
        }

        public string DeleteProductReview(int reviewId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ReviewID", reviewId }
            };

            return ExecuteNonQuery("DeleteProductReview", parameters);
        }

        // reusable method to execute non-query commands ---- this can be used for get.
        private List<ProductReviewModel> GetProductReviews(string storedProcedures, Dictionary<string, object> parameters)
        {
            var productReviews = new List<ProductReviewModel>();

            using(var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand(storedProcedures, connection))
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
                        while (sqlDataReader.Read())
                        {
                            var productReview = new ProductReviewModel
                            {
                                ReviewID = (int)sqlDataReader["ReviewID"],
                                ProductID = (int)sqlDataReader["ProductID"],
                                UserID = (int)sqlDataReader["UserID"],
                                Rating = sqlDataReader["Rating"] != DBNull.Value ? (int)sqlDataReader["Rating"] : default,
                                Comment = sqlDataReader["Comment"].ToString(),
                                CreatedAt = sqlDataReader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["CreatedAt"]) : Convert.ToDateTime(null),
                                ModifiedAt = sqlDataReader["ModifiedAt"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["ModifiedAt"]) : Convert.ToDateTime(null),
                                Username = sqlDataReader["Username"].ToString()
                            };
                            productReviews.Add(productReview);
                        }
                    }

                    connection.Close();
                }
            }
            return productReviews;
        }

        public List<ProductReviewModel> GetProductReviews(int productId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ProductID", productId }
            };

            return GetProductReviews("GetProductReviews", parameters);
        }

        public decimal GetAverageRatingOfProduct(int productId)
        {
            using (var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetAverageRatingByProductId", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@ProductID", productId);

                    connection.Open();
                    var averageRating = (decimal)sqlCommand.ExecuteScalar();
                    connection.Close();
                    return averageRating;
                }
            }
        }

        public AverageRatingModel GetAverageRatingOfAllProducts()
        {
            AverageRatingModel averageRatingModel = new AverageRatingModel();
            using (var connection = _connection)
            {
                using(SqlCommand sqlCommand = new SqlCommand("GetAverageRatingOfAllProducts", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.Read())
                        {
                            averageRatingModel = new AverageRatingModel
                            {
                                ProductID = (int)sqlDataReader["ProductID"],
                                ProductName = sqlDataReader["ProductName"].ToString(),
                                AverageRating = (decimal)sqlDataReader["AverageRating"]
                            };
                        }
                    }

                    connection.Close();
                }
            }
            return averageRatingModel;
        }
    }
}