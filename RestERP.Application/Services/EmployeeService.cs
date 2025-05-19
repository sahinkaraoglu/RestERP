using RestERP.Domain.Entities;
using RestERP.Domain.Interfaces;
using RestERP.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _unitOfWork.Repository<Employee>().GetAllAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
            if (employee == null)
                throw new KeyNotFoundException($"Çalışan bulunamadı. Id: {id}");
            return employee;
        }

        public async Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            await _unitOfWork.Repository<Employee>().AddAsync(employee);
            await _unitOfWork.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            var existingEmployee = await _unitOfWork.Repository<Employee>().GetByIdAsync(employee.Id);
            if (existingEmployee == null)
                return false;

            existingEmployee.Name = employee.Name;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.Email = employee.Email;
            existingEmployee.Role = employee.Role;
            existingEmployee.IsActive = employee.IsActive;

            await _unitOfWork.Repository<Employee>().UpdateAsync(existingEmployee);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employee = await _unitOfWork.Repository<Employee>().GetByIdAsync(id);
            if (employee == null)
                return false;

            await _unitOfWork.Repository<Employee>().DeleteAsync(employee);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}