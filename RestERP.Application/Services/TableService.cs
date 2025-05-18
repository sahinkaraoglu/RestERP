using Microsoft.Extensions.Logging;
using RestERP.Application.Services.Interfaces;
using RestERP.Domain.Entities;
using RestERP.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Application.Services
{
    public class TableService : ITableService
    {
        private readonly IRepository<Table> _tableRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TableService> _logger;

        public TableService(IRepository<Table> tableRepository, IUnitOfWork unitOfWork, ILogger<TableService> logger)
        {
            _tableRepository = tableRepository ?? throw new ArgumentNullException(nameof(tableRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Table>> GetAllTablesAsync()
        {
            try
            {
                var tables = await _tableRepository.GetAllAsync();
                return tables;
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
                await _unitOfWork.SaveChangesAsync();
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
                await _tableRepository.UpdateAsync(table);
                await _unitOfWork.SaveChangesAsync();
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

                await _tableRepository.DeleteAsync(table);
                await _unitOfWork.SaveChangesAsync();
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
                await _tableRepository.UpdateAsync(table);
                await _unitOfWork.SaveChangesAsync();
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