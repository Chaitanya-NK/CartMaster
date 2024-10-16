using CartMaster.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Data.IRepositories
{
    public interface ICategoryRepository
    {
        public string AddCategory(CategoryModel categoryModel, IFormFile imageURL);
        public string UpdateCategory(CategoryModel categoryModel, IFormFile imageURL);
        public string DeleteCategory(int categoryId);
        public List<CategoryModel> GetAllCategories();
        public CategoryModel GetCategoryById(int categoryId);
    }
}
