using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace chat_service.Hubs
{
	//[EnableCors()]
	[Authorize]
	public class ChatHub : Hub
	{
		private ConnectionManager _connectionManager;

		public ChatHub(ConnectionManager connectionManager)
		{
			_connectionManager = connectionManager;
		}

		// event handler
		public async Task SendMessage(object message)
		{
			await Clients.Caller.SendAsync("new-message", message);
		}

		public async Task SendMessageToUser(string connection, dynamic message)
		{
			await Clients.Client(connection).SendCoreAsync("new-message", message);
		}

		// connection manager
		public override Task OnConnectedAsync()
		{
			_connectionManager.AddConnection(Context.ConnectionId, true);
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			_connectionManager.RemoveConnection(Context.ConnectionId);
			return base.OnDisconnectedAsync(exception);
		}
	}
}
