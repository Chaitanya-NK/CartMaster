using CartMaster.Business.IServices;
using CartMaster.Data.IRepositories;
using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public string AddCategory(CategoryModel categoryModel)
        {
            return _categoryRepository.AddCategory(categoryModel);
        }

        public string DeleteCategory(int categoryId)
        {
            return _categoryRepository.DeleteCategory(categoryId);
        }

        public List<CategoryModel> GetAllCategories()
        {
            return _categoryRepository.GetAllCategories();
        }

        public CategoryModel GetCategoryById(int categoryId)
        {
            return _categoryRepository.GetCategoryById(categoryId);
        }

        public string UpdateCategory(CategoryModel categoryModel)
        {
            return _categoryRepository.UpdateCategory(categoryModel);
        }
    }
}
