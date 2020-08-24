using Microsoft.AspNetCore.Mvc;

namespace Hmm.WebConsole.Controllers
{
    public class AuthorizationController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}