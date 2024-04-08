using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace chat_service.Helpers
{
	public class Validator
	{
		public static bool IsValidJson(string jsonString)
		{
			try
			{
				JToken.Parse(jsonString);
				return true;
			}
			catch (JsonReaderException)
			{
				return false;
			}
		}
	}
}
