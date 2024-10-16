using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public string AddProduct(ProductModel productModel, IFormFile imageURL)
        {
            return _productRepository.AddProduct(productModel, imageURL);
        }

        public string DeleteProduct(int productId)
        {
            return _productRepository.DeleteProduct(productId);
        }

        public List<ProductModel> GetAllProducts()
        {
            return _productRepository.GetAllProducts();
        }

        public ProductModel GetProductById(int productId)
        {
            return _productRepository.GetProductById(productId);
        }

        public List<ProductModel> GetProductsByCategoryId(int categoryId)
        {
            return _productRepository.GetProductsByCategoryId(categoryId);
        }

        public string UpdateProduct(ProductModel productModel, IFormFile imageURL)
        {
            return _productRepository.UpdateProduct(productModel, imageURL);
        }
    }
}
