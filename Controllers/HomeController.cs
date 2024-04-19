using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace chat_service.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() {
            
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
