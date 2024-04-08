using Microsoft.AspNetCore.Mvc;

namespace chat_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DialogController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult Create()
        {
            return Ok();
        }
    }
}
