using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using ZaloDotNetSDK;

namespace chat_service.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class TagController : ControllerBase
	{
		public TagController() {}

		[HttpGet]
		public IActionResult Index()
		{
			try
			{
				var accessToken = Request.Headers["access_token"];
				object? result = null;

				var zClient = new ZaloClient(accessToken);
				result = zClient.getAllTagOfOfficialAccount();

				return new ContentResult
				{
					Content = result.ToString(),
					ContentType = "application/json",
					StatusCode = 200
				};
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

		[HttpDelete]
		public IActionResult Delete([FromBody] string tag_name)
		{
			try
			{
				var accessToken = Request.Headers["access_token"];
				object? result = null;
				var zClient = new ZaloClient(accessToken);
				result = zClient.deleteTag(tag_name);
				
				return new ContentResult
				{
					Content = result.ToString(),
					ContentType = "application/json",
					StatusCode = 200
				};
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


		[HttpPost("create")]
		public IActionResult Create([FromBody]Tag tag)
		{
			try
			{
				var accessToken = Request.Headers["access_token"];
				object? result = null;
				var zClient = new ZaloClient(accessToken);
				result = zClient.tagFollower(tag.user_id, tag.tag_name);
				
				return new ContentResult
				{
					Content = result.ToString(),
					ContentType = "application/json",
					StatusCode = 200
				};
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

		[HttpPost("remove")]
		public IActionResult Remove([FromBody]Tag tag)
		{
			try
			{
				var accessToken = Request.Headers["access_token"];
				object? result = null;
				var zClient = new ZaloClient(accessToken);
				result = zClient.removeTagFromFollower(tag.user_id, tag.tag_name);
				
				return new ContentResult
				{
					Content = result.ToString(),
					ContentType = "application/json",
					StatusCode = 200
				};
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

	public class Tag
	{
		public required string user_id { get; set; }	
		public required string tag_name {  get; set; }
	}
}
