using System.ComponentModel.DataAnnotations;

namespace DotNetCore_Web_Application.Models
{
	public class LoginViewModel
	{
		//attribute tanımlama
		[Required(ErrorMessage ="Username is required")]
		[StringLength(15,ErrorMessage ="Username can be max 15 character")]
		public string Username { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage ="Password is required")]
		[MinLength(6,ErrorMessage ="Username can be min 6 character")]
		[MaxLength(15,ErrorMessage ="Username can be max 15 character")]
		public string Password { get; set; }

	}
}
