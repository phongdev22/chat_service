namespace chat_service.Models
{
	public class ChatContentModel
	{
		public required string user_id {get; set;}
		public required string type_message {get; set;}
		public required Message message {get; set;}
	}

	public class Message {
		public string? quote_message_id {get; set;}
		public string? text {get; set;}
		public Attachment? attachment {get; set;}
	}

	public class Attachment {
		public required string attachment_type {get; set;}
		public required Payload payload {get; set;}
	}

    public class  Payload
    {
		public required string template_type {get; set;}
		public required List<dynamic> elements {get; set;}
    }
}
