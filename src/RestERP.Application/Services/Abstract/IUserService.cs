using RestERP.Application.Features.Users;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Services.Abstract
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser?> GetUserByIdAsync(int id);
        Task<ApplicationUser?> GetUserByUsernameAsync(string username);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<ApplicationUser?> GetCurrentUserAsync();
        Task<UserCommandResult> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<UserCommandResult> ResetPasswordAsync(int userId, string newPassword);
        Task<bool> DeleteUserAsync(int id);
    }
}
