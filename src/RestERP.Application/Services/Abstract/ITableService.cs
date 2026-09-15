using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Services.Abstract
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
