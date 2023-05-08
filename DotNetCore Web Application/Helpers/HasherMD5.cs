using NETCore.Encrypt.Extensions;

namespace DotNetCore_Web_Application.Helpers
{
	public interface IHasherMD5
	{
		string HashedString(string model);
	}

	public class HasherMD5 : IHasherMD5
	{
		private readonly IConfiguration _configuration;

		public HasherMD5(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string HashedString(string model)
		{
			string md5salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
			string salted = model + md5salt;
			string hashed = salted.MD5();
			return hashed;
		}


	}
}
