using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using CartMaster.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _productReviewRepository;
        public ProductReviewService(IProductReviewRepository productReviewRepository)
        {
            _productReviewRepository = productReviewRepository;
        }
        public string AddProductReview(ProductReviewModel productReviewModel)
        {
            bool hasPurchased = _productReviewRepository.HasUserPurchasedProduct(productReviewModel.ProductID, productReviewModel.UserID);
            if(!hasPurchased)
            {
                return StaticProduct.ProductNotPurchased;
            }
            return _productReviewRepository.AddProductReview(productReviewModel);
        }

        public string DeleteProductReview(int reviewId)
        {
            return _productReviewRepository.DeleteProductReview(reviewId);
        }

        public decimal GetAverageRatingOfProduct(int productId)
        {
            return _productReviewRepository.GetAverageRatingOfProduct(productId);
        }

        public AverageRatingModel GetAverageRatingOfAllProducts()
        {
            return _productReviewRepository.GetAverageRatingOfAllProducts();
        }

        public List<ProductReviewModel> GetProductReviews(int productId)
        {
            return _productReviewRepository.GetProductReviews(productId);
        }

        public string UpdateProductReview(ProductReviewModel productReviewModel)
        {
            return _productReviewRepository.UpdateProductReview(productReviewModel);
        }

        public bool HasUserPurchasedProduct(int productId, int userId)
        {
            return _productReviewRepository.HasUserPurchasedProduct(productId, userId);
        }
    }
}
