using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.IServices
{
    public interface IProductReviewService
    {
        public string AddProductReview(ProductReviewModel productReviewModel);
        public string UpdateProductReview(ProductReviewModel productReviewModel);
        public string DeleteProductReview(int reviewId);
        public List<ProductReviewModel> GetProductReviews(int productId);
        public decimal GetAverageRatingOfProduct(int productId);
        public AverageRatingModel GetAverageRatingOfAllProducts();
    }
}
