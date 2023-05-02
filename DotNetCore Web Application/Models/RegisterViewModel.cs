using System.ComponentModel.DataAnnotations;

namespace DotNetCore_Web_Application.Models
{
	public class RegisterViewModel : LoginViewModel
	{
		//attribute tanımlama

		[Required(ErrorMessage ="NameSurname is required")]
		[StringLength(15,ErrorMessage ="NameSurname can be max 15 character")]
		public string NameSurname { get; set; }

		[Required(ErrorMessage ="Username is required")]
		[StringLength(15,ErrorMessage ="Username can be max 15 character")]
		public string Username { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage ="Password is required")]
		[MinLength(6,ErrorMessage ="Username can be min 6 character")]
		[MaxLength(15,ErrorMessage ="Username can be max 15 character")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage ="Re-Password is required")]
		[MinLength(6,ErrorMessage ="Re-Password can be min 6 character")]
		[MaxLength(15,ErrorMessage ="Re-Password can be max 15 character")]
		[Compare(nameof(Password))]//üstteki password ile karşılaştır //nameof kullanmamız eğer üstteki password değişirise bu repassword propertisinde hata almamaız için, string olarak yazarsak hata vermez ama kod çalışmaz
		public string RePassword { get; set; }

	}
}
