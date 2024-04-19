using Microsoft.AspNetCore.Mvc;
using ZaloDotNetSDK;
using NuGet.Protocol;
using Microsoft.AspNetCore.Authorization;

namespace chat_service.Controllers
{
    [Authorize]
    [ApiController]
	[Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;
        private HttpClient _client;
        private string _url;

        public CustomerController(IConfiguration config) {
            _config = config;
            _client = new HttpClient();
            _url = _config.GetSection("Zalo")["API_V3"] ?? "";
        }

		[HttpGet]
		public async Task<IActionResult> Customer([FromQuery] Dictionary<string, int> data)
		{
			try
			{
				string? accessToken = Request.Headers["zToken"] ;
				string api_url = _url + "/user/getlist?data=" + data.ToJson();
				var request = new HttpRequestMessage(HttpMethod.Get, api_url);
				request.Headers.Add("access_token", accessToken);

				var response = await _client.SendAsync(request);
				var stringContent = await response.Content.ReadAsStringAsync();
				return Ok(stringContent);
			}
			catch (Exception e)
			{
				return Ok(e);
			}
		}

		[HttpGet("following")]
        public IActionResult GetFollowers([FromQuery]Dictionary<string, int> data)
        {
			try
			{
				var accessToken = Request.Headers["zToken"];
				object? result = null;
				var zClient = new ZaloClient(accessToken);

				// query param
				var offset = data.GetValueOrDefault("offset", 0);
				var count = data.GetValueOrDefault("offset", 10);

				result = zClient.getListFollower(offset, count);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return Ok(new
				{
					Code = 1,
					Errors = ex.Message
				});
			}
		}

		/*
		[HttpGet("followers/profile/{id}")]
		public IActionResult GetFollowerInfo(string id)
		{
			try
			{
				var accessToken = Request.Headers["access_token"];
				object? result = null;
				var zClient = new ZaloClient(access_token);
				result = zClient.getProfileOfFollower(id);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return Ok(new
				{
					Code = 1,
					Errors = ex.Message
				});
			}
		}
		*/

        [HttpGet("profile/{id}")]
		public async Task<IActionResult> Info(string id)
        {
			try
			{
				string? accessToken = Request.Headers["zToken"];

				string api_url = _url + "/user/detail?data={" + $"\"user_id\" = \"{id}" + "\"}";
				var request = new HttpRequestMessage(HttpMethod.Get, api_url);
				request.Headers.Add("access_token", accessToken);

				var response = await _client.SendAsync(request);
				var stringContent = await response.Content.ReadAsStringAsync();
				return Ok(stringContent);
			}
			catch (Exception ex)
			{
				return Ok(new
				{
					Code = 1,
					Errors = ex.Message
				});
			}
		}
    
    }
}
