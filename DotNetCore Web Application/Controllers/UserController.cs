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
		#region Create
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
                if (_databaseContext.Users.Any(x=>x.Username.ToLower() == model.Username.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Username),"UserName Already avaliable");
                    return View(model);   
                }
                User user = _mapper.Map<User>(model); //modeli users'a çevir
                _databaseContext.Users.Add(user);   
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
		#endregion


        #region Edit
		[HttpGet]
        public IActionResult Edit(Guid id)
        {
            User user=_databaseContext.Users.Find(id);
            EditUserModel model=_mapper.Map<EditUserModel>(user);   

            return View(model );
        }
        [HttpPost]
        public IActionResult Edit(Guid id,EditUserModel model)
        {
            if (ModelState.IsValid) 
            {
                if (_databaseContext.Users.Any(x=>x.Username.ToLower() == model.Username.ToLower() && x.Id != id))
                {
                    ModelState.AddModelError(nameof(model.Username),"UserName Already avaliable");
                    return View(model);   
                }

                User user = _databaseContext.Users.Find(id);
                _mapper.Map(model, user); //modeli user'a çevir  
                _databaseContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
		#endregion
	}
}
