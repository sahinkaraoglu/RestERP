using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Services.Abstract
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
        Task<IEnumerable<Image>> GetAllFoodImagesAsync();
        Task SaveFoodImageAsync(int foodId, string path);
    }
}
