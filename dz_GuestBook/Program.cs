using dz_GuestBook.Models;
using Microsoft.EntityFrameworkCore;

namespace dz_GuestBook
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10); // Длительность сеанса (тайм-аут завершения сеанса)
                options.Cookie.Name = "Session"; // Каждая сессия имеет свой идентификатор, который сохраняется в куках.

            });

            string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<GuestBookContext>(options => options.UseSqlServer(connection));


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();
            app.UseSession();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            //if (!app.Environment.IsDevelopment())
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //    The default HSTS value is 30 days.You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();
           

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
