using RestERP.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Application.Services.Abstract
{
    public interface IFoodCategoryService
    {
        Task<IEnumerable<FoodCategory>> GetAllCategoriesAsync();
        Task<FoodCategory> GetCategoryByIdAsync(int id);
        Task<FoodCategory> CreateCategoryAsync(FoodCategory category);
        Task UpdateCategoryAsync(FoodCategory category);
        Task DeleteCategoryAsync(int id);
    }
} 