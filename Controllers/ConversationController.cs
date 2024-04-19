using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ZaloDotNetSDK;
using chat_service.MyDbContext;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using Microsoft.AspNetCore.Http.HttpResults;

namespace chat_service.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {

        private ApplicationDbContext _context;

        public ConversationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("chats")]
        public IActionResult GetListChat([FromQuery]int offset = 0, [FromQuery]int count = 10)
        {
            try
            {
				var accessToken = Request.Headers["zToken"];
				object? result = null;

				if (User.IsInRole("Admin"))
				{
					var zClient = new ZaloClient(accessToken);
					result = zClient.getListRecentChat(offset, count);
				}
				else if (User.IsInRole("Normal"))
				{
					var dialogs = _context.Dialogs.Where(d => d.UserId == HttpContext.User.Identity.Name).Skip(offset).Take(count);
                    result = dialogs.ToList();
				}
                else
                {
                    result = new { 
                        Code = 1,
                        Errors = "Error"
                    };
                }

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
                    Errors = ex
                });
            }
        }

        [HttpGet("detail/{id}")]
        public IActionResult DetailConversation(long id, [FromQuery]int offset, [FromQuery]int count)
        {
            try
            {
				var accessToken = Request.Headers["zToken"];
				var zClient = new ZaloClient(accessToken);
				object? result = zClient.getListConversationWithUser(id, offset, count);
                
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
                    Code = 0,
                    Errors = ex
                });
            }
        }
    }
}
