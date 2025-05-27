using RestERP.Core.Doman.Entities.Base;
using System.Linq.Expressions;

namespace RestERP.Domain.Interfaces
{
    /// <summary>
    /// Generic repository arayüzü
    /// </summary>
    /// <typeparam name="T">Entity türü</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, string includeString);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy, List<Expression<Func<T, object>>> includes);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(int id);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
    }
} 