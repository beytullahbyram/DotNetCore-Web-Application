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
using System;
using System.ComponentModel.DataAnnotations;
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
				string hashedpswd = HashedString(model.Password);
				//_databaseContext.Users.FirstOrDefault(p=>p.Passowrd==hashedpswd);
				User user = _databaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Passowrd == hashedpswd);
				if (user != null)
				{
					if (user.Locked == true)
					{
						ModelState.AddModelError(nameof(model.Username), "username is locked");
						return View(model);
					}
					//cookie işlemleri
					//sisteme giriş yapan kullanıcıların bilgilerinin tutulduğu ek bir yapıdır.
					//kullanıcı hakkında key – value şeklinde hususi bilgiler tuta
					List<Claim> claims = new List<Claim>();

					claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
					claims.Add(new Claim("NameSurname", user.NameSurname ?? string.Empty));
					claims.Add(new Claim(ClaimTypes.Role, user.Role));
					claims.Add(new Claim("Username", user.Username));


					ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

					return RedirectToAction("Index", "Home");

				}
				else
				{
					ModelState.AddModelError("", "username or password is incorrect!");
				}

			}
			return View(model); //varsa hata bunları görmek için tekrardan gönderdik
        }

		private string HashedString(string model)
		{
			string md5salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
			string salted = model+ md5salt;
			string hashed = salted.MD5();
			return hashed;
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
                string haspswd= HashedString(model.Password);
                User user = new()
                {
                   Username= model.Username,
                   Passowrd=haspswd
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
			ProfileInfoLoader();

			return View();
		}

		private void ProfileInfoLoader()
		{
			Guid userid = new Guid(User.FindFirstValue(ClaimTypes.NameIdentifier));
			User user = _databaseContext.Users.FirstOrDefault(x => x.Id == userid);
			ViewData["FullName"] = user.NameSurname;
		}


		[HttpPost]
        public IActionResult ProfilChangeFullname([Required][StringLength(50)] string? fullname)//inputun nameini buraya aldık
        {
            if (ModelState.IsValid)
            {
                Guid userid=new Guid( User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user= _databaseContext.Users.FirstOrDefault(x => x.Id== userid);
                user.NameSurname=fullname;
                _databaseContext.SaveChanges();
                return RedirectToAction(nameof(Profil));
            }
			ProfileInfoLoader();
            //hata aldığımızda sayfa tekrar yükleniyor ve var olan değişmeyen kullanıcı adımız inputta gözükmüyordu
            //o yüzden bunu methoda aldıgımızda isvalide false olsa bile viewdatada ismimiz gözükecek
            return View("Profil");
        }

        [HttpPost]
        public IActionResult ProfilChangePassword([Required][MinLength(6)][MaxLength(16)] string? password)//inputun nameini buraya aldık
        {
            if (ModelState.IsValid)
            {
                Guid userid=new Guid( User.FindFirstValue(ClaimTypes.NameIdentifier));
                User user= _databaseContext.Users.FirstOrDefault(x => x.Id== userid);
                string hashedpswd=HashedString(password);
                user.Passowrd=hashedpswd;
                _databaseContext.SaveChanges();
                //return RedirectToAction(nameof(Profil));
                ViewData["result"]="ChangePasswordOK";
            }
			ProfileInfoLoader();
            //hata aldığımızda sayfa tekrar yükleniyor ve var olan değişmeyen kullanıcı adımız inputta gözükmüyordu
            //o yüzden bunu methoda aldıgımızda isvalide false olsa bile viewdatada ismimiz gözükecek
            return View("Profil");
        }

        public IActionResult Logout()
        {       
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
