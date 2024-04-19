using chat_service.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace chat_service.MyDbContext
{
	public class ApplicationDbContext : IdentityDbContext<IdentityUser>
	{
		public DbSet<MessageLog> MessageLogs { get; set; }
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Dialog> Dialogs { get; set; }
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {	}

		public ApplicationDbContext() { }	
	}
}
