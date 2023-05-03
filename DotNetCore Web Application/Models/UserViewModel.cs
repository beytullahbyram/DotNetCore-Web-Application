using System.ComponentModel.DataAnnotations;

namespace DotNetCore_Web_Application.Models
{
	public class UserModel
	{
		public Guid Id { get; set; } 
		public string? NameSurname { get; set; } // "?" null olabilir
		public string Username { get; set; }
		public bool Locked { get; set; }=false;
		public DateTime Created { get; set; }=DateTime.Now;
		public string? ImageFileName { get; set; }="userimage.png";
		public string Role { get; set; }="user";
	}



	public class CreateUserModel
	{
        //[Display(Name ="Kullanıcı Adı", Prompt ="johndoe")]
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username can be max 30 characters.")]
        public string Username { get; set; }

        [Required]
        [StringLength(50)]
        public string NameSurname { get; set; }

        public bool Locked { get; set; }

        //[DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password can be min 6 characters.")]
        [MaxLength(16, ErrorMessage = "Password can be max 16 characters.")]
        public string Passowrd { get; set; }

        [Required(ErrorMessage = "Re-Password is required.")]
        [MinLength(6, ErrorMessage = "Re-Password can be min 6 characters.")]
        [MaxLength(16, ErrorMessage = "Re-Password can be max 16 characters.")]
        [Compare(nameof(Passowrd))]
        public string RePassword { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "user";

	}
}
