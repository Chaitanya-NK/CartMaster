using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CartMaster.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CategoryRepository(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString(StaticStrings.DBString);
            _connection = new SqlConnection(connectionString);
            _httpContextAccessor = httpContextAccessor;
        }

        // reusable method to execute non-query commands ---- this can be used for add, update, and delete
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
            return StaticCategory.OperationSuccess;
        }

        public string AddCategory(CategoryModel categoryModel, IFormFile imageURL)
        {
            if (imageURL != null && imageURL.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageURL.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/CategoryImages", fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageURL.CopyTo(stream);
                }
                categoryModel.ImageURL = fileName;
            }

            var parameters = new Dictionary<string, object>
            {
                { "@CategoryName", categoryModel.CategoryName },
                { "@ImageURL", categoryModel.ImageURL }
            };

            return ExecuteNonQuery("AddCategory", parameters);
        }

        public string UpdateCategory(CategoryModel categoryModel, IFormFile imageURL)
        {
            if (imageURL != null && imageURL.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageURL.FileName);
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/CategoryImages", fileName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imageURL.CopyTo(stream);
                }
                categoryModel.ImageURL = fileName;
            }
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryID", categoryModel.CategoryID },
                { "@CategoryName", categoryModel.CategoryName },
                { "@ImageURL", categoryModel.ImageURL }
            };

            return ExecuteNonQuery("UpdateCategory", parameters);
        }

        public string DeleteCategory(int categoryId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryID", categoryId },
            };

            return ExecuteNonQuery("DeleteCategory", parameters);
        }

        // reusable method to execute non-query commands ---- this can be used for get.
        private List<CategoryModel> GetCategories(string storedProcedure, Dictionary<string, object> parameters = null)
        {
            var categories = new List<CategoryModel>();

            using (var connection = _connection)
            {
                using(SqlCommand sqlCommand = new SqlCommand(storedProcedure, connection))
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
                            var category = new CategoryModel
                            {
                                CategoryID = Convert.ToInt32(sqlDataReader["CategoryID"]),
                                CategoryName = sqlDataReader["CategoryName"].ToString(),
                                ImageURL = $"https://{_httpContextAccessor.HttpContext.Request.Host}/images/CategoryImages/{sqlDataReader["ImageURL"].ToString()}",
                                CreatedAt = sqlDataReader["CreatedAt"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["CreatedAt"]) : Convert.ToDateTime(null),
                                ModifiedAt = sqlDataReader["ModifiedAt"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["ModifiedAt"]) : Convert.ToDateTime(null)
                            };
                            categories.Add(category);
                        }
                    }
                    connection.Close();
                }
            }
            return categories;
        }

        public List<CategoryModel> GetAllCategories()
        {
            return GetCategories("GetAllCategories");
        }

        public CategoryModel GetCategoryById(int categoryId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@CategoryID", categoryId }
            };
            return GetCategories("GetCategoryById", parameters).FirstOrDefault();
        }
    }
}
