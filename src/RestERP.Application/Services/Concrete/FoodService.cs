using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;

namespace RestERP.Application.Services.Concrete
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IImageRepository _imageRepository;

        public FoodService(IFoodRepository foodRepository, IImageRepository imageRepository)
        {
            _foodRepository = foodRepository;
            _imageRepository = imageRepository;
        }

        public async Task<Food> CreateFoodAsync(Food food)
        {
            if (food == null)
                throw new ArgumentNullException(nameof(food));

            await _foodRepository.AddAsync(food);
            await _foodRepository.SaveChangesAsync();
            return food;
        }

        public async Task DeleteFoodAsync(int id)
        {
            var food = await _foodRepository.GetByIdAsync(id);
            if (food == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {id}");

            _foodRepository.Delete(food);
            await _foodRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Food>> GetAllFoodsAsync()
        {
            return await _foodRepository.GetAllAsync();
        }

        public async Task<Food> GetFoodByIdAsync(int id)
        {
            var food = await _foodRepository.GetByIdAsync(id);
            if (food == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {id}");

            return food;
        }

        public async Task<IEnumerable<Food>> GetFoodsByCategoryAsync(int categoryId)
        {
            return await _foodRepository.GetAsync(p => p.CategoryId == categoryId);
        }

        public async Task<IEnumerable<Food>> GetFoodsBySubCategoryAsync(int subCategoryId)
        {
            return await _foodRepository.GetAllAsync();
        }

        public async Task UpdateFoodAsync(Food food)
        {
            if (food == null)
                throw new ArgumentNullException(nameof(food));

            var existingFood = await _foodRepository.GetByIdAsync(food.Id);
            if (existingFood == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {food.Id}");

            existingFood.Name = food.Name;
            existingFood.TurkishName = food.TurkishName;
            existingFood.Description = food.Description;
            existingFood.Price = food.Price;
            existingFood.CategoryId = food.CategoryId;

            await _foodRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Image>> GetAllFoodImagesAsync()
        {
            return await _imageRepository.GetAllAsync();
        }

        public async Task SaveFoodImageAsync(int foodId, string path)
        {
            var images = await _imageRepository.GetAsync(i => i.FoodId == foodId);
            var existing = images.FirstOrDefault();

            if (existing != null)
            {
                existing.Path = path;
            }
            else
            {
                await _imageRepository.AddAsync(new Image
                {
                    FoodId = foodId,
                    Path = path
                });
            }

            await _imageRepository.SaveChangesAsync();
        }
    }
}
