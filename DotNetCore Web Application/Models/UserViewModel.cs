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
}
