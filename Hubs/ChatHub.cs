using Microsoft.AspNetCore.SignalR;

namespace chat_service.Hubs
{
	public class ChatHub : Hub
    {
		private ConnectionManager _connectionManager;

		public ChatHub(ConnectionManager connectionManager)
		{
			_connectionManager = connectionManager;
		}
		// event handler

		public async Task SendMessage(dynamic message)
		{
			await Clients.All.SendCoreAsync("server-send-message", message);
		}

		// connection manager
		public override Task OnConnectedAsync()
		{
			_connectionManager.AddConnection(Context.ConnectionId, "");
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			_connectionManager.RemoveConnection(Context.ConnectionId);
			return base.OnDisconnectedAsync(exception);
		}
	}
}
