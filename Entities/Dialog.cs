using System.ComponentModel.DataAnnotations;
using chat_service.MyDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace chat_service.Entities
{
	public class Dialog
	{
		[Key]
		public required string Id { get; set; }
		public required string UserId { get; set; }	
		public required string ZaloUserId { get; set; }
		public DateTime Updated { get; set; }
	}
}
