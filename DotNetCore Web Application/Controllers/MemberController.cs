using AutoMapper;
using DotNetCore_Web_Application.Entities;
using DotNetCore_Web_Application.Helpers;
using DotNetCore_Web_Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_Web_Application.Controllers
{
	public class MemberController : Controller
	{

		private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;
        private readonly IHasherMD5 _hasherMD5;


		public MemberController(DatabaseContext databaseContext, IMapper mapper, IHasherMD5 hasherMD5)
		{
			_databaseContext = databaseContext;
			_mapper = mapper;
			_hasherMD5 = hasherMD5;
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult MemberListPartial()
		{
            List<UserModel> model = _databaseContext.Users.ToList().Select(u => _mapper.Map<UserModel>(u)).ToList();
			return PartialView("_MemberListPartial",model);
		}

		public IActionResult AddNewUserPartial()
		{
			return PartialView("_AddNewUserPartial",new CreateUserModel());
		}
		[HttpPost]
		public IActionResult AddNewUser(CreateUserModel model) 
		{
            if (ModelState.IsValid) 
            {
				if (_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
				{
					ModelState.AddModelError(nameof(model.Username), "UserName Already avaliable");
			return PartialView("_AddNewUserPartial",model);
				}
				User user = _mapper.Map<User>(model); //modeli users'a çevir
				user.Passowrd=_hasherMD5.HashedString(model.Passowrd);

				_databaseContext.Users.Add(user);
				_databaseContext.SaveChanges();
			return PartialView("_AddNewUserPartial",new CreateUserModel(){ Done="user Added."});

			}
			return PartialView("_AddNewUserPartial",model);
		}


		#region Edit
		public IActionResult EditUserPartial(Guid id) 
		{
            User user=_databaseContext.Users.Find(id);
            EditUserModel model=_mapper.Map<EditUserModel>(user);   


			return PartialView("_EditUserPartial",model);
		}


		[HttpPost]
        public IActionResult EditUser(Guid id,EditUserModel model)
        {
            if (ModelState.IsValid) 
            {
                if (_databaseContext.Users.Any(x=>x.Username.ToLower() == model.Username.ToLower() && x.Id != id))
                {
                    ModelState.AddModelError(nameof(model.Username),"UserName Already avaliable");
			return PartialView("_EditUserPartial",model); 
                }

                User user = _databaseContext.Users.Find(id);
                _mapper.Map(model, user); 
                _databaseContext.SaveChanges();
			return PartialView("_EditUserPartial",new EditUserModel(){ Done="user edited."});
            }

			return PartialView("_EditUserPartial",model);
        }
		#endregion


		public IActionResult DeleteUser(Guid id)
        {
            User user= _databaseContext.Users.Find(id);

		    if (user != null)
            {
                _databaseContext.Users.Remove(user);
                _databaseContext.SaveChanges();
            }
			return MemberListPartial();

		}

	}
} 
