
using CarShareBLL.Helpers;
using CarShareBLL.Interfaces;
using CarShareBLL.Services;
using CarShareDAL.Data;
using CarShareDAL.Interfaces;
using CarShareDAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CarSharePL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //DB
            builder.Services.AddDbContext<CarShareDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // DI
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAdminService, AdminService>();




            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
