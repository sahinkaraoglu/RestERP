using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RestERP.Domain.Interfaces;
using RestERP.Core.Doman.Entities.Base;
using RestERP.Infrastructure.Context;

namespace RestERP.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly RestERPDbContext _dbContext;

        public Repository(RestERPDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> AddAsync(T entity)
        {
            entity.CreatedDate = DateTime.UtcNow;
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            entity.IsDeleted = true;
            entity.UpdatedDate = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Set<T>().AnyAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().Where(e => !e.IsDeleted).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).Where(e => !e.IsDeleted).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(predicate).Where(e => !e.IsDeleted);
            return await orderBy(query).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeString)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(predicate).Where(e => !e.IsDeleted);
            
            if (!string.IsNullOrWhiteSpace(includeString))
                query = query.Include(includeString);
                
            return await orderBy(query).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, List<Expression<Func<T, object>>> includes)
        {
            IQueryable<T> query = _dbContext.Set<T>().Where(predicate).Where(e => !e.IsDeleted);
            
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
                
            return await orderBy(query).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

        public async Task UpdateAsync(T entity)
        {
            entity.UpdatedDate = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }
        
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().Where(predicate).Where(e => !e.IsDeleted).CountAsync();
        }
    }
} 