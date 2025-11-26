using RestERP.Core.Domain.Entities.Base;
using System.Linq.Expressions;

namespace RestERP.Core.Interfaces
{
    /// <summary>
    /// Generic repository arayüzü
    /// </summary>
    /// <typeparam name="T">Entity türü</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<T?> GetByIdAsync(int id, bool includeDeleted);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(bool includeDeleted);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, string includeString);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, List<Expression<Func<T, object>>>? includes);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, string includeString);
        Task<T?> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate, List<Expression<Func<T, object>>> includes);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
} 