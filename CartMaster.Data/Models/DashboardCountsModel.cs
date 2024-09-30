using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.Models
{
    public class DashboardCountsModel
    {
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public decimal Revenue { get; set; }
        public int TotalReviews { get; set; }
        public int PendingReturns { get; set; }
        public int OutOfStockProducts { get; set; }
        public int RepeatCustomersCount { get; set; }
        public int CancelledOrders {  get; set; }
        public int Coupons {  get; set; }
    }

    public class WishlistInsight
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int WishlistCount { get; set; }
    }

    public class  SalesDataPerMonth
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Sales {  get; set; }
    }

    public class UserGrowthData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int UserRegistrations { get; set; }
    }

    public class TopSellingProduct
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int TotalSold { get; set; }
    }

    public class TopReviewedProducts
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int ReviewCount { get; set; }
        public decimal AverageRating { get; set; }
    }

    public class LowStockProducts
    {
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int StockQuantity { get; set; }
    }

    public class InactiveUsers
    {
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

    public class CategorySales
    {
        public string? CategoryName { get; set; }
        public int TotalSold { get; set; }
    }

    public class DashboardDataModel
    {
        public DashboardCountsModel Counts { get; set; }
        public List<WishlistInsight> WishlistInsights { get; set; }
        public List<SalesDataPerMonth> SalesData { get; set; }
        public List<UserGrowthData> UserGrowth {  get; set; }
        public List<TopSellingProduct> TopSellingProducts { get; set; }
        public List<TopReviewedProducts> TopReviewedProducts { get; set; }
        public List<LowStockProducts> LowStockProducts { get; set; }
        public List<InactiveUsers> InactiveUsers { get; set; }
        public List<CategorySales> BestCategories {  get; set; }
    }
}
