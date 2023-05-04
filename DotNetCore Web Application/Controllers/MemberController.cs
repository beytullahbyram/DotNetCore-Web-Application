using AutoMapper;
using DotNetCore_Web_Application.Entities;
using DotNetCore_Web_Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore_Web_Application.Controllers
{
	public class MemberController : Controller
	{

		private readonly DatabaseContext _databaseContext;
        private readonly IMapper _mapper;


		public MemberController(DatabaseContext databaseContext, IMapper mapper)
		{
			_databaseContext = databaseContext;
            _mapper = mapper;
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
	}
} 
