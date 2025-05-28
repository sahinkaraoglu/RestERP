using Microsoft.EntityFrameworkCore;
using RestERP.Application.Services.Interfaces;
using RestERP.Core.Doman.Entities;
using RestERP.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestERP.Application.Services
{
    public class UserService : IUserService
    {
        private readonly RestERPDbContext _context;

        public UserService(RestERPDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.ApplicationUsers.ToListAsync();
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _context.ApplicationUsers.FindAsync(id);
        }

        public async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == email);
        }
        
        public async Task<bool> CreateUserAsync(ApplicationUser user)
        {
            try
            {
                await _context.ApplicationUsers.AddAsync(user);
                await _context.SaveChangesAsync();
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
                var existingUser = await _context.ApplicationUsers.FindAsync(user.Id);
                if (existingUser == null)
                    return false;

                _context.Entry(existingUser).CurrentValues.SetValues(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            try
            {
                var user = await _context.ApplicationUsers.FindAsync(id);
                if (user == null)
                    return false;

                _context.ApplicationUsers.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
} 