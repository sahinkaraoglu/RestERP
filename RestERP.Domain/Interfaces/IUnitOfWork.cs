using RestERP.Domain.Common;

namespace RestERP.Domain.Interfaces
{
    /// <summary>
    /// UnitOfWork deseni için arayüz
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
} 