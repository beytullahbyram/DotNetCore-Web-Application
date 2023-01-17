using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace DotNetCore_Web_Application.Entities
{
	[Table("Users")]
	public class User
	{
		public Guid Id { get; set; } 
		[StringLength(50)]
		public string? NameSurname { get; set; } // "?" null olabilir
		[Required]
		[StringLength(15)]
		public string Username { get; set; }
		[Required]
		[StringLength(100)]
		public string Passowrd { get; set; }
		public bool? Locked { get; set; }=false;
		public DateTime Created { get; set; }=DateTime.Now;
		[StringLength(255)]
		public string? ImageFileName { get; set; }="userimage.png";

		[Required,StringLength(50)]
		public string Role { get; set; }="user";

	}
}
