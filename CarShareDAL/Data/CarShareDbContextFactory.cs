/*using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShareDAL.Data
{
    
        public class CarShareDbContextFactory : IDesignTimeDbContextFactory<CarShareDbContext>
        {
            public CarShareDbContext CreateDbContext(string[] args)
            {
                // Path to your Presentation Layer (where appsettings.json lives)
                var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../CarSharePL");

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json")
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<CarShareDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                optionsBuilder.UseSqlServer(connectionString);

                return new CarShareDbContext(optionsBuilder.Options);
            }
        }
    }
*/