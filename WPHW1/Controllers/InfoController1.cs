using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace WPHW1.Controllers
{
    public class InfoController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
