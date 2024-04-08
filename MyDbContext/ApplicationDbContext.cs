using chat_service.Entities;
using Microsoft.EntityFrameworkCore;

namespace chat_service.MyDbContext
{
	public class ApplicationDbContext : DbContext
	{

		public DbSet<MessageLog> MessageLogs { get; set; }
		public DbSet<Account> Accounts { get; set; }

		public ApplicationDbContext(DbContextOptions options) : base(options) {	}
	}
}
