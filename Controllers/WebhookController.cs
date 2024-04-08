using chat_service.Hubs;
using chat_service.MyDbContext;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace chat_service.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WebhookController : ControllerBase
	{
		private ConnectionManager _connectionManager;
		private readonly ApplicationDbContext _context;
		private readonly IHubContext<ChatHub> _hubContext;

		public WebhookController(ApplicationDbContext context, IHubContext<ChatHub> hubcontext ,ConnectionManager connectionManager) {
			_connectionManager = connectionManager;
			_context = context;
			_hubContext = hubcontext;
		}

		[HttpPost("event")]		
		public async Task<IActionResult> Index([FromBody]WebhookData data)
		{
			// save log lại
			var msg_id = data.message.GetValueOrDefault("msg_id");
			_context.MessageLogs.Add(new Entities.MessageLog() { 
				message_id = msg_id,
				to_user_id = data.sender.id,
				data_request = JsonConvert.SerializeObject(data).ToString(),
				data_response = ""
			});
			
			_context.SaveChanges();


			// connection rỗi thì annouce tới conenction đó
			var connection = _connectionManager.FilterConnectionFree();


			_hubContext.Clients.Client(connection).SendMessage(data);

			return Ok(data);
		}


		private void handleZaloWebhook()
		{

		}
	}



	public class WebhookData
	{
		public string app_id { get; set; }
		public string user_id_by_app { get; set; }
		public string event_name { get; set; }
		public User sender { get; set; }
		public User recipient {	get; set; }
		public Dictionary<string, dynamic> message { get; set; }	
		public long time { get; set; }
	}

	public class User
	{
		public string id { get; set; }
	}

}
