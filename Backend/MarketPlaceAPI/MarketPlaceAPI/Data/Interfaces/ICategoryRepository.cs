using System.Collections.Generic;
using MarketPlaceAPI.Data.Models;

namespace MarketPlaceAPI.Data.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryById(int id);
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(int id);
    }
}
