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
using static System.Net.Mime.MediaTypeNames;

namespace CartMaster.Data.Repositories
{
    public class DashboardCountsRepository : IDashboardCountsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public DashboardCountsRepository (IConfiguration configuration)
        {
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString(StaticStrings.DBString);
            _connection = new SqlConnection(connectionString);
        }
        public async Task<DashboardDataModel> GetDashboardDataAsync()
        {
            var dashboardData = new DashboardDataModel();

            using (var connection = _connection)
            {
                using (SqlCommand sqlCommand = new SqlCommand("GetDashboardData", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();

                    using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if(sqlDataReader.Read())
                        {
                            dashboardData.Counts = new DashboardCountsModel
                            {
                                TotalProducts = sqlDataReader.GetInt32(0),
                                TotalUsers = sqlDataReader.GetInt32(1),
                                TotalOrders = sqlDataReader.GetInt32(2),
                                Revenue = sqlDataReader.GetDecimal(3),
                                TotalReviews = sqlDataReader.GetInt32(4),
                                PendingReturns = sqlDataReader.GetInt32(5),
                                OutOfStockProducts = sqlDataReader.GetInt32(6),
                                RepeatCustomersCount = sqlDataReader.GetInt32(7),
                                CancelledOrders = sqlDataReader.GetInt32(8)
                            };
                        }

                        if (sqlDataReader.NextResult())
                        {
                            dashboardData.WishlistInsights = new List<WishlistInsight>();
                            while(sqlDataReader.Read())
                            {
                                dashboardData.WishlistInsights.Add(new WishlistInsight
                                {
                                    ProductID = Convert.ToInt32(sqlDataReader["ProductID"]),
                                    ProductName = sqlDataReader["ProductName"].ToString(),
                                    WishlistCount = Convert.ToInt32(sqlDataReader["WishlistCount"])
                                });
                            }
                        }

                        if (sqlDataReader.NextResult())
                        {
                            dashboardData.SalesData = new List<SalesDataPerMonth>();
                            while(sqlDataReader.Read())
                            {
                                dashboardData.SalesData.Add(new SalesDataPerMonth
                                {
                                    Year = Convert.ToInt32(sqlDataReader["Year"]),
                                    Month = Convert.ToInt32(sqlDataReader["Month"]),
                                    Sales = Convert.ToDecimal(sqlDataReader["Sales"])
                                });
                            }
                        }

                        if(sqlDataReader.NextResult())
                        {
                            dashboardData.UserGrowth = new List<UserGrowthData>();
                            while(sqlDataReader.Read())
                            {
                                dashboardData.UserGrowth.Add(new UserGrowthData
                                {
                                    Year = Convert.ToInt32(sqlDataReader["Year"]),
                                    Month = Convert.ToInt32(sqlDataReader["Month"]),
                                    UserRegistrations = Convert.ToInt32(sqlDataReader["UserRegistrations"])
                                });
                            }
                        }

                        if(sqlDataReader.NextResult())
                        {
                            dashboardData.TopSellingProducts = new List<TopSellingProduct>();
                            while(sqlDataReader.Read())
                            {
                                dashboardData.TopSellingProducts.Add(new TopSellingProduct
                                {
                                    ProductID = Convert.ToInt32(sqlDataReader["ProductID"]),
                                    ProductName = sqlDataReader["ProductName"].ToString(),
                                    TotalSold = Convert.ToInt32(sqlDataReader["TotalSold"])
                                });
                            }
                        }

                        if(sqlDataReader.NextResult())
                        {
                            dashboardData.TopReviewedProducts = new List<TopReviewedProducts>();
                            while(sqlDataReader.Read())
                            {
                                dashboardData.TopReviewedProducts.Add(new TopReviewedProducts
                                {
                                    ProductID = (int)sqlDataReader["ProductID"],
                                    ProductName = sqlDataReader["ProductName"].ToString(),
                                    ReviewCount = (int)sqlDataReader["ReviewCount"],
                                    AverageRating = (decimal)sqlDataReader["AverageRating"]
                                });
                            }
                        }

                        if (sqlDataReader.NextResult())
                        {
                            dashboardData.LowStockProducts = new List<LowStockProducts>();
                            while(sqlDataReader.Read())
                            {
                                dashboardData.LowStockProducts.Add(new LowStockProducts
                                {
                                    ProductID = (int)sqlDataReader["ProductID"],
                                    ProductName = sqlDataReader["ProductName"].ToString(),
                                    StockQuantity = (int)sqlDataReader["StockQuantity"]
                                });
                            }
                        }

                        if (sqlDataReader.NextResult())
                        {
                            dashboardData.InactiveUsers = new List<InactiveUsers>();
                            while(sqlDataReader.Read())
                            {
                                dashboardData.InactiveUsers.Add(new InactiveUsers
                                {
                                    Username = sqlDataReader["Username"].ToString(),
                                    Name = sqlDataReader["Name"].ToString(),
                                    Email = sqlDataReader["Email"].ToString()
                                });
                            }
                        }

                        if (sqlDataReader.NextResult())
                        {
                            dashboardData.BestCategories = new List<CategorySales>();
                            while(sqlDataReader.Read())
                            {
                                dashboardData.BestCategories.Add(new CategorySales
                                {
                                    CategoryName = sqlDataReader["CategoryName"].ToString(),
                                    TotalSold = (int)sqlDataReader["TotalSold"]
                                });
                            }
                        }
                    }
                }
            }
            return dashboardData;
        }
    }
}