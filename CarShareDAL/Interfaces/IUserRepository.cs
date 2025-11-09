using CarShareDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareDAL.Interfaces
{

    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetPendingOwnersAsync();
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(User user);
        Task<IEnumerable<User>> GetAllOwnersAsync();

    }

}
