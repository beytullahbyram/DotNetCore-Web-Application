using DotNetCore_Web_Application.Entities;
using DotNetCore_Web_Application.Helpers;
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

            // Add services to the container.       //we activate the service we added
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //Runs the Map file by scanning the running assembly file.
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); 
            builder.Services.AddDbContext<DatabaseContext>(opts =>
            {
                opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
                    ,opts => opts.EnableRetryOnFailure());  
            });
            
            //using HasherMD5 class when IHasherMD5 interfaces called
            builder.Services.AddScoped<IHasherMD5,HasherMD5>();

            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(opts=>
                {
                    //cookie information will be stored in the user's browser with this name
                    opts.Cookie.Name=".DotNetCoreWebApplication.auth";
                    //how long will it be kept
                    opts.ExpireTimeSpan=TimeSpan.FromMinutes(10.0);
                    //To ensure the continuous extension of the cookie period
                    opts.SlidingExpiration=false;
                    //path redirection if user is not logged in
                    opts.LoginPath="/Account/Login";
                    opts.LogoutPath="/Account/Logout";
                    //page to go when not authorized
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