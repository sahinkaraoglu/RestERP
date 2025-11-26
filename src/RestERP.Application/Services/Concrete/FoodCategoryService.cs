using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces;
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
                
            _unitOfWork.Repository<FoodCategory>().Delete(category);
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
            
            // Entity Tracking sorunu için mevcut entity üzerinde güncelleme yap
            existingCategory.Name = category.Name;
            existingCategory.TurkishName = category.TurkishName;
            existingCategory.Description = category.Description;
                
            _unitOfWork.Repository<FoodCategory>().Update(existingCategory);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 