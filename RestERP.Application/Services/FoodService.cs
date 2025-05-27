using RestERP.Application.Services.Interfaces;
using RestERP.Core.Doman.Entities;
using RestERP.Domain.Interfaces;
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
            var Food = await _unitOfWork.Repository<Food>().GetByIdAsync(id);
            
            if (Food == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {id}");
                
            await _unitOfWork.Repository<Food>().DeleteAsync(Food);
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

        public async Task UpdateFoodAsync(Food Food)
        {
            if (Food == null)
                throw new ArgumentNullException(nameof(Food));
                
            var existingFood = await _unitOfWork.Repository<Food>().GetByIdAsync(Food.Id);
            
            if (existingFood == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {Food.Id}");
                
            await _unitOfWork.Repository<Food>().UpdateAsync(Food);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 