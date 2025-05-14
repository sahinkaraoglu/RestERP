using System;
using System.Threading.Tasks;

namespace RestERP.Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
} 