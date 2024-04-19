using System.ComponentModel.DataAnnotations;

namespace chat_service.Entities
{
	public class MessageLog
	{
		[Key]
		public string Id { get; set; }	= Guid.NewGuid().ToString();
		public string? message_id { get; set; }
		public string? data_request {  get; set; }
		public string? data_response {  get; set; }
		public bool status { get; set; }
		public DateTime time { get; set; } = DateTime.UtcNow;
	}
}
