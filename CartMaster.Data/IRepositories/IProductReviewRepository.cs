using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.IRepositories
{
    public interface IProductReviewRepository
    {
        public string AddProductReview(ProductReviewModel productReviewModel);
        public string UpdateProductReview(ProductReviewModel productReviewModel);
        public string DeleteProductReview(int reviewId);
        public List<ProductReviewModel> GetProductReviews(int productId);
        public decimal GetAverageRatingOfProduct(int productId);
        public AverageRatingModel GetAverageRatingOfAllProducts();
        public bool HasUserPurchasedProduct(int productId, int userId);
    }
}
