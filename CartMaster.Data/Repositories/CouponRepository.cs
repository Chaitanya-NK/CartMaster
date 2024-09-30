using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CartMaster.Data.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public CouponRepository(IConfiguration configuration)
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
            return StaticCategory.OperationSuccess;
        }

        public string CreateCoupon(CouponModel couponModel)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@CouponName", couponModel.CouponName },
                { "@CouponDescription", couponModel.CouponDescription },
                { "@ValidFrom", couponModel.ValidFrom },
                { "@ValidTo", couponModel.ValidTo },
                { "@IsValid", couponModel.IsValid },
                { "@DiscountPercentage", couponModel.DiscountPercentage }
            };

            return ExecuteNonQuery("CreateCoupon", parameters);
        }

        public string UpdateCoupon(CouponModel couponModel)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@CouponID", couponModel.CouponID },
                { "@CouponName", couponModel.CouponName },
                { "@CouponDescription", couponModel.CouponDescription },
                { "@ValidFrom", couponModel.ValidFrom },
                { "@ValidTo", couponModel.ValidTo },
                { "@IsValid", couponModel.IsValid },
                { "@DiscountPercentage", couponModel.DiscountPercentage }
            };

            return ExecuteNonQuery("UpdateCoupon", parameters);
        }

        private List<CouponModel> GetCoupons(string storedProcedure, Dictionary<string, object> parameters = null)
        {
            var coupons = new List<CouponModel>();

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

                    using(SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            var coupon = new CouponModel
                            {
                                CouponID = (int)sqlDataReader["CouponID"],
                                CouponName = sqlDataReader["CouponName"].ToString(),
                                CouponDescription = sqlDataReader["CouponDescription"].ToString(),
                                ValidFrom = (DateTime)sqlDataReader["ValidFrom"],
                                ValidTo = (DateTime)sqlDataReader["ValidTo"],
                                IsValid = (bool)sqlDataReader["IsValid"],
                                DiscountPercentage = (int)sqlDataReader["DiscountPercentage"]
                            };
                            coupons.Add(coupon);
                        }
                    }
                    connection.Close();
                }
            }
            return coupons;
        }

        public List<CouponModel> GetAllCoupons()
        {
            return GetCoupons("GetAllCoupons");
        }

        public List<CouponModel> GetValidCoupons()
        {
            return GetCoupons("GetValidCoupons");
        }  
    }
}
