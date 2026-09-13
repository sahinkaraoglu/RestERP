using RestERP.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestERP.Application.Services.Abstract
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(int id);
        Task<ApplicationUser> GetUserByUsernameAsync(string username);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<bool> CreateUserAsync(ApplicationUser user);
        Task<(bool Succeeded, IReadOnlyList<string> Errors)> CreateUserWithPasswordAsync(ApplicationUser user, string password);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<(bool Succeeded, IReadOnlyList<string> Errors)> ResetPasswordAsync(int userId, string newPassword);
        Task<bool> DeleteUserAsync(int id);
        Task<ApplicationUser> GetCurrentUserAsync();
    }
} 