using chat_service.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using chat_service.Services;

namespace chat_service.Controllers
{
    public class HomeController : Controller
    {
        private MessageSenderService _messageSenderService;
        public HomeController(MessageSenderService messageSenderService) {
            _messageSenderService = messageSenderService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
