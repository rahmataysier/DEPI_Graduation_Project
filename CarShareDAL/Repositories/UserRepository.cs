using CarShareDAL.Interfaces;
using CarShareDAL.Models;
using CarShareDAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarShareDbContext _context;
        public UserRepository(CarShareDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetPendingOwnersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "Owner" && !u.IsApproved)
                .OrderBy(u => u.CreatedAt)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllOwnersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "Owner")
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();
        }
    }
}