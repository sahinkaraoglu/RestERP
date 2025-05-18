using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Entities;
using RestERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Application.Services
{
    public class FoodCategoryService : IFoodCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FoodCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<FoodCategory> CreateCategoryAsync(FoodCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            await _unitOfWork.Repository<FoodCategory>().AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Repository<FoodCategory>().GetByIdAsync(id);
            
            if (category == null)
                throw new KeyNotFoundException($"Kategori bulunamadı. Id: {id}");
                
            await _unitOfWork.Repository<FoodCategory>().DeleteAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<FoodCategory>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.Repository<FoodCategory>().GetAllAsync();
        }

        public async Task<FoodCategory> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Repository<FoodCategory>().GetByIdAsync(id);
            
            if (category == null)
                throw new KeyNotFoundException($"Kategori bulunamadı. Id: {id}");
                
            return category;
        }

        public async Task UpdateCategoryAsync(FoodCategory category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));
                
            var existingCategory = await _unitOfWork.Repository<FoodCategory>().GetByIdAsync(category.Id);
            
            if (existingCategory == null)
                throw new KeyNotFoundException($"Kategori bulunamadı. Id: {category.Id}");
                
            await _unitOfWork.Repository<FoodCategory>().UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 