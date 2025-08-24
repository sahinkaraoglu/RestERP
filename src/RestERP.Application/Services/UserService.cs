using Microsoft.EntityFrameworkCore;
using RestERP.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using RestERP.Infrastructure.Context;
using RestERP.Core.Domain.Entities;
using RestERP.Core.Interfaces;

namespace RestERP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            var result = await _unitOfWork.Repository<ApplicationUser>().GetAllAsync();
            return result.ToList();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.Repository<ApplicationUser>().GetByIdAsync(id);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            var users = await _unitOfWork.Repository<ApplicationUser>().GetAllAsync();
            var user = users.FirstOrDefault(u => u.UserName == username);
            
            // Debug için konsola yazdır
            Console.WriteLine($"GetUserByUsernameAsync called with username: {username}");
            Console.WriteLine($"Total users found: {users.Count()}");
            Console.WriteLine($"User found: {user != null}");
            if (user != null)
            {
                Console.WriteLine($"User details - Id: {user.Id}, UserName: {user.UserName}, IsActive: {user.IsActive}");
            }
            
            return user;
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            var users = await _unitOfWork.Repository<ApplicationUser>().GetAllAsync();
            return users.FirstOrDefault(u => u.Email == email);
        }
        
        public async Task<bool> CreateUserAsync(ApplicationUser user)
        {
            try
            {
                await _unitOfWork.Repository<ApplicationUser>().AddAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
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
                await _unitOfWork.Repository<ApplicationUser>().UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
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
                var user = await _unitOfWork.Repository<ApplicationUser>().GetByIdAsync(id);
                if (user != null)
                {
                    await _unitOfWork.Repository<ApplicationUser>().DeleteAsync(user);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
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