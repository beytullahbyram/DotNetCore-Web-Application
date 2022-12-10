using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_Web_Application.Controllers
{
    [Authorize(Roles ="admin")]//admin rolüne sahip kiişiler bu sayfaya erişebilecek
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
