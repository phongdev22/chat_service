using System.Collections.Concurrent;

namespace chat_service.Hubs
{
	public class ConnectionManager
	{
		private readonly ConcurrentDictionary<string, dynamic> _connections = new ConcurrentDictionary<string, dynamic>();

		public void AddConnection(string connectionId, dynamic value)
		{
			_connections.TryAdd(connectionId, value);
		}

		public string FilterConnectionFree()
		{
			var conenctionId = "";

			var connection = _connections.FirstOrDefault(value => value.Value?.status == true);
			if (!connection.Equals(default(KeyValuePair<string, dynamic>)))
			{
				return connection.Key;
			}

			return conenctionId;
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
			_connections.TryGetValue(connectionId, out dynamic? value );
			return value;
		}
	}
}
