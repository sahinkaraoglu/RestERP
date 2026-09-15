using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;

namespace RestERP.Application.Services.Concrete
{
    public class FoodCategoryService : IFoodCategoryService
    {
        private readonly IFoodCategoryRepository _foodCategoryRepository;

        public FoodCategoryService(IFoodCategoryRepository foodCategoryRepository)
        {
            _foodCategoryRepository = foodCategoryRepository;
        }

        public async Task<FoodCategory> CreateCategoryAsync(FoodCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            await _foodCategoryRepository.AddAsync(category);
            await _foodCategoryRepository.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _foodCategoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Kategori bulunamadı. Id: {id}");

            _foodCategoryRepository.Delete(category);
            await _foodCategoryRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<FoodCategory>> GetAllCategoriesAsync()
        {
            return await _foodCategoryRepository.GetAllAsync();
        }

        public async Task<FoodCategory> GetCategoryByIdAsync(int id)
        {
            var category = await _foodCategoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Kategori bulunamadı. Id: {id}");

            return category;
        }

        public async Task UpdateCategoryAsync(FoodCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var existingCategory = await _foodCategoryRepository.GetByIdAsync(category.Id);
            if (existingCategory == null)
                throw new KeyNotFoundException($"Kategori bulunamadı. Id: {category.Id}");

            existingCategory.Name = category.Name;
            existingCategory.TurkishName = category.TurkishName;
            existingCategory.Description = category.Description;

            _foodCategoryRepository.Update(existingCategory);
            await _foodCategoryRepository.SaveChangesAsync();
        }
    }
}
