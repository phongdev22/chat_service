using System.ComponentModel.DataAnnotations;

namespace chat_service.Entities
{
	public class Account
	{
		[Key]
		public string Id { get; set; }  = Guid.NewGuid().ToString();
		public required string Username { get; set; }	
		public required string Password { get; set; }
	}
}
