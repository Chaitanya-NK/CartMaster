using CartMaster.Data.Models;

namespace CartMaster.Data.IRepositories
{
    public interface IProductRepository
    {
        public string AddProduct(ProductModel productModel);
        public string UpdateProduct(ProductModel productModel);
        public List<ProductModel> GetAllProducts();
        public ProductModel GetProductById(int productId);
        public string DeleteProduct(int productId);
        public List<ProductModel> GetProductsByCategoryId(int categoryId);
    }
}
