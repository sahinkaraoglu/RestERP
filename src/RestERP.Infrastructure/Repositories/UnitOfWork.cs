using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RestERP.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestERP.Core.Domain.Entities.Base;
using RestERP.Infrastructure.Context;

namespace RestERP.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RestERPDbContext _context;
        private readonly ConcurrentDictionary<string, object> _repositories;
        private IDbContextTransaction _transaction;

        public UnitOfWork(RestERPDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();
        }

        public async Task<int> SaveChangesAsync()
        {
            // Otomatik olarak oluşturulma ve güncellenme tarih bilgilerini set et
            foreach (var entry in _context.ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.UtcNow;
                        break;
                }
            }
            
            return await _context.SaveChangesAsync();
        }

        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            var type = typeof(T).Name;

            return (IRepository<T>)_repositories.GetOrAdd(type, _ =>
                Activator.CreateInstance(typeof(Repository<>).MakeGenericType(typeof(T)), _context)!);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException("Transaction başlatılmamış. Önce BeginTransactionAsync çağrılmalı.");
                
            try
            {
                await SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
            
            _context?.Dispose();
        }
    }
} 