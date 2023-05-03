using AutoMapper;
using DotNetCore_Web_Application.Entities;
using DotNetCore_Web_Application.Models;

namespace DotNetCore_Web_Application
{
	public class AutoMapperConfig : Profile
	{
		public AutoMapperConfig()
		{
			//Mapping
			//User classı UserModel classına çevirir.ReverseMap ile tam tersi işlemi yapar.
			CreateMap<User,UserModel>().ReverseMap();
			CreateMap<User,CreateUserModel>().ReverseMap();

		}
	}
}
