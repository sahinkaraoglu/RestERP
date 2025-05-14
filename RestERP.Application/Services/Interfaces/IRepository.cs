using RestERP.Domain.Common;
using System.Linq.Expressions;

namespace RestERP.Domain.Interfaces
{
    /// <summary>
    /// Generic repository arayüzü
    /// </summary>
    /// <typeparam name="T">Entity türü</typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Guid id);
    }
} 