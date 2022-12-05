using DotNetCore_Web_Application.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore_Web_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.       //eklediðimiz servisi devreye alýyoruz
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddDbContext<DatabaseContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opts=>
                {
                    //kullanýcýnýn tarayýcýsýnda cookie bilgileri bu isimle tutulacak
                    opts.Cookie.Name=".DotNetCoreWebApplication.auth";
                    //ne kadar süre tutulacak
                    opts.ExpireTimeSpan=TimeSpan.FromDays(7);
                    //cookie süresinin sürekli uzamasýný saðlamak için
                    opts.SlidingExpiration=false;
                    //kullanýcý login deðilse path yönlendirmesi
                    opts.LoginPath="/Account/Login";
                    opts.LoginPath="/Account/Logout";
                    //yetkisi olmadýðýnda gideceði sayfa
                    opts.AccessDeniedPath="/Home/AccessDenied";
                 });

            
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}