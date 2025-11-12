namespace CarShareMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // For MVC
            builder.Services.AddControllers();          // For API
            builder.Services.AddEndpointsApiExplorer(); // For Swagger (API documentation)
            // builder.Services.AddSwaggerGen();           // API documentation

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            app.UseCors("AllowAll");
            app.UseSession();
    
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapControllers(); // For API controllers

            app.Run();
        }
    }
}
