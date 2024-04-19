using chat_service.Entities;
using chat_service.MyDbContext;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace chat_service.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class DialogController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public DialogController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllDialog(int offset = 0, int count = 10)
		{
			var dialogQuery = _context.Dialogs.AsQueryable();

			int totalDialogs = await dialogQuery.CountAsync();
			var dialogs = await dialogQuery.Skip(offset).Take(count).ToListAsync();

			return Ok(new { TotalDialogs = totalDialogs, Dialogs = dialogs });
		}

		[HttpGet("{zaloUserId}")]
		public async Task<IActionResult> GetDialogByUser(string zaloUserId, int offset = 0 , int count = 10)
		{
			var dialogQuery = _context.Dialogs.AsQueryable();

			int totalDialogs = await dialogQuery.CountAsync();
			var dialogs = await dialogQuery.Where(dlg => dlg.ZaloUserId.Equals(zaloUserId)).Skip(offset).Take(count).ToListAsync();
			return Ok(new { TotalDialogs = totalDialogs, Dialogs = dialogs });
		}

		[HttpPost("assign")]
		public async Task<IActionResult> AssignZaloUserIdToUser([FromBody]AssignModel data)
		{
			try
			{
				var dlg = new Dialog()
				{
					Id = Guid.NewGuid().ToString(),
					UserId = data.UserId,
					ZaloUserId = data.ZaloUserId
				};

				_context.Dialogs.Add(dlg);
				await _context.SaveChangesAsync();

				return Ok(new {Code = 0, Message = "Assign success!"});
			}
			catch(Exception ex)
			{
				return Ok(new { 
					Code = 1,
					Errors = ex
				});
			}
		}

	}

	public class AssignModel
	{
		public required string UserId { get; set; }
		public required string ZaloUserId { get; set; } 
	}
}
