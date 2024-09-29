using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;
#pragma warning disable 8600
#pragma warning disable 8602
#pragma warning disable 8603
#pragma warning disable 8604
#pragma warning disable 8619
#pragma warning disable 8625

namespace CartMaster.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString(StaticStrings.DBString);
            _connection = new SqlConnection(connectionString);
        }

        // Reusable method to execute non-query commands (e.g., Add, Update, Delete)
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
            return StaticUser.UserUpdateSuccess;
        }

        // Reusable method to execute commands that return user data
        private List<UserModel> GetEntities(string storedProcedure, Dictionary<string, object> parameters = null)
        {
            var users = new List<UserModel>();

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
                            var user = new UserModel
                            {
                                UserID = Convert.ToInt32(sqlDataReader["UserID"]),
                                Username = sqlDataReader["Username"].ToString(),
                                Email = sqlDataReader["Email"].ToString(),
                                Password = sqlDataReader["Password"].ToString(),
                                FirstName = sqlDataReader["FirstName"].ToString(),
                                LastName = sqlDataReader["LastName"].ToString(),
                                Address = sqlDataReader["Address"].ToString(),
                                PhoneNumber = sqlDataReader["PhoneNumber"].ToString()
                            };
                            users.Add(user);
                        }
                    }

                    connection.Close();
                }
            }
            return users;
        }

        public List<UserModel> GetAllUsers()
        {
            return GetEntities("GetAllUsers");
        }

        public UserModel GetUserById(int userId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userId }
            };
            return GetEntities("GetUserById", parameters).FirstOrDefault();
        }

        public string SaveUserAddress(int userId, string address)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userId },
                { "@Address", address }
            };
            return ExecuteNonQuery("SaveUserAddress", parameters);
        }

        public string UpdateUser(UserModel userModel)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userModel.UserID },
                { "@Username", userModel.Username },
                { "@Password", userModel.Password },
                { "@Email", userModel.Email },
                { "@FirstName", userModel.FirstName },
                { "@LastName", userModel.LastName },
                { "@Address", userModel.Address },
                { "@PhoneNumber", userModel.PhoneNumber }
            };
            return ExecuteNonQuery("UpdateUser", parameters);
        }

        public string GetUserAddressByUserId(int userId)
        {
            UserModel userModel = null;
            using (var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetUserAddressByUserId", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();
                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.Read())
                        {
                            userModel = new UserModel
                            {
                                Address = sqlDataReader["Address"].ToString()
                            };
                        }
                    }
                }
            }
            return userModel.Address;
        }

        public string GetToken(string username)
        {
            using (var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand("TokenDetails", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue(StaticToken.Username, username);
                    connection.Open();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                    DataTable dt = new DataTable();
                    sqlDataAdapter.Fill(dt);
                    connection.Close();

                    if (dt.Rows.Count > 0)
                    {
                        return JsonConvert.SerializeObject(dt);
                    }
                    else
                    {
                        return "User Not Found";
                    }
                }
            }
        }

        public async Task<(int, string)> RegisterUser(UserModel userModel)
        {
            using (var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand("Register", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.AddWithValue("@Username", userModel.Username);
                    sqlCommand.Parameters.AddWithValue("@Password", userModel.Password);
                    sqlCommand.Parameters.AddWithValue("@Email", userModel.Email);
                    sqlCommand.Parameters.AddWithValue("@FirstName", userModel.FirstName);
                    sqlCommand.Parameters.AddWithValue("@LastName", userModel.LastName);
                    sqlCommand.Parameters.AddWithValue("@PhoneNumber", userModel.PhoneNumber);

                    var userIdParam = new SqlParameter("@UserID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    sqlCommand.Parameters.Add(userIdParam);

                    var emailParam = new SqlParameter("@OutputEmail", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                    sqlCommand.Parameters.Add(emailParam);

                    await connection.OpenAsync();
                    await sqlCommand.ExecuteNonQueryAsync();
                    int userId = (int)userIdParam.Value;
                    string email = emailParam.Value.ToString();
                    await connection.CloseAsync();

                    return (userId, email);
                }
            }
        }

        public UserWithRole LoginUser(UserDto userDto)
        {
            if (userDto == null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }

            using (var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand("Login", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    sqlCommand.Parameters.AddWithValue("@Username", userDto.Username);
                    sqlCommand.Parameters.AddWithValue("@Password", userDto.Password);

                    using SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    var dataset = new DataSet();
                    sqlDataAdapter.Fill(dataset);
                    connection.Close();

                    if (dataset.Tables.Count > 0 && dataset.Tables[0].Rows.Count > 0)
                    {
                        var user = new UserWithRole
                        {
                            UserID = (int)dataset.Tables[0].Rows[0]["UserID"],
                            Username = dataset.Tables[0].Rows[0]["Username"].ToString(),
                            Email = dataset.Tables[0].Rows[0]["Email"].ToString(),
                            FirstName = dataset.Tables[0].Rows[0]["FirstName"].ToString(),
                            LastName = dataset.Tables[0].Rows[0]["LastName"].ToString(),
                            RoleID = (int)dataset.Tables[0].Rows[0]["RoleID"],
                            RoleName = dataset.Tables[0].Rows[0]["RoleName"].ToString(),
                            CartID = (int)dataset.Tables[0].Rows[0]["CartID"]
                        };
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
