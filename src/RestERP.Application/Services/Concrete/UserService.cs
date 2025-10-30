using Microsoft.EntityFrameworkCore;
using RestERP.Application.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using RestERP.Infrastructure.Context;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace RestERP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(int id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            
            // Debug için konsola yazdır
            Console.WriteLine($"GetUserByUsernameAsync called with username: {username}");
            Console.WriteLine($"User found: {user != null}");
            if (user != null)
            {
                Console.WriteLine($"User details - Id: {user.Id}, UserName: {user.UserName}, IsActive: {user.IsActive}");
            }
            
            return user;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }
        
        public async Task<bool> CreateUserAsync(ApplicationUser user)
        {
            try
            {
                // Not: Parola ile oluşturmak için UserManager.CreateAsync(user, password) kullanılmalı
                // Bu metot parola olmadan çalışmaz, AuthService.Register kullanın
                return false;
            }
            catch
            {
                return false;
            }
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

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    // Soft delete: sadece IsActive'i false yap
                    user.IsActive = false;
                    var result = await _userManager.UpdateAsync(user);
                    return result.Succeeded;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return null;

            return await GetUserByUsernameAsync(username);
        }
    }
} 