
using ANP_Academy.DAL.Models;
using ANP_Academy.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ANP_Academy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Configure the ConnectionString and DbContext class
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<AnpdesarrolloContext>(options => options.UseSqlServer(connectionString));


            //Config of the Identity Conecction
            builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<Usuario ,IdentityRole >(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultUI();



            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.MapRazorPages();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}