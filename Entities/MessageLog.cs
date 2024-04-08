using System.ComponentModel.DataAnnotations;

namespace chat_service.Entities
{
	public class MessageLog
	{
		[Key]
		public required string message_id { get; set; }
		public required string to_user_id { get; set; }
		public required string data_request {  get; set; }	
		public required string data_response {  get; set; }	
		public string? from_user_id { get; set; }
		public bool status { get; set; }
	}
}
