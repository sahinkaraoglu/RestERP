using RestERP.Core.Doman.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Application.Services.Interfaces
{
    public interface IFoodService
    {
        Task<IEnumerable<Food>> GetAllFoodsAsync();
        Task<Food> GetFoodByIdAsync(int id);
        Task<IEnumerable<Food>> GetFoodsByCategoryAsync(int categoryId);
        Task<IEnumerable<Food>> GetFoodsBySubCategoryAsync(int subCategoryId);
        Task<Food> CreateFoodAsync(Food food);
        Task UpdateFoodAsync(Food food);
        Task DeleteFoodAsync(int id);
        Task<IEnumerable<FoodCategory>> GetAllFoodCategoriesAsync();
        Task<IEnumerable<Image>> GetAllFoodImagesAsync();
    }
} 