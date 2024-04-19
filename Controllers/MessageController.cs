using chat_service.MyDbContext;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using chat_service.Entities;
using ZaloDotNetSDK;
using Microsoft.AspNetCore.Authorization;


namespace chat_service.Controllers
{

	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class MessageController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		public MessageController(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		[HttpPost("send")]
		public IActionResult Send([FromBody] ApiRequest content)
		{
			try
			{
				content.username = HttpContext.User.Identity.Name;
				var accessToken = Request.Headers["zToken"];
				object? result = null;
				
				var zClient = new ZaloClient(accessToken);

				if (content.reply_id != null)
				{
					result = zClient.sendTextMessageToMessageId(content.reply_id, content.text);
				}
				else if (content.file != null)
				{
					var file = new ZaloFile(content.file.FileName);
					result = new { Message = "No support in recent!" };
				}
				else
				{
					result = zClient.sendTextMessageToUserIdV3(content.to_user_id, content.text);
					_dbContext.MessageLogs.Add(new MessageLog()
					{
						data_request = JsonConvert.SerializeObject(content),
						data_response = JsonConvert.SerializeObject(result),
						status =  true
					});
					_dbContext.SaveChanges();
				}

				return new ContentResult
				{
					Content = result.ToString(),
					ContentType = "application/json",
					StatusCode = 200
				};
			}
			catch (Exception ex)
			{
				return Ok(new { Code = 1, Message = ex.Message });
			}
		}
	}

	public class Quota
	{
		public string? type { get; set; }
		public string? quota_type { get; set; }
		public int remain { get; set; }
		public int total { get; set; }
		public DateTime expired_date { get; set; }
	}

	public class Data
	{
		public Quota? quota { get; set; }
		public required string message_id { get; set; }
		public required string user_id { get; set; }
	}

	public class ApiResponse
	{
		public required Data data { get; set; }
		public int error { get; set; }
		public string? message { get; set; }
	}

	public class ApiRequest
	{
		// username hệ thống cấp
		public string? username { get; set; }

		// user_id của zalo
		public required string to_user_id { get; set; }
		public required string text { get; set; }
		public string? reply_id { get; set; }
		public IFormFile? file { get; set; }
	}

}
