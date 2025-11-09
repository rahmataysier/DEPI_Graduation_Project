using CarShareDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareDAL.Data
{
    public class CarShareDbContext : DbContext
    {
        public CarShareDbContext(DbContextOptions<CarShareDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
