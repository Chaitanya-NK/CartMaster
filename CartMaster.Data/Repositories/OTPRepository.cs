using CartMaster.Data.IRepositories;
using CartMaster.Static;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CartMaster.Data.Repositories
{
    public class OTPRepository : IOTPRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public OTPRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString(StaticStrings.DBString);
            _connection = new SqlConnection(connectionString);
        }
        public async Task SaveOTPAsync(int userId, string otpCode)
        {
            using(var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand("SaveUserOTP", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserID", userId);
                    sqlCommand.Parameters.AddWithValue("@OTPCode", otpCode);
                    sqlCommand.Parameters.AddWithValue("@ExpirationTime", DateTime.Now.AddMinutes(5));

                    await connection.OpenAsync();
                    await sqlCommand.ExecuteNonQueryAsync();
                    await connection.CloseAsync();
                }
            }
        }

        public bool VerifyOTP(int userId, string otpCode)
        {
            using (var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand("VerifyUserOTP", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@UserID", userId);
                    sqlCommand.Parameters.AddWithValue("@OTPCode", otpCode);
                    connection.Open();
                    var result = (int)sqlCommand.ExecuteScalar();
                    return result == 1;
                }
            }
        }
    }
}
