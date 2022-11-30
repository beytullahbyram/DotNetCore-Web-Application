using DotNetCore_Web_Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_Web_Application.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)//Herhangi bir hata eklenip eklenmediğini gösterir 
            {
                //login işlemleri
            }
            return View(model); //varsa hata bunları görmek için tekrardan gönderdik
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost] 
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //register işlemleri
            }
            return View();
        }
        public IActionResult Profil()
        {
            return View();
        }
    }
}
