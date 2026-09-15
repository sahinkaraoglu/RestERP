using Microsoft.Extensions.Logging;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces.Repositories;

namespace RestERP.Application.Services.Concrete
{
    public class TableService : ITableService
    {
        private readonly ITableRepository _tableRepository;
        private readonly ILogger<TableService> _logger;

        public TableService(ITableRepository tableRepository, ILogger<TableService> logger)
        {
            _tableRepository = tableRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            try
            {
                return await _tableRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masalar alınırken hata oluştu");
                throw;
            }
        }

        public async Task<Table> GetTableByIdAsync(int id)
        {
            try
            {
                var table = await _tableRepository.GetByIdAsync(id);
                if (table == null)
                {
                    _logger.LogWarning("Masa bulunamadı: {Id}", id);
                    throw new KeyNotFoundException($"ID'si {id} olan masa bulunamadı");
                }

                return table;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Masa alınırken hata oluştu: {Id}", id);
                throw;
            }
        }

        public async Task<Table> CreateTableAsync(Table table)
        {
            try
            {
                var newTable = await _tableRepository.AddAsync(table);
                await _tableRepository.SaveChangesAsync();
                return newTable;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masa oluşturulurken hata oluştu");
                throw;
            }
        }

        public async Task UpdateTableAsync(Table table)
        {
            try
            {
                _tableRepository.Update(table);
                await _tableRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Masa güncellenirken hata oluştu: {Id}", table.Id);
                throw;
            }
        }

        public async Task DeleteTableAsync(int id)
        {
            try
            {
                var table = await _tableRepository.GetByIdAsync(id);
                if (table == null)
                {
                    _logger.LogWarning("Silinecek masa bulunamadı: {Id}", id);
                    throw new KeyNotFoundException($"ID'si {id} olan masa bulunamadı");
                }

                _tableRepository.Delete(table);
                await _tableRepository.SaveChangesAsync();
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Masa silinirken hata oluştu: {Id}", id);
                throw;
            }
        }

        public async Task<bool> SetTableOccupiedStatusAsync(int id, bool isOccupied)
        {
            try
            {
                var table = await _tableRepository.GetByIdAsync(id);
                if (table == null)
                {
                    _logger.LogWarning("Masa bulunamadı: {Id}", id);
                    throw new KeyNotFoundException($"ID'si {id} olan masa bulunamadı");
                }

                table.IsOccupied = isOccupied;
                _tableRepository.Update(table);
                await _tableRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) when (ex is not KeyNotFoundException)
            {
                _logger.LogError(ex, "Masa durumu güncellenirken hata oluştu: {Id}", id);
                throw;
            }
        }
    }
}
