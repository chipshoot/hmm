using Microsoft.AspNetCore.Mvc;

namespace Hmm.WebConsole.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}