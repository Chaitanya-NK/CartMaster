﻿using CartMaster.Data.Models;

namespace CartMaster.Business.IServices
{
    public interface IProductService
    {
        public string AddProduct(ProductModel productModel);
        public string UpdateProduct(ProductModel productModel);
        public List<ProductModel> GetAllProducts();
        public ProductModel GetProductById(int productId);
        public string DeleteProduct(int productId);
        public List<ProductModel> GetProductsByCategoryId(int categoryId);
    }
}
