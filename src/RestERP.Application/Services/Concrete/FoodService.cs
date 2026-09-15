using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestERP.Application.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly IFoodCategoryRepository _foodCategoryRepository;
        private readonly IImageRepository _imageRepository;

        public FoodService(
            IFoodRepository foodRepository,
            IFoodCategoryRepository foodCategoryRepository,
            IImageRepository imageRepository)
        {
            _foodRepository = foodRepository;
            _foodCategoryRepository = foodCategoryRepository;
            _imageRepository = imageRepository;
        }

        public async Task<Food> CreateFoodAsync(Food Food)
        {
            if (Food == null)
                throw new ArgumentNullException(nameof(Food));

            await _foodRepository.AddAsync(Food);
            await _foodRepository.SaveChangesAsync();
            return Food;
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
            var Food = await _foodRepository.GetByIdAsync(id);
            
            if (Food == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {id}");
                
            return Food;
        }

        public async Task<IEnumerable<Food>> GetFoodsByCategoryAsync(int categoryId)
        {
            return await _foodRepository.GetAsync(p => p.CategoryId == categoryId);
        }

        public async Task<IEnumerable<Food>> GetFoodsBySubCategoryAsync(int subCategoryId)
        {
            // Alt kategoriye göre ürün filtreleme işlemini gerçekleştir
            // Burada SubCategory-Food ilişkisi farklı olabileceğinden dolayı
            // doğrudan filtreleme yerine ilişkili tabloları kullanmamız gerekebilir
            
            // Şu anlık doğrudan CategoryId ile filtreliyoruz
            // İleride alt kategori ilişkisi eklendiğinde güncellenecek
            return await _foodRepository.GetAllAsync();
        }

        public async Task UpdateFoodAsync(Food food)
        {
            if (food == null)
                throw new ArgumentNullException(nameof(food));
                
            var existingFood = await _foodRepository.GetByIdAsync(food.Id);
            
            if (existingFood == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {food.Id}");
            
            // Entity zaten tracked olduğu için property'leri güncellemek yeterli
            existingFood.Name = food.Name;
            existingFood.TurkishName = food.TurkishName;
            existingFood.Description = food.Description;
            existingFood.Price = food.Price;
            existingFood.CategoryId = food.CategoryId;
                
            // Update çağrısı gereksiz - Entity zaten tracked, SaveChanges yeterli
            await _foodRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<FoodCategory>> GetAllFoodCategoriesAsync()
        {
            return await _foodCategoryRepository.GetAllAsync();
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
