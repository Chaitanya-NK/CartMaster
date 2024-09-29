using CartMaster.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartMaster.Business.IServices
{
    public interface ICategoryService
    {
        public string AddCategory(CategoryModel categoryModel);
        public string UpdateCategory(CategoryModel categoryModel);
        public string DeleteCategory(int categoryId);
        public List<CategoryModel> GetAllCategories();
        public CategoryModel GetCategoryById(int categoryId);
    }
}
