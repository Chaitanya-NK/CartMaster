using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CartMaster.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString(StaticStrings.DBString);
            _connection = new SqlConnection(connectionString);
            _httpContextAccessor = httpContextAccessor;
        }

        // reusable method to execute non-query commands ----- this can be used for add, update, and delete.
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

        public string AddProduct(ProductModel productModel, IFormFile imageURL)
        {
            if(imageURL != null && imageURL.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageURL.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProductImages", fileName);

                using(var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageURL.CopyTo(stream);
                }
                productModel.ImageURL = fileName;
            }

            var parameters = new Dictionary<string, object>
            {
                { "@ProductName", productModel.ProductName },
                { "@ProductDescription", productModel.ProductDescription },
                { "@Price", productModel.Price },
                { "@StockQuantity", productModel.StockQuantity },
                { "@CategoryID", productModel.CategoryID },
                { "@ImageURL", productModel.ImageURL }
            };

            return ExecuteNonQuery("AddProduct", parameters);
        }

        public string UpdateProduct(ProductModel productModel, IFormFile imageURL)
        {
            if (imageURL != null && imageURL.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageURL.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ProductImages", fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageURL.CopyTo(stream);
                }
                productModel.ImageURL = fileName;
            }

            var parameters = new Dictionary<string, object>
            {
                { "@ProductID", productModel.ProductID },
                { "@ProductName", productModel.ProductName },
                { "@ProductDescription", productModel.ProductDescription },
                { "@Price", productModel.Price },
                { "@StockQuantity", productModel.StockQuantity },
                { "@CategoryID", productModel.CategoryID },
                { "@ImageURL", productModel.ImageURL }
            };

            return ExecuteNonQuery("UpdateProduct", parameters);
        }

        public string DeleteProduct(int productId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ProductID", productId }
            };

            return ExecuteNonQuery("DeleteProduct", parameters);
        }

        // reusable method to execute non-query commands ----- this can be used for get.
        private List<ProductModel> GetProducts(string storedProcedure, Dictionary<string, object> parameters = null)
        {
            var products = new List<ProductModel>();

            using(var connection = _connection)
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
                            var product = new ProductModel
                            {
                                ProductID = Convert.ToInt32(sqlDataReader["ProductID"]),
                                ProductName = sqlDataReader["ProductName"].ToString(),
                                ProductDescription = sqlDataReader["ProductDescription"].ToString(),
                                Price = Convert.ToInt32(sqlDataReader["Price"]),
                                StockQuantity = Convert.ToInt32(sqlDataReader["StockQuantity"]),
                                ImageURL = $"https://{_httpContextAccessor.HttpContext.Request.Host}/images/ProductImages/{sqlDataReader["ImageURL"].ToString()}",
                                CategoryID = Convert.ToInt32(sqlDataReader["CategoryID"]),
                                CreatedAt = sqlDataReader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["CreatedAt"]) : Convert.ToDateTime(null),
                                ModifiedAt = sqlDataReader["ModifiedAt"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["ModifiedAt"]) : Convert.ToDateTime(null)
                            };
                            products.Add(product);
                        }
                    }

                    connection.Close();
                }
            }
            return products;
        }

        public List<ProductModel> GetAllProducts()
        {
            return GetProducts("GetAllProducts");
        }

        public ProductModel GetProductById(int productId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ProductID", productId }
            };
            return GetProducts("GetProductById", parameters).FirstOrDefault();
        }

        public List<ProductModel> GetProductsByCategoryId(int categoryId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryID", categoryId }
            };
            return GetProducts("GetProductsByCategoryId", parameters);
        }
    }
}