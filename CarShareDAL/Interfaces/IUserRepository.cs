using CarShareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareDAL.Interfaces
{

    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetPendingOwnersAsync();
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllOwnersAsync();

        // User-Auth methods
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
    }

}
