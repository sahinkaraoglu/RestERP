using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RestERP.Core.Interfaces;
using RestERP.Core.Domain.Entities.Base;
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
            await _dbContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            entity.IsDeleted = true;
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

    public async Task<bool> ExistsAsync(int id)
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        return await _dbContext.Set<T>().AnyAsync(e => e.Id == id);
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(bool includeDeleted)
    {
        // includeDeleted = true ise global query filter'ı devre dışı bırak
        var query = _dbContext.Set<T>().AsQueryable();
        
        if (includeDeleted)
            query = query.IgnoreQueryFilters();
            
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        return await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy)
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        IQueryable<T> query = _dbContext.Set<T>().Where(predicate);
        
        if (orderBy != null)
            query = orderBy(query);
            
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, string includeString)
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        IQueryable<T> query = _dbContext.Set<T>().Where(predicate);
        
        if (!string.IsNullOrWhiteSpace(includeString))
            query = query.Include(includeString);
        
        if (orderBy != null)
            query = orderBy(query);
            
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, List<Expression<Func<T, object>>>? includes)
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        IQueryable<T> query = _dbContext.Set<T>().Where(predicate);
        
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        
        if (orderBy != null)
            query = orderBy(query);
            
        return await query.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        return await _dbContext.Set<T>()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<T?> GetByIdAsync(int id, bool includeDeleted)
    {
        // includeDeleted = true ise global query filter'ı devre dışı bırak
        var query = _dbContext.Set<T>().AsQueryable();
        
        if (includeDeleted)
            query = query.IgnoreQueryFilters();
            
        return await query.Where(e => e.Id == id).FirstOrDefaultAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        return await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, string includeString)
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        IQueryable<T> query = _dbContext.Set<T>().Where(predicate);
        
        if (!string.IsNullOrWhiteSpace(includeString))
            query = query.Include(includeString);
            
        return await query.FirstOrDefaultAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes)
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        IQueryable<T> query = _dbContext.Set<T>().Where(predicate);
        
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
            
        return await query.FirstOrDefaultAsync();
    }

    public void Update(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
    }
    
    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        // Global query filter otomatik olarak !e.IsDeleted kontrolü yapıyor
        return await _dbContext.Set<T>().Where(predicate).CountAsync();
    }
    }
} 