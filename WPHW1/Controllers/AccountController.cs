using Microsoft.AspNetCore.Mvc;

namespace WPHW1.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
