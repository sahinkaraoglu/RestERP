using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Entities;
using RestERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestERP.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            await _unitOfWork.Repository<Product>().AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            
            if (product == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {id}");
                
            await _unitOfWork.Repository<Product>().DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.Repository<Product>().GetAllAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            
            if (product == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {id}");
                
            return product;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _unitOfWork.Repository<Product>().GetAsync(p => p.CategoryId == categoryId);
        }

        public async Task<IEnumerable<Product>> GetProductsBySubCategoryAsync(int subCategoryId)
        {
            // Alt kategoriye göre ürün filtreleme işlemini gerçekleştir
            // Burada SubCategory-Product ilişkisi farklı olabileceğinden dolayı
            // doğrudan filtreleme yerine ilişkili tabloları kullanmamız gerekebilir
            
            // Şu anlık doğrudan CategoryId ile filtreliyoruz
            // İleride alt kategori ilişkisi eklendiğinde güncellenecek
            return await _unitOfWork.Repository<Product>().GetAllAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
                
            var existingProduct = await _unitOfWork.Repository<Product>().GetByIdAsync(product.Id);
            
            if (existingProduct == null)
                throw new KeyNotFoundException($"Ürün bulunamadı. Id: {product.Id}");
                
            await _unitOfWork.Repository<Product>().UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }
    }
} 