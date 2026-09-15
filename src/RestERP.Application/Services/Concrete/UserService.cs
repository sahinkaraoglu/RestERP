using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestERP.Application.Features.Users;
using RestERP.Application.Services.Abstract;
using RestERP.Core.Domain.Entities;

namespace RestERP.Application.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.Where(u => u.IsActive).ToListAsync();
        }

        public Task<ApplicationUser?> GetUserByIdAsync(int id)
        {
            return _userManager.FindByIdAsync(id.ToString());
        }

        public Task<ApplicationUser?> GetUserByUsernameAsync(string username)
        {
            return _userManager.FindByNameAsync(username);
        }

        public Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public async Task<ApplicationUser?> GetCurrentUserAsync()
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return null;

            return await GetUserByUsernameAsync(username);
        }

        public async Task<UserCommandResult> CreateUserAsync(ApplicationUser user, string password)
        {
            if (user == null)
                return UserCommandResult.Fail("Kullanıcı bilgisi boş olamaz.");

            if (string.IsNullOrWhiteSpace(password))
                return UserCommandResult.Fail("Şifre zorunludur.");

            var existingByEmail = await _userManager.FindByEmailAsync(user.Email);
            if (existingByEmail != null)
                return UserCommandResult.Fail("Bu e-posta adresi zaten kullanılıyor.");

            user.UserName ??= user.Email;
            user.IsActive = true;

            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
                return UserCommandResult.Fail(createResult.Errors.Select(e => e.Description));

            var roleName = user.RoleType.ToString();
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }

            var roleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!roleResult.Succeeded)
                return UserCommandResult.Fail(roleResult.Errors.Select(e => e.Description));

            return UserCommandResult.Success();
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            try
            {
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserCommandResult> ResetPasswordAsync(int userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return UserCommandResult.Fail("Kullanıcı bulunamadı.");

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
            if (!result.Succeeded)
                return UserCommandResult.Fail(result.Errors.Select(e => e.Description));

            return UserCommandResult.Success();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                    return false;

                user.IsActive = false;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }
    }
}
