using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RestERP.Domain.Entities.Base;

namespace RestERP.Infrastructure.Repositories
{
    public class Repository<T> : RestERP.Domain.Interfaces.IRepository<T> where T : BaseEntity
    {
        protected readonly RestERPDbContext _dbContext;

        public Repository(RestERPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            // int türüne çevirme gerekebilir, proje yapısına göre kontrol edilmeli
            return await _dbContext.Set<T>().AnyAsync(e => e.Id == Convert.ToInt32(id));
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            // int türüne çevirme gerekebilir, proje yapısına göre kontrol edilmeli
            return await _dbContext.Set<T>().FindAsync(Convert.ToInt32(id));
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }
    }
} 