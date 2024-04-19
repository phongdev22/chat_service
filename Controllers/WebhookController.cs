using chat_service.Hubs;
using chat_service.MyDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Net.WebSockets;

namespace chat_service.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class WebhookController : ControllerBase
	{
		private ConnectionManager _connectionManager;
		private readonly ApplicationDbContext _context;
		private readonly IHubContext<ChatHub> _hubContext;

		public WebhookController(ConnectionManager connectionManager, IHubContext<ChatHub> hubContext, ApplicationDbContext context)
		{
			_connectionManager = connectionManager;
			_hubContext = hubContext;
			_context = context;
		}

		[HttpPost("event")]
		public async Task<IActionResult> Index([FromBody] WebhookData data)
		{
			try
			{
				//var msg_id = data.message.GetValueOrDefault("msg_id");
				var usersChats = _context.Dialogs.Where(uc => uc.ZaloUserId.Equals(data.recipient.id)).ToList();

				if (usersChats.Count > 0)
				{
					List<string> lstUser = new List<string>();
					usersChats.ForEach(item => lstUser.Add(item.UserId));

					var listClient = _connectionManager.GetListOnline(lstUser);

					await _hubContext.Clients.Clients(listClient).SendAsync("new-message", data);
				}

				return Ok(data);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			finally
			{
				var msg_id = data.message.GetValueOrDefault("msg_id");
				_context.MessageLogs.Add(new Entities.MessageLog()
				{
					message_id = msg_id,
					data_request = JsonConvert.SerializeObject(data).ToString(),
					data_response = ""
				});
				_context.SaveChanges();
			}
			return Ok();
		}
	}



	public class WebhookData
	{
		public string? app_id { get; set; }
		public string? user_id_by_app { get; set; }
		public string? event_name { get; set; }
		public required User sender { get; set; }
		public required User recipient { get; set; }
		public required Dictionary<string, dynamic> message { get; set; }
		public long time { get; set; }
	}

	public class User
	{
		public string? id { get; set; }
	}

}
