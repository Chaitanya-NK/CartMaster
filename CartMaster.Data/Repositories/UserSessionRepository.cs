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
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public UserSessionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString(StaticStrings.DBString);
            _connection = new SqlConnection(connectionString);
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
        public string InsertUserSession(Guid sessionID, int userID, DateTime loginTime)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@UserID", userID },
                { "@SessionID", sessionID },
                { "@LoginTime", loginTime }
            };

            return ExecuteNonQuery("CreateUserSession", parameters);
        }

        public string UpdateLogoutTime(Guid sessionID, DateTime logoutTime)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@SessionID", sessionID },
                { "@LogOutTime", logoutTime }
            };

            return ExecuteNonQuery("UpdateUserSession", parameters);
        }

        public void InsertUserSessionTracking(UserSessionTracking sessionTracking)
        {
            using (var connection = _connection)
            {
                using (var command = new SqlCommand("InsertUserSessionTracking", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", sessionTracking.UserID);
                    command.Parameters.AddWithValue("@SessionId", sessionTracking.SessionID);
                    command.Parameters.AddWithValue("@IpAddress", sessionTracking.IpAddress);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<UserSessionTracking> GetUserSessionTrackingByUserId(int userId)
        {
            var sessionTrackings = new List<UserSessionTracking>();

            using (var connection = _connection)
            {
                using (var command = new SqlCommand("GetUserSessionTrackingByUserId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", userId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sessionTrackings.Add(new UserSessionTracking
                            {
                                UserSessionTrackingID = (int)reader["UserSessionTrackingID"],
                                UserID = (int)reader["UserID"],
                                SessionID = (Guid)reader["SessionID"],
                                IpAddress = reader["IpAddress"].ToString(),
                                ChangeDate = (DateTime)reader["ChangeDate"],
                            });
                        }
                    }
                }
            }

            return sessionTrackings;
        }
    }
}
