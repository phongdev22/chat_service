using System.Collections.Concurrent;

namespace chat_service.Hubs
{
	public class ConnectionManager
	{
		private readonly ConcurrentDictionary<string, dynamic> _connections = new ConcurrentDictionary<string, dynamic>();

		public void AddConnection(string connectionId, dynamic value)
		{
			_connections.TryAdd(connectionId, "");
		}

		public List<string> GetListOnline(List<string> users)
		{
			var res = new List<string>();
			foreach (var user in users)
			{
				var connectionId = GetConnectionId(user);
				if (connectionId != null)
				{
					res.Add(connectionId);
				}
			}
			return res;
		}


		public void RemoveConnection(string connectionId)
		{
			_connections.TryRemove(connectionId, out _);
		}

		public string? GetConnectionId(string user_id)
		{
			var connection = _connections.FirstOrDefault(value => value.Value?.user_id == user_id);
			if (!connection.Equals(default(KeyValuePair<string, dynamic>)))
			{
				return connection.Key;
			}
			return null;
		}

		public dynamic? GetUserId(string connectionId)
		{
			_connections.TryGetValue(connectionId, out dynamic? value);
			return value;
		}
	}
}
