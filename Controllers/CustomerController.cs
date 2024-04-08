using Microsoft.AspNetCore.Mvc;
using chat_service.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace chat_service.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;
        private HttpClient _client;
        private string _url;
        private string? access_token;

        public CustomerController(IConfiguration config) {
            _config = config;
            _client = new HttpClient();
            access_token = Request.Headers["access_token"];
            _url = _config.GetSection("Zalo")["API_V3"] ?? "";
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]string data)
        {
            try
            {
				if (data != null && !Validator.IsValidJson(data))
					return Ok(new { Code = 0, Message = "Data is not json format!" });

				if (data == null) { data = "{\"offset\":0,\"count\":10}"; }

				string api_url = _url + "/user/getlist?data=" + data;
				var request = new HttpRequestMessage(HttpMethod.Get, api_url);

				// Add access token to the headers
				request.Headers.Add("access_token", access_token);
				var response = await _client.GetAsync(api_url);

				return Ok(response);
			}
            catch(Exception e) {
                return Ok(e);
            }
        }
        
        [HttpGet("info/{id}")]
		public async Task<IActionResult> Info(string id)
        {
			string api_url = _url + "/user/detail?data={" + $"\"user_id\" = \"{id}" + "\"}";
			var request = new HttpRequestMessage(HttpMethod.Get, api_url);
			// Add access token to the headers
			request.Headers.Add("access_token", access_token);
			var response = await _client.GetAsync(api_url);
            return Ok(response);
		}
    
    }
}
