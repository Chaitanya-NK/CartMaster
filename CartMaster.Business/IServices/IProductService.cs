using CartMaster.Data.Models;
using Microsoft.AspNetCore.Http;

namespace CartMaster.Business.IServices
{
    public interface IProductService
    {
        public string AddProduct(ProductModel productModel, IFormFile imageURL);
        public string UpdateProduct(ProductModel productModel, IFormFile imageURL);
        public List<ProductModel> GetAllProducts();
        public ProductModel GetProductById(int productId);
        public string DeleteProduct(int productId);
        public List<ProductModel> GetProductsByCategoryId(int categoryId);
    }
}
