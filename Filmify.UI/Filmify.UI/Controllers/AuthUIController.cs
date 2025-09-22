using Microsoft.AspNetCore.Mvc;

namespace Filmify.UI.Controllers
{
    public class AuthUIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
