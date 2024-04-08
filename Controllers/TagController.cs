using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace chat_service.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TagController : ControllerBase
	{
		private HttpClient _client;
		private IConfiguration _config;
		private string? access_token;
		private string url_api;

		public TagController(IConfiguration config) {
			_client = new HttpClient();
			_config = config;
			access_token = "";  // Request.Headers["access_token"];
			url_api = _config["Zalo:API_V2"] + "/tag";
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			if (access_token == null) return Ok(new { Code = 0, Message = "Access token is invalid!" });

			var request = new HttpRequestMessage(HttpMethod.Post, url_api + "/gettagsofoa");
			request.Headers.Add("access_token", access_token);
			var response = await _client.SendAsync(request);

			return Ok(response);
		}

		[HttpDelete]
		public async Task<IActionResult> Index([FromBody] string tag_name)
		{
			if (access_token == null) return Ok(new { Code = 0, Message = "Access token is invalid!" });

			var request = new HttpRequestMessage(HttpMethod.Post, url_api + "/rmtag");
			request.Headers.Add("access_token", access_token);
			var response = await _client.SendAsync(request);

			request.Content = new StringContent(JsonSerializer.Serialize(new { tag_name = tag_name })
												, Encoding.UTF8, "application/json");

			return Ok(response);
		}


		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody]Tag tag)
		{
			if (access_token == null) return Ok(new {Code = 0, Message = "Access token is invalid!" });

			var request = new HttpRequestMessage(HttpMethod.Post, url_api + "/tagfollower");
			request.Headers.Add("access_token", access_token);
			request.Content = new StringContent(JsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");
			var response = await _client.SendAsync(request);

			return Ok(response);
		}

		[HttpPost("remove")]
		public async Task<IActionResult> Remove([FromBody]Tag tag)
		{
			if (access_token == null) return Ok(new { Code = 0, Message = "Access token is invalid!" });

			var request = new HttpRequestMessage(HttpMethod.Post, url_api + "/rmfollowerfromtag");
			request.Headers.Add("access_token", access_token);
			request.Content = new StringContent(JsonSerializer.Serialize(tag), Encoding.UTF8, "application/json");
			var response = await _client.SendAsync(request);

			return Ok(response);
		}
	}

	public class Tag
	{
		public required string user_id { get; set; }	
		public required string tag_name {  get; set; }
	}
}
