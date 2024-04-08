using chat_service.Controllers;
using chat_service.Hubs;
using chat_service.Models;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Text;

namespace chat_service.Services
{
	public class MessageSenderService
	{
		private readonly ConnectionManager _connectionManager;
		private IHubContext<ChatHub> _hubContext;
		private HttpClient _client = new HttpClient();

		public MessageSenderService(ConnectionManager connectionManager, IHubContext<ChatHub> hubContext)
		{
			_connectionManager = connectionManager;
			_hubContext = hubContext;
		}

		public async Task SendMessageToUser(string userId,string message_id, ChatContentModel content)
		{
			var connectionId = _connectionManager.GetConnectionId(userId);
			if (connectionId != null)
			{
				await _hubContext.Clients.Client(connectionId).SendAsync("new-message", new { userId, message_id, content });
			}
		}
	}
}
