using Microsoft.EntityFrameworkCore;

namespace DotNetCore_Web_Application.Entities
{
	public class DatabaseContext : DbContext
	{
		//dbcontexti newlemek istemediğimiz için options ile oluşturduk bunu program.cs'den ayarlıyoruz. Dependency Injection mekanizması. newleme işlemini orası bu bizim için yapacak 
		public DatabaseContext(DbContextOptions options) : base(options)
		{
		
			
		}
			public DbSet<User> Users { get; set; } //tabloyu temsil eder

	}
}
