using RestERP.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Application.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}