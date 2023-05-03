using AutoMapper;
using DotNetCore_Web_Application.Entities;
using DotNetCore_Web_Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_Web_Application.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;


		public UserController(DatabaseContext databaseContext, IMapper mapper)
		{
            //injection
			_databaseContext = databaseContext;
            _mapper = mapper;
		}

		public IActionResult Index()
        {
            //Users tablosunu getir ve  userları UserModel classı ile mapleme işlemi
            List<UserModel> model = _databaseContext.Users.ToList().Select(u => _mapper.Map<UserModel>(u)).ToList();
            return View(model);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateUserModel model)
        {
            if (ModelState.IsValid) 
            {
                User user = _mapper.Map<User>(model); //modeli users'a çevir
                _databaseContext.Users.Add(user);   
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
