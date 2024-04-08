using System.ComponentModel.DataAnnotations;

namespace chat_service.Entities
{
	public class Dialog
	{
		[Key]
		public required string Id { get; set; } = Guid.NewGuid().ToString();
		public required string from_user_id { get; set; }	
		public required string to_user_id { get; set; }
		public int unread_message { get; set; } = 0;
		public DateTime last_time { get; set; }
	}
}
