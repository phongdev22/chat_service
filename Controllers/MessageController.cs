using chat_service.Models;
using chat_service.MyDbContext;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Newtonsoft.Json;
using chat_service.Services;
using Microsoft.AspNetCore.Authorization;

namespace chat_service.Controllers
{

    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
		private IConfiguration _config;
		private readonly ApplicationDbContext _dbContext;
		private MessageSenderService _messageSenderService;


		private HttpClient _client = new HttpClient();
		private string? access_token;
		public MessageController(IConfiguration config,
								 ApplicationDbContext dbContext,
								 MessageSenderService messageSenderService)
        {
			_config = config;
			_dbContext = dbContext;
			access_token = "" ;//Request.Headers["access_token"].ToString() ?? "";
			_messageSenderService = messageSenderService;
		}

        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody]ChatContentModel content)
        {

			return Ok(content);

			try
			{
				// send text
				var PostData = this.TypeMessageHandler(content);
				var request = new HttpRequestMessage(HttpMethod.Post, _config["Zalo:API_V3"] + "/message/cs");

				request.Headers.Add("access_token", this.access_token);
				request.Content = new StringContent(JsonConvert.SerializeObject(PostData), Encoding.UTF8, "application/json");

				var response = await _client.SendAsync(request);
				if (response.IsSuccessStatusCode)
				{
					var stringContent = await response.Content.ReadAsStringAsync();
					var JsonResponse = JsonConvert.DeserializeObject<ApiResponse>(stringContent);
					
					// save log data response, data post and message_id
					if(JsonResponse.error == 0 && JsonResponse.message == "Success")
					{

						// add log message & add dialog status
						_dbContext.MessageLogs.Add(new Entities.MessageLog
						{
							message_id = JsonResponse.data.message_id,
							to_user_id = content.user_id,
							data_request = PostData.ToString(),
							data_response = stringContent,
						});

						_dbContext.SaveChanges();

						//======================================
						
						await _messageSenderService.SendMessageToUser(content.user_id, JsonResponse.data.message_id, content);
						return Ok(new { Code = 0, Data = content });
					}

				}
				return Ok(new {Code = 1, Data = response});
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return Ok(new { Code = 1, Message = ex.Message});
			}
        }

		private dynamic TypeMessageHandler(ChatContentModel content)
		{
			dynamic data = new System.Dynamic.ExpandoObject();

			data.recipient = new { user_id = content.user_id };
			switch (content.type_message)
			{
				case "text":
					data.message = new { text = content.message.text };
					break;

				case "reply":
					data.message = new
					{
						text = content.message.text,
						quote_message_id = content.message.quote_message_id
					};
					break;
				default:
					break;
			}

			return data;
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

}
