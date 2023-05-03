using DotNetCore_Web_Application.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DotNetCore_Web_Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.       //ekledi�imiz servisi devreye al�yoruz
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //�al��an assembly dosyas�n� tarayarak Map dosyas�n� �al��t�r�r.
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); 
            builder.Services.AddDbContext<DatabaseContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                    ,opts => opts.EnableRetryOnFailure());  
            });
            
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opts=>
                {
                    //kullan�c�n�n taray�c�s�nda cookie bilgileri bu isimle tutulacak
                    opts.Cookie.Name=".DotNetCoreWebApplication.auth";
                    //ne kadar s�re tutulacak
                    opts.ExpireTimeSpan=TimeSpan.FromMinutes(2.0);
                    //cookie s�resinin s�rekli uzamas�n� sa�lamak i�in
                    opts.SlidingExpiration=false;
                    //kullan�c� login de�ilse path y�nlendirmesi
                    opts.LoginPath="/Account/Login";
                    opts.LogoutPath="/Account/Logout";
                    //yetkisi olmad���nda gidece�i sayfa
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