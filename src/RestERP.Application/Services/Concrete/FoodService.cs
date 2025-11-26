using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestERP.Application.Services
{
    public class FoodService : IFoodService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Food> CreateFoodAsync(Food Food)
        {
            if (Food == null)
                throw new ArgumentNullException(nameof(Food));

            await _unitOfWork.Repository<Food>().AddAsync(Food);
            await _unitOfWork.SaveChangesAsync();
            return Food;
        }

        public async Task DeleteFoodAsync(int id)
        {
            var food = await _unitOfWork.Repository<Food>().GetByIdAsync(id);
            
            if (food == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {id}");
                
            _unitOfWork.Repository<Food>().Delete(food);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Food>> GetAllFoodsAsync()
        {
            return await _unitOfWork.Repository<Food>().GetAllAsync();
        }

        public async Task<Food> GetFoodByIdAsync(int id)
        {
            var Food = await _unitOfWork.Repository<Food>().GetByIdAsync(id);
            
            if (Food == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {id}");
                
            return Food;
        }

        public async Task<IEnumerable<Food>> GetFoodsByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.Repository<Food>().GetAsync(p => p.CategoryId == categoryId);
        }

        public async Task<IEnumerable<Food>> GetFoodsBySubCategoryAsync(int subCategoryId)
        {
            // Alt kategoriye göre ürün filtreleme işlemini gerçekleştir
            // Burada SubCategory-Food ilişkisi farklı olabileceğinden dolayı
            // doğrudan filtreleme yerine ilişkili tabloları kullanmamız gerekebilir
            
            // Şu anlık doğrudan CategoryId ile filtreliyoruz
            // İleride alt kategori ilişkisi eklendiğinde güncellenecek
            return await _unitOfWork.Repository<Food>().GetAllAsync();
        }

        public async Task UpdateFoodAsync(Food food)
        {
            if (food == null)
                throw new ArgumentNullException(nameof(food));
                
            var existingFood = await _unitOfWork.Repository<Food>().GetByIdAsync(food.Id);
            
            if (existingFood == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {food.Id}");
            
            // Entity Tracking sorunu için mevcut entity üzerinde güncelleme yap
            existingFood.Name = food.Name;
            existingFood.TurkishName = food.TurkishName;
            existingFood.Description = food.Description;
            existingFood.Price = food.Price;
            existingFood.CategoryId = food.CategoryId;
                
            _unitOfWork.Repository<Food>().Update(existingFood);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<FoodCategory>> GetAllFoodCategoriesAsync()
        {
            return await _unitOfWork.Repository<FoodCategory>().GetAllAsync();
        }

        public async Task<IEnumerable<Image>> GetAllFoodImagesAsync()
        {
            return await _unitOfWork.Repository<Image>().GetAllAsync();
        }
    }
} 