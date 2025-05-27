using RestERP.Core.Doman.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Application.Services.Interfaces
{
    public interface ITableService
    {
        Task<IEnumerable<Table>> GetAllTablesAsync();
        Task<Table> GetTableByIdAsync(int id);
        Task<Table> CreateTableAsync(Table table);
        Task UpdateTableAsync(Table table);
        Task DeleteTableAsync(int id);
        Task<bool> SetTableOccupiedStatusAsync(int id, bool isOccupied);
    }
} 