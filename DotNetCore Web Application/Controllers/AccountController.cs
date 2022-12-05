using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using DotNetCore_Web_Application.Entities;
using DotNetCore_Web_Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NETCore.Encrypt.Extensions;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace DotNetCore_Web_Application.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        //readonly değişkenler sadece okunabilmektedir, set işlemi constructor da yapılabilir.
        //Çalışma zamanında sadece constructor içerisinde değer ataması yapılabilmektedir
        //kısaca account classını databasecontext sınıfına bağımlı yaptık
        private readonly DatabaseContext _databaseContext;
        private readonly IConfiguration _configuration;
		public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
		{
			_databaseContext = databaseContext;
			_configuration = configuration;
		}
        [AllowAnonymous]
		public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)//Herhangi bir hata eklenip eklenmediğini gösterir 
            {   
                string md5salt= _configuration.GetValue<string>("AppSettings:MD5Salt");
                string saltPswd=model.Password+md5salt;
                string hashedpswd=saltPswd.MD5();
                //_databaseContext.Users.FirstOrDefault(p=>p.Passowrd==hashedpswd);
                User user=_databaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Passowrd==hashedpswd );
                if(user != null)
                {
                    if(user.Locked==true)
                    {
                        ModelState.AddModelError(nameof(model.Username),"username is locked");
                        return View(model);
                    }
                    //cookie işlemleri
                    //sisteme giriş yapan kullanıcıların bilgilerinin tutulduğu ek bir yapıdır.
                    //kullanıcı hakkında key – value şeklinde hususi bilgiler tuta
                    List<Claim> claims= new List<Claim>();
                    
                    claims.Add(new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()));
                    claims.Add(new Claim("NameSurname", user.NameSurname ?? string.Empty));
                    claims.Add(new Claim("Username", user.Username));


                    ClaimsIdentity claimsIdentity =new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,claimsPrincipal);
    
                    return RedirectToAction("Index", "Home");   

                }
                else
                {
                    ModelState.AddModelError("","username or password is incorrect!");
                }
    
            }
            return View(model); //varsa hata bunları görmek için tekrardan gönderdik
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost] 
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Users.Any(u=>u.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username),"this user is already registered");
                    return View();
                }
                //appsettings.json dosyasındaki md5salt değişkenine ulaştık
                string md5salt= _configuration.GetValue<string>("AppSettings:MD5Salt");
                string saltPswd=model.Password+md5salt;
                string hashedpswd=saltPswd.MD5();
                User user = new()
                {
                   Username= model.Username,
                   Passowrd=hashedpswd
                };
                _databaseContext.Users.Add(user);   
                int affectedRowCount= _databaseContext.SaveChanges();
                if (affectedRowCount>0)
                    return RedirectToAction(nameof(Login));
                else
                    ModelState.AddModelError("","user can not be added.");//ilk parametre hangi property ile olduğu ile ilgili
            }

            return View();
        }
        public IActionResult Profil()
        {
            return View();
        }
        public IActionResult Logout()
        {       
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
