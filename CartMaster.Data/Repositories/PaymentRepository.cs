using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.AspNetCore.Http;
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
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public PaymentRepository(IConfiguration configuration)
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

        public string InsertPaymentDetails(PaymentModel paymentModel)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", paymentModel.OrderID },
                { "@PaymentMethod", paymentModel.PaymentMethod },
                { "@CardName", paymentModel.CardName },
                { "@CardNumber", paymentModel.CardNumber },
                { "@ExpiryDate", paymentModel.ExpiryDate },
                { "@CVV", paymentModel.CVV },
                { "@UpiId", paymentModel.UpiId }
            };

            return ExecuteNonQuery("InsertPaymentDetails", parameters);
        }

        // reusable method to execute non-query commands ---- this can be used for get.
        private List<PaymentModel> GetPayments(string storedProcedure, Dictionary<string, object> parameters = null)
        {
            var payments = new List<PaymentModel>();

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
                            var payment = new PaymentModel
                            {
                                PaymentID = Convert.ToInt32(sqlDataReader["PaymentID"]),
                                OrderID = Convert.ToInt32(sqlDataReader["OrderID"]),
                                PaymentMethod = sqlDataReader["PaymentMethod"].ToString(),
                                CardName = sqlDataReader["CardName"].ToString(),
                                CardNumber = sqlDataReader["CardNumber"].ToString(),
                                ExpiryDate = sqlDataReader["ExpiryDate"].ToString(),
                                CVV = sqlDataReader["CVV"].ToString(),
                                UpiId = sqlDataReader["UpiId"].ToString(),
                                PaymentDate = sqlDataReader["PaymentDate"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["PaymentDate"]) : Convert.ToDateTime(null),
                                PaymentStatus = sqlDataReader["PaymentStatus"].ToString()
                            };
                            payments.Add(payment);
                        }
                    }

                    connection.Close();
                }
            }
            return payments;
        }

        public PaymentModel GetPaymentDetailsByOrderId(int orderId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@OrderID", orderId }
            };

            return GetPayments("GetPaymentDetailsByOrderID", parameters).FirstOrDefault();
        }
    }
}