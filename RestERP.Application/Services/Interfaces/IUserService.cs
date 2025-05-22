using RestERP.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(ApplicationUser user);
    }
} 